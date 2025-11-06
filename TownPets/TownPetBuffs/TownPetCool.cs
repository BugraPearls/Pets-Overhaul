using PetsOverhaul.Systems;
using System;
using Terraria;

namespace PetsOverhaul.TownPets.TownPetBuffs
{
    public class TownPetCool : TownPetBuff
    {
        public readonly float val = 0.22f;
        public override void UpdateEffects(Player player, PetModPlayer pet, ref int buffIndex)
        {
            pet.petDirectDamageMultiplier += val;
        }
        public override string BuffTooltip => base.BuffTooltip.Replace("<dmg>", Math.Round(val * 100, 2).ToString());
    }
}
