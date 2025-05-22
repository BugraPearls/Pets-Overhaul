using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class TikiSpirit : PetEffect
    {
        public override int PetItemID => ItemID.TikiTotem;
        public int whipCritBonus = 13;
        public int nonWhipCrit = 8; //Negative
        public float atkSpdToDmgConversion = 0.30f;
        public float atkSpdToRangeConversion = 0.15f;

        public override PetClasses PetClassPrimary => PetClasses.Summoner;
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                if (Player.GetTotalAttackSpeed<SummonMeleeSpeedDamageClass>() > Player.GetAttackSpeed<SummonMeleeSpeedDamageClass>())
                {
                    Player.GetDamage<SummonDamageClass>() += (Player.GetTotalAttackSpeed<SummonMeleeSpeedDamageClass>() - Player.GetAttackSpeed<SummonMeleeSpeedDamageClass>()) * atkSpdToDmgConversion;
                    Player.whipRangeMultiplier += (Player.GetTotalAttackSpeed<SummonMeleeSpeedDamageClass>() - Player.GetAttackSpeed<SummonMeleeSpeedDamageClass>()) * atkSpdToRangeConversion;
                }
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped())
            {
                int playersCrit = (int)Player.GetTotalCritChance<GenericDamageClass>();
                if (ProjectileID.Sets.IsAWhip[proj.type])
                {
                    if (playersCrit + whipCritBonus >= 100)
                    {
                        modifiers.SetCrit();
                    }
                    else if (Main.rand.NextBool(playersCrit + whipCritBonus, 100))
                    {
                        modifiers.SetCrit();
                    }
                    else
                    {
                        modifiers.DisableCrit();
                    }
                }
                else if (proj.IsMinionOrSentryRelated)
                {
                    if (playersCrit - nonWhipCrit >= 100)
                    {
                        modifiers.SetCrit();
                    }
                    else if (Main.rand.NextBool(playersCrit - nonWhipCrit, 100))
                    {
                        modifiers.SetCrit();
                    }
                    else
                    {
                        modifiers.DisableCrit();
                    }
                }
            }
        }
    }
    public sealed class TikiTotem : PetTooltip
    {
        public override PetEffect PetsEffect => tikiSpirit;
        public static TikiSpirit tikiSpirit
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out TikiSpirit pet))
                    return pet;
                else
                    return ModContent.GetInstance<TikiSpirit>();
            }
        }
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.TikiTotem")
                       .Replace("<atkSpdToDmg>", Math.Round(tikiSpirit.atkSpdToDmgConversion * 100, 2).ToString())
                       .Replace("<atkSpdToRange>", Math.Round(tikiSpirit.atkSpdToRangeConversion * 100, 2).ToString())
                       .Replace("<nonWhipCrit>", tikiSpirit.nonWhipCrit.ToString())
                       .Replace("<whipCrit>", tikiSpirit.whipCritBonus.ToString());
        public override string SimpleTooltip => PetTextsColors.LocVal("SimpleTooltips.TikiTotem");
    }
}
