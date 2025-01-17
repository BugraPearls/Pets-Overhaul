using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class JackOLanternEffect : LightPetEffect
    {
        public override int LightPetItemID => ItemID.PumpkingPetItem;
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out JackOLantern jackOLantern))
            {
                Player.GetAttackSpeed<GenericDamageClass>() += jackOLantern.AttackSpeed.CurrentStatFloat;
                Pet.harvestingFortune += jackOLantern.HarvestingFortune.CurrentStatInt;
            }
        }
        public override void ModifyLuck(ref float luck)
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out JackOLantern jackOLantern))
            {
                luck += jackOLantern.Luck.CurrentStatFloat;
            }

        }
    }
    public sealed class JackOLantern : LightPetItem
    {
        public LightPetStat AttackSpeed = new(30, 0.003f, 0.04f);
        public LightPetStat Luck = new(15, 0.01f, 0.03f);
        public LightPetStat HarvestingFortune = new(20, 1, 10);
        public override int LightPetItemID => ItemID.PumpkingPetItem;
        public override void UpdateInventory(Item item, Player player)
        {
            AttackSpeed.SetRoll(player.luck);
            Luck.SetRoll(player.luck);
            HarvestingFortune.SetRoll(player.luck);
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)AttackSpeed.CurrentRoll);
            writer.Write((byte)HarvestingFortune.CurrentRoll);
            writer.Write((byte)Luck.CurrentRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            AttackSpeed.CurrentRoll = reader.ReadByte();
            HarvestingFortune.CurrentRoll = reader.ReadByte();
            Luck.CurrentRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("PumpkinAtkSpd", AttackSpeed.CurrentRoll);
            tag.Add("PumpkinLuck", Luck.CurrentRoll);
            tag.Add("PumpkinExp", HarvestingFortune.CurrentRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("PumpkinAtkSpd", out int aSpd))
            {
                AttackSpeed.CurrentRoll = aSpd;
            }

            if (tag.TryGet("PumpkinLuck", out int luck))
            {
                Luck.CurrentRoll = luck;
            }

            if (tag.TryGet("PumpkinExp", out int exp))
            {
                HarvestingFortune.CurrentRoll = exp;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.JackOLantern")

                        .Replace("<atkSpd>", AttackSpeed.BaseAndPerQuality())
                        .Replace("<luck>", Luck.BaseAndPerQuality(Luck.StatPerRoll.ToString(), Luck.BaseStat.ToString()))
                        .Replace("<fortune>", HarvestingFortune.BaseAndPerQuality())

                        .Replace("<atkSpdLine>", AttackSpeed.StatSummaryLine())
                        .Replace("<luckLine>", Luck.StatSummaryLine(Math.Round(Luck.CurrentStatFloat, 2).ToString()))
                        .Replace("<fortuneLine>", HarvestingFortune.StatSummaryLine())
                        ));
            if (Luck.CurrentRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", PetTextsColors.RollMissingText()));
            }
        }
    }
}
