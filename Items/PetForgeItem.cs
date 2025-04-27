using PetsOverhaul.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Items
{
    public class PetForgeItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<PetForge>());
            Item.width = 28; // The item texture's width
            Item.height = 14; // The item texture's height
            Item.value = Item.buyPrice(gold: 50);
            Item.rare = ItemRarityID.LightRed;
        }
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.CraftingObjects;
        }
    }
}
