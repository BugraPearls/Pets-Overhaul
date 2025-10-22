using PetsOverhaul.Systems;
using Terraria;

namespace PetsOverhaul.TownPets.TownPetBuffs
{
    public class TownPetClumsy : TownPetBuff
    {
        public readonly int val = 18;
        public override void UpdateEffects(Player player, GlobalPet pet, ref int buffIndex)
        {
            pet.globalFortune += val;
        }
        public override string BuffTooltip => base.BuffTooltip.Replace("<fort>", val.ToString());
    }
}
