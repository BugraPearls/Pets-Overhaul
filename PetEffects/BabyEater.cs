using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class BabyEater : PetEffect
    {
        public override int PetItemID => ItemID.EatersBone;
        public override PetClasses PetClassPrimary => PetClasses.Mobility;
        public float moveSpd = 0.11f;
        public float jumpSpd = 0.65f;
        public int fallDamageTile = 30;
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
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
            if (PetIsEquipped(false))
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
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.EatersBone")
                .Replace("<moveSpd>", Math.Round(babyEater.moveSpd * 100, 2).ToString())
                .Replace("<jumpSpd>", Math.Round(babyEater.jumpSpd * 100, 2).ToString())
                .Replace("<fallRes>", babyEater.fallDamageTile.ToString());

    }
}
