using MonoMod.RuntimeDetour;
using PetsOverhaul.Achievements;
using PetsOverhaul.NPCs;
using PetsOverhaul.PetEffects;
using PetsOverhaul.Systems;
using PetsOverhaul.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

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

            //USE THIS WITH CAUTION, only intended use is with HandleBasicSyncMessage!
            Player EasyPlayer()
            {
                if (Main.netMode == NetmodeID.Server)
                {
                    return Main.player[whoAmI];
                }
                return Main.player[reader.ReadByte()];
            }
            void HandleBasicSyncMessage(Action method)
            {
                method.Invoke();
                if (Main.netMode == NetmodeID.Server)
                {
                    ModPacket packet = GetPacket();
                    packet.Write((byte)msgType);
                    packet.Write((byte)whoAmI);
                    packet.Send(ignoreClient: whoAmI);
                }
            }


            switch (msgType)
            {
                case MessageType.MultiplayerDebugText:
                    if (Main.netMode == NetmodeID.Server)
                    {
                        ModPacket packet = GetPacket();
                        packet.Write((byte)msgType);
                        packet.Write(reader.ReadString());
                        packet.Send();
                    }
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        Main.NewText(reader.ReadString());
                    }
                    break;

                case MessageType.ShieldFullAbsorb:
                    int damage = reader.ReadInt32();
                    PetModPlayer.HandleShieldBlockMessage(reader, whoAmI, damage);
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
                    PetModPlayer.updateReplacedTile.Add(new Point16(xReplace, yReplace));
                    break;
                case MessageType.BlockRemove: //Currently only sent to Server.
                    int xRemove = reader.ReadInt32();
                    int yRemove = reader.ReadInt32();
                    PetModPlayer.CoordsToRemove.Add(new Point16(xRemove, yRemove));
                    break;
                case MessageType.PetSlow:
                    NPC npc = Main.npc[reader.ReadByte()];
                    float slowAmount = reader.ReadSingle();
                    int slowTime = reader.ReadInt32();
                    sbyte slowId = reader.ReadSByte();
                    PetSlow slow = new(slowAmount, slowTime, slowId);
                    if (npc.active)
                        PetGlobalNPC.AddToSlowList(slow, npc);
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
                    PetGlobalNPC.OnKillInvokeDeathEffects(playerWho, Main.npc[reader.ReadByte()]);
                    break;
                case MessageType.ActivePetSlot:
                    if (Main.netMode == NetmodeID.Server)
                    {
                        Item CurrentPetItemActive = ItemIO.Receive(reader);
                        Main.player[whoAmI].GetModPlayer<ActivePetSlotPlayer>().RegularPetItemSlot[Main.player[whoAmI].CurrentLoadoutIndex] = CurrentPetItemActive;
                        ModPacket packet = GetPacket();
                        packet.Write((byte)MessageType.ActivePetSlot);
                        packet.Write((byte)whoAmI);
                        ItemIO.Send(CurrentPetItemActive, packet);
                        packet.Send(ignoreClient: whoAmI);
                    }
                    else if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        int playrWho = reader.ReadByte();
                        Item CurrentPetItemActive = ItemIO.Receive(reader);
                        Main.player[playrWho].GetModPlayer<ActivePetSlotPlayer>().RegularPetItemSlot[Main.player[playrWho].CurrentLoadoutIndex] = CurrentPetItemActive;
                    }
                    break;
                case MessageType.ActiveLightPetSlot:
                    if (Main.netMode == NetmodeID.Server)
                    {
                        Item CurrentLightPetItemActive = ItemIO.Receive(reader);
                        Main.player[whoAmI].GetModPlayer<ActivePetSlotPlayer>().LightPetItemSlot[Main.player[whoAmI].CurrentLoadoutIndex] = CurrentLightPetItemActive;
                        ModPacket packet = GetPacket();
                        packet.Write((byte)MessageType.ActiveLightPetSlot);
                        packet.Write((byte)whoAmI);
                        ItemIO.Send(CurrentLightPetItemActive, packet);
                        packet.Send(ignoreClient: whoAmI);
                    }
                    else if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        int playrWho = reader.ReadByte();
                        Item CurrentLightPetItemActive = ItemIO.Receive(reader);
                        Main.player[playrWho].GetModPlayer<ActivePetSlotPlayer>().RegularPetItemSlot[Main.player[playrWho].CurrentLoadoutIndex] = CurrentLightPetItemActive;
                    }
                    break;
                case MessageType.CustomEffectSwitch:
                    if (Main.netMode == NetmodeID.Server)
                    {
                        ModPacket packet = GetPacket();
                        ushort modPlayerIndex = reader.ReadUInt16();
                        if (Main.player[whoAmI].ModPlayers[modPlayerIndex].GetType().IsSubclassOf(typeof(PetEffect)))
                        {
                            PetEffect current = (PetEffect)Main.player[whoAmI].ModPlayers[modPlayerIndex];
                            current.CustomEffectActive = !current.CustomEffectActive;
                        }
                        packet.Write((byte)msgType);
                        packet.Write((byte)whoAmI);
                        packet.Write(modPlayerIndex);
                        packet.Send(ignoreClient: whoAmI);
                    }
                    else
                    {
                        byte switchinPlayer = reader.ReadByte();
                        ushort modPlayerIndex2 = reader.ReadUInt16();
                        if (Main.player[switchinPlayer].ModPlayers[modPlayerIndex2].GetType().IsSubclassOf(typeof(PetEffect)))
                        {
                            PetEffect current = (PetEffect)Main.player[switchinPlayer].ModPlayers[modPlayerIndex2];
                            current.CustomEffectActive = !current.CustomEffectActive;
                        }
                    }
                    break;

                //Achievement messages, these are received by the Multiplayer Client
                case MessageType.LootChaser:
                    ModContent.GetInstance<LootChaser>().Count.Value++;
                    break;
                case MessageType.QuestionableKibble:
                    ModContent.GetInstance<QuestionableKibble>().PetShimmered.Complete();
                    break;
                case MessageType.CrispyFriedCalamari:
                    ModContent.GetInstance<CrispyFriedCalamari>().PetFried.Complete();
                    break;

                //Pet Trigger Syncs
                case MessageType.AlienSkater:
                    HandleBasicSyncMessage(EasyPlayer().GetModPlayer<AlienSkater>().Fly);
                    break;
                case MessageType.BabyRedPanda:
                    HandleBasicSyncMessage(EasyPlayer().GetModPlayer<BabyRedPanda>().Alert);
                    break;
                case MessageType.BlackCat:
                    HandleBasicSyncMessage(EasyPlayer().GetModPlayer<BlackCat>().Moonlight);
                    break;
                case MessageType.Lizard:
                    HandleBasicSyncMessage(EasyPlayer().GetModPlayer<Lizard>().Decoy);
                    break;
                case MessageType.Moonling:
                    HandleBasicSyncMessage(EasyPlayer().GetModPlayer<Moonling>().SwapClass);
                    break;
                case MessageType.PhantasmalDragonSpell:
                    HandleBasicSyncMessage(EasyPlayer().GetModPlayer<PhantasmalDragon>().CastSpell);
                    break;
                case MessageType.PhantasmalDragonAbilitySwap:
                    HandleBasicSyncMessage(EasyPlayer().GetModPlayer<PhantasmalDragon>().SwitchSpell);
                    break;
                case MessageType.SlimePrince:
                    HandleBasicSyncMessage(EasyPlayer().GetModPlayer<SlimePrince>().Summon);
                    break;
                case MessageType.SugarGliderGlide: //Currently unused
                    HandleBasicSyncMessage(EasyPlayer().GetModPlayer<SugarGlider>().Glide);
                    break;
                case MessageType.SugarGliderAbility:
                    HandleBasicSyncMessage(EasyPlayer().GetModPlayer<SugarGlider>().Shuricorn);
                    break;
                case MessageType.SuspiciousEye:
                    HandleBasicSyncMessage(EasyPlayer().GetModPlayer<SuspiciousEye>().ForceEnrage);
                    break;
                case MessageType.TinyDeerclops:
                    HandleBasicSyncMessage(EasyPlayer().GetModPlayer<TinyDeerclops>().InitiateStrike);
                    break;
                case MessageType.Turtle:
                    HandleBasicSyncMessage(EasyPlayer().GetModPlayer<Turtle>().ShellUp);
                    break;
                case MessageType.SugarGliderAbilityHit:
                    short npcWho = reader.ReadInt16();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        ModPacket packet = GetPacket();
                        packet.Write((byte)msgType);
                        packet.Write(npcWho);
                        packet.Write((byte)whoAmI);
                        packet.Send(ignoreClient: whoAmI);

                        Main.player[whoAmI].GetModPlayer<SugarGlider>().shuricornTaggedNpc = npcWho;
                        if (Main.npc[npcWho].active && Main.npc[npcWho].TryGetGlobalNPC(out PetGlobalNPC shuricornNpc))
                        {
                            shuricornNpc.shuricornMark = 300;
                        }
                    }
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        Main.player[reader.ReadByte()].GetModPlayer<SugarGlider>().shuricornTaggedNpc = npcWho;
                        if (Main.npc[npcWho].active && Main.npc[npcWho].TryGetGlobalNPC(out PetGlobalNPC shuricornNpc))
                        {
                            shuricornNpc.shuricornMark = 300;
                        }
                    }
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