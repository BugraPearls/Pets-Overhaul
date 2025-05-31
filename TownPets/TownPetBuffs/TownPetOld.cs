using PetsOverhaul.Systems;
using PetsOverhaul.TownPets;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace PetsOverhaul.TownPets.TownPetBuffs
{
    public class TownPetOld : TownPetBuff
    {
        public readonly float val = 0.2f;
        public override void UpdateEffects(Player player, GlobalPet pet, ref int buffIndex)
        {
            pet.abilityHaste += val;
        }
        public override string BuffTooltip => base.BuffTooltip.Replace("<val>", Math.Round(val * 100, 2).ToString());
    }
}
