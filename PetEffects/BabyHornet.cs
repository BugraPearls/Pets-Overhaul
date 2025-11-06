using Microsoft.Xna.Framework;
using PetsOverhaul.Projectiles;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class BabyHornet : PetEffect
    {
        public override int PetItemID => ItemID.Nectar;
        public override PetClasses PetClassPrimary => PetClasses.Summoner;
        public override PetClasses PetClassSecondary => PetClasses.Utility;
        public int beeCooldown = 90;
        public int beeDmg = 6;
        public float beeKb = 0.9f;
        public float dmgReduction = 0.04f;
        public int defReduction = 4;
        public int maxMinion = 1;
        public int critReduction = 4;
        public float moveSpdIncr = 0.04f;
        public int beeChance = 7;
        public override int PetAbilityCooldown => beeCooldown;
        public override void PostUpdateBuffs()
        {
            if (PetIsEquipped())
            {
                Player.buffImmune[BuffID.Poisoned] = true;
                Player.buffImmune[BuffID.Venom] = true;
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.statDefense -= defReduction;
                Player.GetDamage<GenericDamageClass>() -= dmgReduction;
                Player.GetCritChance<GenericDamageClass>() -= critReduction;
                Player.moveSpeed += moveSpdIncr;
                Player.maxMinions += maxMinion;
            }
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && Pet.timer <= 0)
            {
                SpawnBees(hit.DamageType, target.Center);
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.timer <= 0 && PetIsEquipped() && proj.GetGlobalProjectile<ProjectileSourceChecks>().petProj == false)
            {
                SpawnBees(hit.DamageType, target.Center);
            }
        }
        public void SpawnBees(DamageClass dmgType, Vector2 pos)
        {
            int summonMult = 1;
            if (dmgType == DamageClass.Summon || dmgType == DamageClass.SummonMeleeSpeed || dmgType == DamageClass.MagicSummonHybrid)
            {
                summonMult = 2;
            }
            for (int i = 0; i < GlobalPet.Randomizer(beeChance * summonMult); i++)
            {
                if (Player.strongBees == true && Main.rand.NextBool(1, 3))
                {
                    Projectile petProjectile = Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), pos, Main.rand.NextVector2CircularEdge(7f, 7f), ProjectileID.GiantBee, Pet.PetDamage(beeDmg * 2, DamageClass.Summon), beeKb * 2, Player.whoAmI);
                    petProjectile.DamageType = DamageClass.Summon;
                    petProjectile.CritChance = (int)Player.GetTotalCritChance(DamageClass.Summon);
                }
                else
                {
                    Projectile petProjectile = Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), pos, Main.rand.NextVector2CircularEdge(7f, 7f), ProjectileID.Bee, Pet.PetDamage(beeDmg, DamageClass.Summon), beeKb, Player.whoAmI);
                    petProjectile.DamageType = DamageClass.Summon;
                    petProjectile.CritChance = (int)Player.GetTotalCritChance(DamageClass.Summon);
                }
                Pet.timer = Pet.timerMax;
            }
        }
    }
    public sealed class Nectar : PetTooltip
    {
        public override PetEffect PetsEffect => babyHornet;
        public static BabyHornet babyHornet
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out BabyHornet pet))
                    return pet;
                else
                    return ModContent.GetInstance<BabyHornet>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.Nectar")
                .Replace("<moveSpd>", Math.Round(babyHornet.moveSpdIncr * 100, 2).ToString())
                .Replace("<def>", babyHornet.defReduction.ToString())
                .Replace("<dmgCrit>", Math.Round(babyHornet.dmgReduction * 100, 2).ToString())
                .Replace("<maxMinion>", babyHornet.maxMinion.ToString())
                .Replace("<regularChance>", babyHornet.beeChance.ToString())
                .Replace("<summonChance>", (babyHornet.beeChance * 2).ToString())
                .Replace("<beeDmg>", babyHornet.beeDmg.ToString())
                .Replace("<beeKb>", babyHornet.beeKb.ToString())
                .Replace("<beeCd>", Math.Round(babyHornet.beeCooldown / 60f, 2).ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.Nectar");
    }
}
