using PetsOverhaul.Config;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
namespace PetsOverhaul.Systems
{
    public class ItemTooltipChanges : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return PetItemIDs.PetNamesAndItems.ContainsValue(entity.type) || entity.type == ItemID.JojaCola || PetItemIDs.LightPetNamesAndItems.ContainsValue(entity.type);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.JojaCola)
            {
                tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.Misc.JojaCola")));
            }
            else if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && PetKeybinds.PetTooltipHide != null && !PetKeybinds.PetTooltipHide.Current)
            {
                tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.Config.TooltipToggleInGame")
                    .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.PetTooltipHide))));
            }
        }
    }
}