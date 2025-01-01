using PetsOverhaul.NPCs;
using PetsOverhaul.PetEffects;
using PetsOverhaul.Systems;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Items
{
    /// <summary>
    /// GlobalItem class that contains many useful booleans for mainly gathering purposes. This class is bread & butter of all Gathering Pets.
    /// </summary>
    public sealed class ItemPet : GlobalItem
    {
        public override bool InstancePerEntity => true;
        /// <summary>
        /// 1000 is 10 exp.
        /// </summary>
        public const int MinimumExpForRarePlant = 1000;
        #region Bool Sets
        /// <summary>
        /// Includes Gem tiles.
        /// </summary>
        public static bool[] gemTile = TileID.Sets.Factory.CreateBoolSet(false, TileID.Amethyst, TileID.Topaz, TileID.Sapphire, TileID.Emerald, TileID.Ruby, TileID.AmberStoneBlock, TileID.Diamond, TileID.ExposedGems, TileID.Crystals);
        /// <summary>
        /// Includes tiles that are extractable by an Extractinator and a few other stuff that aren't recognized as ores such as Obsidian and Luminite
        /// </summary>
        public static bool[] extractableAndOthers = TileID.Sets.Factory.CreateBoolSet(false, TileID.DesertFossil, TileID.Slush, TileID.Silt, TileID.Obsidian, TileID.LunarOre);
        /// <summary>
        /// Includes tiles that counts as trees.
        /// </summary>
        public static bool[] treeTile = TileID.Sets.Factory.CreateBoolSet(false, TileID.TreeAmber, TileID.TreeAmethyst, TileID.TreeAsh, TileID.TreeDiamond, TileID.TreeEmerald, TileID.TreeRuby, TileID.Trees, TileID.TreeSapphire, TileID.TreeTopaz, TileID.MushroomTrees, TileID.PalmTree, TileID.VanityTreeSakura, TileID.VanityTreeYellowWillow, TileID.Bamboo, TileID.Cactus);
        /// <summary>
        /// Contains items dropped by gemstone trees. Current only use is Caveling Gardener and checking for the Gemstone Tree
        /// </summary>
        public static bool[] gemstoneTreeItem = ItemID.Sets.Factory.CreateBoolSet(false, ItemID.GemTreeAmberSeed, ItemID.GemTreeAmethystSeed, ItemID.GemTreeDiamondSeed, ItemID.GemTreeEmeraldSeed, ItemID.GemTreeRubySeed, ItemID.GemTreeSapphireSeed, ItemID.GemTreeTopazSeed, ItemID.Amethyst, ItemID.Topaz, ItemID.Sapphire, ItemID.Emerald, ItemID.Ruby, ItemID.Amber, ItemID.Diamond, ItemID.StoneBlock);
        /// <summary>
        /// Contains items dropped by trees. Only used by Blue Chicken.
        /// </summary>
        public static bool[] treeItem = ItemID.Sets.Factory.CreateBoolSet(false, ItemID.Acorn, ItemID.BambooBlock, ItemID.Cactus, ItemID.Wood, ItemID.AshWood, ItemID.BorealWood, ItemID.PalmWood, ItemID.Ebonwood, ItemID.Shadewood, ItemID.RichMahogany, ItemID.Pearlwood, ItemID.SpookyWood);
        /// <summary>
        /// Contains forageable items on Ocean biomes that counts as herb item for Harvesting Pet purposes.
        /// </summary>
        public static bool[] seaPlantItem = ItemID.Sets.Factory.CreateBoolSet(false, ItemID.Coral, ItemID.Seashell, ItemID.Starfish, ItemID.LightningWhelkShell, ItemID.TulipShell, ItemID.JunoniaShell);
        /// <summary>
        /// Contains plants that cannot be planted by using a Seed.
        /// </summary>
        public static bool[] plantsWithNoSeeds = ItemID.Sets.Factory.CreateBoolSet(false, ItemID.Hay, ItemID.Mushroom, ItemID.GlowingMushroom, ItemID.VileMushroom, ItemID.ViciousMushroom, ItemID.GreenMushroom, ItemID.TealMushroom, ItemID.SkyBlueFlower, ItemID.YellowMarigold, ItemID.BlueBerries, ItemID.LimeKelp, ItemID.PinkPricklyPear, ItemID.OrangeBloodroot, ItemID.StrangePlant1, ItemID.StrangePlant2, ItemID.StrangePlant3, ItemID.StrangePlant4, ItemID.LifeFruit);
        #endregion

        #region Item checks to determine which Pet benefits
        public bool herbBoost = false;
        public bool oreBoost = false;
        public bool blockNotByPlayer = false;
        public bool pickedUpBefore = false;
        public bool harvestingDrop = false;
        public bool miningDrop = false;
        public bool fishingDrop = false;
        public bool globalDrop = false;
        public bool fortuneHarvestingDrop = false;
        public bool fortuneMiningDrop = false;
        public bool fortuneFishingDrop = false;

        public override void UpdateInventory(Item item, Player player)
        {
            if (pickedUpBefore == false)
            {
                pickedUpBefore = true;
            }
        }
        public override void OnSpawn(Item item, IEntitySource source) //This is called on server
        {
            if (WorldGen.generatingWorld)
            {
                return;
            }
            if (source is EntitySource_Pet petSource)
            {
                globalDrop = petSource.ContextType == EntitySourcePetIDs.GlobalItem;

                harvestingDrop = petSource.ContextType == EntitySourcePetIDs.HarvestingItem;

                miningDrop = petSource.ContextType == EntitySourcePetIDs.MiningItem;

                fishingDrop = petSource.ContextType == EntitySourcePetIDs.FishingItem;

                fortuneHarvestingDrop = petSource.ContextType == EntitySourcePetIDs.HarvestingFortuneItem;

                fortuneMiningDrop = petSource.ContextType == EntitySourcePetIDs.MiningFortuneItem;

                fortuneFishingDrop = petSource.ContextType == EntitySourcePetIDs.FishingFortuneItem;
            }
            else if (source is EntitySource_ShakeTree && item.IsACoin == false)
            {
                herbBoost = true;
            }
            else if (source is EntitySource_TileBreak brokenTile)
            {
                ushort tileType = Main.tile[brokenTile.TileCoords].TileType;

                if (PlayerPlacedBlockList.placedBlocksByPlayer.Contains(new Point16(brokenTile.TileCoords.X, brokenTile.TileCoords.Y)) == false)
                {
                    oreBoost = TileID.Sets.Ore[tileType] || gemTile[tileType] || extractableAndOthers[tileType] || Junimo.MiningXpPerBlock.Exists(x => x.oreList.Contains(item.type));
                    blockNotByPlayer = true;
                }

                TilePlacement.RemoveFromList(brokenTile.TileCoords.X, brokenTile.TileCoords.Y);

                herbBoost = Junimo.HarvestingXpPerGathered.Exists(x => x.plantList.Contains(item.type));

                if (TileID.Sets.CountsAsGemTree[tileType] == false && gemstoneTreeItem[item.type] || treeTile[tileType] == false && treeItem[item.type] || blockNotByPlayer == false && seaPlantItem[item.type] || blockNotByPlayer == false && plantsWithNoSeeds[item.type]) //Excluding other plants if their certain condition is not met
                {
                    herbBoost = false;
                }
            }
        }
        #endregion

        #region Netcode for checks
        public override void NetSend(Item item, BinaryWriter writer)
        {
            BitsByte sources1 = new(blockNotByPlayer, pickedUpBefore, herbBoost, oreBoost,  globalDrop, harvestingDrop, miningDrop, fishingDrop);
            BitsByte sources2 = new(fortuneHarvestingDrop, fortuneMiningDrop, fortuneFishingDrop);
            writer.Write(sources1);
            writer.Write(sources2);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            BitsByte sources1 = reader.ReadByte();
            sources1.Retrieve(ref blockNotByPlayer, ref pickedUpBefore, ref herbBoost, ref oreBoost,  ref globalDrop, ref harvestingDrop, ref miningDrop, ref fishingDrop);
            BitsByte sources2 = reader.ReadByte();
            sources2.Retrieve(ref fortuneHarvestingDrop, ref fortuneMiningDrop, ref fortuneFishingDrop);
        }
        #endregion
    }
}
