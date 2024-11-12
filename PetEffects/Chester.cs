﻿using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Chester : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public int placementRange = 2;
        public int chestOpenDef = 10;
        public int suckingUpRange = 80;
        public override void PostUpdateMiscEffects()
        {
            if (Pet.PetInUse(ItemID.ChesterPetItem))
            {
                Player.blockRange += placementRange;
                if (Player.chest != -1)
                {
                    Player.statDefense += chestOpenDef;
                }
            }
        }
    }
    public sealed class ChesterItemGrab : GlobalItem
    {
        public override void GrabRange(Item item, Player player, ref int grabRange)
        {
            if (player.GetModPlayer<GlobalPet>().PetInUse(ItemID.ChesterPetItem) && grabRange > 0)
            {
                grabRange += player.GetModPlayer<Chester>().suckingUpRange;
            }
        }
    }
    public sealed class ChesterPetItem : PetTooltip
    {
        public override PetEffect PetsEffect => chester;
        public static Chester chester
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out Chester pet))
                    return pet;
                else
                    return ModContent.GetInstance<Chester>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.ChesterPetItem")
                .Replace("<pickupRange>", Math.Round(chester.suckingUpRange / 16f, 2).ToString())
                .Replace("<placementRange>", chester.placementRange.ToString())
                .Replace("<chestDef>", chester.chestOpenDef.ToString());
    }
}
