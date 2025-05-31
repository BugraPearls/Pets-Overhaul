using PetsOverhaul.Systems;
using PetsOverhaul.TownPets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets.TownPetBuffs
{
    public class TownPetDog : TownPetBuff
    {
        public readonly int val = 23;
        public override void UpdateEffects(Player player, GlobalPet pet, ref int buffIndex)
        {
            pet.miningFortune += val;
        }
        public override string BuffTooltip => base.BuffTooltip.Replace("<mining>", val.ToString());
    }
}
