﻿using PetsOverhaul.Config;
using PetsOverhaul.NPCs;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class IceQueen : PetEffect
    {
        public override int PetItemID => ItemID.IceQueenPetItem;
        public int cooldown = 7200;
        private bool frozenTomb = false;
        private int iceQueenFrame = 0;
        public int queenRange = 480;
        public float slowAmount = 10f;
        public int freezeDamage = 200;
        public int immuneTime = 90;
        public int tombTime = 300;
        public int shieldAmount = 50;
        public int shieldDuration = 420;

        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override int PetAbilityCooldown => cooldown;
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (PetIsEquipped() && frozenTomb == true)
            {
                GlobalPet.CircularDustEffect(Player.Center, DustID.SnowflakeIce, queenRange, 50);
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped() && frozenTomb == true)
            {
                foreach (var npc in Main.ActiveNPCs)
                {
                    if (Player.Distance(npc.Center) < queenRange)
                    {
                        NpcPet.AddSlow(new NpcPet.PetSlow(slowAmount, 1, PetSlowIDs.IceQueen), npc);
                    }
                }
                if (iceQueenFrame % 30 == 0 && ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                {
                    if (Main.rand.NextBool())
                    {
                        SoundEngine.PlaySound(SoundID.Item48 with { PitchVariance = 0.3f, Volume = 0.8f }, Player.Center);
                    }
                    else
                    {
                        SoundEngine.PlaySound(SoundID.Item49 with { PitchVariance = 0.3f, Volume = 0.8f }, Player.Center);
                    }
                }
                iceQueenFrame++;
                Player.buffImmune[BuffID.Frozen] = false;
                Player.AddBuff(BuffID.Frozen, 1);
                Player.SetImmuneTimeForAllTypes(1);
                if (iceQueenFrame % 3 == 0)
                {
                    Player.statLife++;
                }
                if (iceQueenFrame >= tombTime)
                {
                    foreach (var npc in Main.ActiveNPCs)
                    {
                        if (NPCID.Sets.ImmuneToAllBuffs[npc.type] == false && Player.Distance(npc.Center) < queenRange && GlobalPet.LifestealCheck(npc))
                        {
                            npc.SimpleStrikeNPC(Pet.PetDamage(freezeDamage, DamageClass.Generic), npc.direction, Main.rand.NextBool((int)Math.Min(Player.GetTotalCritChance<GenericDamageClass>(), 100), 100), 0, DamageClass.Generic, true, Player.luck);
                        }
                    }
                    if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                    {
                        SoundEngine.PlaySound(SoundID.Shatter with { PitchVariance = 0.2f }, Player.Center);
                    }
                    Player.HealEffect(100);
                    Pet.AddShield(shieldAmount, shieldDuration);
                    Player.immune = false;
                    Player.SetImmuneTimeForAllTypes(immuneTime);
                    frozenTomb = false;
                    iceQueenFrame = 0;
                }
            }
        }
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (PetIsEquipped() && Pet.timer <= 0)
            {
                if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                {
                    SoundEngine.PlaySound(SoundID.Item30 with { PitchVariance = 0.5f, MaxInstances = 5, Pitch = -0.5f }, Player.Center);
                }

                frozenTomb = true;
                Player.statLife = 1;
                Pet.timer = Pet.timerMax;
                return false;
            }
            else
            {
                return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
            }
        }
    }
    public sealed class IceQueenPetItem : PetTooltip
    {
        public override PetEffect PetsEffect => iceQueen;
        public static IceQueen iceQueen
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out IceQueen pet))
                    return pet;
                else
                    return ModContent.GetInstance<IceQueen>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.IceQueenPetItem")
                        .Replace("<frozenTombTime>", Math.Round(iceQueen.tombTime / 60f, 2).ToString())
                        .Replace("<range>", Math.Round(iceQueen.queenRange / 16f, 2).ToString())
                        .Replace("<slowAmount>", Math.Round(iceQueen.slowAmount * 100, 2).ToString())
                        .Replace("<healthRecovery>", (iceQueen.tombTime / 3).ToString())
                        .Replace("<shield>", iceQueen.shieldAmount.ToString())
                        .Replace("<shieldDuration>", Math.Round(iceQueen.shieldDuration / 60f, 2).ToString())
                        .Replace("<baseDmg>", iceQueen.freezeDamage.ToString())
                        .Replace("<postTombImmunity>", Math.Round(iceQueen.immuneTime / 60f, 2).ToString())
                        .Replace("<tombCooldown>", Math.Round(iceQueen.cooldown / 3600f, 2).ToString());
    }
}
