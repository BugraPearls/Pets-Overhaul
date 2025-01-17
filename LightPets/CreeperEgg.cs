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
    public sealed class CreeperEggEffect : LightPetEffect
    {
        public override int LightPetItemID => ItemID.DD2PetGhost;
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out CreeperEgg creeperEgg))
            {
                Player.GetDamage<SummonDamageClass>() += creeperEgg.SummonDamage.CurrentStatFloat;
                Player.GetDamage<MeleeDamageClass>() += creeperEgg.MeleeDamage.CurrentStatFloat;
                Player.GetAttackSpeed<MeleeDamageClass>() += creeperEgg.AttackSpeed.CurrentStatFloat;
            }
        }
    }
    public sealed class CreeperEgg : LightPetItem
    {
        public override int LightPetItemID => ItemID.DD2PetGhost;
        public LightPetStat SummonDamage = new(16, 0.004f, 0.04f);
        public LightPetStat MeleeDamage = new(16, 0.004f, 0.04f);
        public LightPetStat AttackSpeed = new(20, 0.004f, 0.025f);
        public override void UpdateInventory(Item item, Player player)
        {
            SummonDamage.SetRoll(player.luck);
            MeleeDamage.SetRoll(player.luck);
            AttackSpeed.SetRoll(player.luck);
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)SummonDamage.CurrentRoll);
            writer.Write((byte)MeleeDamage.CurrentRoll);
            writer.Write((byte)AttackSpeed.CurrentRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            SummonDamage.CurrentRoll = reader.ReadByte();
            MeleeDamage.CurrentRoll = reader.ReadByte();
            AttackSpeed.CurrentRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("FlickerwickSum", SummonDamage.CurrentRoll);
            tag.Add("FlickerwickMelee", MeleeDamage.CurrentRoll);
            tag.Add("FlickerwickAtkSpd", AttackSpeed.CurrentRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("FlickerwickSum", out int sum))
            {
                SummonDamage.CurrentRoll = sum;
            }

            if (tag.TryGet("FlickerwickMelee", out int melee))
            {
                MeleeDamage.CurrentRoll = melee;
            }

            if (tag.TryGet("FlickerwickAtkSpd", out int aSpd))
            {
                AttackSpeed.CurrentRoll = aSpd;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.CreeperEgg")

                        .Replace("<sum>", SummonDamage.BaseAndPerQuality())
                        .Replace("<melee>", MeleeDamage.BaseAndPerQuality())
                        .Replace("<atkSpd>", AttackSpeed.BaseAndPerQuality())

                        .Replace("<sumLine>", SummonDamage.StatSummaryLine())
                        .Replace("<meleeLine>", MeleeDamage.StatSummaryLine())
                        .Replace("<atkSpdLine>", AttackSpeed.StatSummaryLine())
                        ));
            if (AttackSpeed.CurrentRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", PetTextsColors.RollMissingText()));
            }
        }
    }
}
