using PetsOverhaul.Items;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Systems
{
    /// <summary>
    /// GlobalItem class that contains many useful booleans for mainly gathering purposes. This class is bread & butter of all Gathering Pets.
    /// </summary>
    public sealed class PetGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        /// <summary>
        /// 1000 is 10 exp.
        /// </summary>
        public const int MinimumExpForRarePlant = 1000;

        #region Bool Sets
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
                    oreBoost = TileID.Sets.Ore[tileType] || PetIDs.gemTile[tileType] || PetIDs.extractableAndOthers[tileType] || PetIDs.MiningXpPerBlock.Exists(x => x.oreList.Contains(item.type));
                    blockNotByPlayer = true;
                }

                PetTilePlacement.RemoveFromList(brokenTile.TileCoords.X, brokenTile.TileCoords.Y);

                herbBoost = PetIDs.HarvestingXpPerGathered.Exists(x => x.plantList.Contains(item.type));

                if (herbBoost)
                {
                    if (TileID.Sets.CountsAsGemTree[tileType] == false && PetIDs.gemstoneTreeItem[item.type] || PetIDs.treeTile[tileType] == false && PetIDs.treeItem[item.type] || blockNotByPlayer == false && PetIDs.seaPlantItem[item.type] || blockNotByPlayer == false && PetIDs.plantsWithNoSeeds[item.type]) //Excluding other plants if their certain condition is not met
                    {
                        herbBoost = false;
                    }
                }
            }
        }
        #endregion

        #region Netcode for checks
        public override void NetSend(Item item, BinaryWriter writer)
        {
            BitsByte sources1 = new(blockNotByPlayer, pickedUpBefore, herbBoost, oreBoost, globalDrop, harvestingDrop, miningDrop, fishingDrop);
            BitsByte sources2 = new(fortuneHarvestingDrop, fortuneMiningDrop, fortuneFishingDrop);
            writer.Write(sources1);
            writer.Write(sources2);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            BitsByte sources1 = reader.ReadByte();
            sources1.Retrieve(ref blockNotByPlayer, ref pickedUpBefore, ref herbBoost, ref oreBoost, ref globalDrop, ref harvestingDrop, ref miningDrop, ref fishingDrop);
            BitsByte sources2 = reader.ReadByte();
            sources2.Retrieve(ref fortuneHarvestingDrop, ref fortuneMiningDrop, ref fortuneFishingDrop);
        }
        #endregion

        public override void ExtractinatorUse(int extractType, int extractinatorBlockType, ref int resultType, ref int resultStack)
        {
            if (resultType == ItemID.CopperCoin) //Only replacing Copper Coin drops
            {
                if (extractinatorBlockType == TileID.ChlorophyteExtractinator && Main.rand.NextBool(10)) //10% chance to replace if Chlorophyte
                {
                    resultType = ModContent.ItemType<PetFood>(); //This way its IN the loot table, but actually replaces through the Copper Coin.
                    resultStack = 1;
                }
                else if (Main.rand.NextBool(20)) //5% chance to replace if not
                {
                    resultType = ModContent.ItemType<PetFood>();
                    resultStack = 1;
                }
            }
        }
    }
}
