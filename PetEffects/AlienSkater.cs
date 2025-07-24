using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class AlienSkater : PetEffect
    {
        public override int PetItemID => ItemID.MartianPetItem;
        public override PetClasses PetClassPrimary => PetClasses.Mobility;
        public float accelerator = 0.10f;
        public float speedMult = 1.05f;
        public float accMult = 1.1f;
        public float speedAccIncr = 0.25f;
        public float wingTimeStore = 0.5f;
        private float wingTimeBank = 0;
        public override void PostUpdateRunSpeeds()
        {
            if (PetIsEquipped())
            {
                Player.runAcceleration *= accelerator + 1f;
                if (wingTimeBank >= 1 && Player.wingTime < Player.wingTimeMax)
                {
                    Player.wingTime++;
                    wingTimeBank--;
                }
            }
        }
        public override void ExtraProcessTriggers(TriggersSet triggersSet)
        {
            if (Player.wingTime > 0 && PetIsEquipped() && triggersSet.Jump && Player.dead == false && Player.equippedWings is not null)
            {
                float total = Math.Abs(Player.velocity.Y) + Math.Abs(Player.velocity.X);
                float xRemain = Math.Abs(Player.velocity.X) / total;
                if (xRemain is float.NaN)
                {
                    xRemain = 0;
                }
                wingTimeBank += Math.Abs(xRemain * wingTimeStore);
            }
        }
    }
    public sealed class AlienSkaterWing : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override void HorizontalWingSpeeds(Item item, Player player, ref float speed, ref float acceleration)
        {
            if (player.TryGetModPlayer(out AlienSkater alienSkater) && player.GetModPlayer<GlobalPet>().PetInUseWithSwapCd(ItemID.MartianPetItem))
            {
                speed *= alienSkater.speedMult;
                acceleration *= alienSkater.accMult;
                speed += alienSkater.speedAccIncr;
                acceleration += alienSkater.speedAccIncr;
            }
        }
    }
    public sealed class MartianPetItem : PetTooltip
    {
        public override PetEffect PetsEffect => alienSkater;
        public static AlienSkater alienSkater
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out AlienSkater pet))
                    return pet;
                else
                    return ModContent.GetInstance<AlienSkater>();
            }
        }
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.MartianPetItem")
                .Replace("<wingTimeSave>", Math.Round(alienSkater.wingTimeStore * 100, 2).ToString())
                .Replace("<acc>", Math.Round(alienSkater.accelerator * 100, 2).ToString())
                .Replace("<speedMult>", alienSkater.speedMult.ToString())
                .Replace("<accMult>", alienSkater.accMult.ToString())
                .Replace("<flatSpdAcc>", Math.Round(alienSkater.speedAccIncr * 100, 2).ToString());
        public override string SimpleTooltip => PetTextsColors.LocVal("SimpleTooltips.MartianPetItem");
    }
}
