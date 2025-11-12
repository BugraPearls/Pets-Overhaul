using Microsoft.Xna.Framework;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class BabySnowman : PetEffect
    {
        public override int PetItemID => ItemID.ToySled;
        public int frostburnTime = 300;
        public float snowmanSlow = 0.3f;
        public int slowTime = 180;
        public int frostMult = 3;
        public int FrostArmorMult => Player.frostBurn ? frostMult : 1;

        /// <summary>
        /// Pet is obtainable Pre-Hm in some seeds iirc.
        /// </summary>
        public static int FrostBurnId => Main.hardMode ? BuffID.Frostburn2 : BuffID.Frostburn;

        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override PetClasses PetClassSecondary => PetClasses.Utility;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped())
            {
                target.AddBuff(FrostBurnId, frostburnTime * FrostArmorMult);
                PetGlobalNPC.AddSlow(new PetSlow(snowmanSlow * FrostArmorMult, slowTime * FrostArmorMult, PetSlowID.Snowman), target);
            }
        }
        public override void MeleeEffects(Item item, Rectangle hitbox)
        {
            if (PetIsEquipped())
            {
                if (item.CountsAsClass(DamageClass.Melee) && !item.noMelee && !item.noUseGraphic && Main.rand.NextBool(2))
                {
                    if (Player.frostBurn)
                    {
                        return;
                    }
                    int num19 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.IceTorch, Player.velocity.X * 0.2f + Player.direction * 3, Player.velocity.Y * 0.2f, 100, default, 2.5f);
                    Main.dust[num19].noGravity = true;
                    Main.dust[num19].velocity *= 0.7f;
                    Main.dust[num19].velocity.Y -= 0.5f;
                }
            }
        }
        public override void EmitEnchantmentVisualsAt(Projectile projectile, Vector2 boxPosition, int boxWidth, int boxHeight)
        {
            if (projectile.active)
            {
                if (PetIsEquipped())
                {
                    if ((projectile.CountsAsClass(DamageClass.Melee) || projectile.CountsAsClass(DamageClass.Ranged)) && Player.frostBurn)
                    {
                        return;
                    }
                    if (projectile.friendly && !projectile.hostile && Main.rand.NextBool(2 * (1 + projectile.extraUpdates)) && projectile.damage > 0)
                    {
                        int num = Dust.NewDust(boxPosition, boxWidth, boxHeight, DustID.IceTorch, projectile.velocity.X * 0.2f + projectile.direction * 3, projectile.velocity.Y * 0.2f, 100, default, 2f);
                        Main.dust[num].noGravity = true;
                        Main.dust[num].velocity *= 0.7f;
                        Main.dust[num].velocity.Y -= 0.5f;
                    }
                }
            }
        }
    }
    public sealed class ToySled : PetTooltip
    {
        public override PetEffect PetsEffect => babySnowman;
        public static BabySnowman babySnowman
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out BabySnowman pet))
                    return pet;
                else
                    return ModContent.GetInstance<BabySnowman>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.ToySled")
                .Replace("<frostburnTime>", Math.Round(babySnowman.frostburnTime / 60f * babySnowman.FrostArmorMult, 2).ToString())
                .Replace("<slowAmount>", Math.Round(babySnowman.snowmanSlow * 100 * babySnowman.FrostArmorMult, 2).ToString())
                .Replace("<slowTime>", Math.Round(babySnowman.slowTime / 60f * babySnowman.FrostArmorMult, 2).ToString())
                .Replace("<frostMult>", babySnowman.frostMult.ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.ToySled");
    }
}
