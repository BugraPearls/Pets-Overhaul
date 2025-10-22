using PetsOverhaul.Systems;
using System;
using Terraria;
namespace PetsOverhaul.TownPets.TownPetBuffs
{
    public class TownPetSquire : TownPetBuff
    {
        public readonly float val = 0.22f;
        public override void UpdateEffects(Player player, GlobalPet pet, ref int buffIndex)
        {
            pet.petShieldMultiplier += val;
        }
        public override string BuffTooltip => base.BuffTooltip.Replace("<shield>", Math.Round(val * 100, 2).ToString());
    }
}
