using MonoMod.RuntimeDetour;
using PetsOverhaul.NPCs;
using PetsOverhaul.PetEffects;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul
{
    public class PetsOverhaul : Mod
    {
        public static Action<Item, Player> OnPickupActions;
        private delegate bool orig_ItemLoaderOnPickup(Item item, Player player);
        private delegate bool hook_ItemLoaderOnPickup(orig_ItemLoaderOnPickup orig, Item item, Player player);
        private static readonly MethodInfo OnPickupInfo = typeof(ItemLoader).GetMethod("OnPickup");

        private readonly List<Hook> hooks = [];
        public override void Load()
        {
            hooks.Add(new(OnPickupInfo, ItemLoaderOnPickupDetour));
            foreach (Hook hook in hooks)
            {
                hook.Apply();
            }
        }
        private static bool ItemLoaderOnPickupDetour(orig_ItemLoaderOnPickup orig, Item item, Player player)
        {
            OnPickupActions?.Invoke(item, player);

            return orig(item, player);
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType msgType = (MessageType)reader.ReadByte();
            switch (msgType)
            {
                case MessageType.ShieldFullAbsorb:
                    int damage = reader.ReadInt32();
                    GlobalPet.HandleShieldBlockMessage(reader, whoAmI, damage);
                    break;
                case MessageType.SeaCreatureOnKill:
                    int npcId = reader.ReadInt32();
                    Junimo.RunSeaCreatureOnKill(reader, whoAmI, npcId);
                    break;
                case MessageType.HoneyBeeHeal:
                    bool bottledHoney = reader.ReadBoolean();
                    int honeyBeeWhoAmI = reader.ReadByte();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        ModPacket packet = GetPacket();
                        packet.Write((byte)msgType);
                        packet.Write(bottledHoney);
                        packet.Write((byte)honeyBeeWhoAmI);
                        packet.Send(ignoreClient: honeyBeeWhoAmI);
                    }
                    HoneyBee.HealByHoneyBee(bottledHoney, honeyBeeWhoAmI, false);
                    break;
                case MessageType.BlockPlace: //Currently only sent to Server.
                    int xPlace = reader.ReadInt32();
                    int yPlace = reader.ReadInt32();
                    PlayerPlacedBlockList.placedBlocksByPlayer.Add(new Point16(xPlace, yPlace));
                    break;
                case MessageType.BlockReplace: //Currently only sent to Server.
                    int xReplace = reader.ReadInt32();
                    int yReplace = reader.ReadInt32();
                    GlobalPet.updateReplacedTile.Add(new Point16(xReplace, yReplace));
                    break;
                case MessageType.BlockRemove: //Currently only sent to Server.
                    int xRemove = reader.ReadInt32();
                    int yRemove = reader.ReadInt32();
                    GlobalPet.CoordsToRemove.Add(new Point16(xRemove, yRemove));
                    break;
                case MessageType.PetSlow:
                    NPC npc = Main.npc[reader.ReadByte()];
                    float slowAmount = reader.ReadSingle();
                    int slowTime = reader.ReadInt32();
                    sbyte slowId = reader.ReadSByte();
                    PetSlow slow = new(slowAmount, slowTime, slowId);
                    if (npc.active)
                        NPCGlobalPet.AddToSlowList(slow, npc);
                    if (Main.netMode == NetmodeID.Server)
                    {
                        ModPacket packet = GetPacket();
                        packet.Write((byte)MessageType.PetSlow);
                        packet.Write((byte)npc.whoAmI);
                        packet.Write(slowAmount);
                        packet.Write(slowTime);
                        packet.Write(slowId);
                        packet.Send();
                    }
                    break;
                case MessageType.NPCOnDeathEffect: //Only sent to Multiplayer clients currently, in NpcPet GlobalNPC class, inside OnKill hook.
                    int playerWho = reader.ReadByte();
                    NPCGlobalPet.OnKillInvokeDeathEffects(playerWho, Main.npc[reader.ReadByte()]);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(msgType));
            }
        }
        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("Census", out Mod censusMod))
            {
                censusMod.Call("TownNPCCondition", ModContent.NPCType<PetTamer>(), ModContent.GetInstance<PetTamer>().GetLocalization("Census.SpawnCondition"));
            }
            if (ModLoader.TryGetMod("AlchemistNPCLite", out Mod alchemistNPCLite))
            {
                if (alchemistNPCLite.TryFind("VanTankComb", out ModBuff vanTank))
                {
                    BabyImp.ObsidianSkinEffects[vanTank.Type] = true;
                    Destroyer.IronskinEffects[vanTank.Type] = true;
                }
                if (alchemistNPCLite.TryFind("TankComb", out ModBuff tank))
                {
                    BabyImp.ObsidianSkinEffects[tank.Type] = true;
                    Destroyer.IronskinEffects[tank.Type] = true;
                }
                if (alchemistNPCLite.TryFind("BattleComb", out ModBuff battle))
                {
                    Destroyer.IronskinEffects[battle.Type] = true;
                }
                if (alchemistNPCLite.TryFind("FishingComb", out ModBuff fish))
                {
                    Destroyer.IronskinEffects[fish.Type] = true;
                }
            }
        }
    }
}