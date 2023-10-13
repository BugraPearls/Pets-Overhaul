﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using PetsOverhaul.TownPets;
namespace PetsOverhaul.Buffs.TownPetBuffs
{
    public class TownPetSurly : ModBuff
    {
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.LocalPlayer.Distance(Main.npc[i].Center) < Main.LocalPlayer.GetModPlayer<TownPet>().auraRange && Main.npc[i].type == NPCID.TownSlimeRed)
                {
                    buffName = Lang.GetBuffName(ModContent.BuffType<TownPetSurly>()).Replace("<Name>", Main.npc[i].FullName);
                    break;
                }
                else
                {
                    buffName = "Grumpy Aura";
                }
            }
            float spawnRate;
            if (NPC.downedMoonlord)
            {
                spawnRate = 10f;
            }
            else if (Main.hardMode)
            {
                spawnRate = 6.6f;
            }
            else
            {
                spawnRate = 5f;
            }
            tip = Lang.GetBuffDescription(ModContent.BuffType<TownPetSurly>()).Replace("<SurlySpawn>", spawnRate.ToString());
            rare = 0;
        }
    }
}
