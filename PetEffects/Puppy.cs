using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
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
        public override void UpdateEquips()
        {
            if (PetIsEquipped(false))
            {
                Player.AddBuff(BuffID.Hunter, 1);
            }
        }
        public override void Load()
        {
            GlobalPet.OnEnemyDeath += OnEnemyKill;
        }
        public override void Unload()
        {
            GlobalPet.OnEnemyDeath -= OnEnemyKill;
        }
        public static void OnEnemyKill(NPC npc, Player player) //Remember, DO NOT use instanced stuff (Ex. Pet.PetInUse() is bad, use player.TryGetModPlayer() or player.GetModPlayer() to get the Pet class instances. EVERYTHING HAS TO BE from objects passed from the Event.
        {
            if (player.TryGetModPlayer(out Puppy pup) && pup.PetIsEquipped(false) && npc.rarity > 0 && npc.CountsAsACritter == false && npc.SpawnedFromStatue == false)
            {
                pup.Pet.GiveCoins(GlobalPet.Randomizer(pup.rareEnemyCoin * npc.rarity));
            }
        }
        public override void OnCatchNPC(NPC npc, Item item, bool failed)
        {
            if (PetIsEquipped(false) && failed == false && npc.CountsAsACritter && npc.SpawnedFromStatue == false && npc.releaseOwner == 255)
            {
                if (npc.rarity > 0)
                {
                    Pet.GiveCoins(GlobalPet.Randomizer(rareCritterCoin * npc.rarity));
                    for (int i = 0; i < GlobalPet.Randomizer(rareCatchChance); i++)
                    {
                        Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.GlobalItem), npc.catchItem, 1);
                        if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                        {
                            SoundEngine.PlaySound(SoundID.Item65 with { PitchVariance = 0.3f, MaxInstances = 5, Volume = 0.5f }, Player.Center);
                        }
                    }

                }
                else
                {
                    for (int i = 0; i < GlobalPet.Randomizer(catchChance); i++)
                    {
                        Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.GlobalItem), npc.catchItem, 1);
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
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.DogWhistle")
                .Replace("<critter>", puppy.catchChance.ToString())
                .Replace("<rareCritter>", puppy.rareCatchChance.ToString())
                .Replace("<rareCritterCoin>", Math.Round(puppy.rareCritterCoin / 100f, 2).ToString())
                .Replace("<rareEnemyCoin>", Math.Round(puppy.rareEnemyCoin / 100f, 2).ToString());
    }
}
