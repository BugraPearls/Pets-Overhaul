using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class GlitteryButterfly : PetEffect
    {
        public override int PetItemID => ItemID.BedazzledNectar;
        public override PetClasses PetClassPrimary => PetClasses.Mobility;
        public int wingTime = 45;
        public float currentWingPercIncr = 0.5f;
        public float healthPenalty = 0.15f;
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                if (Player.equippedWings == null)
                {
                    Player.statLifeMax2 -= (int)(Player.statLifeMax2 * healthPenalty);
                    Player.wings = 5; //butterfly wings
                    Player.wingsLogic = 5; //I don't exactly know what wingsLogic is.
                    Player.wingTimeMax = wingTime;
                    Player.noFallDmg = true;
                }
                else
                {
                    Player.wingTimeMax += (int)(Player.wingTimeMax * currentWingPercIncr) + wingTime;
                }
            }
        }
    }
    public sealed class BedazzledNectar : PetTooltip
    {
        public override PetEffect PetsEffect => glitteryButterfly;
        public static GlitteryButterfly glitteryButterfly
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out GlitteryButterfly pet))
                    return pet;
                else
                    return ModContent.GetInstance<GlitteryButterfly>();
            }
        }
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.BedazzledNectar")
                        .Replace("<flight>", Math.Round(glitteryButterfly.wingTime / 60f, 2).ToString())
                        .Replace("<percFlight>", Math.Round(glitteryButterfly.currentWingPercIncr * 100, 2).ToString())
                        .Replace("<healthNerf>", Math.Round(glitteryButterfly.healthPenalty * 100, 2).ToString());
    }
}
