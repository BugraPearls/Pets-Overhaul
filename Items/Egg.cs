using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Items
{
    public class Egg : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 30;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.RottenEgg);
            Item.maxStack = 9999;
            Item.height = 26;
            Item.width = 20;
        }
        public override void AddRecipes()
        {
            Recipe.Create(ItemID.FriedEgg)
                .AddIngredient(ModContent.ItemType<Egg>(), 3)
                .AddTile(TileID.CookingPots)
                .Register();
        }
    }
}
