﻿using Microsoft.Xna.Framework;
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
        }
        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.Yellow.ToVector3() * 0.3f);
        }
    }
}
