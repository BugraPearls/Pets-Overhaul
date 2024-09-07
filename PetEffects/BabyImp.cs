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
    public sealed class BabyImp : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public override PetClasses PetClassSecondary => PetClasses.Defensive;
        public int lavaImmune = 600;
        public int lavaDef = 10;
        public float lavaSpd = 0.15f;
        public int obbyDef = 8;
        public float obbySpd = 0.08f;
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.HellCake))
            {
                Player.lavaMax += lavaImmune;
                if (Collision.LavaCollision(Player.position, Player.width, Player.height))
                {
                    Player.accFlipper = true;
                    Player.statDefense += lavaDef;
                    Player.moveSpeed += lavaSpd;
                }
                if (Player.HasBuff(BuffID.ObsidianSkin))
                {
                    Player.statDefense += obbyDef;
                    Player.moveSpeed -= obbySpd;
                }
            }
        }
    }
    public sealed class HellCake : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.HellCake;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            BabyImp babyImp = Main.LocalPlayer.GetModPlayer<BabyImp>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.HellCake")
                .Replace("<class>", PetTextsColors.ClassText(babyImp.PetClassPrimary, babyImp.PetClassSecondary))
                .Replace("<immuneTime>", Math.Round(babyImp.lavaImmune / 60f, 2).ToString())
                .Replace("<lavaDef>", babyImp.lavaDef.ToString())
                .Replace("<lavaSpd>", Math.Round(babyImp.lavaSpd * 100, 2).ToString())
                .Replace("<obbyDef>", babyImp.obbyDef.ToString())
                .Replace("<obbySpd>", Math.Round(babyImp.obbySpd * 100, 2).ToString())
            ));
        }
    }
}
