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
    public sealed class BabyEater : PetEffect
    {
        public override int PetItemID => ItemID.EatersBone;
        public override PetClasses PetClassPrimary => PetClasses.Mobility;
        public float moveSpd = 0.10f;
        public float jumpSpd = 0.5f;
        public int fallDamageTile = 20;
        public override void PostUpdateMiscEffects()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.EatersBone))
            {
                if (Player.ZoneCorrupt || Player.ZoneCrimson)
                {
                    Player.extraFall += fallDamageTile;
                }

                Player.moveSpeed += moveSpd;
                Player.jumpSpeedBoost += jumpSpd;
                Player.autoJump = true;
            }
        }
        public override void PostUpdate()
        {
            if (Pet.PetInUse(ItemID.EatersBone))
            {
                Player.armorEffectDrawShadow = true;
            }
        }
    }
    public sealed class EatersBone : PetTooltip
    {
        public override PetEffect PetsEffect => babyEater;
        public static BabyEater babyEater
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out BabyEater pet))
                    return pet;
                else
                    return ModContent.GetInstance<BabyEater>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.EatersBone")
                .Replace("<moveSpd>", Math.Round(babyEater.moveSpd * 100, 2).ToString())
                .Replace("<jumpSpd>", Math.Round(babyEater.jumpSpd * 100, 2).ToString())
                .Replace("<fallRes>", babyEater.fallDamageTile.ToString());
        
    }
}
