using Microsoft.Xna.Framework;
using PetsOverhaul.Buffs;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using PetsOverhaul.TownPets.TownPetBuffs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets
{
    public sealed class TownPetModPlayer : ModPlayer
    {
        public override void Load()
        {
            On_Player.PetAnimal += UponPettingTownPet;
        }

        private static void UponPettingTownPet(On_Player.orig_PetAnimal orig, Player self, int animalNpcIndex)
        {
            NPC townPet = Main.npc[animalNpcIndex];
            foreach (int type in self.buffType)
            {
                if (PetItemIDs.TownPetBuffIDs[type])
                {
                    self.ClearBuff(type);
                }
            }
            switch (townPet.type)
            {
                case NPCID.TownBunny:
                    self.AddBuff(ModContent.BuffType<TownPetBunny>(), 1);
                    break;
                case NPCID.TownCat:
                    self.AddBuff(ModContent.BuffType<TownPetCat>(), 1);
                    break;
                case NPCID.TownSlimePurple:
                    self.AddBuff(ModContent.BuffType<TownPetClumsy>(), 1);
                    break;
                case NPCID.TownSlimeGreen:
                    self.AddBuff(ModContent.BuffType<TownPetCool>(), 1);
                    break;
                case NPCID.TownSlimeRainbow:
                    self.AddBuff(ModContent.BuffType<TownPetDiva>(), 1);
                    break;
                case NPCID.TownDog:
                    self.AddBuff(ModContent.BuffType<TownPetDog>(), 1);
                    break;
                case NPCID.TownSlimeYellow:
                    self.AddBuff(ModContent.BuffType<TownPetMystic>(), 1);
                    break;
                case NPCID.TownSlimeBlue:
                    self.AddBuff(ModContent.BuffType<TownPetNerd>(), 1);
                    break;
                case NPCID.TownSlimeOld:
                    self.AddBuff(ModContent.BuffType<TownPetOld>(), 1);
                    break;
                case NPCID.TownSlimeCopper:
                    self.AddBuff(ModContent.BuffType<TownPetSquire>(), 1);
                    break;
                case NPCID.TownSlimeRed:
                    self.AddBuff(ModContent.BuffType<TownPetSurly>(), 1);
                    break;
                default:
                    break;
            }
            GlobalPet.LastTownPet = townPet.GivenName;
            orig(self,animalNpcIndex);
        }
    }
}
