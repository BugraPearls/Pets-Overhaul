using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Puppy : PetEffect
    {
        public override int PetItemID => ItemID.DogWhistle;
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public int catchChance = 65;
        public int rareCatchChance = 15;
        public int rareCritterCoin = 25000;
        public int rareEnemyCoin = 70000;
        public override void Load()
        {
            GlobalPet.OnEnemyDeath += OnEnemyKill;
            On_NPC.GetNPCColorTintedByBuffs += On_NPC_GetNPCColorTintedByBuffs;
        }
        public override void Unload()
        {
            GlobalPet.OnEnemyDeath -= OnEnemyKill;
        }
        private static Color On_NPC_GetNPCColorTintedByBuffs(On_NPC.orig_GetNPCColorTintedByBuffs orig, NPC self, Color npcColor)
        {
            Color tempColor = npcColor;
            if (!self.canDisplayBuffs)
            {
                return tempColor;
            }
            tempColor = orig(self, npcColor);
            if (self.rarity > 0 && Main.LocalPlayer.miscEquips[0].type == ItemID.DogWhistle && self.lifeMax > 1)
            {
                byte b;
                byte b2;
                byte b3;
                if (self.friendly || self.catchItem > 0 || (self.damage == 0 && self.lifeMax == 5))
                {
                    b = byte.MaxValue;
                    b2 = byte.MaxValue;
                    b3 = 40;
                }
                else
                {
                    b = byte.MaxValue;
                    b2 = 40;
                    b3 = byte.MaxValue;
                }
                if (tempColor.R < b)
                {
                    tempColor.R = b;
                }
                if (tempColor.G < b2)
                {
                    tempColor.G = b2;
                }
                if (tempColor.B < b3)
                {
                    tempColor.B = b3;
                }
            }
            return tempColor;
        }

        public static void OnEnemyKill(NPC npc, Player player) //Remember, DO NOT use instanced stuff (Ex. Pet.PetInUse() is bad, use player.TryGetModPlayer() or player.GetModPlayer() to get the Pet class instances. EVERYTHING HAS TO BE from objects passed from the Event.
        {
            if (player.TryGetModPlayer(out Puppy pup) && pup.PetIsEquipped(false) && npc.rarity > 0 && npc.CountsAsACritter == false && npc.SpawnedFromStatue == false)
            {
                pup.Pet.GiveCoins(PetUtils.Randomizer(pup.rareEnemyCoin * npc.rarity));
            }
        }
        public override void OnCatchNPC(NPC npc, Item item, bool failed)
        {
            if (PetIsEquipped(false) && failed == false && npc.CountsAsACritter && npc.SpawnedFromStatue == false && npc.releaseOwner == 255)
            {
                if (npc.rarity > 0)
                {
                    Pet.GiveCoins(PetUtils.Randomizer(rareCritterCoin * npc.rarity));
                    for (int i = 0; i < PetUtils.Randomizer(rareCatchChance); i++)
                    {
                        Player.QuickSpawnItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.GlobalItem), npc.catchItem, 1);
                        if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                        {
                            SoundEngine.PlaySound(SoundID.Item65 with { PitchVariance = 0.3f, MaxInstances = 5, Volume = 0.5f }, Player.Center);
                        }
                    }

                }
                else
                {
                    for (int i = 0; i < PetUtils.Randomizer(catchChance); i++)
                    {
                        Player.QuickSpawnItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.GlobalItem), npc.catchItem, 1);
                        if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                        {
                            SoundEngine.PlaySound(SoundID.Item65 with { PitchVariance = 0.3f, MaxInstances = 1, Volume = 0.5f }, Player.Center);
                        }
                    }

                }
            }
        }
    }
    public sealed class DogWhistle : PetTooltip
    {
        public override PetEffect PetsEffect => puppy;
        public static Puppy puppy
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out Puppy pet))
                    return pet;
                else
                    return ModContent.GetInstance<Puppy>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.DogWhistle")
                .Replace("<critter>", puppy.catchChance.ToString())
                .Replace("<rareCritter>", puppy.rareCatchChance.ToString())
                .Replace("<rareCritterCoin>", Math.Round(puppy.rareCritterCoin / 10000f, 2).ToString())
                .Replace("<rareEnemyCoin>", Math.Round(puppy.rareEnemyCoin / 10000f, 2).ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.DogWhistle");
    }
}
