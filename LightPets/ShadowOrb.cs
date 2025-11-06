using PetsOverhaul.Systems;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class ShadowOrbEffect : LightPetEffect
    {
        public override int LightPetItemID => ItemID.ShadowOrb;
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out ShadowOrb shadowOrb))
            {
                Player.statManaMax2 += shadowOrb.Mana.CurrentStatInt;
                Pet.petShieldMultiplier += shadowOrb.ShieldingPower.CurrentStatFloat;
                Pet.harvestingFortune += shadowOrb.HarvestingFortune.CurrentStatInt;
            }
        }
    }
    public sealed class ShadowOrb : LightPetItem
    {
        public LightPetStat Mana = new(10, 2, 20);
        public LightPetStat ShieldingPower = new(15, 0.005f, 0.025f);
        public LightPetStat HarvestingFortune = new(15, 1, 5);
        public override int LightPetItemID => ItemID.ShadowOrb;
        public override void UpdateInventory(Item item, Player player)
        {
            Mana.SetRoll(player.luck);
            ShieldingPower.SetRoll(player.luck);
            HarvestingFortune.SetRoll(player.luck);
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)Mana.CurrentRoll);
            writer.Write((byte)ShieldingPower.CurrentRoll);
            writer.Write((byte)HarvestingFortune.CurrentRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            Mana.CurrentRoll = reader.ReadByte();
            ShieldingPower.CurrentRoll = reader.ReadByte();
            HarvestingFortune.CurrentRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("ShadowMana", Mana.CurrentRoll);
            tag.Add("ShadowExp", ShieldingPower.CurrentRoll); //exp stats are obsolete
            tag.Add("ShadowFort", HarvestingFortune.CurrentRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("ShadowMana", out int mana))
            {
                Mana.CurrentRoll = mana;
            }

            if (tag.TryGet("ShadowExp", out int exp))
            {
                ShieldingPower.CurrentRoll = exp;
            }

            if (tag.TryGet("ShadowFort", out int fort))
            {
                HarvestingFortune.CurrentRoll = fort;
            }
        }
        public override int GetRoll() => Mana.CurrentRoll;
        public override string PetsTooltip => PetUtils.LocVal("LightPetTooltips.ShadowOrb")

                        .Replace("<mana>", Mana.BaseAndPerQuality())
                        .Replace("<shield>", ShieldingPower.BaseAndPerQuality())
                        .Replace("<fortune>", HarvestingFortune.BaseAndPerQuality())

                        .Replace("<manaLine>", Mana.StatSummaryLine())
                        .Replace("<shieldLine>", ShieldingPower.StatSummaryLine())
                        .Replace("<fortuneLine>", HarvestingFortune.StatSummaryLine());
    }
}
