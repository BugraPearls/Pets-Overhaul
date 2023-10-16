﻿using Terraria;
using Terraria.ID;
using PetsOverhaul.Systems;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.GameInput;
using PetsOverhaul.Config;

using PetsOverhaul.Config;
using Terraria.GameInput;

namespace PetsOverhaul.PetEffects.Vanilla
{
    sealed public class BlackCat : ModPlayer
    {
        GlobalPet Pet { get => Player.GetModPlayer<GlobalPet>(); }
        public float luckFlat = 0.12f;
        public float luckMoonLowest = 0.03f;
        public float luckMoonLow = 0.01f;
        public float luckMoonMid = 0.01f;
        public float luckMoonHigh = 0.03f;
        public float luckMoonHighest = 0.05f;
        public override void ModifyLuck(ref float luck)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.UnluckyYarn))
            {
                luck += luckFlat;
                if (Main.dayTime == false)
                {
                    switch (Main.moonPhase)
                    {
                        case 0:
                            luck -= luckMoonLowest;
                            break;
                        case 1:
                            luck -= luckMoonLow;
                            break;
                        case 2:
                            luck += luckMoonMid;
                            break;
                        case 3:
                            luck += luckMoonHigh;
                            break;
                        case 4:
                            luck += luckMoonHighest;
                            break;
                        case 5:
                            luck += luckMoonHigh;
                            break;
                        case 6:
                            luck += luckMoonMid;
                            break;
                        case 7:
                            luck -= luckMoonLow;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
    sealed public class UnluckyYarn : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.UnluckyYarn;

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down]) return;
            BlackCat blackCat = Main.LocalPlayer.GetModPlayer<BlackCat>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.UnluckyYarn")
                .Replace("<flatLuck>", blackCat.luckFlat.ToString())
                .Replace("<minimumMoon>", blackCat.luckMoonLowest.ToString())
                .Replace("<maximumMoon>", blackCat.luckMoonHighest.ToString())
            ));
        }
    }
}
