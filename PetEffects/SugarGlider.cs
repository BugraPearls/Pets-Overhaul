﻿using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class SugarGlider : PetEffect
    {
        public override int PetItemID => ItemID.EucaluptusSap;
        public float speedMult = 1.1f;
        public float accMult = 1.2f;
        public float accSpeedRaise = 0.1f;
        public float glideSpeedMult = 0.25f;
        public override PetClasses PetClassPrimary => PetClasses.Mobility;
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Player.velocity.Y > 0 && PetIsEquipped() && triggersSet.Jump && Player.dead == false)
            {
                Player.maxFallSpeed *= 0.25f;
                Player.fallStart = (int)((double)Player.position.Y / 16.0);
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
    public sealed class EucaluptusSap : PetTooltip
    {
        public override PetEffect PetsEffect => sugarGlider;
        public static SugarGlider sugarGlider
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out SugarGlider pet))
                    return pet;
                else
                    return ModContent.GetInstance<SugarGlider>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.EucaluptusSap")
                .Replace("<glide>", sugarGlider.glideSpeedMult.ToString())
                        .Replace("<speed>", sugarGlider.speedMult.ToString())
                        .Replace("<acceleration>", sugarGlider.accMult.ToString())
                        .Replace("<flatIncrease>", Math.Round(sugarGlider.accSpeedRaise * 100, 2).ToString());
    }
}
