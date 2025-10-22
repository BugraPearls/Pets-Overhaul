using Terraria;

namespace PetsOverhaul.Systems
{
    public static class PetCraftingConditions
    {
        public static Condition ConsumedHead = new("Mods.PetsOverhaul.Misc.RequireHead", () => GlobalPet.pumpkingConsumed);
        public static Condition ConsumedOptic = new("Mods.PetsOverhaul.Misc.RequireOptic", () => GlobalPet.eolConsumed);
        public static Condition ConsumedWrench = new("Mods.PetsOverhaul.Misc.RequireWrench", () => GlobalPet.golemConsumed);
    }
}