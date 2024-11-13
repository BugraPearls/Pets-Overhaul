﻿using PetsOverhaul.Buffs.TownPetBuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets
{
    public class SquireSlime : TownPet
    {
        public float squireDamage = 0.008f;
        public override void PreUpdate()
        {
            if (NPC.downedMoonlord)
            {
                squireDamage = 0.03f;
            }
            else if (Main.hardMode)
            {
                squireDamage = 0.02f;
            }
            else
            {
                squireDamage = 0.008f;
            }
        }
        public override void PostUpdateBuffs()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Player.Distance(Main.npc[i].Center) < auraRange && Main.npc[i].type == NPCID.TownSlimeCopper && Main.npc[i].active == true)
                {
                    Player.AddBuff(ModContent.BuffType<TownPetSquire>(), 2);
                    break;
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Player.HasBuff(ModContent.BuffType<TownPetSquire>()))
            {
                Player.GetDamage<GenericDamageClass>() += squireDamage;
                Pet.miningFortune += DefaultMiningFort;
            }
        }
    }

}