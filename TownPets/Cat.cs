﻿using PetsOverhaul.Buffs.TownPetBuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets
{
    public class Cat : TownPet
    {
        public override void PostUpdateBuffs()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Player.Distance(Main.npc[i].Center) < auraRange && Main.npc[i].type == NPCID.TownCat && Main.npc[i].active == true)
                {
                    Player.AddBuff(ModContent.BuffType<TownPetCat>(), 2);
                    break;
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Player.HasBuff(ModContent.BuffType<TownPetCat>()))
            {
                Player.moveSpeed *= catSpeed;
                Pet.fishingFortune += FishFort;
            }
        }
    }
}