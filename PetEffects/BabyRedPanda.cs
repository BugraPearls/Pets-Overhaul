using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class BabyRedPanda : PetEffect
    {
        public override int PetItemID => ItemID.BambooLeaf;
        public override PetClass PetClassPrimary => PetClassID.Offensive;
        public override PetClass PetClassSecondary => PetClassID.Utility;
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
        public override int PetStackCurrent => alertEnemies;
        public override int PetStackMax => alertEnemiesMax;
        public override string PetStackText => PetUtils.LocVal("PetItemTooltips.BambooLeafStack");
        public override void ExtraPreUpdate()
        {
            alertTimer--;
            if (alertTimer <= 0)
            {
                alertTimer = 0;
                alertEnemies = 0;
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
        public override void ExtraProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && PetIsEquipped())
            {
                Alert();
                BasicSyncMessage(MessageType.BabyRedPanda);
            }
        }
        public void Alert()
        {
            PetUtils.CircularDustEffect(Player.Center, 170, alertRadius, 80);
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
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public static void PreOnPickup(Item item, Player player)
        {
            BabyRedPanda panda = player.GetModPlayer<BabyRedPanda>();
            if (player.PetPlayer().PickupChecks(item, panda.PetItemID, out PetGlobalItem _) && item.type == ItemID.BambooBlock)
            {
                panda.Pet.SpawnItemSourcingFromPet(EntitySourcePetIDs.HarvestingItem, item.type, PetUtils.Randomizer(panda.bambooChance * item.stack));
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
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.BambooLeaf")
                .Replace("<keybind>", PetUtils.KeybindText(PetKeybinds.UsePetAbility))
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
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.BambooLeaf").Replace("<keybind>", PetUtils.KeybindText(PetKeybinds.UsePetAbility));
    }
}
