using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class CrimsonHeartEffect : LightPetEffect
    {
        public override int LightPetItemID => ItemID.CrimsonHeart;
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out CrimsonHeart crimsonHeart))
            {
                Player.statLifeMax2 += crimsonHeart.Health.CurrentStatInt;
                Pet.petHealMultiplier += crimsonHeart.HealingPower.CurrentStatFloat;
                Pet.fishingFortune += crimsonHeart.FishingFortune.CurrentStatInt;
            }
        }
    }
    public sealed class CrimsonHeart : LightPetItem
    {
        public LightPetStat Health = new(10, 1, 10);
        public LightPetStat HealingPower = new(15, 0.005f, 0.025f);
        public LightPetStat FishingFortune = new(15, 1, 5);
        public override int LightPetItemID => ItemID.CrimsonHeart;
        public override void UpdateInventory(Item item, Player player)
        {
            Health.SetRoll(player.luck);
            HealingPower.SetRoll(player.luck);
            FishingFortune.SetRoll(player.luck);
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)Health.CurrentRoll);
            writer.Write((byte)HealingPower.CurrentRoll);
            writer.Write((byte)FishingFortune.CurrentRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            Health.CurrentRoll = reader.ReadByte();
            HealingPower.CurrentRoll = reader.ReadByte();
            FishingFortune.CurrentRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("CrimsonHealth", Health.CurrentRoll);
            tag.Add("CrimsonExp", HealingPower.CurrentRoll); //Exp stats are obsolete
            tag.Add("CrimsonFort", FishingFortune.CurrentRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("CrimsonHealth", out int hp))
            {
                Health.CurrentRoll = hp;
            }

            if (tag.TryGet("CrimsonExp", out int exp))
            {
                HealingPower.CurrentRoll = exp;
            }

            if (tag.TryGet("CrimsonFort", out int fort))
            {
                FishingFortune.CurrentRoll = fort;
            }
        }
        public override int GetRoll() => HealingPower.CurrentRoll;
        public override string PetsTooltip => PetTextsColors.LocVal("LightPetTooltips.CrimsonHeart")

                        .Replace("<hp>", Health.BaseAndPerQuality())
                        .Replace("<heal>", HealingPower.BaseAndPerQuality())
                        .Replace("<fortune>", FishingFortune.BaseAndPerQuality())

                        .Replace("<hpLine>", Health.StatSummaryLine())
                        .Replace("<healLine>", HealingPower.StatSummaryLine())
                        .Replace("<fortuneLine>", FishingFortune.StatSummaryLine());
    }
}
