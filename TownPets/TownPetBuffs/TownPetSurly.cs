using PetsOverhaul.Systems;
using PetsOverhaul.TownPets;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace PetsOverhaul.TownPets.TownPetBuffs
{
    public class TownPetSurly : TownPetBuff
    {
        public readonly float val = 0.1f;
        public readonly float val2 = 0.15f;
        public override void UpdateEffects(Player player, GlobalPet pet, ref int buffIndex)
        {
            pet.petDirectDamageMultiplier += val;
            pet.abilityHaste += val2;
        }
        public override string BuffTooltip => base.BuffTooltip.Replace("<dmg>", Math.Round(val * 100, 2).ToString()).Replace("<haste>", Math.Round(val2 * 100, 2).ToString());
    }
}
