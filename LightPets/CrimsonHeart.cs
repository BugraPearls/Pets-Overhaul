using PetsOverhaul.Systems;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class CrimsonHeartEffect : LightPetEffect
    {
        public override int LightPetItemID => ItemID.CrimsonHeart;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out CrimsonHeart crimsonHeart))
            {
                Player.statLifeMax2 += crimsonHeart.Health.CurrentStatInt;
                Pet.petHealMultiplier += crimsonHeart.HealingPower.CurrentStatFloat;
                Pet.fishingFortune += crimsonHeart.FishingFortune.CurrentStatInt;
            }
        }
    }
    public sealed class CrimsonHeart : LightPetItem
    {
        public LightPetStat Health = new(10, 1, "CrimsonHealth",10);
        public LightPetStat HealingPower = new(15, 0.005f, "CrimsonExp", 0.025f);
        public LightPetStat FishingFortune = new(15, 1, "CrimsonFort", 5);
        public override int LightPetItemID => ItemID.CrimsonHeart;
        public override string BaseTooltip => PetUtils.LocVal("LightPetTooltips.CrimsonHeart");
    }
}
