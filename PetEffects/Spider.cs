﻿using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Spider : PetEffect
    {
        public int venomFlatDmg = 13;
        public int poisonFlatDmg = 5;
        public float venomDmgMult = 1.16f;
        public float poisonDmgMult = 1.08f;
        public int venomCrit = 10;
        public int poisonCrit = 6;
        public float kbIncreaseVenom = 1.6f;
        public float kbIncreasePoison = 1.25f;
        public int poisonTime = 600;

        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.SpiderEgg))
            {
                if (target.HasBuff(BuffID.Venom))
                {
                    modifiers.FinalDamage *= venomDmgMult;
                    modifiers.FlatBonusDamage += venomFlatDmg;
                    modifiers.Knockback *= kbIncreaseVenom;
                    if (item.crit + Player.GetTotalCritChance(modifiers.DamageType) + venomCrit >= 100)
                    {
                        modifiers.SetCrit();
                    }
                    else if (Main.rand.NextBool(item.crit + (int)Player.GetTotalCritChance(modifiers.DamageType) + venomCrit, 100))
                    {
                        modifiers.SetCrit();
                    }
                    else
                    {
                        modifiers.DisableCrit();
                    }
                }
                else if (target.HasBuff(BuffID.Poisoned))
                {
                    modifiers.FinalDamage *= poisonDmgMult;
                    modifiers.FlatBonusDamage += poisonFlatDmg;
                    modifiers.Knockback *= kbIncreasePoison;
                    if (item.crit + Player.GetTotalCritChance(modifiers.DamageType) + poisonCrit >= 100)
                    {
                        modifiers.SetCrit();
                    }
                    else if (Main.rand.NextBool(item.crit + (int)Player.GetTotalCritChance(modifiers.DamageType) + poisonCrit, 100))
                    {
                        modifiers.SetCrit();
                    }
                    else
                    {
                        modifiers.DisableCrit();
                    }
                }
                target.AddBuff(BuffID.Poisoned, poisonTime);
                if (target.buffImmune[BuffID.Venom] == false)
                {
                    target.buffImmune[BuffID.Poisoned] = false;
                }
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.SpiderEgg))
            {
                if (target.HasBuff(BuffID.Venom))
                {
                    modifiers.FinalDamage *= venomDmgMult;
                    modifiers.FlatBonusDamage += venomFlatDmg;
                    modifiers.Knockback *= kbIncreaseVenom;
                    if (proj.CritChance + venomCrit >= 100)
                    {
                        modifiers.SetCrit();
                    }
                    else if (Main.rand.NextBool(proj.CritChance + venomCrit, 100))
                    {
                        modifiers.SetCrit();
                    }
                    else
                    {
                        modifiers.DisableCrit();
                    }
                }
                else if (target.HasBuff(BuffID.Poisoned))
                {
                    modifiers.FinalDamage *= poisonDmgMult;
                    modifiers.FlatBonusDamage += poisonFlatDmg;
                    modifiers.Knockback *= kbIncreasePoison;
                    if (proj.CritChance + poisonCrit >= 100)
                    {
                        modifiers.SetCrit();
                    }
                    else if (Main.rand.NextBool(proj.CritChance + poisonCrit, 100))
                    {
                        modifiers.SetCrit();
                    }
                    else
                    {
                        modifiers.DisableCrit();
                    }
                }
                target.AddBuff(BuffID.Poisoned, poisonTime);
                if (target.buffImmune[BuffID.Venom] == false)
                {
                    target.buffImmune[BuffID.Poisoned] = false;
                }
            }
        }
    }
    public sealed class SpiderEgg : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.SpiderEgg;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            Spider spider = Main.LocalPlayer.GetModPlayer<Spider>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.SpiderEgg")
                .Replace("<class>", PetTextsColors.ClassText(spider.PetClassPrimary, spider.PetClassSecondary))
                        .Replace("<poisonTime>", Math.Round(spider.poisonTime / 60f, 2).ToString())
                        .Replace("<poiPerc>", spider.poisonDmgMult.ToString())
                        .Replace("<poiFlat>", spider.poisonFlatDmg.ToString())
                        .Replace("<poiKb>", spider.kbIncreasePoison.ToString())
                        .Replace("<poiCrit>", spider.poisonCrit.ToString())
                        .Replace("<venomPerc>", spider.venomDmgMult.ToString())
                        .Replace("<venomFlat>", spider.venomFlatDmg.ToString())
                        .Replace("<venomKb>", spider.kbIncreaseVenom.ToString())
                        .Replace("<venomCrit>", spider.venomCrit.ToString())
                        ));
        }
    }
}
