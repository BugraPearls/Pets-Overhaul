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
                            Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.HarvestingItem), ItemID.BlueEgg);
                        }
                        else
                        {
                            Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.HarvestingItem), ItemID.RottenEgg);
                        }
                    }
                    else
                    {
                        Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.HarvestingItem), ModContent.ItemType<Egg>());
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
            GlobalPet.ItemWeight(ItemID.GrassSeeds, 300);
            GlobalPet.ItemWeight(ItemID.JungleGrassSeeds, 275);
            GlobalPet.ItemWeight(ItemID.AshGrassSeeds, 250);
            GlobalPet.ItemWeight(ItemID.CorruptSeeds, 225);
            GlobalPet.ItemWeight(ItemID.CrimsonSeeds, 225);
            GlobalPet.ItemWeight(ItemID.MushroomGrassSeeds, 125);
            GlobalPet.ItemWeight(ItemID.BlinkrootSeeds, 125);
            GlobalPet.ItemWeight(ItemID.DaybloomSeeds, 125);
            GlobalPet.ItemWeight(ItemID.DeathweedSeeds, 125);
            GlobalPet.ItemWeight(ItemID.FireblossomSeeds, 125);
            GlobalPet.ItemWeight(ItemID.MoonglowSeeds, 125);
            GlobalPet.ItemWeight(ItemID.ShiverthornSeeds, 125);
            GlobalPet.ItemWeight(ItemID.WaterleafSeeds, 125);
            GlobalPet.ItemWeight(ItemID.PumpkinSeed, 75);
            GlobalPet.ItemWeight(ItemID.SpicyPepper, 20);
            GlobalPet.ItemWeight(ItemID.Pomegranate, 20);
            GlobalPet.ItemWeight(ItemID.Elderberry, 20);
            GlobalPet.ItemWeight(ItemID.BlackCurrant, 20);
            GlobalPet.ItemWeight(ItemID.Rambutan, 20);
            GlobalPet.ItemWeight(ItemID.MagicalPumpkinSeed, 2);
            if (Main.hardMode)
            {
                GlobalPet.ItemWeight(ItemID.Grapes, 3);
            }
        }
        public static void PoolRarePlant()
        {
            GlobalPet.ItemWeight(ItemID.JungleSpores, 60);
            GlobalPet.ItemWeight(ItemID.SpicyPepper, 24);
            GlobalPet.ItemWeight(ItemID.Pomegranate, 24);
            GlobalPet.ItemWeight(ItemID.Elderberry, 24);
            GlobalPet.ItemWeight(ItemID.BlackCurrant, 24);
            GlobalPet.ItemWeight(ItemID.Rambutan, 24);
            GlobalPet.ItemWeight(ItemID.StrangePlant1, 4);
            GlobalPet.ItemWeight(ItemID.StrangePlant2, 4);
            GlobalPet.ItemWeight(ItemID.StrangePlant3, 4);
            GlobalPet.ItemWeight(ItemID.StrangePlant4, 4);
            GlobalPet.ItemWeight(ItemID.MagicalPumpkinSeed, 1);
            if (Main.hardMode)
            {
                GlobalPet.ItemWeight(ItemID.Grapes, 3);
            }
        }
        public static void PoolTree()
        {
            GlobalPet.ItemWeight(ItemID.Acorn, 300);
            GlobalPet.ItemWeight(ItemID.Wood, 300);
            GlobalPet.ItemWeight(ItemID.BorealWood, 250);
            GlobalPet.ItemWeight(ItemID.RichMahogany, 250);
            GlobalPet.ItemWeight(ItemID.Ebonwood, 250);
            GlobalPet.ItemWeight(ItemID.Shadewood, 250);
            GlobalPet.ItemWeight(ItemID.PalmWood, 250);
            GlobalPet.ItemWeight(ItemID.AshWood, 150);
            GlobalPet.ItemWeight(ItemID.Apple, 15);
            GlobalPet.ItemWeight(ItemID.Apricot, 15);
            GlobalPet.ItemWeight(ItemID.Banana, 15);
            GlobalPet.ItemWeight(ItemID.BloodOrange, 15);
            GlobalPet.ItemWeight(ItemID.Cherry, 15);
            GlobalPet.ItemWeight(ItemID.Coconut, 15);
            GlobalPet.ItemWeight(ItemID.Grapefruit, 15);
            GlobalPet.ItemWeight(ItemID.Lemon, 15);
            GlobalPet.ItemWeight(ItemID.Mango, 15);
            GlobalPet.ItemWeight(ItemID.Peach, 15);
            GlobalPet.ItemWeight(ItemID.Pineapple, 15);
            GlobalPet.ItemWeight(ItemID.Plum, 15);
            GlobalPet.ItemWeight(ItemID.GemTreeAmethystSeed, 15);
            GlobalPet.ItemWeight(ItemID.GemTreeTopazSeed, 14);
            GlobalPet.ItemWeight(ItemID.GemTreeSapphireSeed, 13);
            GlobalPet.ItemWeight(ItemID.GemTreeEmeraldSeed, 12);
            GlobalPet.ItemWeight(ItemID.GemTreeRubySeed, 11);
            GlobalPet.ItemWeight(ItemID.GemTreeAmberSeed, 11);
            GlobalPet.ItemWeight(ItemID.GemTreeDiamondSeed, 10);
            GlobalPet.ItemWeight(ItemID.EucaluptusSap, 1);
            if (Main.hardMode)
            {
                GlobalPet.ItemWeight(ItemID.Pearlwood, 250);
                GlobalPet.ItemWeight(ItemID.Dragonfruit, 3);
                GlobalPet.ItemWeight(ItemID.Starfruit, 3);
            }
            if (NPC.downedPlantBoss)
            {
                GlobalPet.ItemWeight(ItemID.SpookyWood, 30);
            }
        }
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public static void PreOnPickup(Item item, Player player)
        {
            GlobalPet PickerPet = player.GetModPlayer<GlobalPet>();
            BlueChicken chick = player.GetModPlayer<BlueChicken>();
            if (PickerPet.PickupChecks(item, chick.PetItemID, out ItemPet itemChck))
            {
                if (itemChck.herbBoost)
                {
                    if (Junimo.HarvestingXpPerGathered.Find(x => x.plantList.Contains(item.type)).expAmount >= ItemPet.MinimumExpForRarePlant)
                    {
                        PoolRarePlant();
                        if (GlobalPet.ItemPool.Count > 0)
                        {
                            for (int i = 0; i < GlobalPet.Randomizer(chick.rarePlantChance * item.stack); i++)
                            {
                                player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.HarvestingItem), GlobalPet.ItemPool[Main.rand.Next(GlobalPet.ItemPool.Count)], 1);
                            }
                        }
                    }
                    else if (ItemPet.treeItem[item.type])
                    {
                        PoolTree();
                        if (GlobalPet.ItemPool.Count > 0)
                        {
                            for (int i = 0; i < GlobalPet.Randomizer(chick.treeChance * item.stack); i++)
                            {
                                player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.HarvestingItem), GlobalPet.ItemPool[Main.rand.Next(GlobalPet.ItemPool.Count)], 1);
                            }
                        }
                    }
                    else
                    {
                        PoolPlant();
                        if (GlobalPet.ItemPool.Count > 0)
                        {
                            for (int i = 0; i < GlobalPet.Randomizer(chick.plantChance * item.stack); i++)
                            {
                                player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.HarvestingItem), GlobalPet.ItemPool[Main.rand.Next(GlobalPet.ItemPool.Count)], 1);
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
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.BlueEgg")
                .Replace("<plantChance>", blueChicken.plantChance.ToString())
                .Replace("<rarePlantChance>", blueChicken.rarePlantChance.ToString())
                .Replace("<choppableChance>", blueChicken.treeChance.ToString())
                .Replace("<eggMinute>", (blueChicken.blueEggTimer / 3600).ToString())
                .Replace("<egg>", ModContent.ItemType<Egg>().ToString());
    }
}