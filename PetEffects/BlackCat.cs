using PetsOverhaul.Achievements;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.PetEffects
{
    public sealed class BlackCat : PetEffect
    {
        public override int PetItemID => ItemID.UnluckyYarn;
        public override PetClass PetClassPrimary => PetClassID.Utility;
        public float luckFlat = 0.09f;
        public float luckMoonLowest = -0.03f;
        public float luckMoonLow = -0.01f;
        public float luckMoonMid = 0.01f;
        public float luckMoonHigh = 0.03f;
        public float luckMoonHighest = 0.05f;
        public int moonlightCd = 420;
        public int moonlightLowest = -20;
        public int moonlightHighest = 25;
        public float currentMoonLuck = 0;
        public override int PetAbilityCooldown => CustomEffectActive ? lunarVeilCooldown : moonlightCd;
        public override int PetStackCurrent => lunarVeilDuration > 0 ? lunarVeilDuration : lunarVeilPostDuration;
        public override string PetStackText => PetUtils.LocVal("CustomPetEffects.UnluckyYarnStack");
        public override string PetStackSpecial => CustomEffectActive ? PetUtils.SecondsOutOfText(PetStackCurrent, 0) : string.Empty;
        public override bool CustomEffectIsContributor => false;
        public override bool HasCustomEffect => true;  //Dedicated to Kinga
        public override PetClass CustomPrimaryClass => PetClassID.Defensive;
        public int lunarVeilDuration = 0;
        public int lunarVeilDurationMax = 600;
        public int lunarVeilCooldown = 7200;
        public int lunarVeilPostDuration = 0;
        public int lunarVeilPostDurationMax = 480;
        public float lunarVeilMs = 0.06f;
        public int lunarVeilManaRegen = 10;
        public override void SaveData(TagCompound tag)
        {
            tag.Add("CustomActive", CustomEffectActive);
        }
        public override void LoadData(TagCompound tag)
        {
            if (tag.TryGet("CustomActive", out bool custom))
            {
                CustomEffectActive = custom;
            }
        }
        public override void ExtraPreUpdateNoCheck()
        {
            if (PetIsEquippedForCustom())
            {
                if (lunarVeilDuration > 0)
                {
                    lunarVeilDuration--;
                }
                if (lunarVeilPostDuration > 0)
                {
                    lunarVeilPostDuration--;
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquippedForCustom() && lunarVeilPostDuration > 0)
            {
                Player.moveSpeed += lunarVeilMs;
                Player.manaRegenBonus += lunarVeilManaRegen;
            }
        }
        public override void UpdateBadLifeRegen()
        {
            if (PetIsEquippedForCustom() && lunarVeilPostDuration > 0 && Player.lifeRegen > 0)
            {
                Player.lifeRegen = 0;
            }
        }
        public override void ModifyLuck(ref float luck)
        {
            if (PetIsEquipped())
            {
                currentMoonLuck = 0;
                if (Main.dayTime == false)
                {
                    currentMoonLuck = Main.moonPhase switch
                    {
                        0 => luckMoonLowest,
                        1 => luckMoonLow,
                        2 => luckMoonMid,
                        3 => luckMoonHigh,
                        4 => luckMoonHighest,
                        5 => luckMoonHigh,
                        6 => luckMoonMid,
                        7 => luckMoonLow,
                        _ => 0,
                    };
                }
                luck += currentMoonLuck + luckFlat;
            }
        }
        public override void ExtraProcessTriggers(TriggersSet triggersSet)
        {
            if (Main.dayTime == false && Pet.AbilityPressCheck())
            {
                Moonlight();
                BasicSyncMessage(MessageType.BlackCat);
            }
        }
        public void Moonlight()
        {
            if (PetIsEquipped())
            {
                if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                {
                    SoundEngine.PlaySound(SoundID.Item29 with { PitchRange = (-1f, -0.8f) }, Player.Center);
                }

                int moonlightRoll = 0;
                while (moonlightRoll == 0) //keeps re-rolling if we get 0 from rand.Next().
                {
                    if (Player.luck == 0)
                    {
                        moonlightRoll = Main.rand.Next(moonlightLowest, moonlightHighest + 1);
                    }
                    else if (Player.luck > 0)
                    {
                        moonlightRoll = Main.rand.Next(moonlightLowest, PetUtils.Randomizer((int)(moonlightHighest * (Player.luck + 1) * 100)) + 1);
                    }
                    else
                    {
                        moonlightRoll = Main.rand.Next(PetUtils.Randomizer((int)(moonlightLowest * (Player.luck * -1 + 1) * 100)), moonlightHighest + 1);
                    }
                }

                if (moonlightRoll < 0)
                {
                    moonlightRoll *= -1;
                    string reason = Main.rand.Next(5) switch
                    {
                        0 => PetUtils.LocVal("PetItemTooltips.BlackCatDeath1"),
                        1 => PetUtils.LocVal("PetItemTooltips.BlackCatDeath2"),
                        2 => PetUtils.LocVal("PetItemTooltips.BlackCatDeath3"),
                        3 => PetUtils.LocVal("PetItemTooltips.BlackCatDeath4"),
                        4 => PetUtils.LocVal("PetItemTooltips.BlackCatDeath5"),
                        _ => PetUtils.LocVal("PetItemTooltips.BlackCatDeath1"),
                    };
                    Player.Hurt(new Player.HurtInfo() with { Damage = moonlightRoll, Dodgeable = false, Knockback = 0, DamageSource = PlayerDeathReason.ByCustomReason(reason.Replace("<name>", Player.name)) });

                    if (Main.myPlayer == Player.whoAmI)
                        ModContent.GetInstance<BringsBadLuckAfterall>().Unluckies.Value++;
                }
                else
                {
                    Pet.PetRecovery(moonlightRoll, 1f, isLifesteal: false);
                    if (Main.myPlayer == Player.whoAmI)
                        ModContent.GetInstance<BringsBadLuckAfterall>().Unluckies.Value = 0;
                }
                Pet.timer = Pet.timerMax;
            }
            else if (PetIsEquippedForCustom())
            {
                if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                {
                    SoundEngine.PlaySound(SoundID.Item29 with { PitchRange = (-1f, -0.8f) }, Player.Center);
                }
                lunarVeilDuration = lunarVeilDurationMax;
                Pet.timer = Pet.timerMax;
            }
        }
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
        {
            if (PetIsEquippedForCustom() && lunarVeilDuration > 0)
            {
                if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                {
                    SoundEngine.PlaySound(SoundID.Item29 with { PitchRange = (0.5f, 1f) }, Player.Center);
                }
                Player.statLife = 1;
                lunarVeilPostDuration = lunarVeilPostDurationMax;
                lunarVeilDuration = 0;
                Player.SetImmuneTimeForAllTypes(30); //gives half a second IFrames, this isn't mentioned in tooltip though
                return false;
            }
            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
        }
    }

    public sealed class UnluckyYarn : PetTooltip
    {
        public override PetEffect PetsEffect => blackCat;
        public static BlackCat blackCat
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out BlackCat pet))
                    return pet;
                else
                    return ModContent.GetInstance<BlackCat>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.UnluckyYarn")
                .Replace("<keybind>", PetUtils.KeybindText(PetKeybinds.UsePetAbility))
                .Replace("<moonlightMin>", blackCat.moonlightLowest.ToString())
                .Replace("<moonlightMax>", blackCat.moonlightHighest.ToString())
                .Replace("<moonlightCd>", Math.Round(blackCat.moonlightCd / 60f, 2).ToString())
                .Replace("<flatLuck>", blackCat.luckFlat.ToString())
                .Replace("<minimumMoon>", blackCat.luckMoonLowest.ToString())
                .Replace("<maximumMoon>", blackCat.luckMoonHighest.ToString())
                .Replace("<moonLuck>", Math.Round(blackCat.currentMoonLuck, 2).ToString())
                .Replace("<playerLuck>", Math.Round(blackCat.Player.luck, 2).ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.UnluckyYarn").Replace("<keybind>", PetUtils.KeybindText(PetKeybinds.UsePetAbility));
        public override string CustomTooltip => PetUtils.LocVal("CustomPetEffects.UnluckyYarn")
            .Replace("<keybind>", PetUtils.KeybindText(PetKeybinds.UsePetAbility))
            .Replace("<seconds>", Math.Round(blackCat.lunarVeilDurationMax / 60f, 2).ToString())
            .Replace("<cooldown>", Math.Round(blackCat.lunarVeilCooldown / 60f, 2).ToString())
            .Replace("<postDuration>", Math.Round(blackCat.lunarVeilPostDurationMax / 60f, 2).ToString())
            .Replace("<ms>", Math.Round(blackCat.lunarVeilMs * 100, 2).ToString())
            .Replace("<manaRegen>", blackCat.lunarVeilManaRegen.ToString());
        public override string CustomSimpleTooltip => PetUtils.LocVal("SimpleCustomPetEffects.UnluckyYarn").Replace("<keybind>", PetUtils.KeybindText(PetKeybinds.UsePetAbility));
    }
}
