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
    public sealed class FairyBellEffect : LightPetEffect
    {
        public override int LightPetItemID => ItemID.FairyBell;
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out FairyBell fairyBell))
            {
                Pet.abilityHaste += fairyBell.AbilityHaste.CurrentStatFloat;
                Pet.globalFortune += fairyBell.GlobalFortune.CurrentStatInt;
            }
        }
    }
    public sealed class FairyBell : LightPetItem
    {
        public LightPetStat AbilityHaste = new(15, 0.012f, 0.1f);
        public LightPetStat GlobalFortune = new(20, 1, 5);
        public override int LightPetItemID => ItemID.FairyBell;
        public override void UpdateInventory(Item item, Player player)
        {
            AbilityHaste.SetRoll(player.luck);
            GlobalFortune.SetRoll(player.luck);
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)AbilityHaste.CurrentRoll);
            writer.Write((byte)GlobalFortune.CurrentRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            AbilityHaste.CurrentRoll = reader.ReadByte();
            GlobalFortune.CurrentRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("FairyHaste", AbilityHaste.CurrentRoll);
            tag.Add("FairyFort", GlobalFortune.CurrentRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("FairyHaste", out int haste))
            {
                AbilityHaste.CurrentRoll = haste;
            }

            if (tag.TryGet("FairyFort", out int fort))
            {
                GlobalFortune.CurrentRoll = fort;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.FairyBell")

                        .Replace("<haste>", AbilityHaste.BaseAndPerQuality())
                        .Replace("<fortune>", GlobalFortune.BaseAndPerQuality())

                        .Replace("<hasteLine>", AbilityHaste.StatSummaryLine())
                        .Replace("<fortuneLine>", GlobalFortune.StatSummaryLine())
                        ));
            if (GlobalFortune.CurrentRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", PetTextsColors.RollMissingText()));
            }
        }
    }
}
