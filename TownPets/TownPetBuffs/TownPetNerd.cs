using PetsOverhaul.Systems;
using System;
using Terraria;

namespace PetsOverhaul.TownPets.TownPetBuffs
{
    public class TownPetNerd : TownPetBuff
    {
        public readonly float val = 0.12f;
        public readonly float val2 = 0.15f;
        public override void UpdateEffects(Player player, GlobalPet pet, ref int buffIndex)
        {
            pet.petShieldMultiplier += val;
            pet.abilityHaste += val2;
        }
        public override string BuffTooltip => base.BuffTooltip.Replace("<shield>", Math.Round(val * 100, 2).ToString()).Replace("<haste>", Math.Round(val2 * 100, 2).ToString());
    }
}
