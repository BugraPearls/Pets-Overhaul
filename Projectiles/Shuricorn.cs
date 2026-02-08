using MonoMod.Core.Platforms;
using PetsOverhaul.PetEffects;
using PetsOverhaul.Systems;
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
            if (target.active && target.TryGetGlobalNPC(out PetGlobalNPC npc) && Projectile.owner != 255 && Main.player[Projectile.owner].TryGetModPlayer(out SugarGlider glider) && glider.PetIsEquippedForCustom())
            {
                glider.shuricornTaggedNpc = target.whoAmI;
                npc.shuricornMark = SugarGlider.shuricornDuration;
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    ModPacket packet = ModContent.GetInstance<PetsOverhaul>().GetPacket();
                    packet.Write((byte)MessageType.SugarGliderAbilityHit);
                    packet.Write((short)target.whoAmI);
                    packet.Send();
                }
            }
        }
    }
}