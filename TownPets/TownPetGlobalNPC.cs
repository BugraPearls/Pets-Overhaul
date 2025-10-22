using PetsOverhaul.Systems;
using PetsOverhaul.TownPets.TownPetBuffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets
{
    public sealed class TownPetGlobalNPC : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return NPCID.Sets.IsTownPet[entity.type];
        }
        public override void GetChat(NPC npc, ref string chat)
        {
            bool found = false;
            for (int i = 0; i < Main.LocalPlayer.buffType.Length; i++)
            {
                if (PetItemIDs.TownPetBuffIDs[Main.LocalPlayer.buffType[i]])
                {
                    found = true;
                    break; //No need to continue the for if found
                }
            }
            if (found == false)
            {
                chat += PetTextsColors.LocVal("Misc.TownPetPetMe");
            }
        }
        public override void Load()
        {
            On_Player.PetAnimal += UponPettingTownPet;
        }

        private static void UponPettingTownPet(On_Player.orig_PetAnimal orig, Player self, int animalNpcIndex)
        {
            NPC townPet = Main.npc[animalNpcIndex];
            int prevId = -1;
            foreach (int type in self.buffType)
            {
                if (PetItemIDs.TownPetBuffIDs[type])
                {
                    prevId = type;
                    self.ClearBuff(type);
                }
            }
            int buffToAdd = townPet.type switch
            {
                NPCID.TownBunny => ModContent.BuffType<TownPetBunny>(),
                NPCID.TownCat => ModContent.BuffType<TownPetCat>(),
                NPCID.TownSlimePurple => ModContent.BuffType<TownPetClumsy>(),
                NPCID.TownSlimeGreen => ModContent.BuffType<TownPetCool>(),
                NPCID.TownSlimeRainbow => ModContent.BuffType<TownPetDiva>(),
                NPCID.TownDog => ModContent.BuffType<TownPetDog>(),
                NPCID.TownSlimeYellow => ModContent.BuffType<TownPetMystic>(),
                NPCID.TownSlimeBlue => ModContent.BuffType<TownPetNerd>(),
                NPCID.TownSlimeOld => ModContent.BuffType<TownPetOld>(),
                NPCID.TownSlimeCopper => ModContent.BuffType<TownPetSquire>(),
                NPCID.TownSlimeRed => ModContent.BuffType<TownPetSurly>(),
                _ => -1,
            };
            if (buffToAdd != -1)
            {
                self.AddBuff(buffToAdd, 1);
                if (prevId == -1 || prevId != buffToAdd)
                {
                    SoundEngine.PlaySound(SoundID.Item44 with { Pitch = 0.15f, PitchVariance = 0.6f, Volume = 0.7f });
                }
            }
            GlobalPet.LastTownPet = townPet.GivenName;
            orig(self, animalNpcIndex);
        }
    }
}
