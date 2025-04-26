using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class EyeballSpring : PetEffect
    {
        public override int PetItemID => ItemID.EyeSpring;
        public override PetClasses PetClassPrimary => PetClasses.Mobility;
        public float acceleration = 0.16f;
        public float jumpBoost = 4.55f;
        public float ascentPenaltyMult = 0.55f;
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.jumpSpeedBoost += jumpBoost;
            }
        }
        public override void PostUpdateRunSpeeds()
        {
            if (PetIsEquipped())
            {
                if (Player.jump > 0 && Pet.jumpRegistered == false)
                {
                    if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                    {
                        SoundEngine.PlaySound(SoundID.Item56 with { Volume = 0.5f, Pitch = -0.3f, PitchVariance = 0.1f }, Player.Center);
                    }

                    Pet.jumpRegistered = true;
                }
                Player.runAcceleration *= acceleration + 1f;
                Player.jumpSpeedBoost += jumpBoost;
            }
        }
    }
    public sealed class EyeballSpringWing : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override void VerticalWingSpeeds(Item item, Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            if (player.TryGetModPlayer(out EyeballSpring eyeballs) && player.GetModPlayer<GlobalPet>().PetInUseWithSwapCd(ItemID.EyeSpring))
            {
                maxAscentMultiplier *= eyeballs.ascentPenaltyMult;
            }

        }
    }
    public sealed class EyeSpring : PetTooltip
    {
        public override PetEffect PetsEffect => eyeballSpring;
        public static EyeballSpring eyeballSpring
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out EyeballSpring pet))
                    return pet;
                else
                    return ModContent.GetInstance<EyeballSpring>();
            }
        }
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.EyeSpring")
                        .Replace("<jumpBoost>", Math.Round(eyeballSpring.jumpBoost * 100, 2).ToString())
                        .Replace("<acceleration>", Math.Round(eyeballSpring.acceleration * 100, 2).ToString())
                        .Replace("<ascNerf>", eyeballSpring.ascentPenaltyMult.ToString());
    }
}
