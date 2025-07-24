using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader;

namespace PetsOverhaul.Systems
{
    public class PetKeybinds : ModSystem
    {
        public static ModKeybind UsePetAbility { get; private set; }
        public static ModKeybind ShowDetailedTip { get; private set; }
        public static ModKeybind PetAbilitySwitch { get; private set; }
        public static ModKeybind PetCustomSwitch { get; private set; }
        public override void Load()
        {
            UsePetAbility = KeybindLoader.RegisterKeybind(Mod, "UsePetAbility", Keys.Z);
            ShowDetailedTip = KeybindLoader.RegisterKeybind(Mod, "PetTooltipSwap", Keys.LeftShift);
            PetAbilitySwitch = KeybindLoader.RegisterKeybind(Mod, "PetAbilitySwitch", Keys.LeftAlt);
            PetCustomSwitch = KeybindLoader.RegisterKeybind(Mod, "PetCustomEffectSwitch", Keys.LeftControl);
        }
        public override void Unload()
        {
            UsePetAbility = null;
            ShowDetailedTip = null;
            PetAbilitySwitch = null;
            PetCustomSwitch = null;
        }
    }
}
