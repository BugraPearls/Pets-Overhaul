using PetsOverhaul.Items;
using PetsOverhaul.Tiles;
using System;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
namespace PetsOverhaul.Systems
{
    public class PetRecipes : ModSystem
    {
        /// <summary>
        /// Obsolete, will be removed.
        /// </summary>
        public static void MasterPetCraft(int result, int itemToPairWithMasteryShard)
        {
            Recipe.Create(result)
            .AddIngredient(ModContent.ItemType<MasteryShard>())
            .AddIngredient(itemToPairWithMasteryShard)
            .AddTile(ModContent.TileType<PetForge>())
            .Register();
        }
        /// <summary>
        /// Pet Recipes all have Pet Food in their recipes, and crafted on Pet Forge.
        /// </summary>
        /// <param name="recipe">Recipe that will get registered, to be added Pet Food and to be crafted on a Pet Forge.</param>
        /// <param name="petFoodAmount"></param>
        public static void PetRecipe(Recipe recipe, int petFoodAmount = 1)
        {
            recipe.AddIngredient(ModContent.ItemType<PetFood>(), Math.Max(petFoodAmount,1))
                .AddTile(ModContent.TileType<PetForge>())
                .Register();
        }
        /// <summary>
        /// Creates a Pet Recipe that also has a Mastery Shard as one of its ingredients.
        /// </summary>
        public static void MasterModePetRecipe(Recipe recipe, int petFoodAmount = 1, int masteryShardAmount = 1) => PetRecipe(recipe.AddIngredient<MasteryShard>(Math.Max(masteryShardAmount,1)), petFoodAmount);

        public override void AddRecipes()
        {
            MasterModePetRecipe(Recipe.Create(ItemID.MartianPetItem).AddIngredient(ItemID.MartianConduitPlating, 500).AddIngredient(ItemID.Hoverboard), 1500); //Example: This is 500 Martian Conduit Plating, 1 Hoverboard, 75~ Gold Coins (1500 Pet Food) and 1 Mastery Shard.
        }
    }
}
