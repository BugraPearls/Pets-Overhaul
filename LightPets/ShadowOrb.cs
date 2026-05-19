using PetsOverhaul.Systems;
using Terraria.ID;

namespace PetsOverhaul.LightPets
{
    public sealed class ShadowOrbEffect : LightPetEffect
    {
        public override int LightPetItemID => ItemID.ShadowOrb;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out ShadowOrb shadowOrb))
            {
                Pet.petSlowPotency += shadowOrb.Slow;
                Pet.petShieldMultiplier += shadowOrb.ShieldingPower.CurrentStatFloat;
                Pet.harvestingFortune += shadowOrb.HarvestingFortune.CurrentStatInt;
            }
        }
    }
    public sealed class ShadowOrb : LightPetItem
    {
        public LightPetStat Slow = new(10, 0.01f, "Slow", 0.1f, LegacyKeysToInherit: ("ShadowMana", 10));
        public LightPetStat ShieldingPower = new(15, 0.01f, "Shield", 0.05f, LegacyKeysToInherit: ("ShadowExp", 15));
        public LightPetStat HarvestingFortune = new(15, 1, "Fortune", 5, LegacyKeysToInherit: ("ShadowFort", 15));
        public override int LightPetItemID => ItemID.ShadowOrb;
        public override string BaseTooltip => PetUtils.LocVal("LightPetTooltips.ShadowOrb");
    }
}
