using PetsOverhaul.Systems;
using Terraria;

namespace PetsOverhaul.TownPets.TownPetBuffs
{
    public class TownPetCat : TownPetBuff
    {
        public readonly int val = 21;
        public override void UpdateEffects(Player player, PetModPlayer pet, ref int buffIndex)
        {
            pet.fishingFortune += val;
        }
        public override string BuffTooltip => base.BuffTooltip.Replace("<fish>", val.ToString());
    }
}
