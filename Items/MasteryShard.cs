using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Items
{
    public class MasteryShard : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(7, 8));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Master;
            Item.value = Item.sellPrice(gold: 10);
            Item.master = true;
            Item.width = 24;
            Item.height = 24;
        }
        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, new Color(255, (byte)(Main.masterColor * 200f), 0, Main.mouseTextColor).ToVector3() * 0.8f * Main.essScale);
        }
    }
}
