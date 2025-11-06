using Microsoft.Xna.Framework;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Items
{
    public class LihzahrdWrench : ModItem
    {
        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Master;
            Item.value = 0;
            Item.master = true;
            Item.width = 38;
            Item.height = 38;
            Item.consumable = true;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item29;
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer && PetModPlayer.golemConsumed == false)
            {
                PetModPlayer.golemConsumed = true;
                return true;
            }
            return false;
        }
        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.DarkOrange.ToVector3() * 0.3f);
        }
    }
}
