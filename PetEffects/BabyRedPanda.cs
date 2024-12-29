using PetsOverhaul.Config;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class BabyRedPanda : PetEffect
    {
        public override int PetItemID => ItemID.BambooLeaf;
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override PetClasses PetClassSecondary => PetClasses.Utility;
        public float regularAtkSpd = 0.035f;
        public float jungleBonusSpd = 0.025f;
        public int bambooChance = 50;
        public int alertTime = 480;
        public int alertCd = 1800;
        public float alertMs = 0.03f;
        public float alertAs = 0.015f;
        public int alertAggro = 125;
        public int alertRadius = 800;
        private int alertEnemies = 1;
        public int alertEnemiesMax = 6;
        private int alertTimer = 0;
        public override int PetAbilityCooldown => alertCd;
        public override void ExtraPreUpdate()
        {
            alertTimer--;
            if (alertTimer <= 0)
            {
                alertTimer = 0;
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.GetAttackSpeed<GenericDamageClass>() += regularAtkSpd + (Player.ZoneJungle ? jungleBonusSpd : 0);
                if (alertTimer > 0)
                {
                    Player.GetAttackSpeed<GenericDamageClass>() += alertAs * alertEnemies;
                    Player.moveSpeed += alertMs * alertEnemies;
                    Player.aggro -= alertAggro * alertEnemies;
                    Player.AddBuff(BuffID.Hunter, 1);
                }
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && PetIsEquipped())
            {
                GlobalPet.CircularDustEffect(Player.Center, 170, alertRadius, 80);
                if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                    SoundEngine.PlaySound(SoundID.Item37 with { Pitch = 1f }, Player.Center);
                EmoteBubble.MakePlayerEmote(Player, EmoteID.EmotionAlert);
                alertEnemies = 1;
                foreach (var npc in Main.ActiveNPCs)
                {
                    if (Player.Distance(npc.Center) < alertRadius)
                    {
                        alertEnemies++;
                    }
                }
                if (alertEnemies > alertEnemiesMax)
                {
                    alertEnemies = alertEnemiesMax;
                }
                alertTimer = alertTime;
                Pet.timer = Pet.timerMax;
            }
        }
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public static void PreOnPickup(Item item, Player player)
        {
            GlobalPet PickerPet = player.GetModPlayer<GlobalPet>();
            BabyRedPanda panda = player.GetModPlayer<BabyRedPanda>();
            if (PickerPet.PickupChecks(item, panda.PetItemID, out ItemPet _) && item.type == ItemID.BambooBlock)
            {
                for (int i = 0; i < GlobalPet.Randomizer(panda.bambooChance * item.stack); i++)
                {
                    player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.HarvestingItem), item.type, 1);
                }
            }
        }
    }
    public sealed class BambooLeaf : PetTooltip
    {
        public override PetEffect PetsEffect => babyRedPanda;
        public static BabyRedPanda babyRedPanda
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out BabyRedPanda pet))
                    return pet;
                else
                    return ModContent.GetInstance<BabyRedPanda>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.BambooLeaf")
                .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility))
                .Replace("<alertAs>", Math.Round(babyRedPanda.alertAs * 100, 2).ToString())
                .Replace("<alertMs>", Math.Round(babyRedPanda.alertMs * 100, 2).ToString())
                .Replace("<alertAggro>", babyRedPanda.alertAggro.ToString())
                .Replace("<alertRadius>", Math.Round(babyRedPanda.alertRadius / 16f, 2).ToString())
                .Replace("<alertMax>", babyRedPanda.alertEnemiesMax.ToString())
                .Replace("<alertDuration>", Math.Round(babyRedPanda.alertTime / 60f, 2).ToString())
                .Replace("<alertCd>", Math.Round(babyRedPanda.alertCd / 60f, 2).ToString())
                .Replace("<atkSpd>", Math.Round(babyRedPanda.regularAtkSpd * 100, 2).ToString())
                .Replace("<jungleAtkSpd>", Math.Round(babyRedPanda.jungleBonusSpd * 100, 2).ToString())
                .Replace("<bambooChance>", babyRedPanda.bambooChance.ToString());
    }
}
