using PetsOverhaul.Systems;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class FairyBellEffect : LightPetEffect
    {
        public override int LightPetItemID => ItemID.FairyBell;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out FairyBell fairyBell))
            {
                Pet.abilityHaste += fairyBell.AbilityHaste.CurrentStatFloat;
                Pet.globalFortune += fairyBell.GlobalFortune.CurrentStatInt;
            }
        }
    }
    public sealed class FairyBell : LightPetItem
    {
        public LightPetStat AbilityHaste = new(15, 0.012f, "Haste", 0.1f, LegacyKeysToInherit: ("FairyHaste",15));
        public LightPetStat GlobalFortune = new(20, 1, "Fortune", 5, LegacyKeysToInherit: ("FairyFort", 20));
        public LightPetStat Healing = new(10, 1, "Heal", 1);
        public override int LightPetItemID => ItemID.FairyBell;
        public override string BaseTooltip => PetUtils.LocVal("LightPetTooltips.FairyBell");
    }
}
