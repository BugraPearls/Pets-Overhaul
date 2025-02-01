using PetsOverhaul.Systems;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Items
{
    public class PrismaticOptic : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Master;
            Item.value = 0;
            Item.master = true;
            Item.width = 42;
            Item.height = 42;
            Item.alpha = 100;
            Item.consumable = true;
        }
        public override bool ConsumeItem(Player player)
        {
            if (player.TryGetModPlayer(out GlobalPet pet) && pet.eolConsumed == false)
            {
                pet.eolConsumed = true;
                return true;
            }
            return false;
        }
        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, PetTextsColors.MaxQuality.ToVector3() * 0.7f);
        }
    }
}
