using PetsOverhaul.Systems;
using Terraria;
using Terraria.ID;

namespace PetsOverhaul.LightPets
{
    public sealed class CrimsonHeartEffect : LightPetEffect
    {
        public override int LightPetItemID => ItemID.CrimsonHeart;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out CrimsonHeart crimsonHeart))
            {
                Player.statLifeMax2 += crimsonHeart.Health;
                Pet.petHealMultiplier += crimsonHeart.HealingPower;
                Pet.fishingFortune += crimsonHeart.FishingFortune;
            }
        }
    }
    public sealed class CrimsonHeart : LightPetItem
    {
        public LightPetStat Health = new(10, 1, "Health", 5, LegacyKeysToInherit: ("CrimsonHealth", 10));
        public LightPetStat HealingPower = new(15, 0.01f, "Healing", 0.05f, LegacyKeysToInherit: ("CrimsonExp", 15));
        public LightPetStat FishingFortune = new(15, 1, "Fishing", 5, LegacyKeysToInherit: ("CrimsonFort", 15));
        public override int LightPetItemID => ItemID.CrimsonHeart;
        public override string BaseTooltip => PetUtils.LocVal("LightPetTooltips.CrimsonHeart");
    }
}
