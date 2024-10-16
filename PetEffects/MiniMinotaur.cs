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
    public sealed class MiniMinotaur : PetEffect
    {
        public int minotaurStack = 0;
        public int minotaurCd = 12;
        public int oocTimer = 600;
        public int maxStack = 80;
        public float meleeSpd = 0.0023f;
        public float meleeDmg = 0.00125f;
        public float moveSpd = 0.0025f;
        public float defMult = 0.0023f;

        public override PetClasses PetClassPrimary => PetClasses.Melee;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.TartarSauce))
            {
                Pet.SetPetAbilityTimer(minotaurCd);
                oocTimer--;
                if (oocTimer <= 0 && minotaurStack > 0)
                {
                    oocTimer = 15;
                    minotaurStack--;
                }
                if (minotaurStack > maxStack)
                {
                    minotaurStack = maxStack;
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.TartarSauce) && minotaurStack > 0)
            {
                Player.GetAttackSpeed<MeleeDamageClass>() += meleeSpd * minotaurStack;
                Player.statDefense.FinalMultiplier *= 1 + defMult * minotaurStack;
                Player.GetDamage<MeleeDamageClass>() += meleeDmg * minotaurStack;
                Player.moveSpeed -= moveSpd * minotaurStack;
            }
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.TartarSauce) && GlobalPet.LifestealCheck(target) && Pet.timer <= 0 && item.CountsAsClass<MeleeDamageClass>())
            {
                if (minotaurStack < 80)
                {
                    minotaurStack += 2;
                }
                if (minotaurStack > 80)
                {
                    minotaurStack = 80;
                }
                Pet.timer = Pet.timerMax;
                oocTimer = 600;
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.TartarSauce) && GlobalPet.LifestealCheck(target) && Pet.timer <= 0 && proj.CountsAsClass<MeleeDamageClass>())
            {
                if (minotaurStack < 80)
                {
                    minotaurStack += 1;
                }
                if (minotaurStack > 80)
                {
                    minotaurStack = 80;
                }
                Pet.timer = Pet.timerMax;
                oocTimer = 600;
            }
        }
        public override void UpdateDead()
        {
            minotaurStack = 0;
        }
    }
    public sealed class TartarSauce : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.TartarSauce;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            MiniMinotaur miniMinotaur = Main.LocalPlayer.GetModPlayer<MiniMinotaur>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.TartarSauce")
                .Replace("<class>", PetTextsColors.ClassText(miniMinotaur.PetClassPrimary, miniMinotaur.PetClassSecondary))
                        .Replace("<cooldown>", Math.Round(miniMinotaur.minotaurCd / 60f, 2).ToString())
                        .Replace("<maxStack>", miniMinotaur.maxStack.ToString())
                        .Replace("<maxDef>", Math.Round(miniMinotaur.defMult * 100 * miniMinotaur.maxStack, 2).ToString())
                        .Replace("<maxMeleeSpd>", Math.Round(miniMinotaur.meleeSpd * 100 * miniMinotaur.maxStack, 2).ToString())
                        .Replace("<maxDmg>", Math.Round(miniMinotaur.meleeDmg * 100 * miniMinotaur.maxStack, 2).ToString())
                        .Replace("<maxSpd>", Math.Round(miniMinotaur.moveSpd * 100 * miniMinotaur.maxStack, 2).ToString())
                        .Replace("<meleeSpd>", Math.Round(miniMinotaur.meleeSpd * 100, 2).ToString())
                        .Replace("<moveSpd>", Math.Round(miniMinotaur.moveSpd * 100, 2).ToString())
                        .Replace("<dmg>", Math.Round(miniMinotaur.meleeDmg * 100, 2).ToString())
                        .Replace("<def>", Math.Round(miniMinotaur.defMult * 100, 2).ToString())
                        ));
        }
    }
}
