using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.Items
{
    public class PetMonitoringTablet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Pink;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Main.LocalPlayer.TryGetModPlayer(out GlobalPet pet))
            {
                foreach (TooltipLine line in tooltips)
                {
                    switch (line.Name)
                    {
                        case "Tooltip1":
                            line.Text = line.Text.Replace("<haste>", Math.Round(pet.abilityHaste * 100, 2).ToString());
                            break;
                        case "Tooltip2":
                            double currentHaste = Math.Round(pet.abilityHaste * 100, 2);
                            line.Text = line.Text.Replace("<hasteValue>", currentHaste.ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"))
                                .Replace("<hastePosiOrNega>",(currentHaste >= 0 ? Language.GetTextValue("Mods.PetsOverhaul.Faster") : Language.GetTextValue("Mods.PetsOverhaul.Slower")));
                            break;
                        case "Tooltip3":
                            line.Text = line.Text.Replace("<damage>", pet.petDirectDamageMultiplier.ToString());
                            break;
                        case "Tooltip4":
                            line.Text = line.Text.Replace("<damageTooltip>", Math.Round(pet.petDirectDamageMultiplier * 100, 2).ToString());
                            break;
                        case "Tooltip5":
                            line.Text = line.Text.Replace("<heal>", pet.petHealMultiplier.ToString());
                            break;
                        case "Tooltip6":
                            line.Text = line.Text.Replace("<healTooltip>", Math.Round(pet.petHealMultiplier * 100, 2).ToString());
                            break;
                        case "Tooltip7":
                            line.Text = line.Text.Replace("<shield>", pet.petShieldMultiplier.ToString());
                            break;
                        case "Tooltip8":
                            line.Text = line.Text.Replace("<shieldTooltip>", Math.Round(pet.petShieldMultiplier * 100, 2).ToString());
                            break;
                        case "Tooltip9":
                            line.Text = line.Text.Replace("<global>", pet.globalFortune.ToString());
                            break;
                        case "Tooltip10":
                            line.Text = line.Text.Replace("<globalTooltip>", Math.Round(pet.globalFortune * 0.5f, 2).ToString());
                            break;
                        case "Tooltip11":
                            line.Text = line.Text.Replace("<globalTooltipIncr>", pet.globalFortune.ToString());
                            break;
                        case "Tooltip12":
                            line.Text = line.Text.Replace("<mining>", pet.miningFortune.ToString());
                            break;
                        case "Tooltip13":
                            line.Text = line.Text.Replace("<miningTooltip>", Math.Round(pet.miningFortune * 0.5f, 2).ToString())
                                .Replace("<miningTooltipIncr>", pet.miningFortune.ToString());
                            break;
                        case "Tooltip14":
                            line.Text = line.Text.Replace("<harvesting>", pet.harvestingFortune.ToString());
                            break;
                        case "Tooltip15":
                            line.Text = line.Text.Replace("<harvestingTooltip>", Math.Round(pet.harvestingFortune * 0.5f, 2).ToString())
                                .Replace("<harvestingTooltipIncr>", pet.miningFortune.ToString());
                            break;
                        case "Tooltip16":
                            line.Text = line.Text.Replace("<fishing>", pet.fishingFortune.ToString());
                            break;
                        case "Tooltip17":
                            line.Text = line.Text.Replace("<fishingTooltip>", Math.Round(pet.fishingFortune * 0.5f, 2).ToString())
                                .Replace("<fishingTooltipIncr>", pet.fishingFortune.ToString());
                            break;
                        case "Tooltip18":
                            List<int> items = new();
                            if (pet.eolConsumed)
                                items.Add(ModContent.ItemType<PrismaticOptic>());
                            if (pet.golemConsumed)
                                items.Add(ModContent.ItemType<LihzahrdWrench>());
                            if (pet.pumpkingConsumed) 
                                items.Add(ModContent.ItemType<PumpkingsHead>());
                            line.Text = line.Text.Replace("<items>", PetTextsColors.ItemsToTooltipImages(items));
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
