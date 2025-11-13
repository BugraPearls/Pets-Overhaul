using PetsOverhaul.Config;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace PetsOverhaul.PetEffects
{
    public sealed class BlueChicken : PetEffect
    {
        public override int PetItemID => ItemID.BlueEgg;
        public override PetClass PetClassPrimary => PetClassID.Harvesting;
        public int blueEggTimer = 39600;
        public float tipsyMovespd = 0.1f;
        public int plantChance = 28;
        public int rarePlantChance = 50;
        public int treeChance = 14;
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped(false))
            {
                if (Main.rand.NextBool(blueEggTimer))
                {
                    if (Main.rand.NextBool(15, 100))
                    {
                        if (Main.rand.NextBool(4, 100))
                        {
                            Player.QuickSpawnItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.HarvestingItem), ItemID.BlueEgg);
                        }
                        else
                        {
                            Player.QuickSpawnItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.HarvestingItem), ItemID.RottenEgg);
                        }
                    }
                    else
                    {
                        Player.QuickSpawnItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.HarvestingItem), ModContent.ItemType<Egg>());
                    }

                    if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                    {
                        SoundEngine.PlaySound(SoundID.NPCDeath3 with { PitchVariance = 0.1f, Pitch = 0.9f }, Player.Center);
                    }
                }
            }
        }
        public static int PoolPlant()
        {
            WeightedRandom<int> itemsToDrop = new();

            itemsToDrop.Add(ItemID.GrassSeeds, 300);
            itemsToDrop.Add(ItemID.JungleGrassSeeds, 275);
            itemsToDrop.Add(ItemID.AshGrassSeeds, 250);
            itemsToDrop.Add(ItemID.CorruptSeeds, 225);
            itemsToDrop.Add(ItemID.CrimsonSeeds, 225);
            itemsToDrop.Add(ItemID.MushroomGrassSeeds, 125);
            itemsToDrop.Add(ItemID.BlinkrootSeeds, 125);
            itemsToDrop.Add(ItemID.DaybloomSeeds, 125);
            itemsToDrop.Add(ItemID.DeathweedSeeds, 125);
            itemsToDrop.Add(ItemID.FireblossomSeeds, 125);
            itemsToDrop.Add(ItemID.MoonglowSeeds, 125);
            itemsToDrop.Add(ItemID.ShiverthornSeeds, 125);
            itemsToDrop.Add(ItemID.WaterleafSeeds, 125);
            itemsToDrop.Add(ItemID.PumpkinSeed, 75);
            itemsToDrop.Add(ItemID.SpicyPepper, 20);
            itemsToDrop.Add(ItemID.Pomegranate, 20);
            itemsToDrop.Add(ItemID.Elderberry, 20);
            itemsToDrop.Add(ItemID.BlackCurrant, 20);
            itemsToDrop.Add(ItemID.Rambutan, 20);
            itemsToDrop.Add(ItemID.MagicalPumpkinSeed, 2);
            if (Main.hardMode)
            {
                itemsToDrop.Add(ItemID.Grapes, 3);
            }

            int result = itemsToDrop;
            return result;
        }
        public static int PoolRarePlant()
        {
            WeightedRandom<int> itemsToDrop = new();

            itemsToDrop.Add(ItemID.JungleSpores, 60);
            itemsToDrop.Add(ItemID.SpicyPepper, 24);
            itemsToDrop.Add(ItemID.Pomegranate, 24);
            itemsToDrop.Add(ItemID.Elderberry, 24);
            itemsToDrop.Add(ItemID.BlackCurrant, 24);
            itemsToDrop.Add(ItemID.Rambutan, 24);
            itemsToDrop.Add(ItemID.StrangePlant1, 4);
            itemsToDrop.Add(ItemID.StrangePlant2, 4);
            itemsToDrop.Add(ItemID.StrangePlant3, 4);
            itemsToDrop.Add(ItemID.StrangePlant4, 4);
            itemsToDrop.Add(ItemID.MagicalPumpkinSeed, 1);
            if (Main.hardMode)
            {
                itemsToDrop.Add(ItemID.Grapes, 3);
            }

            int result = itemsToDrop;
            return result;
        }
        public static int PoolTree()
        {
            WeightedRandom<int> itemsToDrop = new();

            itemsToDrop.Add(ItemID.Acorn, 300);
            itemsToDrop.Add(ItemID.Wood, 300);
            itemsToDrop.Add(ItemID.BorealWood, 250);
            itemsToDrop.Add(ItemID.RichMahogany, 250);
            itemsToDrop.Add(ItemID.Ebonwood, 250);
            itemsToDrop.Add(ItemID.Shadewood, 250);
            itemsToDrop.Add(ItemID.PalmWood, 250);
            itemsToDrop.Add(ItemID.AshWood, 150);
            itemsToDrop.Add(ItemID.Apple, 15);
            itemsToDrop.Add(ItemID.Apricot, 15);
            itemsToDrop.Add(ItemID.Banana, 15);
            itemsToDrop.Add(ItemID.BloodOrange, 15);
            itemsToDrop.Add(ItemID.Cherry, 15);
            itemsToDrop.Add(ItemID.Coconut, 15);
            itemsToDrop.Add(ItemID.Grapefruit, 15);
            itemsToDrop.Add(ItemID.Lemon, 15);
            itemsToDrop.Add(ItemID.Mango, 15);
            itemsToDrop.Add(ItemID.Peach, 15);
            itemsToDrop.Add(ItemID.Pineapple, 15);
            itemsToDrop.Add(ItemID.Plum, 15);
            itemsToDrop.Add(ItemID.GemTreeAmethystSeed, 15);
            itemsToDrop.Add(ItemID.GemTreeTopazSeed, 14);
            itemsToDrop.Add(ItemID.GemTreeSapphireSeed, 13);
            itemsToDrop.Add(ItemID.GemTreeEmeraldSeed, 12);
            itemsToDrop.Add(ItemID.GemTreeRubySeed, 11);
            itemsToDrop.Add(ItemID.GemTreeAmberSeed, 11);
            itemsToDrop.Add(ItemID.GemTreeDiamondSeed, 10);
            itemsToDrop.Add(ItemID.EucaluptusSap, 1);
            if (Main.hardMode)
            {
                itemsToDrop.Add(ItemID.Pearlwood, 250);
                itemsToDrop.Add(ItemID.Dragonfruit, 3);
                itemsToDrop.Add(ItemID.Starfruit, 3);
            }
            if (NPC.downedPlantBoss)
            {
                itemsToDrop.Add(ItemID.SpookyWood, 30);
            }

            int result = itemsToDrop;
            return result;
        }
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public static void PreOnPickup(Item item, Player player)
        {
            BlueChicken chick = player.GetModPlayer<BlueChicken>();
            if (player.PetPlayer().PickupChecks(item, chick.PetItemID, out PetGlobalItem itemChck))
            {
                if (itemChck.herbBoost)
                {
                    if (PetIDs.HarvestingXpPerGathered.Find(x => x.plantList.Contains(item.type)).expAmount >= PetGlobalItem.MinimumExpForRarePlant)
                    {
                        for (int i = 0; i < PetUtils.Randomizer(chick.rarePlantChance * item.stack); i++)
                        {
                            player.QuickSpawnItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.HarvestingItem), PoolRarePlant(), 1);
                        }
                    }
                    else if (PetIDs.treeItem[item.type])
                    {
                        for (int i = 0; i < PetUtils.Randomizer(chick.treeChance * item.stack); i++)
                        {
                            player.QuickSpawnItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.HarvestingItem), PoolTree(), 1);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < PetUtils.Randomizer(chick.plantChance * item.stack); i++)
                        {
                            player.QuickSpawnItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.HarvestingItem), PoolPlant(), 1);
                        }
                    }
                }
            }
        }
    }
    public sealed class BlueEgg : PetTooltip
    {
        public override PetEffect PetsEffect => blueChicken;
        public static BlueChicken blueChicken
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out BlueChicken pet))
                    return pet;
                else
                    return ModContent.GetInstance<BlueChicken>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.BlueEgg")
                .Replace("<plantChance>", blueChicken.plantChance.ToString())
                .Replace("<rarePlantChance>", blueChicken.rarePlantChance.ToString())
                .Replace("<choppableChance>", blueChicken.treeChance.ToString())
                .Replace("<eggMinute>", (blueChicken.blueEggTimer / 3600).ToString())
                .Replace("<egg>", ModContent.ItemType<Egg>().ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.BlueEgg");
    }
}