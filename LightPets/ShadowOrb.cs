using PetsOverhaul.Systems;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class ShadowOrbEffect : LightPetEffect
    {
        public override int LightPetItemID => ItemID.ShadowOrb;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out ShadowOrb shadowOrb))
            {
                Player.statManaMax2 += shadowOrb.Mana.CurrentStatInt;
                Pet.petShieldMultiplier += shadowOrb.ShieldingPower.CurrentStatFloat;
                Pet.harvestingFortune += shadowOrb.HarvestingFortune.CurrentStatInt;
            }
        }
    }
    public sealed class ShadowOrb : LightPetItem
    {
        public LightPetStat Mana = new(10, 2, "ShadowMana", 20);
        public LightPetStat ShieldingPower = new(15, 0.005f, "ShadowExp", 0.025f);
        public LightPetStat HarvestingFortune = new(15, 1, "ShadowFort", 5);
        public override int LightPetItemID => ItemID.ShadowOrb;
        public override string BaseTooltip => PetUtils.LocVal("LightPetTooltips.ShadowOrb");
    }
}
