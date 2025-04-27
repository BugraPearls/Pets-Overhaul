using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class BlackCat : PetEffect
    {
        public override int PetItemID => ItemID.UnluckyYarn;
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public float luckFlat = 0.09f;
        public float luckMoonLowest = -0.03f;
        public float luckMoonLow = -0.01f;
        public float luckMoonMid = 0.01f;
        public float luckMoonHigh = 0.03f;
        public float luckMoonHighest = 0.05f;
        public int moonlightCd = 1020;
        public int moonlightLowest = -10;
        public int moonlightHighest = 20;
        public float currentMoonLuck = 0;
        public override int PetAbilityCooldown => moonlightCd;
        public override void ModifyLuck(ref float luck)
        {
            if (PetIsEquipped())
            {
                currentMoonLuck = 0;
                if (Main.dayTime == false)
                {
                    switch (Main.moonPhase)
                    {
                        case 0:
                            currentMoonLuck = luckMoonLowest;
                            break;
                        case 1:
                            currentMoonLuck = luckMoonLow;
                            break;
                        case 2:
                            currentMoonLuck = luckMoonMid;
                            break;
                        case 3:
                            currentMoonLuck = luckMoonHigh;
                            break;
                        case 4:
                            currentMoonLuck = luckMoonHighest;
                            break;
                        case 5:
                            currentMoonLuck = luckMoonHigh;
                            break;
                        case 6:
                            currentMoonLuck = luckMoonMid;
                            break;
                        case 7:
                            currentMoonLuck = luckMoonLow;
                            break;
                        default:
                            currentMoonLuck = 0;
                            break;
                    }
                }
                luck += currentMoonLuck + luckFlat;
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Main.dayTime == false && Pet.AbilityPressCheck() && PetIsEquipped())
            {
                SoundEngine.PlaySound(SoundID.Item29 with { PitchRange = (-1f, -0.8f) }, Player.Center);
                int moonlightRoll = Main.rand.Next(moonlightLowest, moonlightHighest + 1);
                moonlightRoll = GlobalPet.Randomizer((int)(moonlightRoll * (Player.luck + 1) * 100));
                if (moonlightRoll == 0)
                {
                    moonlightRoll = Main.rand.NextBool() ? -1 : 1;
                }
                if (moonlightRoll < 0)
                {
                    moonlightRoll *= -1;
                    string reason;
                    switch (Main.rand.Next(5))
                    {
                        case 0:
                            reason = PetTextsColors.LocVal("PetItemTooltips.BlackCatDeath1");
                            break;
                        case 1:
                            reason = PetTextsColors.LocVal("PetItemTooltips.BlackCatDeath2");
                            break;
                        case 2:
                            reason = PetTextsColors.LocVal("PetItemTooltips.BlackCatDeath3");
                            break;
                        case 3:
                            reason = PetTextsColors.LocVal("PetItemTooltips.BlackCatDeath4");
                            break;
                        case 4:
                            reason = PetTextsColors.LocVal("PetItemTooltips.BlackCatDeath5");
                            break;
                        default:
                            reason = PetTextsColors.LocVal("PetItemTooltips.BlackCatDeath1");
                            break;
                    }
                    Player.Hurt(PlayerDeathReason.ByCustomReason(reason.Replace("<name>", Player.name)), moonlightRoll, 0, dodgeable: false, knockback: 0, scalingArmorPenetration: 1f);
                }
                else
                {
                    Pet.PetRecovery(moonlightRoll, 1f, isLifesteal: false);
                }
                Pet.timer = Pet.timerMax;
            }
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
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.UnluckyYarn")
                .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility))
                .Replace("<moonlightMin>", blackCat.moonlightLowest.ToString())
                .Replace("<moonlightMax>", blackCat.moonlightHighest.ToString())
                .Replace("<moonlightCd>", Math.Round(blackCat.moonlightCd / 60f, 2).ToString())
                .Replace("<flatLuck>", blackCat.luckFlat.ToString())
                .Replace("<minimumMoon>", blackCat.luckMoonLowest.ToString())
                .Replace("<maximumMoon>", blackCat.luckMoonHighest.ToString())
                .Replace("<moonLuck>", Math.Round(blackCat.currentMoonLuck, 2).ToString())
                .Replace("<playerLuck>", Math.Round(blackCat.Player.luck, 2).ToString());
    }
}
