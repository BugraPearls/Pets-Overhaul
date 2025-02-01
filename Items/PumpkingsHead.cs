using Microsoft.Xna.Framework;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Items
{
    public class PumpkingsHead : ModItem
    {
        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Master;
            Item.value = 0;
            Item.master = true;
            Item.width = 30;
            Item.height = 30;
            Item.consumable = true;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item29;
        }
        public override bool ConsumeItem(Player player)
        {
            if (player.TryGetModPlayer(out GlobalPet pet) && pet.pumpkingConsumed == false)
            {
                pet.pumpkingConsumed = true;
                return true;
            }
            return false;
        }
        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.Yellow.ToVector3() * 0.3f);
        }
    }
}
