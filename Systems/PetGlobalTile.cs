using PetsOverhaul.Config;
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
    public class PetGlobalTile : GlobalTile
    {
        #region Pet Food related
        public override void Drop(int i, int j, int type)
        {
            if (ModContent.GetInstance<PetPersonalizationServer>().PetFoodDrop)
            {
                if (PetIDs.treeTile[type] && Main.rand.NextBool(50))
                {
                    Item.NewItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.GlobalItem), i * 16, j * 16, 16, 16, ModContent.ItemType<PetFood>());
                }
                else if (Main.rand.NextBool(3) && (type == TileID.MatureHerbs || type == TileID.BloomingHerbs))
                {
                    Item.NewItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.GlobalItem), i * 16, j * 16, 16, 16, ModContent.ItemType<PetFood>());
                }
            }
        }
        public override bool ShakeTree(int x, int y, TreeTypes treeType)
        {
            if (ModContent.GetInstance<PetPersonalizationServer>().PetFoodDrop && Main.rand.NextBool(10))
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
                PetModPlayer.BrokenTiles.Add(new Point16(i, j));
            }
        }
        public static void AddToList(int i, int j)
        {
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
                PlayerPlacedBlockList.PlayerPlacedBlocks.Add(new Point16(i, j));
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
                PetModPlayer.ReplacedTiles.Add(new Point16(i, j));
            }
        }
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            AddToList(i, j);
        }
        public override void ReplaceTile(int i, int j, int type, int targetType, int targetStyle)
        {
            ReplacedBlockToList(i, j);
        }
    }
    public class PlayerPlacedBlockList : ModSystem
    {
        public static HashSet<Point16> PlayerPlacedBlocks = [];
        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("placedBlocksByPlayer", PlayerPlacedBlocks.ToList());
        }
        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.TryGet("placedBlocksByPlayer", out List<Point16> playerPlacedBlocks))
            {
                PlayerPlacedBlocks = playerPlacedBlocks.ToHashSet();
                PlayerPlacedBlocks.RemoveWhere(x => WorldGen.TileEmpty(x.X, x.Y) && Main.tile[x].HasActuator == false);
            }
        }
    }
    #endregion
}