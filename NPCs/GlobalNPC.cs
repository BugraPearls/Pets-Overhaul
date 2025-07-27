using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using PetsOverhaul.Buffs;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.NPCs
{
    public class NotABossCondition : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info) => !info.npc.boss;
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => null;
    }
    public sealed class NpcPet : GlobalNPC
    {
        /// <summary>
        /// Slow thats applied to an NPC.
        /// </summary>
        /// <param name="slowAmount">% of slow to be applied to the NPC. Negative values will speed the enemy up, which cannot go below -0.9f.</param>
        /// <param name="slowTime">Time for slow to be applied in frames.</param>
        /// <param name="slowId">Slows with the ID of -1 (or lower) are independent. If another slow with ID higher than 0 meets itself, it will replace the 'worse slow' of the same ID. Same slow ID cannot exist more than once.</param>
        public struct PetSlow(float slowAmount, int slowTime, int slowId = PetSlowIDs.IndependentSlow)
        {
            public float SlowAmount = slowAmount;
            public int SlowTime = slowTime;
            public int SlowId = slowId;
        }

        /// <summary>
        /// This is cumulative un-balanced slow value just added by all various sources. It is properly calculated in NpcPet.RetrievePetSlowedVelocity().
        /// </summary>
        internal float currentTotalSlow = 0f;
        /// <summary>
        /// Returns the correct velocity of the NPC with total slow value. This is used by IL edits to replace npc.velocity values.
        /// </summary>
        /// <param name="npc"></param>
        /// <returns></returns>
        public static Vector2 RetrievePetSlowedVelocity(NPC npc)
        {
            if (npc.TryGetGlobalNPC(out NpcPet pet))
            {
                float slow = pet.currentTotalSlow;
                if (slow < -0.9f)
                {
                    slow = -0.9f;
                }
                if (npc.noGravity == false)
                {
                    return npc.velocity with { X = npc.velocity.X * 1 / (1 + slow) };
                }
                else
                {
                    return npc.velocity * 1 / (1 + slow);
                }
            }
            else
                return npc.velocity;
        }
        /// <summary>
        /// All slows on this NPC.
        /// </summary>
        public List<PetSlow> SlowList = [];

        public bool electricSlow;
        public bool coldSlow;
        public bool sickSlow;

        public bool seaCreature;
        public int playerThatFishedUp;
        public int maulCounter;
        public int curseCounter;
        public int shuricornMark = 0;
        /// <summary>
        /// Contains all Vanilla bosses that does not return npc.boss = true
        /// </summary>
        public static List<int> NonBossTrueBosses = [NPCID.TheDestroyer, NPCID.TheDestroyerBody, NPCID.TheDestroyerTail, NPCID.EaterofWorldsBody, NPCID.EaterofWorldsTail, NPCID.EaterofWorldsHead, NPCID.LunarTowerSolar, NPCID.LunarTowerNebula, NPCID.LunarTowerStardust, NPCID.LunarTowerVortex, NPCID.TorchGod, NPCID.Retinazer, NPCID.Spazmatism];

        public override bool InstancePerEntity => true;

        #region Pet Tamer stuff
        public override void SetStaticDefaults()
        {
            NPCHappiness.Get(NPCID.BestiaryGirl).SetNPCAffection<PetTamer>(AffectionLevel.Like);
            NPCHappiness.Get(NPCID.Pirate).SetNPCAffection<PetTamer>(AffectionLevel.Like);
            NPCHappiness.Get(NPCID.Angler).SetNPCAffection<PetTamer>(AffectionLevel.Like);
            NPCHappiness.Get(NPCID.Nurse).SetNPCAffection<PetTamer>(AffectionLevel.Dislike);
            NPCHappiness.Get(NPCID.Mechanic).SetNPCAffection<PetTamer>(AffectionLevel.Dislike);
        }
        public override void GetChat(NPC npc, ref string chat)
        {
            if (Main.LocalPlayer.GetModPlayer<GlobalPet>().petObtained && npc.type == NPCID.Guide && Main.rand.NextBool(10))
                chat = PetTextsColors.LocVal("NPCs.PetTamer.GuideQuote");
        }
        #endregion

        #region On Kill & Loot Related stuff
        public static void OnKillInvokeDeathEffects(int playerWhoAmI, NPC npc)
        {
            Player player = Main.player[playerWhoAmI];
            if (player != null && player.active && player.dead == false && playerWhoAmI == Main.myPlayer)
            {
                GlobalPet.OnEnemyDeath?.Invoke(npc, player);
            }
        }
        public override void OnKill(NPC npc)
        {
            if (npc.lastInteraction != 255)
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    OnKillInvokeDeathEffects(npc.lastInteraction, npc);
                }
                if (Main.netMode == NetmodeID.Server)
                {
                    ModPacket packet = ModContent.GetInstance<PetsOverhaul>().GetPacket();
                    packet.Write((byte)MessageType.NPCOnDeathEffect);
                    packet.Write((byte)npc.lastInteraction); //Player's whoAmI
                    packet.Write((byte)Math.Clamp(npc.whoAmI, byte.MinValue, byte.MaxValue));
                    packet.Send();
                }
            }
            if (npc.type == NPCID.EyeofCthulhu)
            {
                MasteryShardCheck.masteryShardObtained1 = true;
            }
            if (npc.type == NPCID.WallofFlesh)
            {
                MasteryShardCheck.masteryShardObtained2 = true;
            }
            if (npc.type == NPCID.Golem)
            {
                MasteryShardCheck.masteryShardObtained3 = true;
            }
            if (npc.type == NPCID.SkeletronHead)
            {
                MasteryShardCheck.masteryShardObtained4 = true;
            }
            if (npc.type == NPCID.MoonLordCore)
            {
                MasteryShardCheck.masteryShardObtained5 = true;
            }
        }
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.EyeofCthulhu)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new FirstKillEoC(), ModContent.ItemType<MasteryShard>()));
            }
            if (npc.type == NPCID.WallofFlesh)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new FirstKillWoF(), ModContent.ItemType<MasteryShard>()));
            }
            if (npc.type == NPCID.Golem)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new FirstKillGolem(), ModContent.ItemType<MasteryShard>()));
            }
            if (npc.type == NPCID.SkeletronHead)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new FirstKillSkeletron(), ModContent.ItemType<MasteryShard>()));
            }
            if (npc.type == NPCID.MoonLordCore)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new FirstKillMoonLord(), ModContent.ItemType<MasteryShard>()));

                if (Main.expertMode == false && Main.masterMode == false)
                {
                    npcLoot.Add(ItemDropRule.Common(ItemID.SuspiciousLookingTentacle));
                }
            }
        }
        public override void ModifyGlobalLoot(GlobalLoot globalLoot)
        {
            globalLoot.Add(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsABoss(), ModContent.ItemType<MasteryShard>(), 100)); //1% for bosses (Most bosses that isn't boss = true does become boss = true at some point. EoW is fine without it as 0.02% is triggered multiple times
            globalLoot.Add(ItemDropRule.ByCondition(new NotABossCondition(), ModContent.ItemType<MasteryShard>(), 5000)); // 0.02% for non-bosses
        }
        #endregion

        #region Sea Creature check
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (source is EntitySource_FishedOut fisherman && fisherman.Fisher is Player player)
            {
                playerThatFishedUp = player.whoAmI;
                seaCreature = true;
            }
            else if (npc.type == NPCID.DukeFishron && source is EntitySource_BossSpawn fisher && fisher.Target is Player player2) //improve for bosses
            {
                playerThatFishedUp = player2.whoAmI;
                seaCreature = true;
            }
            else
            {
                seaCreature = false;
            }
        }
        #endregion

        #region All the slow IL Edits
        public override void Load()
        {
            IL_NPC.UpdateNPC_Inner += SlowILEditForUpdate;
            IL_NPC.Collision_MoveWhileDry += SlowILEditForDry;
            IL_NPC.Collision_MoveWhileWet += SlowILEditForWet;
        }
        private static void SlowILEditForUpdate(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);
                if (c.TryGotoNext(
                      i => i.MatchLdarg(0), //the this instance parameter
                      i => i.MatchLdfld<Terraria.Entity>("position"), //we find where first field is position
                      i => i.MatchLdarg(0),
                      i => i.MatchLdfld<Terraria.Entity>("velocity") //we find where second field is velocity
                  ))
                {
                    c.GotoNext(i => i.MatchLdfld<Terraria.Entity>("velocity")); //Cursor now sits at where velocity field is

                    c.Remove(); //velocity field spesifically is removed. I tried using EmitPop(), but that causes issues & does not work.

                    c.Emit(OpCodes.Call, typeof(NpcPet).GetMethod("RetrievePetSlowedVelocity")); //we replace the velocity field with our custom Method on NpcPet that returns slowed down Velocity. Ldarg is the npc parameter.
                    //Ldarg still stays before this call, as we ONLY REMOVE the velocity field, but not the Ldarg, so loaded argument of npc instance is used for our method.
                }
            }
            catch (Exception e)
            {
                MonoModHooks.DumpIL(ModContent.GetInstance<PetsOverhaul>(), il);
            }
        }
        private static void SlowILEditForDry(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);
                if (c.TryGotoNext(
                    i => i.MatchLdarg(0),
                      i => i.MatchLdarg(0), //the this instance parameter
                      i => i.MatchLdfld<Terraria.Entity>("position"), //we find where first field is position
                      i => i.MatchLdarg(0),
                      i => i.MatchLdfld<Terraria.Entity>("velocity") //we find where second field is velocity
                  ))
                {
                    c.GotoNext(i => i.MatchLdfld<Terraria.Entity>("velocity")); //Cursor now sits at where velocity field is

                    c.Remove(); //velocity field spesifically is removed. I tried using EmitPop(), but that causes issues & does not work.

                    c.Emit(OpCodes.Call, typeof(NpcPet).GetMethod("RetrievePetSlowedVelocity")); //we replace the velocity field with our custom Method on NpcPet that returns slowed down Velocity. Ldarg is the npc parameter.
                    //Ldarg still stays before this call, as we ONLY REMOVE the velocity field, but not the Ldarg, so loaded argument of npc instance is used for our method.
                }
            }
            catch (Exception e)
            {
                MonoModHooks.DumpIL(ModContent.GetInstance<PetsOverhaul>(), il);
            }
        }
        private static void SlowILEditForWet(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);
                if (c.TryGotoNext(
                      i => i.MatchLdarg(0), //this instance param
                      i => i.MatchLdflda<Terraria.Entity>("velocity"), //The 'a' at the end apparently means address.
                      i => i.MatchLdcR4(out _) //ldc.r4 = Float value
                  ))
                {
                    //same as above on Update & Dry, we just replace the velocity
                    c.GotoNext(i => i.MatchLdfld<Terraria.Entity>("velocity"));

                    c.Remove();

                    c.Emit(OpCodes.Call, typeof(NpcPet).GetMethod("RetrievePetSlowedVelocity"));
                }
            }
            catch (Exception e)
            {
                MonoModHooks.DumpIL(ModContent.GetInstance<PetsOverhaul>(), il);
            }
        }
        #endregion

        #region Slow Stuff mostly on maintaining the Slow List & particles
        public override void PostAI(NPC npc)
        {
            if (npc.active)
            {
                electricSlow = false;
                coldSlow = false;
                sickSlow = false;
                currentTotalSlow = 0;

                if (shuricornMark > 0)
                {
                    shuricornMark--;
                }

                if (SlowList.Count > 0)
                {
                    foreach (var slow in SlowList)
                    {
                        currentTotalSlow += slow.SlowAmount;

                        if (PetSlowIDs.ElectricBasedSlows.Exists(x => x == slow.SlowId))
                        {
                            electricSlow = true;
                        }
                        if (PetSlowIDs.ColdBasedSlows.Exists(x => x == slow.SlowId))
                        {
                            coldSlow = true;
                        }
                        if (PetSlowIDs.SicknessBasedSlows.Exists(x => x == slow.SlowId))
                        {
                            sickSlow = true;
                        }
                    }
                    for (int i = 0; i < SlowList.Count; i++) //Since Structs in Lists acts as Readonly, we re-assign the values to the index to decrement the timer.
                    {
                        PetSlow slow = SlowList[i];
                        slow.SlowTime--;
                        SlowList[i] = slow;
                    }

                    SlowList.RemoveAll(x => x.SlowTime <= 0);
                }
            }
        }
        /// <summary>
        /// Use this to add Slow to an NPC. It will send proper messages to the Server, and Server will sync all Clients to match their Slow Lists for consistent slow mechanics.
        /// </summary>
        public static void AddSlow(PetSlow petSlow, NPC npc)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                int npcArrayId = Math.Clamp(npc.whoAmI, byte.MinValue, byte.MaxValue);
                int slowId = Math.Clamp(petSlow.SlowId, sbyte.MinValue, sbyte.MaxValue);
                ModPacket packet = ModContent.GetInstance<PetsOverhaul>().GetPacket();
                packet.Write((byte)MessageType.PetSlow);
                packet.Write((byte)npcArrayId);
                packet.Write(petSlow.SlowAmount);
                packet.Write(petSlow.SlowTime);
                packet.Write((sbyte)slowId);
                packet.Send();
            }
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                AddToSlowList(petSlow, npc);
            }
        }

        /// <summary>
        /// Actually adds to the Slow List of an NPC to slow them. Does the proper checks and replaces weak Slows existing in the List. This DOES NOT Sync with server & other clients, AddSlow() & PetsOverhaul (where packets are handled) is where its done.
        /// </summary>
        internal static void AddToSlowList(PetSlow slowToBeAdded, NPC npc)
        {
            if (npc.active && NPCID.Sets.ImmuneToAllBuffs[npc.type] == false && (npc.isLikeATownNPC == false || npc.friendly == false) && npc.TryGetGlobalNPC(out NpcPet npcPet))
            {
                if (npc.boss && NonBossTrueBosses.Contains(npc.type))
                {
                    slowToBeAdded.SlowAmount *= 0.2f;
                }
                if (slowToBeAdded.SlowId <= -1)
                {
                    npcPet.SlowList.Add(slowToBeAdded);
                    return;
                }
                int indexToReplace;
                if (npcPet.SlowList.Exists(x => x.SlowId == slowToBeAdded.SlowId && x.SlowAmount < slowToBeAdded.SlowAmount))
                {
                    indexToReplace = npcPet.SlowList.FindIndex(x => x.SlowId == slowToBeAdded.SlowId && x.SlowAmount < slowToBeAdded.SlowAmount);
                    npcPet.SlowList[indexToReplace] = slowToBeAdded;
                }
                else if (npcPet.SlowList.Exists(x => x.SlowId == slowToBeAdded.SlowId && x.SlowAmount == slowToBeAdded.SlowAmount && x.SlowTime < slowToBeAdded.SlowTime))
                {
                    indexToReplace = npcPet.SlowList.FindIndex(x => x.SlowId == slowToBeAdded.SlowId && x.SlowAmount == slowToBeAdded.SlowAmount && x.SlowTime < slowToBeAdded.SlowTime);
                    npcPet.SlowList[indexToReplace] = slowToBeAdded;
                }
                else if (npcPet.SlowList.Exists(x => x.SlowId == slowToBeAdded.SlowId) == false)
                {
                    npcPet.SlowList.Add(slowToBeAdded);
                }
            }
        }
        #endregion

        #region Draw Effects Particles
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (currentTotalSlow > 0)
            {
                int dustChance = GlobalPet.Randomizer((int)(1000 / currentTotalSlow));
                if (dustChance <= 0)
                    dustChance = 1;
                bool spawnDust = Main.rand.NextBool(dustChance); //We use random chance to spawn a dust, the chance for gets narrowed down the more slow there is.
                if (electricSlow)
                {
                    drawColor = Color.PaleTurquoise with { A = 235 };

                    if (spawnDust)
                        Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Electric, Alpha: 100, Scale: Main.rand.NextFloat(0.7f, 1.1f))
                        .noGravity = true;
                }
                if (coldSlow)
                {
                    drawColor = Color.DarkTurquoise with { A = 235 };
                    if (spawnDust)
                        Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Water_Snow, Alpha: 100, Scale: Main.rand.NextFloat(0.7f, 1.1f))
                        .noGravity = true;
                }
                if (sickSlow)
                {
                    drawColor = new Color(218, 252, 222, 235);

                    if (spawnDust)
                        Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Poisoned, Alpha: 100, Scale: Main.rand.NextFloat(0.9f, 1.3f))
                        .noGravity = true;
                }
            }
            if (npc.HasBuff(ModContent.BuffType<Mauled>()))
            {
                for (int i = 0; i < maulCounter; i++)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Blood, Main.rand.NextFloat(0f, 1f), Main.rand.NextFloat(0f, 3f), 75, default, Main.rand.NextFloat(0.5f, 0.8f));
                    dust.velocity *= 0.8f;
                }
            }
            if (npc.HasBuff(ModContent.BuffType<QueensDamnation>()))
            {
                if (curseCounter >= 20)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Wraith, Main.rand.NextFloat(0f, 0.6f), Main.rand.NextFloat(-1f, 2f), 75, default, Main.rand.NextFloat(0.7f, 1f));
                    dust.velocity *= 0.2f;
                }
                else
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Pixie, Main.rand.NextFloat(0f, 0.6f), Main.rand.NextFloat(-1f, 2f), 150, default, Main.rand.NextFloat(0.5f, 0.7f));
                    dust.velocity *= 0.2f;
                }
            }
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (shuricornMark > 0)
            {
                spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("PetsOverhaul/Projectiles/Shuricorn"), npc.position with { Y = npc.position.Y - 30f } - screenPos, Main.MouseTextColorReal);
            }
        }
        #endregion
    }
    /// <summary>
    /// Class that contains PetSlowID's, where same slow ID does not overlap with itself, and a slow with greater slow & better remaining time will override the obsolete one.
    /// </summary>
    public class PetSlowIDs
    {
        /// <summary>
        /// This type of slows creates Electricity dusts on enemy.
        /// </summary>
        public static List<int> ElectricBasedSlows = [VoltBunny, PhantasmalLightning];
        /// <summary>
        /// This type of slows creates ice water dusts on enemy.
        /// </summary>
        public static List<int> ColdBasedSlows = [Grinch, Snowman, Deerclops, IceQueen, PhantasmalIce];
        /// <summary>
        /// This type of slows creates 'poisoned' dusts on enemy.
        /// </summary>
        public static List<int> SicknessBasedSlows = [PrincessSlime, PrinceSlime];
        /// <summary>
        /// Slows with ID lower than 0 won't be overriden by itself by any means and can have multiples of the same ID this way. This value defaults to be PetSlowIDs.ColdBasedSlows[Type] == true.
        /// </summary>
        public const int IndependentSlow = -1;
        public const int Grinch = 0;
        public const int Snowman = 1;
        public const int PrincessSlime = 2;
        public const int Deerclops = 3;
        public const int IceQueen = 4;
        public const int VoltBunny = 5;
        public const int PhantasmalIce = 6;
        public const int PhantasmalLightning = 7;
        public const int PrinceSlime = 8;
    }
}
