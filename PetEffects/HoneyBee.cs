using PetsOverhaul.Buffs;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class HoneyBee : PetEffect
    {
        public override int PetItemID => ItemID.QueenBeePetItem;
        public override PetClasses PetClassPrimary => PetClasses.Supportive;
        public float bottledHealth = 0.18f;
        public float honeyfinHealth = 0.25f;
        public float selfPotionIncrease = 0.1f;
        public int honeyOverdoseTime = 3600;
        public int bottledHoneyBuff = 1200;
        public int honeyfinHoneyBuff = 600;
        public float abilityHaste = 0.15f;
        public int range = 880;
        public float currentAbilityHasteBonus = 0;
        public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
        {
            if (PetIsEquipped())
            {
                healValue += (int)(healValue * selfPotionIncrease);
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Player.HasBuff(ModContent.BuffType<HoneyOverdose>()))
            {
                Pet.abilityHaste += currentAbilityHasteBonus;
            }
        }
        public static void HealByHoneyBee(bool isBottledHoney, int healersWhoAmI, bool selfHeal)
        {
            HoneyBee healer = Main.player[healersWhoAmI].GetModPlayer<HoneyBee>();
            GlobalPet.CircularDustEffect(healer.Player.Center, DustID.Honey, healer.range, 130);
            if (selfHeal)
            {
                Player player = healer.Player;
                healer.Pet.PetRecovery(player.statLifeMax2 * (isBottledHoney ? healer.bottledHealth : healer.honeyfinHealth) / 2, 1f, isLifesteal: false);
                player.AddBuff(BuffID.Honey, (isBottledHoney ? healer.bottledHoneyBuff : healer.honeyfinHoneyBuff) / 2);
                player.AddBuff(ModContent.BuffType<HoneyOverdose>(), healer.honeyOverdoseTime);
                player.GetModPlayer<HoneyBee>().currentAbilityHasteBonus = healer.abilityHaste;
            }
            else
            {
                foreach (var player in Main.ActivePlayers)
                {
                    if (player.whoAmI == healersWhoAmI || player.Distance(healer.Player.Center) >= healer.range || player.HasBuff(ModContent.BuffType<HoneyOverdose>()))
                    {
                        continue;
                    }
                    HoneyBee healedHoney = Main.player[healersWhoAmI].GetModPlayer<HoneyBee>();
                    healedHoney.Pet.PetRecovery(player.statLifeMax2 * (isBottledHoney ? healer.bottledHealth : healer.honeyfinHealth), 1f, isLifesteal: false);
                    player.AddBuff(BuffID.Honey, isBottledHoney ? healer.bottledHoneyBuff : healer.honeyfinHoneyBuff);
                    player.AddBuff(ModContent.BuffType<HoneyOverdose>(), (int)(healer.honeyOverdoseTime * (1 / (1 + healer.abilityHaste))));
                    player.GetModPlayer<HoneyBee>().currentAbilityHasteBonus = healer.abilityHaste;
                }
            }
        }
    }
    public sealed class HoneyBeePotions : GlobalItem
    {
        public override bool ConsumeItem(Item item, Player player)
        {
            if (player.GetModPlayer<HoneyBee>().PetIsEquipped() && (item.type == ItemID.BottledHoney || item.type == ItemID.Honeyfin))
            {
                bool isBottledHoney = item.type == ItemID.BottledHoney;
                HoneyBee.HealByHoneyBee(isBottledHoney, player.whoAmI, true);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)MessageType.HoneyBeeHeal);
                    packet.Write(isBottledHoney);
                    packet.Write((byte)player.whoAmI);
                    packet.Send();
                }
            }
            return base.ConsumeItem(item, player);
        }
    }
    public sealed class QueenBeePetItem : PetTooltip
    {
        public override PetEffect PetsEffect => honeyBee;
        public static HoneyBee honeyBee
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out HoneyBee pet))
                    return pet;
                else
                    return ModContent.GetInstance<HoneyBee>();
            }
        }
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.QueenBeePetItem")
                       .Replace("<extraHeal>", Math.Round(honeyBee.selfPotionIncrease * 100, 2).ToString())
                       .Replace("<range>", Math.Round(honeyBee.range / 16f, 2).ToString())
                       .Replace("<bottledHealth>", Math.Round(honeyBee.bottledHealth * 100, 2).ToString())
                       .Replace("<honeyfinHealth>", Math.Round(honeyBee.honeyfinHealth * 100, 2).ToString())
                       .Replace("<bottledHoneyTime>", Math.Round(honeyBee.bottledHoneyBuff / 60f, 2).ToString())
                       .Replace("<honeyfinHoneyTime>", Math.Round(honeyBee.honeyfinHoneyBuff / 60f, 2).ToString())
                       .Replace("<abilityHaste>", Math.Round(honeyBee.abilityHaste * 100, 2).ToString());
        public override string SimpleTooltip => PetTextsColors.LocVal("SimpleTooltips.QueenBeePetItem");
    }
}
