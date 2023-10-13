﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using PetsOverhaul.TownPets;

namespace PetsOverhaul.Buffs.TownPetBuffs
{
    public class TownPetCool : ModBuff
    {
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.LocalPlayer.Distance(Main.npc[i].Center) < Main.LocalPlayer.GetModPlayer<TownPet>().auraRange && Main.npc[i].type == NPCID.TownSlimeGreen)
                {
                    buffName = Lang.GetBuffName(ModContent.BuffType<TownPetCool>()).Replace("<Name>", Main.npc[i].FullName);
                    break;
                }
                else
                {
                    buffName = "Cool Aura";
                }
            }
            tip = Lang.GetBuffDescription(ModContent.BuffType<TownPetCool>()).Replace("<CoolCrit>", Main.LocalPlayer.GetModPlayer<TownPet>().critHitsAreCool.ToString());
            rare = 0;
        }
    }
}
