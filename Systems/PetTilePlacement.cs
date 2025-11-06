using PetsOverhaul.Items;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
namespace PetsOverhaul.Systems
{
    public class PetTilePlacement : GlobalTile
    {
        #region Pet Food related
        public override void Drop(int i, int j, int type)
        {
            if (PetGlobalItem.treeTile[type] && Main.rand.NextBool(50))
            {
                Item.NewItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.GlobalItem), i * 16, j * 16, 16, 16, ModContent.ItemType<PetFood>());
            }
            else if (Main.rand.NextBool(3) && (type == TileID.MatureHerbs || type == TileID.BloomingHerbs))
            {
                Item.NewItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.GlobalItem), i * 16, j * 16, 16, 16, ModContent.ItemType<PetFood>());
            }
        }
        public override bool ShakeTree(int x, int y, TreeTypes treeType)
        {
            if (Main.rand.NextBool(10))
            {
                Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, ModContent.ItemType<PetFood>(), Main.rand.NextBool() ? 1 : 2);
            }
            return false;
        }
        #endregion

        #region Block placement check mechanic related
        public static void RemoveFromList(int i, int j)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = ModContent.GetInstance<PetsOverhaul>().GetPacket();
                packet.Write((byte)MessageType.BlockRemove);
                packet.Write(i);
                packet.Write(j);
                packet.Send();
            }
            else
            {
                GlobalPet.CoordsToRemove.Add(new Point16(i, j));
            }
        }
        public static void AddToList(int i, int j)
        {
            if (PlayerPlacedBlockList.placedBlocksByPlayer.Contains(new Point16(i, j)))
            {
                return;
            }

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = ModContent.GetInstance<PetsOverhaul>().GetPacket();
                packet.Write((byte)MessageType.BlockPlace);
                packet.Write(i);
                packet.Write(j);
                packet.Send();
            }
            else
            {
                PlayerPlacedBlockList.placedBlocksByPlayer.Add(new Point16(i, j));
            }
        }
        public static void ReplacedBlockToList(int i, int j)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = ModContent.GetInstance<PetsOverhaul>().GetPacket();
                packet.Write((byte)MessageType.BlockReplace);
                packet.Write(i);
                packet.Write(j);
                packet.Send();
            }
            else
            {
                GlobalPet.updateReplacedTile.Add(new Point16(i, j));
            }
        }
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            AddToList(i, j);
        }
        public override bool CanReplace(int i, int j, int type, int tileTypeBeingPlaced)
        {
            ReplacedBlockToList(i, j);

            return base.CanReplace(i, j, type, tileTypeBeingPlaced);
        }
    }
    public class PlayerPlacedBlockList : ModSystem
    {
        public static List<Point16> placedBlocksByPlayer = [];
        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("placedBlocksByPlayer", placedBlocksByPlayer);
        }
        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.TryGet("placedBlocksByPlayer", out List<Point16> listOfPlacedBlocks))
            {
                placedBlocksByPlayer = listOfPlacedBlocks;
                placedBlocksByPlayer = [.. placedBlocksByPlayer.Distinct()]; //Removes duplicate entries
                placedBlocksByPlayer.RemoveAll(x => WorldGen.TileEmpty(x.X, x.Y) && Main.tile[x].HasActuator == false); //Removes 'empty' tile entries
            }
        }
    }
    #endregion
}