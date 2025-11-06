using Microsoft.Xna.Framework;
using PetsOverhaul.NPCs;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Hoardagron : PetEffect
    {
        public override int PetItemID => ItemID.DD2PetDragon;
        public bool arrow = false;
        public bool specialist = false;
        public float arrowSpd = 0.8f;
        public float bulletSpd = 2.25f;
        public float specialThreshold = 0.2f;
        public float specialBossThreshold = 0.06f;
        public int arrowPen = 1;
        public override PetClasses PetClassPrimary => PetClasses.Ranged;
        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (PetIsEquipped())
            {

                if (AmmoID.Sets.IsArrow[item.useAmmo])
                {
                    arrow = true;
                    velocity *= arrowSpd;
                }
                else
                {
                    arrow = false;
                }

                if (AmmoID.Sets.IsBullet[item.useAmmo])
                {
                    velocity *= bulletSpd;
                }

                specialist = AmmoID.Sets.IsSpecialist[item.useAmmo];
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && proj.GetGlobalProjectile<HoardagronProj>().special)
            {
                if ((target.boss == true || NpcPet.NonBossTrueBosses.Contains(target.type)) && target.life < (int)(target.lifeMax * specialBossThreshold))
                {
                    modifiers.SetCrit();
                }
                else if (target.life < (int)(target.lifeMax * specialThreshold))
                {
                    modifiers.SetCrit();
                }
            }
        }
    }
    public sealed class HoardagronProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool special;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.owner != 255 && Main.player[projectile.owner].active)
            {
                Hoardagron player = Main.player[projectile.owner].GetModPlayer<Hoardagron>();
                if (player.PetIsEquipped())
                {
                    special = player.specialist;
                    if (player.arrow && projectile.penetrate >= 0)
                    {
                        projectile.penetrate += player.arrowPen;
                        if (projectile.usesLocalNPCImmunity == false)
                        {
                            projectile.usesLocalNPCImmunity = true;
                            projectile.localNPCHitCooldown = 10;
                        }
                    }
                }
            }
        }
    }
    public sealed class DD2PetDragon : PetTooltip
    {
        public override PetEffect PetsEffect => hoardagron;
        public static Hoardagron hoardagron
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out Hoardagron pet))
                    return pet;
                else
                    return ModContent.GetInstance<Hoardagron>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.DD2PetDragon")
                        .Replace("<arrowVelo>", hoardagron.arrowSpd.ToString())
                        .Replace("<arrowPierce>", hoardagron.arrowPen.ToString())
                        .Replace("<bulletVelo>", hoardagron.bulletSpd.ToString())
                        .Replace("<threshold>", Math.Round(hoardagron.specialThreshold * 100, 2).ToString())
                        .Replace("<bossThreshold>", Math.Round(hoardagron.specialBossThreshold * 100, 2).ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.DD2PetDragon");
    }
}