using PetsOverhaul.Config;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class BlueChicken : PetEffect
    {
        public override int PetItemID => ItemID.BlueEgg;
        public override PetClasses PetClassPrimary => PetClasses.Harvesting;
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
        public static void PoolPlant()
        {
            PetModPlayer.ItemWeight(ItemID.GrassSeeds, 300);
            PetModPlayer.ItemWeight(ItemID.JungleGrassSeeds, 275);
            PetModPlayer.ItemWeight(ItemID.AshGrassSeeds, 250);
            PetModPlayer.ItemWeight(ItemID.CorruptSeeds, 225);
            PetModPlayer.ItemWeight(ItemID.CrimsonSeeds, 225);
            PetModPlayer.ItemWeight(ItemID.MushroomGrassSeeds, 125);
            PetModPlayer.ItemWeight(ItemID.BlinkrootSeeds, 125);
            PetModPlayer.ItemWeight(ItemID.DaybloomSeeds, 125);
            PetModPlayer.ItemWeight(ItemID.DeathweedSeeds, 125);
            PetModPlayer.ItemWeight(ItemID.FireblossomSeeds, 125);
            PetModPlayer.ItemWeight(ItemID.MoonglowSeeds, 125);
            PetModPlayer.ItemWeight(ItemID.ShiverthornSeeds, 125);
            PetModPlayer.ItemWeight(ItemID.WaterleafSeeds, 125);
            PetModPlayer.ItemWeight(ItemID.PumpkinSeed, 75);
            PetModPlayer.ItemWeight(ItemID.SpicyPepper, 20);
            PetModPlayer.ItemWeight(ItemID.Pomegranate, 20);
            PetModPlayer.ItemWeight(ItemID.Elderberry, 20);
            PetModPlayer.ItemWeight(ItemID.BlackCurrant, 20);
            PetModPlayer.ItemWeight(ItemID.Rambutan, 20);
            PetModPlayer.ItemWeight(ItemID.MagicalPumpkinSeed, 2);
            if (Main.hardMode)
            {
                PetModPlayer.ItemWeight(ItemID.Grapes, 3);
            }
        }
        public static void PoolRarePlant()
        {
            PetModPlayer.ItemWeight(ItemID.JungleSpores, 60);
            PetModPlayer.ItemWeight(ItemID.SpicyPepper, 24);
            PetModPlayer.ItemWeight(ItemID.Pomegranate, 24);
            PetModPlayer.ItemWeight(ItemID.Elderberry, 24);
            PetModPlayer.ItemWeight(ItemID.BlackCurrant, 24);
            PetModPlayer.ItemWeight(ItemID.Rambutan, 24);
            PetModPlayer.ItemWeight(ItemID.StrangePlant1, 4);
            PetModPlayer.ItemWeight(ItemID.StrangePlant2, 4);
            PetModPlayer.ItemWeight(ItemID.StrangePlant3, 4);
            PetModPlayer.ItemWeight(ItemID.StrangePlant4, 4);
            PetModPlayer.ItemWeight(ItemID.MagicalPumpkinSeed, 1);
            if (Main.hardMode)
            {
                PetModPlayer.ItemWeight(ItemID.Grapes, 3);
            }
        }
        public static void PoolTree()
        {
            PetModPlayer.ItemWeight(ItemID.Acorn, 300);
            PetModPlayer.ItemWeight(ItemID.Wood, 300);
            PetModPlayer.ItemWeight(ItemID.BorealWood, 250);
            PetModPlayer.ItemWeight(ItemID.RichMahogany, 250);
            PetModPlayer.ItemWeight(ItemID.Ebonwood, 250);
            PetModPlayer.ItemWeight(ItemID.Shadewood, 250);
            PetModPlayer.ItemWeight(ItemID.PalmWood, 250);
            PetModPlayer.ItemWeight(ItemID.AshWood, 150);
            PetModPlayer.ItemWeight(ItemID.Apple, 15);
            PetModPlayer.ItemWeight(ItemID.Apricot, 15);
            PetModPlayer.ItemWeight(ItemID.Banana, 15);
            PetModPlayer.ItemWeight(ItemID.BloodOrange, 15);
            PetModPlayer.ItemWeight(ItemID.Cherry, 15);
            PetModPlayer.ItemWeight(ItemID.Coconut, 15);
            PetModPlayer.ItemWeight(ItemID.Grapefruit, 15);
            PetModPlayer.ItemWeight(ItemID.Lemon, 15);
            PetModPlayer.ItemWeight(ItemID.Mango, 15);
            PetModPlayer.ItemWeight(ItemID.Peach, 15);
            PetModPlayer.ItemWeight(ItemID.Pineapple, 15);
            PetModPlayer.ItemWeight(ItemID.Plum, 15);
            PetModPlayer.ItemWeight(ItemID.GemTreeAmethystSeed, 15);
            PetModPlayer.ItemWeight(ItemID.GemTreeTopazSeed, 14);
            PetModPlayer.ItemWeight(ItemID.GemTreeSapphireSeed, 13);
            PetModPlayer.ItemWeight(ItemID.GemTreeEmeraldSeed, 12);
            PetModPlayer.ItemWeight(ItemID.GemTreeRubySeed, 11);
            PetModPlayer.ItemWeight(ItemID.GemTreeAmberSeed, 11);
            PetModPlayer.ItemWeight(ItemID.GemTreeDiamondSeed, 10);
            PetModPlayer.ItemWeight(ItemID.EucaluptusSap, 1);
            if (Main.hardMode)
            {
                PetModPlayer.ItemWeight(ItemID.Pearlwood, 250);
                PetModPlayer.ItemWeight(ItemID.Dragonfruit, 3);
                PetModPlayer.ItemWeight(ItemID.Starfruit, 3);
            }
            if (NPC.downedPlantBoss)
            {
                PetModPlayer.ItemWeight(ItemID.SpookyWood, 30);
            }
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
                        PoolRarePlant();
                        if (PetModPlayer.ItemPool.Count > 0)
                        {
                            for (int i = 0; i < PetUtils.Randomizer(chick.rarePlantChance * item.stack); i++)
                            {
                                player.QuickSpawnItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.HarvestingItem), PetModPlayer.ItemPool[Main.rand.Next(PetModPlayer.ItemPool.Count)], 1);
                            }
                        }
                    }
                    else if (PetIDs.treeItem[item.type])
                    {
                        PoolTree();
                        if (PetModPlayer.ItemPool.Count > 0)
                        {
                            for (int i = 0; i < PetUtils.Randomizer(chick.treeChance * item.stack); i++)
                            {
                                player.QuickSpawnItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.HarvestingItem), PetModPlayer.ItemPool[Main.rand.Next(PetModPlayer.ItemPool.Count)], 1);
                            }
                        }
                    }
                    else
                    {
                        PoolPlant();
                        if (PetModPlayer.ItemPool.Count > 0)
                        {
                            for (int i = 0; i < PetUtils.Randomizer(chick.plantChance * item.stack); i++)
                            {
                                player.QuickSpawnItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.HarvestingItem), PetModPlayer.ItemPool[Main.rand.Next(PetModPlayer.ItemPool.Count)], 1);
                            }
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