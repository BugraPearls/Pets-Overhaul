using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class TinyDeerclops : PetEffect
    {
        public override int PetItemID => ItemID.DeerclopsPetItem;
        public List<(int storedDamage, int timer)> deerclopsTakenDamage = [];
        public float damageReduction = 0.1f;
        public int radius = 320;
        public int abilityRadius = 450;
        public int shieldDuration = 300;
        public int storeDuration = 240;
        public float maxHealthPerc = 0.5f;
        public int cooldown = 1500;
        public float shieldMult = 0.2f;
        public float slowAmount = 0.15f;
        public int slowDuration = 120;
        public float reductionCap = 0.5f;
        public float reductionRaise = 0.1f;
        private int initiateStrike = 0;
        private int consumedDamage = 0;
        public int CurrentTotalDmgStored
        {
            get
            {
                int dmg = 0;
                deerclopsTakenDamage.ForEach(x => dmg += x.storedDamage);
                return dmg;
            }
        }
        public override int PetStackCurrent => CurrentTotalDmgStored;
        public override int PetStackMax => (int)(Player.statLifeMax2 * maxHealthPerc);
        public override string PetStackText => PetUtils.LocVal("PetItemTooltips.DeerclopsPetItemStack");
        public override PetClass PetClassPrimary => PetClassID.Defensive;
        public override PetClass PetClassSecondary => PetClassID.Melee;
        public override int PetAbilityCooldown => cooldown;
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (PetIsEquipped() && modifiers.DamageSource.TryGetCausingEntity(out Entity entity))
            {
                if (entity is Projectile projectile && projectile.TryGetGlobalProjectile(out PetGlobalProjectile proj) && Main.npc[proj.sourceNpcId].active && Main.npc[proj.sourceNpcId].Distance(Player.Center) > radius)
                {
                    modifiers.FinalDamage *= 1f - damageReduction;
                }
                else if (entity is NPC npc && npc.active == true && npc.Distance(Player.Center) > radius)
                {
                    modifiers.FinalDamage *= 1f - damageReduction;
                }
            }
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            if (PetIsEquipped(false))
            {
                deerclopsTakenDamage.Add((info.Damage, storeDuration));
            }
        }
        public override void PostUpdateMiscEffects()
        {
            void PlayChargeSound(float pitch)
            {
                if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                {
                    SoundEngine.PlaySound(SoundID.DeerclopsStep with { Pitch = pitch, Volume = 0.5f, PitchVariance = 0f, MaxInstances = 0 }, Player.Center);
                }
            }
            switch (initiateStrike)
            {
                case 88:
                    PlayChargeSound(-0.2f);
                    break;
                case 60:
                    PlayChargeSound(0f);
                    break;
                case 40:
                    PlayChargeSound(0.3f);
                    break;
                case 30:
                    PlayChargeSound(0.5f);
                    break;
                case 20:
                    PlayChargeSound(0.8f);
                    break;
                case 10:
                    PlayChargeSound(1f);
                    break;
                default:
                    break;
            }
            if (initiateStrike > 0)
            {
                initiateStrike--;
            }
            if (initiateStrike == 1)
            {
                if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                {
                    SoundEngine.PlaySound(SoundID.DeerclopsRubbleAttack with { Pitch = 0.75f, PitchVariance = 0.3f, Volume = 0.8f }, Player.Center);
                }

                float SmallerArc = MathHelper.Pi / 25;
                float BiggerArc = MathHelper.Pi / 5;

                for (int i = 0; i < 25; i++)
                {
                    Dust.NewDustPerfect(Player.Center, DustID.IceTorch, Main.rand.NextVector2Unit((Main.MouseWorld - Player.Center).ToRotation() + SmallerArc, BiggerArc) * 40, Scale: 5.5f).noGravity = true; //Bottom arc, this starts where mouse is
                }
                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDustPerfect(Player.Center, DustID.RedTorch, Main.rand.NextVector2Unit((Main.MouseWorld - Player.Center).ToRotation() - SmallerArc, SmallerArc) * 40, Scale: 5.5f).noGravity = true; //Inner lower
                }
                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDustPerfect(Player.Center, DustID.RedTorch, Main.rand.NextVector2Unit((Main.MouseWorld - Player.Center).ToRotation(), SmallerArc) * 40, Scale: 5.5f).noGravity = true; //Inner upper
                }
                for (int i = 0; i < 25; i++)
                {
                    Dust.NewDustPerfect(Player.Center, DustID.IceTorch, Main.rand.NextVector2Unit((Main.MouseWorld - Player.Center).ToRotation() - BiggerArc - SmallerArc, BiggerArc) * 40, Scale: 5.5f).noGravity = true; //Upper arc, this starts exactly how big arc is on top of where mouse is in radians
                }

                Vector2 ArcUpperEdge = new Vector2(Player.Center.X + abilityRadius, Player.Center.Y).RotatedBy((Main.MouseWorld - Player.Center).ToRotation() - BiggerArc - SmallerArc, Player.Center);
                Vector2 ArcLowerEdge = new Vector2(Player.Center.X + abilityRadius, Player.Center.Y).RotatedBy((Main.MouseWorld - Player.Center).ToRotation() + BiggerArc + SmallerArc, Player.Center);
                Vector2 InnerArcUpperEdge = new Vector2(Player.Center.X + abilityRadius, Player.Center.Y).RotatedBy((Main.MouseWorld - Player.Center).ToRotation() - SmallerArc, Player.Center);
                Vector2 InnerArcLowerEdge = new Vector2(Player.Center.X + abilityRadius, Player.Center.Y).RotatedBy((Main.MouseWorld - Player.Center).ToRotation() + SmallerArc, Player.Center);

                float penetrationReduction = 0f;

                void DoTheStrike(NPC npc)
                {
                    PetGlobalNPC.AddSlow(new PetSlow(slowAmount, slowDuration, PetSlowID.Deerclops), npc);
                    npc.SimpleStrikeNPC(Pet.PetDamage((consumedDamage + Player.statDefense) * (1 + Player.endurance) * (1f - penetrationReduction), DamageClass.Melee), Player.direction, false, 0, DamageClass.Melee, true, Player.luck);
                    penetrationReduction += reductionRaise;
                    if (penetrationReduction > reductionCap)
                    {
                        penetrationReduction = reductionCap;
                    }
                }

                foreach (NPC npc in Main.ActiveNPCs)
                {
                    if (npc.friendly == false && npc.dontTakeDamage == false && npc.Center.Distance(Player.Center) <= abilityRadius)
                    {
                        Vector2 NpcToP = npc.Center - Player.Center;

                        Vector2 InnerUpperToP = InnerArcUpperEdge - Player.Center;
                        float innerUpperResult = InnerUpperToP.X * NpcToP.Y - InnerUpperToP.Y * NpcToP.X;

                        Vector2 InnerLowerToP = InnerArcLowerEdge - Player.Center;
                        float innerLowerResult = InnerLowerToP.X * NpcToP.Y - InnerLowerToP.Y * NpcToP.X;

                        if (innerUpperResult >= 0 && innerLowerResult <= 0) //This is where the imaginary lines of the inner cone where def ignore is checked
                        {
                            if (npc.TryGetGlobalNPC(out DeerclopsDefIgnore defIgnore))
                            {
                                //This is done right before the strike, so the modifiers can apply the 0 multiplier on defense, afterwards this check is removed. DOES NOT allow you to deal more damage to things like Dungeon Guardian & DOES NOT bypass %dmg reductions. Just the defense stat.
                                defIgnore.defShouldBeIgnoredForNextHit = true;
                            }
                            //DR % increase is multiplicative here
                            DoTheStrike(npc);
                        }
                        else
                        {
                            Vector2 UpperToP = ArcUpperEdge - Player.Center;
                            float upperResult = UpperToP.X * NpcToP.Y - UpperToP.Y * NpcToP.X;

                            Vector2 LowerToP = ArcLowerEdge - Player.Center;
                            float lowerResult = LowerToP.X * NpcToP.Y - LowerToP.Y * NpcToP.X;

                            if (upperResult >= 0 && lowerResult <= 0) //normally its supposed to be upperResult < 0 & lowerResult > 0, but Terraria's axises are inverted.
                            {
                                DoTheStrike(npc);
                            }
                        }
                    }
                }
            }
            if (PetIsEquipped())
            {
                PetUtils.CircularDustEffect(Player.Center, DustID.PurpleTorch, radius, 12);
                if (deerclopsTakenDamage.Count > 0)
                {
                    for (int i = 0; i < deerclopsTakenDamage.Count; i++)
                    {
                        (int storedDamage, int timer) value = deerclopsTakenDamage[i];
                        value.timer--;
                        deerclopsTakenDamage[i] = value;
                    }
                    deerclopsTakenDamage.RemoveAll(x => x.timer <= 0);
                }
            }
        }
        public override void ExtraProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && PetIsEquipped())
            {
                initiateStrike = 90;
                Pet.timer = Pet.timerMax;
                int damageVal = Math.Min(CurrentTotalDmgStored, (int)(Player.statLifeMax2 * maxHealthPerc));
                consumedDamage = damageVal;
                Pet.AddShield((int)(damageVal * shieldMult), shieldDuration);
                deerclopsTakenDamage.Clear();
            }
        }
    }
    internal sealed class DeerclopsDefIgnore : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool defShouldBeIgnoredForNextHit = false;
        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (defShouldBeIgnoredForNextHit)
            {
                modifiers.Defense *= 0;
                defShouldBeIgnoredForNextHit = false; //This is done right before the strike, so in theory should only work when the Pet wants to.
            }
        }

    }
    public sealed class DeerclopsPetItem : PetTooltip
    {
        public override PetEffect PetsEffect => tinyDeerclops;
        public static TinyDeerclops tinyDeerclops
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out TinyDeerclops pet))
                    return pet;
                else
                    return ModContent.GetInstance<TinyDeerclops>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.DeerclopsPetItem")
                            .Replace("<dmgReduce>", Math.Round(tinyDeerclops.damageReduction * 100, 2).ToString())
                            .Replace("<reduceRadius>", Math.Round(tinyDeerclops.radius / 16f, 2).ToString())
                            .Replace("<storeDuration>", Math.Round(tinyDeerclops.storeDuration / 60f, 2).ToString())
                            .Replace("<maxStore>", Math.Round(tinyDeerclops.maxHealthPerc * 100, 2).ToString())
                            .Replace("<keybind>", PetUtils.KeybindText(PetKeybinds.UsePetAbility))
                            .Replace("<shieldMult>", Math.Round(tinyDeerclops.shieldMult * 100, 2).ToString())
                            .Replace("<shieldDuration>", Math.Round(tinyDeerclops.shieldDuration / 60f, 2).ToString())
                            .Replace("<abilityRadius>", Math.Round(tinyDeerclops.abilityRadius / 16f, 2).ToString())
                            .Replace("<dmgReduceStrike>", Math.Round(tinyDeerclops.reductionRaise * 100, 2).ToString())
                            .Replace("<maxDmgReduceStrike>", Math.Round(tinyDeerclops.reductionCap * 100, 2).ToString())
                            .Replace("<slow>", Math.Round(tinyDeerclops.slowAmount * 100, 2).ToString())
                            .Replace("<slowDuration>", Math.Round(tinyDeerclops.slowDuration / 60f, 2).ToString())
                            .Replace("<cooldown>", Math.Round(tinyDeerclops.cooldown / 60f, 2).ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.DeerclopsPetItem").Replace("<keybind>", PetUtils.KeybindText(PetKeybinds.UsePetAbility));
    }
}