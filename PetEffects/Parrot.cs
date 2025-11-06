using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Parrot : PetEffect
    {
        public override int PetItemID => ItemID.ParrotCracker;
        public int projChance = 22;
        public int meleeChance = 28;
        public float projDamage = 0.75f;
        public float meleeDamage = 0.75f;
        public override PetClasses PetClassPrimary => PetClasses.Offensive;

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && target.active && target.dontTakeDamage == false)
            {
                for (int i = 0; i < GlobalPet.Randomizer(meleeChance); i++)
                {
                    target.SimpleStrikeNPC(Pet.PetDamage(hit.SourceDamage * meleeDamage, hit.DamageType), hit.HitDirection, Main.rand.NextBool((int)Math.Min(Player.GetTotalCritChance(hit.DamageType), 100), 100), 0, hit.DamageType, true, Player.luck);
                    PlayParrotSound();
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && target.active && target.dontTakeDamage == false && (proj.minion || proj.sentry || proj.usesOwnerMeleeHitCD || proj.ownerHitCheck || proj.type == ProjectileID.TrueNightsEdge))
            {
                for (int i = 0; i < GlobalPet.Randomizer(meleeChance); i++)
                {
                    target.SimpleStrikeNPC(Pet.PetDamage(hit.SourceDamage * projDamage, hit.DamageType), hit.HitDirection, Main.rand.NextBool((int)Math.Min(Player.GetTotalCritChance(hit.DamageType), 100), 100), 0, hit.DamageType, true, Player.luck);
                    PlayParrotSound();
                }
            }
        }

        public void PlayParrotSound()
        {
            if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
            {
                SoundStyle style = default;
                switch (Main.rand.Next(3))
                {
                    case 0:
                        style = SoundID.Zombie78;
                        break;
                    case 1:
                        style = SoundID.Cockatiel;
                        break;
                    case 2:
                        style = SoundID.Macaw;
                        break;
                }
                SoundEngine.PlaySound(style with
                {
                    PitchVariance = 2f,
                    MaxInstances = 1,
                    SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest,
                    Volume = 0.2f
                }, Player.Center);
            }
        }
    }
    public sealed class ParrotExtraProjectile : GlobalProjectile
    {
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (Main.player[projectile.owner].TryGetModPlayer(out Parrot parrot) && parrot.PetIsEquipped() && projectile.usesOwnerMeleeHitCD == false && projectile.ownerHitCheck == false && projectile.damage > 0 && projectile.type != ProjectileID.TrueNightsEdge && projectile.minion == false && projectile.sentry == false) //We check if Projectile is a 'melee' projectile, or a direct 'melee hit' of a Minion/Sentry. True Nights Edge does not use usesOwnerMeleeHitCD or ownerHitCheck, reason for its exception.
            {
                DamageClass damageType = DamageClass.Generic;
                if (source is EntitySource_ItemUse entity1 && entity1.Item is not null)
                {
                    damageType = entity1.Item.DamageType;
                }
                else if (source is EntitySource_Parent parent && parent.Entity is Projectile proj1 && (proj1.minion || proj1.sentry))
                {
                    damageType = proj1.DamageType;
                }

                if (source is EntitySource_ItemUse || source is EntitySource_Parent { Entity: Projectile entity } && (entity.minion || entity.sentry))
                {
                    for (int i = 0; i < GlobalPet.Randomizer(parrot.projChance); i++)
                    {
                        Projectile petProjectile = Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), projectile.Center, projectile.velocity.RotateRandom(0.5f), projectile.type, parrot.Pet.PetDamage(projectile.damage * parrot.projDamage, damageType), projectile.knockBack, projectile.owner);
                        petProjectile.DamageType = damageType;
                        petProjectile.CritChance = (int)parrot.Player.GetTotalCritChance(damageType);
                        parrot.PlayParrotSound();
                    }
                }
            }
        }
    }
    public sealed class ParrotCracker : PetTooltip
    {
        public override PetEffect PetsEffect => parrot;
        public static Parrot parrot
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out Parrot pet))
                    return pet;
                else
                    return ModContent.GetInstance<Parrot>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.ParrotCracker")
                .Replace("<projChance>", parrot.projChance.ToString())
                .Replace("<projDamage>", parrot.projDamage.ToString())
                .Replace("<meleeChance>", parrot.meleeChance.ToString())
                .Replace("<meleeDamage>", parrot.meleeDamage.ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.ParrotCracker");
    }
}
