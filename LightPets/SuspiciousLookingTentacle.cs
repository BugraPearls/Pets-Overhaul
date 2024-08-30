﻿using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class SuspiciousLookingTentacleEffect : LightPetEffect
    {
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out SuspiciousLookingTentacle moonlord))
            {
                Player.statDefense += moonlord.Defense.CurrentStatInt;
                Player.moveSpeed += moonlord.MovementSpeed.CurrentStatFloat;
                Player.GetDamage<GenericDamageClass>() += moonlord.DamageAll.CurrentStatFloat;
                Player.GetCritChance<GenericDamageClass>() += moonlord.CritChanceAll.CurrentStatFloat*100;
                Player.whipRangeMultiplier += moonlord.WhipRange.CurrentStatFloat;
                Player.statManaMax2 += moonlord.Mana.CurrentStatInt;

            }
        }
        public override void GetHealMana(Item item, bool quickHeal, ref int healValue)
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out SuspiciousLookingTentacle moonlord))
            {
                healValue += (int)(moonlord.ManaPotionIncrease.CurrentStatFloat * healValue);
            }
        }
        public override void ModifyItemScale(Item item, ref float scale)
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out SuspiciousLookingTentacle moonlord))
            {
                scale += moonlord.MeleeSize.CurrentStatFloat;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out SuspiciousLookingTentacle moonlord))
            {
                if (modifiers.DamageType == DamageClass.Ranged)
                {
                    modifiers.ScalingArmorPenetration += moonlord.RangedPercentPenetration.CurrentStatFloat;
                    modifiers.CritDamage += moonlord.RangedCritDamage.CurrentStatFloat;
                }
                if (modifiers.DamageType == DamageClass.Summon)
                {
                    modifiers.ArmorPenetration += moonlord.SummonerFlatPenetration.CurrentStatInt;
                }
                if (modifiers.DamageType == DamageClass.Melee && GlobalPet.LifestealCheck(target))
                {
                    Pet.PetRecovery(Player.statDefense * 0.1f, moonlord.MeleeLifesteal.CurrentStatFloat);
                }
            }
        }
    }
    public sealed class SuspiciousLookingTentacle : GlobalItem
    {
        public LightPetStat Defense = new(5,1);
        public LightPetStat MovementSpeed  = new(20,0.004f);
        public LightPetStat DamageAll = new(20,0.0025f);
        public LightPetStat CritChanceAll = new(20,0.0025f);
        public LightPetStat RangedPercentPenetration = new(5,0.025f);
        public LightPetStat RangedCritDamage = new(5,0.008f);
        public LightPetStat SummonerFlatPenetration = new(5, 3);
        public LightPetStat WhipRange = new(5,0.03f);
        public LightPetStat ManaPotionIncrease = new(5, 0.05f);
        public LightPetStat Mana = new(5, 12);
        public LightPetStat MeleeSize = new(5,0.04f);
        public LightPetStat MeleeLifesteal = new(5,0.03f);
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.SuspiciousLookingTentacle;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            Defense.SetRoll();
            MovementSpeed.SetRoll();
            DamageAll.SetRoll();
            CritChanceAll.SetRoll();
            RangedPercentPenetration.SetRoll();
            RangedCritDamage.SetRoll();
            SummonerFlatPenetration.SetRoll();
            WhipRange.SetRoll();
            ManaPotionIncrease.SetRoll();
            Mana.SetRoll();
            MeleeSize.SetRoll();
            MeleeLifesteal.SetRoll();
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)RangedCritDamage.CurrentRoll);
            writer.Write((byte)CritChanceAll.CurrentRoll);
            writer.Write((byte)Defense.CurrentRoll);
            writer.Write((byte)DamageAll.CurrentRoll);
            writer.Write((byte)MeleeLifesteal.CurrentRoll);
            writer.Write((byte)Mana.CurrentRoll);
            writer.Write((byte)SummonerFlatPenetration.CurrentRoll);
            writer.Write((byte)MovementSpeed.CurrentRoll);
            writer.Write((byte)RangedPercentPenetration.CurrentRoll);
            writer.Write((byte)ManaPotionIncrease.CurrentRoll);
            writer.Write((byte)MeleeSize.CurrentRoll);
            writer.Write((byte)WhipRange.CurrentRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            RangedCritDamage.CurrentRoll = reader.ReadByte();
            CritChanceAll.CurrentRoll = reader.ReadByte();
            Defense.CurrentRoll = reader.ReadByte();
            DamageAll.CurrentRoll = reader.ReadByte();
            MeleeLifesteal.CurrentRoll = reader.ReadByte();
            Mana.CurrentRoll = reader.ReadByte();
            SummonerFlatPenetration.CurrentRoll = reader.ReadByte();
            MovementSpeed.CurrentRoll = reader.ReadByte();
            RangedPercentPenetration.CurrentRoll = reader.ReadByte();
            ManaPotionIncrease.CurrentRoll = reader.ReadByte();
            MeleeSize.CurrentRoll = reader.ReadByte();
            WhipRange.CurrentRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("MlCrDmg", RangedCritDamage.CurrentRoll);
            tag.Add("MlCrit", CritChanceAll.CurrentRoll);
            tag.Add("MlDef", Defense.CurrentRoll);
            tag.Add("MlDmg", DamageAll.CurrentRoll);
            tag.Add("MlHeal", MeleeLifesteal.CurrentRoll);
            tag.Add("MlMana", Mana.CurrentRoll);
            tag.Add("MlMin", SummonerFlatPenetration.CurrentRoll);
            tag.Add("MlMs", MovementSpeed.CurrentRoll);
            tag.Add("MlPen", RangedPercentPenetration.CurrentRoll);
            tag.Add("MlPot", ManaPotionIncrease.CurrentRoll);
            tag.Add("MlSize", MeleeSize.CurrentRoll);
            tag.Add("MlWhip", WhipRange.CurrentRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("MlCrDmg", out int crDmg))
            {
                RangedCritDamage.CurrentRoll = crDmg;
            }

            if (tag.TryGet("MlCrit", out int crChance))
            {
                CritChanceAll.CurrentRoll = crChance;
            }

            if (tag.TryGet("MlDef", out int def))
            {
                Defense.CurrentRoll = def;
            }

            if (tag.TryGet("MlDmg", out int dmg))
            {
                DamageAll.CurrentRoll = dmg;
            }

            if (tag.TryGet("MlHeal", out int heal))
            {
                MeleeLifesteal.CurrentRoll = heal;
            }

            if (tag.TryGet("MlMana", out int mana))
            {
                Mana.CurrentRoll = mana;
            }

            if (tag.TryGet("MlMin", out int sumPen))
            {
                SummonerFlatPenetration.CurrentRoll = sumPen;
            }

            if (tag.TryGet("MlMs", out int moveSpd))
            {
                MovementSpeed.CurrentRoll = moveSpd;
            }

            if (tag.TryGet("MlPen", out int pen))
            {
                RangedPercentPenetration.CurrentRoll = pen;
            }

            if (tag.TryGet("MlPot", out int pot))
            {
                ManaPotionIncrease.CurrentRoll = pot;
            }

            if (tag.TryGet("MlSize", out int size))
            {
                MeleeSize.CurrentRoll = size;
            }

            if (tag.TryGet("MlWhip", out int whip))
            {
                WhipRange.CurrentRoll = whip;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.SuspiciousLookingTentacle")

                        .Replace("<def>", Defense.BaseAndPerQuality())
                        .Replace("<defLine>", Defense.StatSummaryLine())

                        .Replace("<ms>", MovementSpeed.BaseAndPerQuality())
                        .Replace("<msLine>", MovementSpeed.StatSummaryLine())

                        .Replace("<dmg>", DamageAll.BaseAndPerQuality())
                        .Replace("<dmgLine>", DamageAll.StatSummaryLine())

                        .Replace("<crit>", CritChanceAll.BaseAndPerQuality())
                        .Replace("<critLine>", CritChanceAll.StatSummaryLine())

                        .Replace("<rangedPen>", RangedPercentPenetration.BaseAndPerQuality())
                        .Replace("<rangedPenLine>", RangedPercentPenetration.StatSummaryLine())

                        .Replace("<rangedCritDmg>", RangedCritDamage.BaseAndPerQuality())
                        .Replace("<rangedCritDmgLine>", RangedCritDamage.StatSummaryLine())

                        .Replace("<summonerPen>", SummonerFlatPenetration.BaseAndPerQuality())
                        .Replace("<summonerPenLine>", SummonerFlatPenetration.StatSummaryLine())

                        .Replace("<whip>", WhipRange.BaseAndPerQuality())
                        .Replace("<whipLine>", WhipRange.StatSummaryLine())

                        .Replace("<manaPotion>", ManaPotionIncrease.BaseAndPerQuality())
                        .Replace("<manaPotionLine>", ManaPotionIncrease.StatSummaryLine())

                        .Replace("<mana>", Mana.BaseAndPerQuality())
                        .Replace("<manaLine>", Mana.StatSummaryLine())

                        .Replace("<size>", MeleeSize.BaseAndPerQuality())
                        .Replace("<sizeLine>", MeleeSize.StatSummaryLine())

                        .Replace("<lifesteal>", MeleeLifesteal.BaseAndPerQuality())
                        .Replace("<lifestealLine>", MeleeLifesteal.StatSummaryLine())
                        .Replace("<healAmount>",(Main.LocalPlayer.statDefense * 0.1f).ToString())
                        ));
            if (CritChanceAll.CurrentRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", PetTextsColors.RollMissingText()));
            }
        }
    }
}
