using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace PetsOverhaul.Items
{
    public class PetFood : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.height = 26;
            Item.width = 20;
            Item.rare = ItemRarityID.Blue;
            Item.value = 500;
        }
    }
    public class PetFoodFromTrees : GlobalTile
    {
        public override bool ShakeTree(int x, int y, TreeTypes treeType)
        {
            if (Main.rand.NextBool(10))
            {
                Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, ModContent.ItemType<PetFood>(), Main.rand.NextBool() ? 1 : 2);
                return true;
            }
            return false;
        }
    }
}
