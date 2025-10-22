using PetsOverhaul.Systems;
using Terraria;

namespace PetsOverhaul.TownPets.TownPetBuffs
{
    public class TownPetBunny : TownPetBuff
    {
        public readonly int val = 25;
        public override void UpdateEffects(Player player, GlobalPet pet, ref int buffIndex)
        {
            pet.harvestingFortune += val;
        }
        public override string BuffTooltip => base.BuffTooltip.Replace("<harv>", val.ToString());
    }
}
