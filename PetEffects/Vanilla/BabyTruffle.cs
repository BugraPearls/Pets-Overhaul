﻿using Terraria;
using Terraria.ID;
using PetsOverhaul.Systems;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.GameInput;
using PetsOverhaul.Config;

using PetsOverhaul.Config;
using Terraria.GameInput;

namespace PetsOverhaul.PetEffects.Vanilla
{
    sealed public class BabyTruffle : ModPlayer
    {
        GlobalPet Pet { get => Player.GetModPlayer<GlobalPet>(); }
        public float increaseFloat = 0.04f;
        public int increaseInt = 4;
        public float moveSpd = 0.2f;
        public int shroomPotionCd = 60;
        public int buffIncrease = 30;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.StrangeGlowingMushroom))
                Pet.timerMax = shroomPotionCd;

        }
        public override void ModifyLuck(ref float luck)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.StrangeGlowingMushroom))
                luck += increaseFloat;
        }
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.StrangeGlowingMushroom))
            {
                Player.buffImmune[BuffID.Confused] = false;
                Player.AddBuff(BuffID.Confused, 1);
                Player.GetAttackSpeed<GenericDamageClass>() += increaseFloat;
                Player.GetDamage<GenericDamageClass>() += increaseFloat;
                Player.GetCritChance<GenericDamageClass>() += increaseInt;
                Player.statManaMax2 += increaseInt;
                Player.statLifeMax2 += increaseInt;
                Player.statDefense += increaseInt;
                Player.GetArmorPenetration<GenericDamageClass>() += increaseInt;
                Player.GetKnockback<GenericDamageClass>() += increaseFloat;
                Player.manaCost -= increaseFloat;
                Player.manaRegenBonus += increaseInt;
                Player.jumpSpeedBoost += increaseFloat;
                Player.moveSpeed += moveSpd;
                Player.wingTimeMax += increaseInt;
                Player.nightVision = true;
                Player.endurance += increaseFloat;
                Player.fishingSkill += increaseInt;
                Pet.abilityHaste += increaseFloat;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.StrangeGlowingMushroom))
                modifiers.CritDamage += increaseFloat;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.StrangeGlowingMushroom) && Pet.timer <= 0)
            {
                for (int i = 0; i < Player.MaxBuffs; i++)
                {
                    if (Main.debuff[Player.buffType[i]] == false && Main.buffNoSave[Player.buffType[i]] == false)
                    {
                        Player.buffTime[i] += buffIncrease;
                    }
                }
                Pet.timer = Pet.timerMax;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.StrangeGlowingMushroom))
                modifiers.Knockback *= 1f - increaseFloat;
        }
    }
    sealed public class StrangeGlowingMushroom : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.StrangeGlowingMushroom;

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down]) return;
            BabyTruffle babyTruffle = Main.LocalPlayer.GetModPlayer<BabyTruffle>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.StrangeGlowingMushroom")
                .Replace("<buffRecover>", (babyTruffle.buffIncrease / 60f).ToString())
                .Replace("<cooldown>", (babyTruffle.shroomPotionCd / 60f).ToString())
                .Replace("<floatIncr>", (babyTruffle.increaseFloat * 100).ToString())
                .Replace("<intIncr>", babyTruffle.increaseInt.ToString())
                .Replace("<moveSpd>", (babyTruffle.moveSpd * 100).ToString())
            ));
        }
    }
}
