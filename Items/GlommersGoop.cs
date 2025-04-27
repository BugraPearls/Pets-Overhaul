using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Items
{
    public class GlommersGoop : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 10;
        }
        public override void SetDefaults()
        {
            Item.healLife = 40;
            Item.maxStack = 9999;
            Item.height = 24;
            Item.width = 34;
            Item.buffType = BuffID.WellFed;
            Item.buffTime = 563; //9.383 seconds (Intended: 9.375)
            Item.consumable = true;
            Item.rare = ItemRarityID.Pink;
            Item.potion = true;
            Item.useTurn = true;
            Item.UseSound = SoundID.Frog;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = Item.buyPrice(copper: 50);
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.EatFood;
        }
        public override bool? UseItem(Player player)
        {
            player.ManaEffect(-50);
            if (player.statMana <= 50)
            {
                player.statMana = 0;
            }
            else
            {
                player.statMana -= 50;
            }

            return true;
        }
        public override void AddRecipes()
        {
            Recipe.Create(ItemID.Fertilizer, 5)
            .AddIngredient(Type)
            .AddIngredient(ItemID.Bone, 5)
            .AddIngredient(ItemID.AshBlock, 5)
            .AddTile(TileID.Bottles)
            .AddTile(TileID.AlchemyTable)
            .Register();
            Recipe.Create(ItemID.Gel, 5)
                .AddIngredient(Type)
                .Register();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Insert(2, new(Mod, "UseMana", PetTextsColors.LocVal("Items.GlommersGoop.Mana")));
        }
    }
}
