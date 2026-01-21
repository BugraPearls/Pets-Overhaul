using Microsoft.Xna.Framework;
using PetsOverhaul.Achievements;
using PetsOverhaul.NPCs;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class DualSlime : PetEffect
    {
        public override int PetItemID => ItemID.ResplendentDessert;
        public override PetClass PetClassPrimary => PetClassID.Offensive;
        public override PetClass PetClassSecondary => PetClassID.Defensive;
        public int shield = 55;
        public int shieldTime = 1500;
        public float mountDmgIncr = 0.45f;
        public int cooldown = 1200;
        public float knockback = 8f;
        public int baseDmg = 18; //Most values are used inside the SlimeServant's code
        public float defMult = 0.2f;
        public float hpMult = 0.45f;
        public int howManyIsAlive = 0;
        public override int PetAbilityCooldown => cooldown;
        public override int PetStackCurrent => howManyIsAlive;
        public override int PetStackMax => 0;
        public override string PetStackText => PetUtils.LocVal("PetItemTooltips.KingSlimePetItemStack");
        public override void ExtraPreUpdate()
        {
            howManyIsAlive = 0;
            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (npc.type == ModContent.NPCType<SlimeServant>() && npc.GetGlobalNPC<SlimeServantOwner>().Owner == Player.whoAmI)
                {
                    howManyIsAlive++;
                    if (howManyIsAlive >= 5)
                    {
                        PetUtils.DoAchievementOnPlayer<YouAndSlimeArmy>(Player.whoAmI);
                    }
                }
            }
        }
        public void OnMountHit(NPC npc)
        {
            if (Pet.timer <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item44 with { PitchVariance = 1.6f, Volume = 0.6f }, Player.Center);
                NPC npC = NPC.NewNPCDirect(PetUtils.GetSource_Pet(EntitySourcePetIDs.PetNPC), (int)npc.position.X, (int)npc.position.Y, ModContent.NPCType<SlimeServant>());
                npC.GetGlobalNPC<SlimeServantOwner>().Owner = Player.whoAmI;
                npC.defense += Player.statDefense * defMult;
                npC.lifeMax += (int)(Player.statLifeMax2 * hpMult);
                npC.life = npC.lifeMax;
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
            if (self.TryGetModPlayer(out DualSlime dual) && dual.PetIsEquipped())
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
                                self.ApplyDamageToNPC(nPC, dual.Pet.PetDamage((dual.mountDmgIncr + 1) * num, DamageClass.Summon), knockback, num2, crit: false, DamageClass.Summon);
                                dual.OnMountHit(nPC);

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
                                self.ApplyDamageToNPC(nPC2, dual.Pet.PetDamage(num3 * (dual.mountDmgIncr + 1), DamageClass.Summon), knockback2, num4);
                                dual.OnMountHit(nPC2);
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
            if (self.TryGetModPlayer(out DualSlime dual) && dual.PetIsEquipped() && dual.Player.mount.Active)
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
                            self.ApplyDamageToNPC(nPC, dual.Pet.PetDamage(Damage * (dual.mountDmgIncr + 1), damageType), Knockback, num2, crit: false, damageType);
                            dual.OnMountHit(nPC); //This is same code as whats called in original method (orig), except it will give Player shield, increase damage by the Mount and slow the npc on hit.
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
            if (PetIsEquipped() && proj.TryGetGlobalProjectile(out PetGlobalProjectile check) && check.fromMount)
            {
                modifiers.FinalDamage *= (1 + mountDmgIncr) * Pet.petDirectDamageMultiplier;
                OnMountHit(target);
            }
        }
    }

    public sealed class ResplendentDessert : PetTooltip
    {
        public override PetEffect PetsEffect => dualSlime;
        public static DualSlime dualSlime
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out DualSlime pet))
                    return pet;
                else
                    return ModContent.GetInstance<DualSlime>();
            }
        }
        public override string PetsTooltip =>
                PetUtils.LocVal("PetItemTooltips.ResplendentDessert")
            .Replace("<dmg>", Math.Round(dualSlime.mountDmgIncr * 100, 2).ToString())
                        .Replace("<shieldAmount>", dualSlime.shield.ToString())
                        .Replace("<shieldDuration>", Math.Round(dualSlime.shieldTime / 60f, 2).ToString())
            .Replace("<cooldown>", Math.Round(dualSlime.cooldown / 60f, 2).ToString())
            .Replace("<defMult>", Math.Round(dualSlime.defMult * 100, 2).ToString())
            .Replace("<hpMult>", Math.Round(dualSlime.hpMult * 100, 2).ToString())
            .Replace("<baseDmg>", dualSlime.baseDmg.ToString())
            .Replace("<lifetime>", Math.Round(SlimePrince.lifetimeOfServant / 60f, 2).ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.ResplendentDessert");
    }
}


