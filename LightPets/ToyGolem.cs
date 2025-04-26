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
    public sealed class ToyGolemEffect : LightPetEffect
    {
        public override int LightPetItemID => ItemID.GolemPetItem;
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out ToyGolem toyGolem))
            {
                Player.lifeRegen += toyGolem.HealthRegen.CurrentStatInt;
                Player.manaRegenBonus += toyGolem.ManaRegen.CurrentStatInt;
                Player.statLifeMax2 += (int)(Player.statLifeMax2 * toyGolem.PercentHealth.CurrentStatFloat);
            }
        }
    }
    public sealed class ToyGolem : LightPetItem
    {
        public LightPetStat HealthRegen = new(4, 1);
        public LightPetStat PercentHealth = new(35, 0.0018f, 0.022f);
        public LightPetStat ManaRegen = new(20, 5, 30);
        public override int LightPetItemID => ItemID.GolemPetItem;
        public override void UpdateInventory(Item item, Player player)
        {
            HealthRegen.SetRoll(player.luck);
            PercentHealth.SetRoll(player.luck);
            ManaRegen.SetRoll(player.luck);
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)HealthRegen.CurrentRoll);
            writer.Write((byte)ManaRegen.CurrentRoll);
            writer.Write((byte)PercentHealth.CurrentRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            HealthRegen.CurrentRoll = reader.ReadByte();
            ManaRegen.CurrentRoll = reader.ReadByte();
            PercentHealth.CurrentRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("GolemRegen", HealthRegen.CurrentRoll);
            tag.Add("GolemHealth", PercentHealth.CurrentRoll);
            tag.Add("GolemExp", ManaRegen.CurrentRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("GolemRegen", out int reg))
            {
                HealthRegen.CurrentRoll = reg;
            }

            if (tag.TryGet("GolemHealth", out int hp))
            {
                PercentHealth.CurrentRoll = hp;
            }

            if (tag.TryGet("GolemExp", out int exp))
            {
                ManaRegen.CurrentRoll = exp;
            }
        }
        public override int GetRoll() => PercentHealth.CurrentRoll;
        public override string PetsTooltip => PetTextsColors.LocVal("LightPetTooltips.ToyGolem")

                        .Replace("<lifeRegen>", HealthRegen.BaseAndPerQuality())
                        .Replace("<healthPercent>", PercentHealth.BaseAndPerQuality())
                        .Replace("<manaRegen>", ManaRegen.BaseAndPerQuality())

                        .Replace("<lifeRegenLine>", HealthRegen.StatSummaryLine())
                        .Replace("<healthPercentLine>", PercentHealth.StatSummaryLine())
                        .Replace("<manaRegenLine>", ManaRegen.StatSummaryLine());
    }
}
