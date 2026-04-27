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
        public LightPetStat AbilityHaste = new(15, 0.012f, "FairyHaste", 0.1f);
        public LightPetStat GlobalFortune = new(20, 1, "FairyFort", 5);
        public override int LightPetItemID => ItemID.FairyBell;
        public override string BaseTooltip => PetUtils.LocVal("LightPetTooltips.FairyBell");
    }
}
