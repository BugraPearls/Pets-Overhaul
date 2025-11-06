using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class MiniMinotaur : PetEffect
    {
        public override int PetItemID => ItemID.TartarSauce;
        public int minotaurStack = 0;
        public int minotaurCd = 12;
        public int oocMaxDuration = 20;
        public int maxStack = 80;
        public float meleeSpd = 0.0023f;
        public float meleeDmg = 0.00125f;
        public float moveSpd = 0.0018f;
        public float defMult = 0.0012f;

        public override PetClasses PetClassPrimary => PetClasses.Melee;
        public override int PetStackCurrent => minotaurStack;
        public override int PetStackMax => maxStack;
        public override string PetStackText => PetUtils.LocVal("PetItemTooltips.TartarSauceStack");
        public override int PetAbilityCooldown => minotaurCd;
        public override void ExtraPreUpdate()
        {
            if (Pet.inCombatTimer <= 0 && minotaurStack > 0)
            {
                Pet.inCombatTimer = oocMaxDuration;
                minotaurStack--;
            }
            if (minotaurStack > maxStack)
            {
                minotaurStack = maxStack;
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped() && minotaurStack > 0)
            {
                Player.GetAttackSpeed<MeleeDamageClass>() += meleeSpd * minotaurStack;
                Player.statDefense.FinalMultiplier *= 1 + defMult * minotaurStack;
                Player.GetDamage<MeleeDamageClass>() += meleeDmg * minotaurStack;
                Player.moveSpeed -= moveSpd * minotaurStack;
            }
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && PetUtils.LifestealCheck(target) && Pet.timer <= 0 && item.CountsAsClass<MeleeDamageClass>())
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
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && PetUtils.LifestealCheck(target) && Pet.timer <= 0 && proj.CountsAsClass<MeleeDamageClass>())
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
            }
        }
        public override void UpdateDead()
        {
            minotaurStack = 0;
        }
    }
    public sealed class TartarSauce : PetTooltip
    {
        public override PetEffect PetsEffect => miniMinotaur;
        public static MiniMinotaur miniMinotaur
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out MiniMinotaur pet))
                    return pet;
                else
                    return ModContent.GetInstance<MiniMinotaur>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.TartarSauce")
                        .Replace("<cooldown>", Math.Round(miniMinotaur.minotaurCd / 60f, 2).ToString())
                        .Replace("<maxStack>", miniMinotaur.maxStack.ToString())
                        .Replace("<oocTimer>", Math.Round(miniMinotaur.oocMaxDuration / 60f, 2).ToString())
                        .Replace("<maxDef>", Math.Round(miniMinotaur.defMult * 100 * miniMinotaur.maxStack, 2).ToString())
                        .Replace("<maxMeleeSpd>", Math.Round(miniMinotaur.meleeSpd * 100 * miniMinotaur.maxStack, 2).ToString())
                        .Replace("<maxDmg>", Math.Round(miniMinotaur.meleeDmg * 100 * miniMinotaur.maxStack, 2).ToString())
                        .Replace("<maxSpd>", Math.Round(miniMinotaur.moveSpd * 100 * miniMinotaur.maxStack, 2).ToString())
                        .Replace("<meleeSpd>", Math.Round(miniMinotaur.meleeSpd * 100, 3).ToString())
                        .Replace("<moveSpd>", Math.Round(miniMinotaur.moveSpd * 100, 3).ToString())
                        .Replace("<dmg>", Math.Round(miniMinotaur.meleeDmg * 100, 3).ToString())
                        .Replace("<def>", Math.Round(miniMinotaur.defMult * 100, 3).ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.TartarSauce");
    }
}
