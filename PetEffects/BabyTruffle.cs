using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class BabyTruffle : PetEffect
    {
        public override int PetItemID => ItemID.StrangeGlowingMushroom;
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public float increaseFloat = 0.04f;
        public int increaseInt = 4;
        public int shroomPotionCd = 60;
        public int buffIncrease = 30;
        public override int PetAbilityCooldown => shroomPotionCd;
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.buffImmune[BuffID.Confused] = false;
                Player.AddBuff(BuffID.Confused, 1);
                Player.nightVision = true;

                #region The wall of Stats xD
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
                Player.jumpSpeedBoost += Player.jumpSpeed * increaseFloat;
                Player.moveSpeed += increaseFloat;
                Player.wingTimeMax += 24; //0.4 second
                Player.endurance += increaseFloat;
                Player.fishingSkill += increaseInt;
                Player.aggro += increaseInt;
                Player.extraFall += increaseInt;
                Player.breathEffectiveness += increaseFloat;
                Player.pickSpeed -= Player.pickSpeed * increaseFloat;
                Player.lavaMax += 24; //0.4 second (Also, this being set low causes issues, game attempts to draw the lava immunity bar, but crashes (freeze) if it cannot divide the value properly to frames.
                Player.tileSpeed += increaseFloat;
                Player.wallSpeed += increaseFloat;
                Pet.abilityHaste += increaseFloat;
                Pet.fishingFortune += increaseInt;
                Pet.globalFortune += increaseInt;
                Pet.harvestingFortune += increaseInt;
                Pet.miningFortune += increaseInt;
                Pet.petShieldMultiplier += increaseFloat;
                Pet.petHealMultiplier += increaseFloat;
                Pet.petDirectDamageMultiplier += increaseFloat;
                #endregion
            }
        }
        public override void ModifyLuck(ref float luck)
        {
            if (PetIsEquipped())
            {
                luck += increaseFloat;
            }
        }
        public override void ModifyItemScale(Item item, ref float scale)
        {
            if (PetIsEquipped())
            {
                scale += increaseFloat;
            }
        }
        public override void UpdateLifeRegen()
        {
            if (PetIsEquipped())
            {
                Player.lifeRegen += increaseInt;
                Player.lifeRegenTime += increaseFloat; //for reference; Regular time is +60 per second, this makes it +62.4 per second.
            }
        }
        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            if (PetIsEquipped() && Main.rand.NextBool(increaseInt, 100))
            {
                return false;
            }
            return base.CanConsumeAmmo(weapon, ammo);
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped())
            {
                modifiers.CritDamage += increaseFloat;
                modifiers.ScalingArmorPenetration += increaseFloat;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && Pet.timer <= 0)
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
            if (PetIsEquipped())
            {
                modifiers.Knockback *= 1f - increaseFloat;
            }
        }
    }
    public sealed class StrangeGlowingMushroom : PetTooltip
    {
        public override PetEffect PetsEffect => babyTruffle;
        public static BabyTruffle babyTruffle
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out BabyTruffle pet))
                    return pet;
                else
                    return ModContent.GetInstance<BabyTruffle>();
            }
        }
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.StrangeGlowingMushroom")
                .Replace("<buffRecover>", Math.Round(babyTruffle.buffIncrease / 60f, 2).ToString())
                .Replace("<cooldown>", Math.Round(babyTruffle.shroomPotionCd / 60f, 2).ToString())
                .Replace("<intIncr>", babyTruffle.increaseInt.ToString());
        public override string SimpleTooltip => PetTextsColors.LocVal("SimpleTooltips.StrangeGlowingMushroom");
    }
}
