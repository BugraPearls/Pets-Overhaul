using PetsOverhaul.Systems;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class MagicLanternEffect : LightPetEffect
    {
        public override int LightPetItemID => ItemID.MagicLantern;
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out MagicLantern magicLantern))
            {
                Player.statDefense += magicLantern.Defense.CurrentStatInt;
                Player.statDefense *= magicLantern.DefensePercent.CurrentStatFloat + 1f;
                Player.endurance += magicLantern.DamageReduction.CurrentStatFloat;
                Pet.miningFortune += magicLantern.MiningFortune.CurrentStatInt;
            }
        }
    }
    public sealed class MagicLantern : LightPetItem
    {
        public LightPetStat Defense = new(3, 1);
        public LightPetStat DefensePercent = new(20, 0.002f, 0.01f);
        public LightPetStat DamageReduction = new(15, 0.002f, 0.01f);
        public LightPetStat MiningFortune = new(15, 1, 5);
        public override int LightPetItemID => ItemID.MagicLantern;
        public override void UpdateInventory(Item item, Player player)
        {
            Defense.SetRoll(player.luck);
            DefensePercent.SetRoll(player.luck);
            DamageReduction.SetRoll(player.luck);
            MiningFortune.SetRoll(player.luck);
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)Defense.CurrentRoll);
            writer.Write((byte)DefensePercent.CurrentRoll);
            writer.Write((byte)DamageReduction.CurrentRoll);
            writer.Write((byte)MiningFortune.CurrentRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            Defense.CurrentRoll = reader.ReadByte();
            DefensePercent.CurrentRoll = reader.ReadByte();
            DamageReduction.CurrentRoll = reader.ReadByte();
            MiningFortune.CurrentRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("LanternDef", Defense.CurrentRoll);
            tag.Add("LanternMult", DefensePercent.CurrentRoll);
            tag.Add("LanternExp", DamageReduction.CurrentRoll);
            tag.Add("LanternFort", MiningFortune.CurrentRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("LanternDef", out int def))
            {
                Defense.CurrentRoll = def;
            }

            if (tag.TryGet("LanternMult", out int perc))
            {
                DefensePercent.CurrentRoll = perc;
            }

            if (tag.TryGet("LanternExp", out int exp))
            {
                DamageReduction.CurrentRoll = exp;
            }

            if (tag.TryGet("LanternFort", out int fort))
            {
                MiningFortune.CurrentRoll = fort;
            }
        }
        public override int GetRoll() => Defense.CurrentRoll;
        public override string PetsTooltip => PetTextsColors.LocVal("LightPetTooltips.MagicLantern")

                        .Replace("<dr>", DamageReduction.BaseAndPerQuality())
                        .Replace("<fortune>", MiningFortune.BaseAndPerQuality())
                        .Replace("<def>", Defense.BaseAndPerQuality())
                        .Replace("<defPercent>", DefensePercent.BaseAndPerQuality())

                        .Replace("<drLine>", DamageReduction.StatSummaryLine())
                        .Replace("<fortuneLine>", MiningFortune.StatSummaryLine())
                        .Replace("<defLine>", Defense.StatSummaryLine())
                        .Replace("<defPercentLine>", DefensePercent.StatSummaryLine());
    }
}
