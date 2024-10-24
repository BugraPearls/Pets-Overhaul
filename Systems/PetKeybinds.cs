﻿using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader;

namespace PetsOverhaul.Systems
{
    public class PetKeybinds : ModSystem
    {
        public static ModKeybind UsePetAbility { get; private set; }
        public static ModKeybind PetTooltipSwap { get; private set; }
        public static ModKeybind PetTooltipHide { get; private set; }
        public static ModKeybind PetAbilitySwitch { get; private set; }
        public override void Load()
        {
            UsePetAbility = KeybindLoader.RegisterKeybind(Mod, "UsePetAbility", Keys.Z);
            PetTooltipSwap = KeybindLoader.RegisterKeybind(Mod, "PetTooltipSwap", Keys.P);
            PetTooltipHide = KeybindLoader.RegisterKeybind(Mod, "PetTooltipHide", Keys.LeftShift);
            PetAbilitySwitch = KeybindLoader.RegisterKeybind(Mod, "PetAbilitySwitch", Keys.LeftControl);
        }
    }
}
