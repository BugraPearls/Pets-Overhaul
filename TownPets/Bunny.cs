﻿using PetsOverhaul.Buffs.TownPetBuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets
{
    public class Bunny : TownPet
    {
        public float bunnyJump = 0.08f;
        public override void PreUpdate()
        {
            if (NPC.downedMoonlord)
            {
                bunnyJump = 0.15f;
            }
            else if (Main.hardMode)
            {
                bunnyJump = 0.11f;
            }
            else
            {
                bunnyJump = 0.08f;
            }
        }
        public override void PostUpdateBuffs()
        {
            foreach (var npc in Main.ActiveNPCs)
            {
                if (Player.Distance(npc.Center) < auraRange && npc.type == NPCID.TownBunny)
                {
                    Player.AddBuff(ModContent.BuffType<TownPetBunny>(), 2);
                    break;
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {

            if (Player.HasBuff(ModContent.BuffType<TownPetBunny>()))
            {
                Player.jumpSpeedBoost += Player.jumpSpeed * bunnyJump;
                Pet.harvestingFortune += DefaultHarvFort;
            }
        }
    }
}