﻿using PetsOverhaul.Systems;
using PetsOverhaul.TownPets;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets.TownPetBuffs
{
    public class TownPetCool : TownPetBuff
    {
        public readonly float val = 0.22f;
        public override void UpdateEffects(Player player, GlobalPet pet, ref int buffIndex)
        {
            pet.petDirectDamageMultiplier += val;
        }
        public override string BuffTooltip => base.BuffTooltip.Replace("<dmg>", Math.Round(val * 100, 2).ToString());
    }
}
