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
        public struct ClassAndItsTooltip(PetClasses petClass, string tooltip)
        {
            public PetClasses Class = petClass;
            public string TooltipOfClass = tooltip;
        }
        public List<ClassAndItsTooltip> Tooltips //Not using Dictionary, because index is required.
        {
            get
            {
                List<ClassAndItsTooltip> tooltips = [new ClassAndItsTooltip(PetClasses.Melee,PetUtils.LocVal("PetItemTooltips.MeleeTooltip")
                                            .Replace("<dr>", Math.Round(meleeDr * 100, 2).ToString())
                                            .Replace("<meleeSpd>", Math.Round(meleeSpd * 100, 2).ToString())
                                            .Replace("<meleeDmg>", Math.Round(meleeDmg * 100, 2).ToString())
                                            .Replace("<def>", defense.ToString())
                                            .Replace("<ms>", Math.Round(msReduce * 100, 2).ToString())),
                    new ClassAndItsTooltip(PetClasses.Ranged,PetUtils.LocVal("PetItemTooltips.RangedTooltip")
                                            .Replace("<armorPen>", rangedPen.ToString())
                                            .Replace("<rangedCrit>", rangedCr.ToString())
                                            .Replace("<rangedCritDmg>", Math.Round(rangedCrDmg * 100, 2).ToString())
                                            .Replace("<rangedDmg>", Math.Round(rangedDmg * 100, 2).ToString())
                                            .Replace("<hp>", Math.Round(hpReduce * 100, 2).ToString())),
                    new ClassAndItsTooltip(PetClasses.Magic,PetUtils.LocVal("PetItemTooltips.MagicTooltip")
                                            .Replace("<mana>", magicMana.ToString())
                                            .Replace("<manaCost>", Math.Round(magicManaCost * 100, 2).ToString())
                                            .Replace("<magicCrit>", magicCrit.ToString())
                                            .Replace("<magicDmg>", Math.Round(magicDmg * 100, 2).ToString())
                                            .Replace("<hp>", Math.Round(hpReduce * 100, 2).ToString())),
                    new ClassAndItsTooltip(PetClasses.Summoner,PetUtils.LocVal("PetItemTooltips.SummonerTooltip")
                                            .Replace("<sumRange>", Math.Round(sumWhipRng * 100, 2).ToString())
                                            .Replace("<sumSpd>", Math.Round(sumWhipSpd * 100, 2).ToString())
                                            .Replace("<sumMax>", sumMinion.ToString())
                                            .Replace("<sumMaxSentry>", sumSentry.ToString())
                                            .Replace("<hp>", Math.Round(hpReduce * 100, 2).ToString()))];
                if (ExternalTooltips.Count != 0)
                    tooltips.AddRange(ExternalTooltips);
                return tooltips;
            }
        }
        /// <summary>
        /// Remember to .Add to this List the tooltip of your class somewhere after ResetEffects() and before Pet Effects are ran.
        /// </summary>
        public List<ClassAndItsTooltip> ExternalTooltips = [];
        public override int PetItemID => ItemID.MoonLordPetItem;
        public float msReduce = 0.15f;
        public float hpReduce = 0.12f;

        public int defense = 10;
        public float meleeDr = 0.1f;
        public float meleeSpd = 0.15f;
        public float meleeDmg = 0.15f;

        public int rangedPen = 15;
        public float rangedDmg = 0.1f;
        public int rangedCr = 10;
        public float rangedCrDmg = 0.05f;

        public int magicMana = 80;
        public float magicDmg = 0.15f;
        public int magicCrit = 10;
        public float magicManaCost = 0.1f;

        public float sumWhipRng = 0.25f;
        public float sumWhipSpd = 0.05f;
        public int sumMinion = 2;
        public int sumSentry = 2;

        public int currentClass = 0; //0=Melee 1=Ranged 2=Magic 3=Summoner more is also added as if their tooltip is in Tooltips property

        public override string PetStackText => PetUtils.LocVal("PetItemTooltips.MoonLordPetItemStack");
        public override string PetStackSpecial => $"[c/{PetUtils.ClassEnumToColor(Tooltips[currentClass].Class).Hex3()}:{PetUtils.PetClassLocalized(Tooltips[currentClass].Class)}]";
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
                        Player.moveSpeed -= msReduce;
                        break;
                    case 1:
                        Player.GetArmorPenetration<RangedDamageClass>() += rangedPen;
                        Player.GetDamage<RangedDamageClass>() += rangedDmg;
                        Player.GetCritChance<RangedDamageClass>() += rangedCr;
                        Player.statLifeMax2 -= (int)(Player.statLifeMax2 * hpReduce);
                        break;
                    case 2:
                        Player.statManaMax2 += magicMana;
                        Player.GetDamage<MagicDamageClass>() += magicDmg;
                        Player.GetCritChance<MagicDamageClass>() += magicCrit;
                        Player.manaCost -= magicManaCost;
                        Player.statLifeMax2 -= (int)(Player.statLifeMax2 * hpReduce);
                        break;
                    case 3:
                        Player.whipRangeMultiplier += sumWhipRng;
                        Player.GetAttackSpeed<SummonMeleeSpeedDamageClass>() += sumWhipSpd;
                        Player.maxMinions += sumMinion;
                        Player.maxTurrets += sumSentry;
                        Player.statLifeMax2 -= (int)(Player.statLifeMax2 * hpReduce);
                        break;
                    default:
                        break;
                }
            }
        }
        public override void ExtraProcessTriggers(TriggersSet triggersSet)
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
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.MoonLordPetItem")
                    .Replace("<switchKeybind>", PetUtils.KeybindText(PetKeybinds.PetAbilitySwitch))
                    .Replace("<tooltip>", moonling.Tooltips[moonling.currentClass].TooltipOfClass);
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.MoonLordPetItem").Replace("<switchKeybind>", PetUtils.KeybindText(PetKeybinds.PetAbilitySwitch));
    }
}
