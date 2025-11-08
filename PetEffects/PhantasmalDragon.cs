using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace PetsOverhaul.PetEffects
{
    public sealed class PhantasmalDragon : PetEffect
    {
        public override int PetItemID => ItemID.LunaticCultistPetItem;
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public int phantasmDragonCooldown = 480;
        public int currentAbility = 0;

        public int iceBase = 300;
        public float iceSlow = 1.5f;
        public int iceSlowDuration = 1200;

        public int lightningOrbBase = 60;
        public int lightningStrikeDivide = 2;
        public int lightningStrikePen = 50; //Orb uses half of Pen aswell
        public float lightningSlow = 10f; //Strike is half
        public int lightningSlowDuration = 10;

        public int fireBase = 170;
        private int fireVolley = 0;
        public int fireVolleyEveryFrame = 15;
        public int fireVolleyFrames = 90;
        public int fireBurnTime = 180;
        public float fireKnockback = 3.8f;
        public string CurrentSpellName => currentAbility switch
        {
            0 => PetUtils.LocVal("PetItemTooltips.DragonIceName"),
            1 => PetUtils.LocVal("PetItemTooltips.DragonLightningName"),
            2 => PetUtils.LocVal("PetItemTooltips.DragonFireName"),
            _ => "Invalid Ability.",
        };
        public override string PetStackSpecial => CurrentSpellName;
        public override string PetStackText => PetUtils.LocVal("PetItemTooltips.LunaticCultistPetItemStack");
        public override int PetAbilityCooldown => phantasmDragonCooldown;
        public override void ExtraPreUpdate()
        {
            if (fireVolley > 0 && fireVolley % fireVolleyEveryFrame == 0)
            {
                Vector2 location = Player.Center;
                if (ModContent.GetInstance<PetPersonalization>().PhantasmalDragonVolleyFromMouth)
                    foreach (var projectile in Main.ActiveProjectiles)
                    {
                        if (projectile is Projectile proj && proj.owner == Player.whoAmI && proj.type == ProjectileID.LunaticCultistPet)
                        {
                            location = proj.Center;
                        }
                    }
                Projectile petProjectile = Projectile.NewProjectileDirect(PetUtils.GetSource_Pet(EntitySourcePetIDs.PetProjectile, "Phantasmal"), location, new Vector2(Main.MouseWorld.X - location.X - Main.rand.NextFloat(3f, -3f), Main.MouseWorld.Y - location.Y - Main.rand.NextFloat(6f, 7f)), ProjectileID.CultistBossFireBall, Pet.PetDamage(fireBase, DamageClass.Generic), fireKnockback, Player.whoAmI);
                petProjectile.DamageType = DamageClass.Generic;
                petProjectile.CritChance = (int)Player.GetTotalCritChance(DamageClass.Generic);
            }
            fireVolley--;
            if (fireVolley < 0)
                fireVolley = 0;
        }
        public override void ExtraProcessTriggers(TriggersSet triggersSet)
        {
            if (PetKeybinds.PetAbilitySwitch.JustPressed)
            {
                currentAbility++;
                if (currentAbility > 2)
                    currentAbility = 0;
            }
            if (Pet.AbilityPressCheck() && PetIsEquipped())
            {
                Pet.timer = Pet.timerMax;
                switch (currentAbility)
                {
                    case 0: //Ice
                        Vector2 velocity = Main.rand.NextVector2CircularEdge(4f, 4f);
                        Projectile petProjectile = Projectile.NewProjectileDirect(PetUtils.GetSource_Pet(EntitySourcePetIDs.PetProjectile, "Phantasmal"), Main.MouseWorld, velocity, ProjectileID.CultistBossIceMist, Pet.PetDamage(iceBase, DamageClass.Generic), 0, Player.whoAmI, 0f, 1f);
                        petProjectile.DamageType = DamageClass.Generic;
                        petProjectile.CritChance = (int)Player.GetTotalCritChance(DamageClass.Generic);
                        petProjectile.netUpdate = true;
                        break;
                    case 1: //Lightning
                        Projectile petProj = Projectile.NewProjectileDirect(PetUtils.GetSource_Pet(EntitySourcePetIDs.PetProjectile, "Phantasmal"), Main.MouseWorld, Vector2.Zero, ProjectileID.CultistBossLightningOrb, Pet.PetDamage(lightningOrbBase, DamageClass.Generic), 0, Player.whoAmI, 0f);
                        petProj.DamageType = DamageClass.Generic;
                        petProj.CritChance = (int)Player.GetTotalCritChance(DamageClass.Generic);
                        petProj.netUpdate = true;
                        break;
                    case 2: //Fire
                        fireVolley = fireVolleyFrames;
                        break;
                    default:
                        break;
                }
            }
        }
        public override void SaveData(TagCompound tag)
        {
            tag.Add("CurrentSpell", currentAbility);
        }
        public override void LoadData(TagCompound tag)
        {
            if (tag.TryGet("CurrentSpell", out int spell))
            {
                currentAbility = spell;
            }
        }
    }
    public sealed class LunaticCultistFriendlyProjectiles : GlobalProjectile //NOTE: Could not run the projectiles on Server properly, other players currently cannot see the projectiles, but they work fine in Multiplayer.
    {
        public override bool InstancePerEntity => true;
        public bool fromPhantasmalPet = false;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if ((source is EntitySource_Pet dragonProj && dragonProj.ContextType == EntitySourcePetIDs.PetProjectile && dragonProj.Context == "Phantasmal") ||
                (source is EntitySource_Parent parent && parent.Entity is Projectile proj && proj.TryGetGlobalProjectile(out LunaticCultistFriendlyProjectiles result) && result.fromPhantasmalPet))
            {
                fromPhantasmalPet = true;
                PhantasmalDragon dragon = Main.player[projectile.owner].GetModPlayer<PhantasmalDragon>();
                if (projectile.type == ProjectileID.CultistBossLightningOrb)
                {
                    projectile.penetrate = -1;
                    projectile.ArmorPenetration += dragon.lightningStrikePen / dragon.lightningStrikeDivide;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 10;
                }
                if (projectile.type == ProjectileID.CultistBossLightningOrbArc)
                {
                    projectile.penetrate = -1;
                    projectile.ArmorPenetration += dragon.lightningStrikePen;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 20;
                }
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (fromPhantasmalPet)
            {
                PhantasmalDragon dragon = Main.player[projectile.owner].GetModPlayer<PhantasmalDragon>();
                if (projectile.type == ProjectileID.CultistBossIceMist)
                {
                    PetGlobalNPC.AddSlow(new PetSlow(dragon.iceSlow, dragon.iceSlowDuration, PetSlowIDs.PhantasmalIce), target);
                }
                else if (projectile.type == ProjectileID.CultistBossLightningOrb)
                {
                    PetGlobalNPC.AddSlow(new PetSlow(dragon.lightningSlow, dragon.lightningSlowDuration, PetSlowIDs.PhantasmalLightning), target);
                }
                else if (projectile.type == ProjectileID.CultistBossLightningOrbArc)
                {
                    PetGlobalNPC.AddSlow(new PetSlow(dragon.lightningSlow / dragon.lightningStrikeDivide, dragon.lightningSlowDuration, PetSlowIDs.PhantasmalLightning), target);
                }
                else if (projectile.type == ProjectileID.CultistBossFireBall)
                {
                    target.AddBuff(BuffID.Daybreak, dragon.fireBurnTime);
                }
            }
        }
        public override void PostAI(Projectile projectile)
        {
            if (fromPhantasmalPet)
            {
                if (Main.netMode != NetmodeID.SinglePlayer && projectile.type == ProjectileID.CultistBossIceMist && projectile.ai[1] == 1f && projectile.ai[0] % 30f == 0f)
                {
                    Projectile petProjectile = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center, projectile.rotation.ToRotationVector2(), 464, projectile.damage, projectile.knockBack, projectile.owner);
                }
                projectile.friendly = true;
                projectile.hostile = false;
                projectile.netUpdate = true;
            }
        }
        public override bool PreAI(Projectile projectile)
        {
            if (fromPhantasmalPet) //Lightning is copy paste of Vanilla code with a few changes
            {
                Vector2 val4;
                if (projectile.type == ProjectileID.CultistBossLightningOrb)
                {
                    if (projectile.localAI[1] == 0f)
                    {
                        SoundEngine.PlaySound(in SoundID.Item121, projectile.Center);
                        projectile.localAI[1] = 1f;
                    }
                    if (projectile.ai[0] < 180f)
                    {
                        projectile.alpha -= 5;
                        if (projectile.alpha < 0)
                        {
                            projectile.alpha = 0;
                        }
                    }
                    else
                    {
                        projectile.alpha += 5;
                        if (projectile.alpha > 255)
                        {
                            projectile.alpha = 255;
                            projectile.Kill();
                            return false;
                        }
                    }
                    projectile.ai[0]++;
                    if (projectile.ai[0] % 30f == 0f && projectile.ai[0] < 180f) //Removed the code regarding targeting the player.
                    {
                        Vector2 vector162 = Main.MouseWorld - projectile.Center; //Directly uses Mouse position for velocity.
                        float ai = Main.rand.Next(100);
                        Vector2 vector163 = Vector2.Normalize(vector162.RotatedByRandom(0.7853981852531433)) * 7f;
                        Projectile petProjectile = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center, vector163, ProjectileID.CultistBossLightningOrbArc, projectile.damage / Main.player[projectile.owner].GetModPlayer<PhantasmalDragon>().lightningStrikeDivide, 0f, projectile.owner, vector162.ToRotation(), ai); //Changed ID for readability and the WhoAmI to .owner
                    }
                    Lighting.AddLight(projectile.Center, 0.4f, 0.85f, 0.9f);
                    if (++projectile.frameCounter >= 4)
                    {
                        projectile.frameCounter = 0;
                        if (++projectile.frame >= Main.projFrames[projectile.type])
                        {
                            projectile.frame = 0;
                        }
                    }
                    if (projectile.alpha >= 150 || !(projectile.ai[0] < 180f))
                    {
                        return false;
                    }
                    for (int num814 = 0; num814 < 1; num814++)
                    {
                        float num815 = (float)Main.rand.NextDouble() * 1f - 0.5f;
                        if (num815 < -0.5f)
                        {
                            num815 = -0.5f;
                        }
                        if (num815 > 0.5f)
                        {
                            num815 = 0.5f;
                        }
                        Vector2 spinningpoint56 = new(-projectile.width * 0.2f * projectile.scale, 0f);
                        double radians43 = num815 * ((float)Math.PI * 2f);
                        val4 = default;
                        Vector2 spinningpoint57 = Utils.RotatedBy(spinningpoint56, radians43, val4);
                        double radians44 = projectile.velocity.ToRotation();
                        val4 = default;
                        Vector2 vector164 = spinningpoint57.RotatedBy(radians44, val4);
                        int num816 = Dust.NewDust(projectile.Center - Vector2.One * 5f, 10, 10, DustID.Electric, (0f - projectile.velocity.X) / 3f, (0f - projectile.velocity.Y) / 3f, 150, Color.Transparent, 0.7f);
                        Main.dust[num816].position = projectile.Center + vector164;
                        Main.dust[num816].velocity = Vector2.Normalize(Main.dust[num816].position - projectile.Center) * 2f;
                        Main.dust[num816].noGravity = true;
                    }
                    for (int num817 = 0; num817 < 1; num817++)
                    {
                        float num818 = (float)Main.rand.NextDouble() * 1f - 0.5f;
                        if (num818 < -0.5f)
                        {
                            num818 = -0.5f;
                        }
                        if (num818 > 0.5f)
                        {
                            num818 = 0.5f;
                        }
                        Vector2 spinningpoint58 = new(-projectile.width * 0.6f * projectile.scale, 0f);
                        double radians45 = num818 * ((float)Math.PI * 2f);
                        val4 = default;
                        Vector2 spinningpoint59 = Utils.RotatedBy(spinningpoint58, radians45, val4);
                        double radians46 = projectile.velocity.ToRotation();
                        val4 = default;
                        Vector2 vector165 = spinningpoint59.RotatedBy(radians46, val4);
                        int num819 = Dust.NewDust(projectile.Center - Vector2.One * 5f, 10, 10, DustID.Electric, (0f - projectile.velocity.X) / 3f, (0f - projectile.velocity.Y) / 3f, 150, Color.Transparent, 0.7f);
                        Main.dust[num819].velocity = Vector2.Zero;
                        Main.dust[num819].position = projectile.Center + vector165;
                        Main.dust[num819].noGravity = true;
                    }
                    return false;
                }
                else if (projectile.type == ProjectileID.CultistBossLightningOrbArc)
                {
                    projectile.frameCounter++;
                    Lighting.AddLight(projectile.Center, 0.3f, 0.45f, 0.5f);
                    if (projectile.velocity == Vector2.Zero)
                    {
                        if (projectile.frameCounter >= projectile.extraUpdates * 2)
                        {
                            projectile.frameCounter = 0;
                            bool flag29 = true;
                            for (int num820 = 1; num820 < projectile.oldPos.Length; num820++)
                            {
                                if (projectile.oldPos[num820] != projectile.oldPos[0])
                                {
                                    flag29 = false;
                                }
                            }
                            if (flag29)
                            {
                                projectile.Kill();
                                return false;
                            }
                        }
                        if (Main.rand.NextBool(projectile.extraUpdates))
                        {
                            Vector2 vector166 = default;
                            for (int num822 = 0; num822 < 2; num822++)
                            {
                                float num823 = projectile.rotation + ((Main.rand.NextBool(2)) ? (-1f) : 1f) * ((float)Math.PI / 2f);
                                float num824 = (float)Main.rand.NextDouble() * 0.8f + 1f;
                                vector166 = new Vector2((float)Math.Cos(num823) * num824, (float)Math.Sin(num823) * num824);
                                int num825 = Dust.NewDust(projectile.Center, 0, 0, DustID.Electric, vector166.X, vector166.Y);
                                Main.dust[num825].noGravity = true;
                                Main.dust[num825].scale = 1.2f;
                            }
                            if (Main.rand.NextBool(5))
                            {
                                Vector2 spinningpoint60 = projectile.velocity;
                                val4 = default;
                                Vector2 vector168 = spinningpoint60.RotatedBy(1.5707963705062866, val4) * ((float)Main.rand.NextDouble() - 0.5f) * projectile.width;
                                int num826 = Dust.NewDust(projectile.Center + vector168 - Vector2.One * 4f, 8, 8, DustID.Smoke, 0f, 0f, 100, default, 1.5f);
                                Dust dust137 = Main.dust[num826];
                                Dust dust212 = dust137;
                                dust212.velocity *= 0.5f;
                                Main.dust[num826].velocity.Y = 0f - Math.Abs(Main.dust[num826].velocity.Y);
                            }
                        }
                    }
                    else
                    {
                        if (projectile.frameCounter < projectile.extraUpdates * 2)
                        {
                            return false;
                        }
                        projectile.frameCounter = 0;
                        float num827 = projectile.velocity.Length();
                        UnifiedRandom unifiedRandom = new((int)projectile.ai[1]);
                        int num828 = 0;
                        Vector2 spinningpoint7 = -Vector2.UnitY;
                        while (true)
                        {
                            int num829 = unifiedRandom.Next();
                            projectile.ai[1] = num829;
                            num829 %= 100;
                            float f = num829 / 100f * ((float)Math.PI * 2f);
                            Vector2 vector169 = f.ToRotationVector2();
                            if (vector169.Y > 0f)
                            {
                                vector169.Y *= -1f;
                            }
                            bool flag30 = false;
                            if (vector169.Y > -0.02f)
                            {
                                flag30 = true;
                            }
                            if (vector169.X * (projectile.extraUpdates + 1) * 2f * num827 + projectile.localAI[0] > 40f)
                            {
                                flag30 = true;
                            }
                            if (vector169.X * (projectile.extraUpdates + 1) * 2f * num827 + projectile.localAI[0] < -40f)
                            {
                                flag30 = true;
                            }
                            if (flag30)
                            {
                                if (num828++ >= 100)
                                {
                                    projectile.velocity = Vector2.Zero;
                                    projectile.localAI[1] = 1f;
                                    break;
                                }
                                continue;
                            }
                            spinningpoint7 = vector169;
                            break;
                        }
                        if (projectile.velocity != Vector2.Zero)
                        {
                            projectile.localAI[0] += spinningpoint7.X * (projectile.extraUpdates + 1) * 2f * num827;
                            Vector2 spinningpoint61 = spinningpoint7;
                            double radians47 = projectile.ai[0] + (float)Math.PI / 2f;
                            val4 = default;
                            projectile.velocity = spinningpoint61.RotatedBy(radians47, val4) * num827;
                            projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
                        }
                    }
                    return false;
                }
            }
            return true;
        }
    }
    public sealed class LunaticCultistPetItem : PetTooltip
    {
        public override PetEffect PetsEffect => phantasmalDragon;
        public static PhantasmalDragon phantasmalDragon
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out PhantasmalDragon pet))
                    return pet;
                else
                    return ModContent.GetInstance<PhantasmalDragon>();
            }
        }
        public override string PetsTooltip
        {
            get
            {
                string currentAbilityTooltip = phantasmalDragon.currentAbility switch
                {
                    0 => PetUtils.LocVal("PetItemTooltips.DragonIce") //Ice
                    .Replace("<damage>", phantasmalDragon.iceBase.ToString())
                    .Replace("<slowAmount>", Math.Round(phantasmalDragon.iceSlow * 100, 2).ToString())
                    .Replace("<slowTime>", Math.Round(phantasmalDragon.iceSlowDuration / 60f, 2).ToString()),
                    1 => PetUtils.LocVal("PetItemTooltips.DragonLightning") //Lightning, slow time not included in tooltip, a bit unnecessary info its 0.166 of a second
                    .Replace("<orbDmg>", phantasmalDragon.lightningOrbBase.ToString())
                    .Replace("<orbPen>", (phantasmalDragon.lightningStrikePen / phantasmalDragon.lightningStrikeDivide).ToString())
                    .Replace("<orbSlow>", Math.Round(phantasmalDragon.lightningSlow * 100, 2).ToString())
                    .Replace("<strikeDmg>", (phantasmalDragon.lightningOrbBase / phantasmalDragon.lightningStrikeDivide).ToString())
                    .Replace("<strikePen>", phantasmalDragon.lightningStrikePen.ToString())
                    .Replace("<strikeSlow>", Math.Round(phantasmalDragon.lightningSlow / phantasmalDragon.lightningStrikeDivide * 100, 2).ToString()),
                    2 => PetUtils.LocVal("PetItemTooltips.DragonFire") //Fireball
                    .Replace("<fireballAmount>", (phantasmalDragon.fireVolleyFrames / phantasmalDragon.fireVolleyEveryFrame).ToString())
                    .Replace("<fireballDuration>", Math.Round(phantasmalDragon.fireVolleyFrames / 60f, 2).ToString())
                    .Replace("<fireDmg>", phantasmalDragon.fireBase.ToString())
                    .Replace("<kb>", phantasmalDragon.fireKnockback.ToString())
                    .Replace("<burnSeconds>", Math.Round(phantasmalDragon.fireBurnTime / 60f, 2).ToString())
                    .Replace("<enabled>", ModContent.GetInstance<PetPersonalization>().PhantasmalDragonVolleyFromMouth ? PetUtils.LocVal("Misc.Enabled") : PetUtils.LocVal("Misc.Disabled")),
                    _ => "Cannot Find current ability.",
                };
                return PetUtils.LocVal("PetItemTooltips.LunaticCultistPetItem")
                    .Replace("<switchKeybind>", PetUtils.KeybindText(PetKeybinds.PetAbilitySwitch))
                    .Replace("<keybind>", PetUtils.KeybindText(PetKeybinds.UsePetAbility))
                    .Replace("<cooldown>", Math.Round(phantasmalDragon.phantasmDragonCooldown / 60f, 2).ToString())
                    .Replace("<tooltip>", "- " + phantasmalDragon.CurrentSpellName + "\n" + currentAbilityTooltip);
            }
        }
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.LunaticCultistPetItem")
            .Replace("<switchKeybind>", PetUtils.KeybindText(PetKeybinds.PetAbilitySwitch)).Replace("<keybind>", PetUtils.KeybindText(PetKeybinds.UsePetAbility));
    }
}
