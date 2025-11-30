using PetsOverhaul.Achievements;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.Projectiles
{
    /// <summary>
    /// ai[0] determines size. Sound not included, play your own sound before creating projectile however you want.
    /// </summary>
    public class PetExplosion : ModProjectile //I cannot figure why, shortswords does not seem to be able to hit a target hit by it with this same or any frame in near time. Possible problem with other weapons too?
    {
        private int achievementStrikes = 0;
        public override LocalizedText DisplayName => Language.GetText("Mods.PetsOverhaul.Projectiles.PetExplosion");
        public override void SetDefaults()
        {
            Projectile.height = 1;
            Projectile.width = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.timeLeft = 1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 1;
        }
        public override void OnSpawn(IEntitySource source)
        {
            achievementStrikes = 0;
            Projectile.Resize((int)Projectile.ai[0], (int)Projectile.ai[0]);
            for (int i = 0; i < Projectile.ai[0] * 0.075f; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 20, default, Main.rand.NextFloat(1.7f, 2f));
                dust.velocity *= Main.rand.NextFloat(1f, 1.3f);
            }
            for (int i = 0; i < Projectile.ai[0] * 0.15f; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 50, default, Main.rand.NextFloat(2.5f, 3f));
                dust.noGravity = true;
                dust.velocity *= Main.rand.NextFloat(1.7f, 2f);
                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 70, default, Main.rand.NextFloat(1.2f, 1.5f));
                dust.velocity *= Main.rand.NextFloat(1.7f, 2f);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetUtils.LifestealCheck(target))
            {
                achievementStrikes++;
                if (achievementStrikes >= 10)
                {
                    ModContent.GetInstance<HeavyDutyPet>().flag.Complete();
                }
            }
        }
    }
}