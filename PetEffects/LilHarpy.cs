using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class LilHarpy : PetEffect
    {
        public override int PetItemID => ItemID.BirdieRattle;
        public int harpyCd = 1080;
        public int fuelMax = 150;
        public int harpyFlight = 150;
        private bool cooldownStarted;
        public override PetClasses PetClassPrimary => PetClasses.Mobility;
        public override int PetAbilityCooldown => harpyCd;
        public override int PetStackCurrent => harpyFlight;
        public override int PetStackMax => fuelMax;
        public override string PetStackText => PetTextsColors.LocVal("PetItemTooltips.BirdieRattleStack");
        public override bool PetStackIsSeconds => true;
        public override void PostUpdateMiscEffects()
        {
            if (Pet.timer <= 0)
            {
                if (cooldownStarted == true) //If Cooldown has been started (if wings was used) and timer has hit 0, then Wing Time is set to max. Without this, its bugged to not give wing time off cooldown if Wing was used halfway.
                {
                    Player.wingTime = fuelMax;
                }
                cooldownStarted = false;
                harpyFlight = fuelMax;
            }
            if (PetIsEquipped())
            {
                if (Player.equippedWings == null)
                {
                    if (harpyFlight == fuelMax && Player.wingTime == 0)
                    {
                        Player.wingTime = fuelMax;
                    }
                    if (harpyFlight > 0)
                    {
                        Player.wings = 7;
                        Player.wingsLogic = 1;
                        Player.wingTimeMax = harpyFlight;
                        harpyFlight = (int)Player.wingTime;
                    }
                    Player.noFallDmg = true;
                }
                if (cooldownStarted == false && harpyFlight < fuelMax)
                {
                    Pet.timer = Pet.timerMax;
                    cooldownStarted = true;
                }
            }
        }
    }
    public sealed class BirdieRattle : PetTooltip
    {
        public override PetEffect PetsEffect => lilHarpy;
        public static LilHarpy lilHarpy
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out LilHarpy pet))
                    return pet;
                else
                    return ModContent.GetInstance<LilHarpy>();
            }
        }
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.BirdieRattle")
                        .Replace("<flightTime>", Math.Round(lilHarpy.fuelMax / 60f, 2).ToString())
                        .Replace("<cooldown>", Math.Round(lilHarpy.harpyCd / 60f, 2).ToString());
    }
}
