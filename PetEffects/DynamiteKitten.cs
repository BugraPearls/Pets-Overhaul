using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.Projectiles;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class DynamiteKitten : PetEffect
    {
        public override int PetItemID => ItemID.BallOfFuseWire;
        public int cooldown = 120;
        public float damageMult = 0.6f;
        public float kbMult = 1.7f;
        public int armorPen = 15;
        public int explosionSize = 200;

        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override int PetAbilityCooldown => cooldown;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && Pet.timer <= 0)
            {
                Projectile petProjectile = Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), target.Center, Vector2.Zero, ModContent.ProjectileType<PetExplosion>(), Pet.PetDamage(hit.SourceDamage * damageMult, hit.DamageType), hit.Knockback * kbMult, Player.whoAmI, explosionSize);
                petProjectile.DamageType = hit.DamageType;
                petProjectile.ArmorPenetration = armorPen;
                petProjectile.CritChance = (int)Player.GetTotalCritChance(hit.DamageType);
                if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                {
                    SoundEngine.PlaySound(SoundID.Item14, target.Center);
                }
                Pet.timer = Pet.timerMax;
            }
        }
    }
    public sealed class BallOfFuseWire : PetTooltip
    {
        public override PetEffect PetsEffect => dynamiteKitten;
        public static DynamiteKitten dynamiteKitten
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out DynamiteKitten pet))
                    return pet;
                else
                    return ModContent.GetInstance<DynamiteKitten>();
            }
        }
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.BallOfFuseWire")
                        .Replace("<kb>", dynamiteKitten.kbMult.ToString())
                        .Replace("<dmg>", dynamiteKitten.damageMult.ToString())
                        .Replace("<armorPen>", dynamiteKitten.armorPen.ToString())
                        .Replace("<size>", dynamiteKitten.explosionSize.ToString());
    }
}
