using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Items
{
    public class PetFood : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
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
}
