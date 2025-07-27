using PetsOverhaul.NPCs;
using PetsOverhaul.PetEffects;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Projectiles
{
    public class Shuricorn : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.ThrowingKnife);
            Projectile.penetrate = 1;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.TryGetGlobalNPC(out NpcPet pet))
            {
                pet.shuricornMark = SugarGlider.shuricornDuration;
            }
        }
    }
}