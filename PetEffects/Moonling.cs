using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
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
                List<string> tooltips = [PetTextsColors.LocVal("PetItemTooltips.MeleeTooltip")
                                            .Replace("<dr>", Math.Round(meleeDr * 100, 2).ToString())
                                            .Replace("<meleeSpd>", Math.Round(meleeSpd * 100, 2).ToString())
                                            .Replace("<meleeDmg>", Math.Round(meleeDmg * 100, 2).ToString())
                                            .Replace("<def>", defense.ToString()),
                    PetTextsColors.LocVal("PetItemTooltips.RangedTooltip")
                                            .Replace("<armorPen>", rangedPen.ToString())
                                            .Replace("<rangedCrit>", rangedCr.ToString())
                                            .Replace("<rangedCritDmg>", Math.Round(rangedCrDmg * 100, 2).ToString())
                                            .Replace("<rangedDmg>", Math.Round(rangedDmg * 100, 2).ToString()),
                    PetTextsColors.LocVal("PetItemTooltips.MagicTooltip")
                                            .Replace("<mana>", magicMana.ToString())
                                            .Replace("<manaCost>", Math.Round(magicManaCost * 100, 2).ToString())
                                            .Replace("<magicCrit>", magicCrit.ToString())
                                            .Replace("<magicDmg>", Math.Round(magicDmg * 100, 2).ToString()),
                    PetTextsColors.LocVal("PetItemTooltips.SummonerTooltip")
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

        public int currentClass = 0; //0=Melee 1=Ranged 2=Magic 3=Summoner more is also added as if their tooltip is in Tooltips property

        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override void ResetEffects()
        {
            ExternalTooltips.Clear();
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                switch (currentClass)
                {
                    case 0:
                        Player.endurance += meleeDr;
                        Player.GetAttackSpeed<MeleeDamageClass>() += meleeSpd;
                        Player.GetDamage<MeleeDamageClass>() += meleeDmg;
                        Player.statDefense += defense;
                        break;
                    case 1:
                        Player.GetArmorPenetration<RangedDamageClass>() += rangedPen;
                        Player.GetDamage<RangedDamageClass>() += rangedDmg;
                        Player.GetCritChance<RangedDamageClass>() += rangedCr;
                        break;
                    case 2:
                        Player.statManaMax2 += magicMana;
                        Player.GetDamage<MagicDamageClass>() += magicDmg;
                        Player.GetCritChance<MagicDamageClass>() += magicCrit;
                        Player.manaCost -= magicManaCost;
                        break;
                    case 3:
                        Player.whipRangeMultiplier += sumWhipRng;
                        Player.GetAttackSpeed<SummonMeleeSpeedDamageClass>() += sumWhipSpd;
                        Player.maxMinions += sumMinion;
                        Player.maxTurrets += sumSentry;
                        break;
                    default:
                        break;
                }
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (PetKeybinds.PetAbilitySwitch.JustPressed)
            {
                currentClass++;
                if (currentClass >= Tooltips.Count)
                    currentClass = 0;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && modifiers.DamageType == DamageClass.Ranged && currentClass == 1)
            {
                modifiers.CritDamage += rangedCrDmg;
            }
        }
        public override void SaveData(TagCompound tag)
        {
            tag.Add("CurrentTooltip", currentClass);
        }
        public override void LoadData(TagCompound tag)
        {
            if (tag.TryGet("CurrentTooltip", out int tooltip))
            {
                currentClass = tooltip;
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
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.MoonLordPetItem")
                    .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.PetAbilitySwitch))
                    .Replace("<tooltip>", moonling.Tooltips[moonling.currentClass]);
    }
}
