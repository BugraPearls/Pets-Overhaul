using Microsoft.Xna.Framework;
using PetsOverhaul.NPCs;
using PetsOverhaul.Projectiles;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public class SlimePrincessJump : ExtraJump
    {
        public override Position GetDefaultPosition() => MountJumpPosition;
        public override float GetDurationMultiplier(Player player) => 2.5f;
        public override void OnStarted(Player player, ref bool playSound) //Same code as Unicorn Mount's double jump
        {
            Vector2 center = player.Center;
            Vector2 vector2 = new(50f, 20f);
            float num10 = (float)Math.PI * 2f * Main.rand.NextFloat();
            for (int m = 0; m < 5; m++)
            {
                for (float num11 = 0f; num11 < 14f; num11 += 1f)
                {
                    Dust obj = Main.dust[Dust.NewDust(center, 0, 0, Utils.SelectRandom(Main.rand, 176, 177, 179))];
                    Vector2 vector3 = Vector2.UnitY.RotatedBy(num11 * ((float)Math.PI * 2f) / 14f + num10);
                    vector3 *= 0.2f * m;
                    obj.position = center + vector3 * vector2;
                    obj.velocity = vector3 + new Vector2(0f, player.gravDir * 4f);
                    obj.noGravity = true;
                    obj.scale = 1f + Main.rand.NextFloat() * 0.8f;
                    obj.fadeIn = Main.rand.NextFloat() * 2f;
                    obj.shader = GameShaders.Armor.GetSecondaryShader(player.cMount, player);
                }
            }
        }

        public override void ShowVisuals(Player player)
        {
            Dust obj = Main.dust[Dust.NewDust(player.position, player.width, player.height, Utils.SelectRandom(Main.rand, 176, 177, 179))];
            obj.velocity = Vector2.Zero;
            obj.noGravity = true;
            obj.scale = 0.5f + Main.rand.NextFloat() * 0.8f;
            obj.fadeIn = 1f + Main.rand.NextFloat() * 2f;
            obj.shader = GameShaders.Armor.GetSecondaryShader(player.cMount, player);
        }

        public override void UpdateHorizontalSpeeds(Player player) //A very strong Jump.
        {
            player.runAcceleration *= 3.5f;
            player.maxRunSpeed *= 2f;
        }
    }
    public sealed class SlimePrincess : PetEffect
    {
        public override int PetItemID => ItemID.QueenSlimePetItem;
        public float slow = 0.15f;
        public int slowDuration = 240;
        public int shield = 15;
        public int shieldTime = 420;
        public float mountDmgIncr = 0.35f;
        public int cooldown = 180;
        public override int PetAbilityCooldown => cooldown;
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public override PetClasses PetClassSecondary => PetClasses.Offensive;
        public void OnMountHit(NPC npc)
        {
            NpcPet.AddSlow(new NpcPet.PetSlow(slow, slowDuration, PetSlowIDs.PrincessSlime), npc);
            if (Pet.timer <= 0)
            {
                Pet.timer = Pet.timerMax;
                Pet.AddShield(shield, shieldTime);
            }
        }
        public override void Load()
        {
            On_Player.CollideWithNPCs += TryToCollideWithNPCs;
            On_Player.JumpMovement += JumpMovementMountHit;
        }

        private static void JumpMovementMountHit(On_Player.orig_JumpMovement orig, Player self) //This is copy paste of Vanilla JumpMovement Code. Done to have damage changes for Slime Mounts & Golf Cart.
        {
            if (self.TryGetModPlayer(out SlimePrincess princess) && princess.PetIsEquipped())
            {
                if (self.mount.Active && self.mount.IsConsideredASlimeMount && self.wetSlime == 0 && self.velocity.Y > 0f)
                {
                    Rectangle rect = self.getRect();
                    rect.Offset(0, self.height - 1);
                    rect.Height = 2;
                    rect.Inflate(12, 6);
                    for (int i = 0; i < 200; i++)
                    {
                        NPC nPC = Main.npc[i];
                        if (!nPC.active || nPC.dontTakeDamage || nPC.friendly || nPC.immune[self.whoAmI] != 0 || !self.CanNPCBeHitByPlayerOrPlayerProjectile(nPC))
                        {
                            continue;
                        }
                        Rectangle rect2 = nPC.getRect();
                        if (rect.Intersects(rect2) && (nPC.noTileCollide || Collision.CanHit(self.position, self.width, self.height, nPC.position, nPC.width, nPC.height)))
                        {
                            float num = 40f;
                            float knockback = 5f;
                            int num2 = self.direction;
                            if (self.velocity.X < 0f)
                            {
                                num2 = -1;
                            }
                            if (self.velocity.X > 0f)
                            {
                                num2 = 1;
                            }
                            if (self.whoAmI == Main.myPlayer)
                            {
                                self.ApplyDamageToNPC(nPC, princess.Pet.PetDamage((princess.mountDmgIncr + 1) * num, DamageClass.Summon), knockback, num2, crit: false, DamageClass.Summon);
                                princess.OnMountHit(nPC);

                            }
                            nPC.immune[self.whoAmI] = 10;
                            self.velocity.Y = -10f;
                            self.GiveImmuneTimeForCollisionAttack(6);
                            break;
                        }
                    }
                }
                if (self.mount.Active && self.mount.Type == 17 && self.velocity.Y > 0f)
                {
                    Rectangle rect3 = self.getRect();
                    rect3.Offset(0, self.height - 1);
                    rect3.Height = 2;
                    rect3.Inflate(12, 6);
                    for (int j = 0; j < 200; j++)
                    {
                        NPC nPC2 = Main.npc[j];
                        if (!nPC2.active || nPC2.dontTakeDamage || nPC2.friendly || nPC2.immune[self.whoAmI] != 0 || !self.CanNPCBeHitByPlayerOrPlayerProjectile(nPC2))
                        {
                            continue;
                        }
                        Rectangle rect4 = nPC2.getRect();
                        if (rect3.Intersects(rect4) && (nPC2.noTileCollide || Collision.CanHit(self.position, self.width, self.height, nPC2.position, nPC2.width, nPC2.height)))
                        {
                            float num3 = 40f;
                            float knockback2 = 5f;
                            int num4 = self.direction;
                            if (self.velocity.X < 0f)
                            {
                                num4 = -1;
                            }
                            if (self.velocity.X > 0f)
                            {
                                num4 = 1;
                            }
                            if (self.whoAmI == Main.myPlayer)
                            {
                                self.ApplyDamageToNPC(nPC2, princess.Pet.PetDamage(num3 * (princess.mountDmgIncr + 1), DamageClass.Summon), knockback2, num4);
                                princess.OnMountHit(nPC2);
                            }
                            nPC2.immune[self.whoAmI] = 12;
                            self.GiveImmuneTimeForCollisionAttack(12);
                            break;
                        }
                    }
                }
                if (self.controlJump)
                {
                    if (self.sliding)
                    {
                        self.autoJump = false;
                    }
                    bool flag = false;
                    if (self.mount.Active && self.mount.IsConsideredASlimeMount && self.wetSlime > 0)
                    {
                        self.wetSlime = 0;
                        flag = true;
                    }
                    if (self.mount.Active && self.mount.Type == 43 && self.releaseJump && self.velocity.Y != 0f)
                    {
                        self.isPerformingPogostickTricks = true;
                    }
                    if (self.jump > 0)
                    {
                        if (self.velocity.Y == 0f)
                        {
                            self.jump = 0;
                        }
                        else
                        {
                            self.velocity.Y = (0f - Player.jumpSpeed) * self.gravDir;
                            if (self.merman && (!self.mount.Active || !self.mount.Cart))
                            {
                                if (self.swimTime <= 10)
                                {
                                    self.swimTime = 30;
                                }
                            }
                            else
                            {
                                self.jump--;
                            }
                        }
                    }
                    else if ((self.sliding || self.velocity.Y == 0f || flag || self.AnyExtraJumpUsable()) && (self.releaseJump || (self.autoJump && (self.velocity.Y == 0f || self.sliding))))
                    {
                        if (self.mount.Active && MountID.Sets.Cart[self.mount.Type])
                        {
                            self.position.Y -= 0.001f;
                        }
                        if (self.sliding || self.velocity.Y == 0f)
                        {
                            self.justJumped = true;
                        }
                        bool flag2 = false;
                        bool attemptDoubleJumps = !flag;
                        self.canRocket = false;
                        self.rocketRelease = false;
                        if (self.velocity.Y == 0f || self.sliding || (self.autoJump && self.justJumped))
                        {
                            self.RefreshExtraJumps();
                        }
                        if (self.velocity.Y == 0f || flag2 || self.sliding || flag)
                        {
                            if (self.mount.Active && self.mount.Type == 43)
                            {
                                SoundEngine.PlaySound(in SoundID.Item168, self.Center);
                            }
                            self.velocity.Y = (0f - Player.jumpSpeed) * self.gravDir;
                            self.jump = Player.jumpHeight;
                            if (self.portableStoolInfo.IsInUse)
                            {
                                self.position.Y -= self.portableStoolInfo.HeightBoost;
                                self.gfxOffY += self.portableStoolInfo.HeightBoost;
                            }
                            if (self.sliding)
                            {
                                self.velocity.X = 3 * -self.slideDir;
                            }
                        }
                        else if (attemptDoubleJumps && !self.blockExtraJumps)
                        {
                            ExtraJumpLoader.ProcessJumps(self);
                        }
                    }
                    self.releaseJump = false;
                }
                else
                {
                    self.jump = 0;
                    self.releaseJump = true;
                    self.rocketRelease = true;
                }
            }
            else
                orig(self);
        }

        private static int TryToCollideWithNPCs(On_Player.orig_CollideWithNPCs orig, Player self, Rectangle myRect, float Damage, float Knockback, int NPCImmuneTime, int PlayerImmuneTime, DamageClass damageType) //This is copy paste of Vanilla CollideWithNPCs Code.
        {
            if (self.TryGetModPlayer(out SlimePrincess princess) && princess.PetIsEquipped() && princess.Player.mount.Active)
            {
                int num = 0;
                for (int i = 0; i < 200; i++)
                {
                    NPC nPC = Main.npc[i];
                    if (!nPC.active || nPC.dontTakeDamage || nPC.friendly || nPC.immune[self.whoAmI] != 0 || !self.CanNPCBeHitByPlayerOrPlayerProjectile(nPC))
                    {
                        continue;
                    }
                    Rectangle rect = nPC.getRect();
                    if (myRect.Intersects(rect) && (nPC.noTileCollide || Collision.CanHit(self.position, self.width, self.height, nPC.position, nPC.width, nPC.height)))
                    {
                        int num2 = self.direction;
                        if (self.velocity.X < 0f)
                        {
                            num2 = -1;
                        }
                        if (self.velocity.X > 0f)
                        {
                            num2 = 1;
                        }
                        if (self.whoAmI == Main.myPlayer)
                        {
                            self.ApplyDamageToNPC(nPC, princess.Pet.PetDamage(Damage * (princess.mountDmgIncr + 1), damageType), Knockback, num2, crit: false, damageType);
                            princess.OnMountHit(nPC); //This is same code as whats called in original method (orig), except it will give Player shield, increase damage by the Mount and slow the npc on hit.
                        }
                        nPC.immune[self.whoAmI] = NPCImmuneTime;
                        self.GiveImmuneTimeForCollisionAttack(PlayerImmuneTime);
                        num++;
                        break;
                    }
                }
                return num;
            }
            else
            {
                return orig(self, myRect, Damage, Knockback, NPCImmuneTime, PlayerImmuneTime, damageType);
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && proj.TryGetGlobalProjectile(out ProjectileSourceChecks check) && check.fromMount)
            {
                modifiers.FinalDamage *= (1 + mountDmgIncr) * Pet.petDirectDamageMultiplier;
                OnMountHit(target);
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped() && Player.mount.Active)
            {
                Player.GetJumpState<SlimePrincessJump>().Enable();
            }
        }
    }
    public sealed class QueenSlimePetItem : PetTooltip
    {
        public override PetEffect PetsEffect => slimePrincess;
        public static SlimePrincess slimePrincess
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out SlimePrincess pet))
                    return pet;
                else
                    return ModContent.GetInstance<SlimePrincess>();
            }
        }
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.QueenSlimePetItem")
            .Replace("<dmg>", Math.Round(slimePrincess.mountDmgIncr * 100, 2).ToString())
                        .Replace("<slowAmount>", Math.Round(slimePrincess.slow * 100, 2).ToString())
            .Replace("<slowDuration>", Math.Round(slimePrincess.slowDuration / 60f, 2).ToString())
                        .Replace("<shieldAmount>", slimePrincess.shield.ToString())
                        .Replace("<shieldDuration>", Math.Round(slimePrincess.shieldTime / 60f, 2).ToString())
            .Replace("<cooldown>", Math.Round(slimePrincess.cooldown / 60f, 2).ToString());
        public override string SimpleTooltip => PetTextsColors.LocVal("SimpleTooltips.QueenSlimePetItem");
    }
}
