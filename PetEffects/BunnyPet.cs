using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class CarrotBunny : PetEffect
    {
        public override int PetItemID => ItemID.Carrot;
        public override PetClasses PetClassPrimary => PetClasses.Mobility;
        private int bunnyTimer = 0;
        public int bunnyStack = 0;
        public int maxStack = 10;
        public float jumpPerStk = 0.02f;
        public float spdPerStk = 0.005f;
        public override int PetStackCurrent => bunnyStack;
        public override int PetStackMax => maxStack;
        public override string PetStackText => PetUtils.LocVal("PetItemTooltips.CarrotStack");
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                bunnyTimer--;
                if (Player.jump < Player.jumpHeight / 2 && Player.jump != 0 && Pet.jumpRegistered == false)
                {
                    if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                    {
                        SoundEngine.PlaySound(SoundID.Item56 with { Volume = 0.5f, Pitch = 0.2f, PitchVariance = 0.3f }, Player.Center);
                    }

                    bunnyStack++;
                    bunnyTimer = 240;
                    Pet.jumpRegistered = true;
                }
                if (bunnyStack > maxStack)
                {
                    bunnyStack = maxStack;
                }
                if (bunnyTimer <= 0)
                {
                    bunnyStack = 0;
                }
                Player.jumpSpeedBoost += bunnyStack * jumpPerStk;
                Player.moveSpeed += bunnyStack * spdPerStk;
            }
        }
    }
    public sealed class Carrot : PetTooltip
    {
        public override PetEffect PetsEffect => carrotBunny;
        public static CarrotBunny carrotBunny
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out CarrotBunny pet))
                    return pet;
                else
                    return ModContent.GetInstance<CarrotBunny>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.Carrot")
                .Replace("<moveSpeed>", Math.Round(carrotBunny.spdPerStk * 100, 2).ToString())
                .Replace("<jumpSpeed>", Math.Round(carrotBunny.jumpPerStk * 100, 2).ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.Carrot");
    }
}
