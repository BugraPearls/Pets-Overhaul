using PetsOverhaul.Systems;
using PetsOverhaul.TownPets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
