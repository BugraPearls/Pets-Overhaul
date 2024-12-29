using Terraria.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Newtonsoft.Json.Linq;
using System.CommandLine.Invocation;
using Microsoft.Xna.Framework;
using System.IO;
using System.Drawing;
using PetsOverhaul.PetEffects;
using PetsOverhaul.Systems;

namespace PetsOverhaul.NPCs
{
    public sealed class SlimeServantOwner : GlobalNPC
    {
        public int Owner = -1;
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return ModContent.NPCType<SlimeServant>() == entity.type;
        }
    }
    public class SlimeServant : ModNPC //I don't know. But this may feel bad due to the IFrames it applies on enemy everytime enemy is hit. Beware. Unsure though.
    {
        public int Owner => NPC.GetGlobalNPC<SlimeServantOwner>().Owner;
        public int lifespan = 0;
        public override string Texture => $"Terraria/Images/NPC_{NPCID.BlueSlime}";
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.BlueSlime];
        }
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.BlueSlime);
            NPC.friendly = true;
            NPCID.Sets.UsesNewTargetting[Type] = true;
            NPC.value = 0;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0.5f;
            AnimationType = NPCID.BlueSlime;
        }
        public override void OnKill()
        {
            int slimyDuration;
            int slowDuration;
            float slow;
            int radius;
            if (Main.player[Owner].TryGetModPlayer(out SlimePrince prince) && prince.PetIsEquipped())
            {
                radius = prince.radius;
                slow = prince.slowAmount;
                slowDuration = prince.slowDuration;
                slimyDuration = prince.slimyDuration;
            }
            else //If Prince isn't equipped, doesn't slow/add slimy
            {
                return;
            }
            GlobalPet.CircularDustEffect(NPC.Center, DustID.BlueMoss, radius, 10, 1);
            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (NPC.Distance(npc.Center) < radius)
                {
                    NpcPet.AddSlow(new NpcPet.PetSlow(slow, slowDuration, PetSlowIDs.PrinceSlime), npc);
                    npc.AddBuff(BuffID.Slimed, slimyDuration);
                }
            }

        }
        public override void AI() //This is taken from Vanilla Slime AI. Cut out the non-blue slime parts and adjusted accordingly for targeting Enemies etc.
        {
            void RunTargeting()
            {
                NPCUtils.TargetSearchResults targeting = NPCUtils.SearchForTarget(NPC, NPCUtils.TargetSearchFlag.NPCs, npcFilter: new NPCUtils.SearchFilter<NPC>(x => x.friendly == false && x.CountsAsACritter == false));
                if (targeting.FoundNPC && targeting.NearestNPCDistance < 800) //Targets nearest NPC if they are within 800 pixels and if they aren't friendly & is a critter.
                {
                    NPC.target = 300 + targeting.NearestNPCIndex;
                    NPC.direction = (!(Main.npc[targeting.NearestNPCIndex].Center.X < NPC.Center.X)) ? 1 : (-1);
                    NPC.directionY = (!(Main.npc[targeting.NearestNPCIndex].Center.Y < NPC.Center.Y)) ? 1 : (-1);
                }
                else if (Owner >= 0 && Owner <= 255)
                {
                    NPC.target = Owner; //If no NPC's in the radius, following its spawner.
                    NPC.direction = (!(Main.player[Owner].Center.X < NPC.Center.X)) ? 1 : (-1);
                    NPC.directionY = (!(Main.player[Owner].Center.Y < NPC.Center.Y)) ? 1 : (-1);
                }
                else
                {
                    NPC.TargetClosest();
                }
            }

            lifespan++;

            if (lifespan > 7200)
            {
                NPC.life = 0;
                NPC.HitEffect();
                NPC.active = false;
                SoundEngine.PlaySound(NPC.DeathSound, NPC.Center);
                OnKill();
            }

            if (NPC.ai[1] == 1f || NPC.ai[1] == 2f || NPC.ai[1] == 3f)
                NPC.ai[1] = -1f;

            if (NPC.ai[1] == 75f)
            {
                float num = 0.3f;
                Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), 0.8f * num, 0.7f * num, 0.1f * num);
                if (Main.rand.NextBool(12))
                {
                    Dust dust = Dust.NewDustPerfect(NPC.Center + new Vector2(0f, NPC.height * 0.2f) + Main.rand.NextVector2CircularEdge(NPC.width, NPC.height * 0.6f) * (0.3f + Main.rand.NextFloat() * 0.5f), 228, new Vector2(0f, (0f - Main.rand.NextFloat()) * 0.3f - 1.5f), 127);
                    dust.scale = 0.5f;
                    dust.fadeIn = 1.1f;
                    dust.noGravity = true;
                    dust.noLight = true;
                }
            }

            if (NPC.ai[0] == -999f)
            {
                NPC.frame.Y = 0;
                NPC.frameCounter = 0.0;
                NPC.rotation = 0f;
                return;
            }
            //Removed aggression conditions, so its always 'agressive'
            if (NPC.ai[2] > 1f)
                NPC.ai[2] -= 1f;

            if (NPC.wet)
            {
                if (NPC.collideY)
                    NPC.velocity.Y = -2f;

                if (NPC.velocity.Y < 0f && NPC.ai[3] == NPC.position.X)
                {
                    NPC.direction *= -1;
                    NPC.ai[2] = 200f;
                }

                if (NPC.velocity.Y > 0f)
                    NPC.ai[3] = NPC.position.X;

                if (NPC.velocity.Y > 2f)
                    NPC.velocity.Y *= 0.9f;

                NPC.velocity.Y -= 0.5f;
                if (NPC.velocity.Y < -4f)
                    NPC.velocity.Y = -4f;

                if (NPC.ai[2] == 1f)
                    RunTargeting();
            }

            NPC.aiAction = 0;
            if (NPC.ai[2] == 0f)
            {
                NPC.ai[0] = -100f;
                NPC.ai[2] = 1f;
                RunTargeting();
            }

            if (NPC.velocity.Y == 0f)
            {
                if (NPC.collideY && NPC.oldVelocity.Y != 0f && Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
                    NPC.position.X -= NPC.velocity.X + NPC.direction;

                if (NPC.ai[3] == NPC.position.X)
                {
                    NPC.direction *= -1;
                    NPC.ai[2] = 200f;
                }

                NPC.ai[3] = 0f;
                NPC.velocity.X *= 0.8f;
                if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
                    NPC.velocity.X = 0f;


                NPC.ai[0] += 2.5f; //Normally +2f in total here while aggressive, but will be 2.5f so its even more agressive

                float num33 = -1000f;

                int num34 = 0;
                if (NPC.ai[0] >= 0f)
                    num34 = 1;

                if (NPC.ai[0] >= num33 && NPC.ai[0] <= num33 * 0.5f)
                    num34 = 2;

                if (NPC.ai[0] >= num33 * 2f && NPC.ai[0] <= num33 * 1.5f)
                    num34 = 3;

                if (num34 > 0)
                {
                    NPC.netUpdate = true;
                    if (NPC.ai[2] == 1f)
                        RunTargeting();

                    if (num34 == 3)
                    {
                        NPC.velocity.Y = -8f;

                        NPC.velocity.X += 3 * NPC.direction;

                        NPC.ai[0] = -200f;
                        NPC.ai[3] = NPC.position.X;
                    }
                    else
                    {
                        NPC.velocity.Y = -6f;
                        NPC.velocity.X += 2 * NPC.direction;

                        NPC.ai[0] = -120f;
                        if (num34 == 1)
                            NPC.ai[0] += num33;
                        else
                            NPC.ai[0] += num33 * 2f;
                    }
                }
                else if (NPC.ai[0] >= -30f)
                {
                    NPC.aiAction = 1;
                }
            }
            else if (NPC.target <= 500 && ((NPC.direction == 1 && NPC.velocity.X < 3f) || (NPC.direction == -1 && NPC.velocity.X > -3f)))
            {
                if (NPC.collideX && Math.Abs(NPC.velocity.X) == 0.2f)
                    NPC.position.X -= 1.4f * NPC.direction;

                if (NPC.collideY && NPC.oldVelocity.Y != 0f && Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
                    NPC.position.X -= NPC.velocity.X + NPC.direction;

                if ((NPC.direction == -1 && NPC.velocity.X < 0.01) || (NPC.direction == 1 && NPC.velocity.X > -0.01))
                    NPC.velocity.X += 0.2f * NPC.direction;
                else
                    NPC.velocity.X *= 0.93f;
            }
            if (Owner >= 0)
            {
                foreach (NPC npc in Main.ActiveNPCs)
                {
                    if (npc.friendly || npc.CountsAsACritter || npc.whoAmI == NPC.whoAmI || npc.immortal || npc.immune[Owner] > 0)
                        continue;
                    if (NPC.getRect().Intersects(npc.getRect()))
                    {
                        int damage = NPC.damage;
                        bool crit = false;
                        float kb = 0f;
                        float luck = 0;
                        if (Main.player[Owner].TryGetModPlayer(out SlimePrince prince) && prince.PetIsEquipped())
                        {
                            damage = prince.Pet.PetDamage(prince.Player.GetTotalDamage<GenericDamageClass>().ApplyTo(prince.baseDmg + damage)); //This deals correct damage with scalings
                            crit = Main.rand.NextBool((int)Math.Min(prince.Player.GetTotalCritChance<GenericDamageClass>(), 100), 100);
                            kb = prince.knockback;
                            luck = prince.Player.luck;
                        }
                        else if (Main.player[Owner].TryGetModPlayer(out DualSlime dual) && dual.PetIsEquipped())
                        {
                            damage = dual.Pet.PetDamage(dual.Player.GetTotalDamage<GenericDamageClass>().ApplyTo(dual.baseDmg + damage)); //This deals correct damage with scalings
                            crit = Main.rand.NextBool((int)Math.Min(dual.Player.GetTotalCritChance<GenericDamageClass>(), 100), 100);
                            kb = dual.knockback;
                            luck = dual.Player.luck;
                        }
                        npc.SimpleStrikeNPC(damage, npc.direction, crit, kb, DamageClass.Generic, true, luck);
                        npc.GetImmuneTime(Owner, 10);
                    }
                }
            }
        }
    }
}
