using PetsOverhaul.Systems;
using System;
using Terraria;

namespace PetsOverhaul.TownPets.TownPetBuffs
{
    public class TownPetDiva : TownPetBuff
    {
        public readonly float val = 0.12f;
        public readonly float val2 = 0.18f;
        public override void UpdateEffects(Player player, PetModPlayer pet, ref int buffIndex)
        {
            pet.petHealMultiplier += val;
            pet.abilityHaste += val2;
        }
        public override string BuffTooltip => base.BuffTooltip.Replace("<heal>", Math.Round(val * 100, 2).ToString()).Replace("<haste>", Math.Round(val2 * 100, 2).ToString());
    }
}
