using Terraria;
using Terraria.ID;
using PetsOverhaul.Systems;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Localization;
using PetsOverhaul.Config;
using Terraria.GameInput;
using PetsOverhaul.ModSupport;
using System;

namespace PetsOverhaul.PetEffects.ThoriumMod
{
    sealed public class ChaoticPet : ModPlayer
    {
        GlobalPet Pet { get => Player.GetModPlayer<GlobalPet>(); }
        public override void PostUpdateEquips()
        {
            
        }
    }
    sealed public class ChaoticMarble : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if(ModManager.ThoriumMod == null) return false;
            if(ModManager.ThoriumMod.InternalNameToModdedItemId == null) return false;
            if(!ModManager.ThoriumMod.InternalNameToModdedItemId.ContainsKey("ChaoticMarble")) return false;

            return entity.type == ModManager.ThoriumMod.InternalNameToModdedItemId["ChaoticMarble"];
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down]) return;
            ChaoticPet chaoticPet = Main.LocalPlayer.GetModPlayer<ChaoticPet>();
            tooltips.Add(new(Mod, "Tooltip0", "Pet Overhaul effects coming soon!"/*Language.GetTextValue("Mods.PetsOverhaul.ChaoticMarbleTooltips.ChaoticMarble")*/
                
            ));
        }
    }
}