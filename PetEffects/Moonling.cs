using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.PetEffects
{
    public sealed class Moonling : PetEffect
    {
        public List<string> Tooltips
        {
            get
            {
                List<string> tooltips = [Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.MeleeTooltip")
                                            .Replace("<dr>", Math.Round(meleeDr * 100, 2).ToString())
                                            .Replace("<meleeSpd>", Math.Round(meleeSpd * 100, 2).ToString())
                                            .Replace("<meleeDmg>", Math.Round(meleeDmg * 100, 2).ToString())
                                            .Replace("<def>", defense.ToString()),
                    Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.RangedTooltip")
                                            .Replace("<armorPen>", rangedPen.ToString())
                                            .Replace("<rangedCrit>", rangedCr.ToString())
                                            .Replace("<rangedCritDmg>", Math.Round(rangedCrDmg * 100, 2).ToString())
                                            .Replace("<rangedDmg>", Math.Round(rangedDmg * 100, 2).ToString()),
                    Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.MagicTooltip")
                                            .Replace("<mana>", magicMana.ToString())
                                            .Replace("<manaCost>", Math.Round(magicManaCost * 100, 2).ToString())
                                            .Replace("<magicCrit>", magicCrit.ToString())
                                            .Replace("<magicDmg>", Math.Round(magicDmg * 100, 2).ToString()),
                    Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.SummonerTooltip")
                                            .Replace("<sumRange>", Math.Round(sumWhipRng * 100, 2).ToString())
                                            .Replace("<sumSpd>", Math.Round(sumWhipSpd * 100, 2).ToString())
                                            .Replace("<sumMax>", sumMinion.ToString())
                                            .Replace("<sumMaxSentry>", sumSentry.ToString())];
                if (ExternalTooltips.Count != 0)
                tooltips.AddRange(ExternalTooltips);
                return tooltips;
            }
        }
        /// <summary>
        /// Remember to .Add to this List the tooltip of your class somewhere after ResetEffects() and before Pet Effects are ran.
        /// </summary>
        public List<string> ExternalTooltips = new();
        public string CurrentClass = PetTextsColors.PetClassLocalized(PetClasses.None);
        public override int PetItemID => ItemID.MoonLordPetItem;
        public int defense = 10;
        public float meleeDr = 0.1f;
        public float meleeSpd = 0.15f;
        public float meleeDmg = 0.2f;

        public int rangedPen = 15;
        public float rangedDmg = 0.1f;
        public int rangedCr = 10;
        public float rangedCrDmg = 0.1f;

        public int magicMana = 150;
        public float magicDmg = 0.15f;
        public int magicCrit = 10;
        public float magicManaCost = 0.1f;

        public float sumWhipRng = 0.45f;
        public float sumWhipSpd = 0.3f;
        public int sumMinion = 2;
        public int sumSentry = 2;
        public List<StatModifier> Stats
        {
            get
            {
                List<StatModifier> stats = [Player.GetTotalDamage<MeleeDamageClass>(), Player.GetTotalDamage<RangedDamageClass>(), Player.GetTotalDamage<MagicDamageClass>(), Player.GetTotalDamage<SummonDamageClass>()];
                if (ExternalStats.Count != 0)
                stats.AddRange(ExternalStats);
                return stats;
            }
        }
        /// <summary>
        /// Remember to .Add to this List the tooltip of your class somewhere after ResetEffects() and before Pet Effects are ran.
        /// </summary>
        public List<StatModifier> ExternalStats = new();
        public StatModifier HighestDamage => Stats.MaxBy(x => x.Additive);

        public int currentTooltip = 0; //0=Melee 1=Ranged 2=Magic 3=Summoner more is also added as if their tooltip is in Tooltips property

        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override void ResetEffects()
        {
            ExternalStats.Clear();
            ExternalTooltips.Clear();
        }
        public override void PostUpdateMiscEffects()
        {
            if (HighestDamage == Player.GetTotalDamage<MeleeDamageClass>())
            {
                if (PetIsEquipped())
                {
                    Player.endurance += meleeDr;
                    Player.GetAttackSpeed<MeleeDamageClass>() += meleeSpd;
                    Player.GetDamage<MeleeDamageClass>() += meleeDmg;
                    Player.statDefense += defense;
                }
                CurrentClass = PetTextsColors.PetClassLocalized(PetClasses.Melee);
            }
            else if (HighestDamage == Player.GetTotalDamage<RangedDamageClass>())
            {
                if (PetIsEquipped())
                {
                    Player.GetArmorPenetration<RangedDamageClass>() += rangedPen;
                    Player.GetDamage<RangedDamageClass>() += rangedDmg;
                    Player.GetCritChance<RangedDamageClass>() += rangedCr;
                }
                CurrentClass = PetTextsColors.PetClassLocalized(PetClasses.Ranged);
            }
            else if (HighestDamage == Player.GetTotalDamage<MagicDamageClass>())
            {
                if (PetIsEquipped())
                {
                    Player.statManaMax2 += magicMana;
                    Player.GetDamage<MagicDamageClass>() += magicDmg;
                    Player.GetCritChance<MagicDamageClass>() += magicCrit;
                    Player.manaCost -= magicManaCost;
                }
                CurrentClass = PetTextsColors.PetClassLocalized(PetClasses.Magic);
            }
            else if (HighestDamage == Player.GetTotalDamage<SummonDamageClass>())
            {
                if (PetIsEquipped())
                {
                    Player.whipRangeMultiplier += sumWhipRng;
                    Player.GetAttackSpeed<SummonMeleeSpeedDamageClass>() += sumWhipSpd;
                    Player.maxMinions += sumMinion;
                    Player.maxTurrets += sumSentry;
                }
                CurrentClass = PetTextsColors.PetClassLocalized(PetClasses.Summoner);
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (PetKeybinds.PetTooltipSwap.JustPressed)
            {
                currentTooltip++;
                if (currentTooltip >= Tooltips.Count)
                    currentTooltip = 0;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && modifiers.DamageType == DamageClass.Ranged && HighestDamage == Player.GetTotalDamage<RangedDamageClass>())
            {
                modifiers.CritDamage += rangedCrDmg;
            }
        }
        public override void SaveData(TagCompound tag)
        {
            tag.Add("CurrentTooltip", currentTooltip);
        }
        public override void LoadData(TagCompound tag)
        {
            if (tag.TryGet("CurrentTooltip", out int tooltip))
            {
                currentTooltip = tooltip;
            }
        }
    }
    public sealed class MoonLordPetItem : PetTooltip
    {
        public override PetEffect PetsEffect => moonling;
        public static Moonling moonling
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out Moonling pet))
                    return pet;
                else
                    return ModContent.GetInstance<Moonling>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.MoonLordPetItem")
                    .Replace("<currentClass>", moonling.CurrentClass)
                    .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.PetTooltipSwap))
                    .Replace("<tooltip>", moonling.Tooltips[moonling.currentTooltip]);
    }
}
