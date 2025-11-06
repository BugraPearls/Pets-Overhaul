using PetsOverhaul.Systems;
using System;
using Terraria;
namespace PetsOverhaul.TownPets.TownPetBuffs
{
    public class TownPetMystic : TownPetBuff
    {
        public readonly float val = 0.22f;
        public override void UpdateEffects(Player player, PetModPlayer pet, ref int buffIndex)
        {
            pet.petHealMultiplier += val;
        }
        public override string BuffTooltip => base.BuffTooltip.Replace("<heal>", Math.Round(val * 100, 2).ToString());
    }
}
