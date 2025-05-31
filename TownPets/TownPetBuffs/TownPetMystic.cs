﻿using PetsOverhaul.Systems;
using PetsOverhaul.TownPets;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace PetsOverhaul.TownPets.TownPetBuffs
{
    public class TownPetMystic : TownPetBuff
    {
        public readonly float val = 0.22f;
        public override void UpdateEffects(Player player, GlobalPet pet, ref int buffIndex)
        {
            pet.petHealMultiplier += val;
        }
        public override string BuffTooltip => base.BuffTooltip.Replace("<heal>", Math.Round(val*100,2).ToString());
    }
}
