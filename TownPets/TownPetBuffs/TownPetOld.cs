using PetsOverhaul.Systems;
using System;
using Terraria;
namespace PetsOverhaul.TownPets.TownPetBuffs
{
    public class TownPetOld : TownPetBuff
    {
        public readonly float val = 0.2f;
        public override void UpdateEffects(Player player, GlobalPet pet, ref int buffIndex)
        {
            pet.abilityHaste += val;
        }
        public override string BuffTooltip => base.BuffTooltip.Replace("<haste>", Math.Round(val * 100, 2).ToString());
    }
}
