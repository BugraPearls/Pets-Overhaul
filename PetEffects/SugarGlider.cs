﻿using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class SugarGlider : PetEffect
    {
        public float speedMult = 1.1f;
        public float accMult = 1.2f;
        public float accSpeedRaise = 0.1f;

        public override PetClasses PetClassPrimary => PetClasses.Mobility;
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.EucaluptusSap))
            {
                if (Player.equippedWings == null)
                {
                    Player.wings = 1;
                    Player.wingsLogic = 1;
                    Player.wingTimeMax = 1;
                }
            }
        }
    }
    public sealed class SugarGliderWing : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override void HorizontalWingSpeeds(Item item, Player player, ref float speed, ref float acceleration)
        {
            if (player.TryGetModPlayer(out SugarGlider sugarGlider) && player.GetModPlayer<GlobalPet>().PetInUseWithSwapCd(ItemID.EucaluptusSap))
            {
                speed *= sugarGlider.speedMult;
                speed += sugarGlider.accSpeedRaise;
                acceleration *= sugarGlider.accMult;
                acceleration += sugarGlider.accSpeedRaise;
            }
        }
    }
    public sealed class EucaluptusSap : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.EucaluptusSap;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            SugarGlider sugarGlider = Main.LocalPlayer.GetModPlayer<SugarGlider>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.EucaluptusSap")
                .Replace("<class>", PetColors.ClassText(sugarGlider.PetClassPrimary, sugarGlider.PetClassSecondary))
                        .Replace("<speed>", sugarGlider.speedMult.ToString())
                        .Replace("<acceleration>", sugarGlider.accMult.ToString())
                        .Replace("<flatIncrease>", Math.Round(sugarGlider.accSpeedRaise * 100, 2).ToString())
                        ));
        }
    }
}
