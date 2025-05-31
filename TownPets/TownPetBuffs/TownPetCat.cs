﻿using PetsOverhaul.Systems;
using PetsOverhaul.TownPets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets.TownPetBuffs
{
    public class TownPetCat : TownPetBuff
    {
        public readonly int val = 21;
        public override void UpdateEffects(Player player, GlobalPet pet, ref int buffIndex)
        {
            pet.fishingFortune += val;
        }
        public override string BuffTooltip => base.BuffTooltip.Replace("<fish>", val.ToString());
    }
}
