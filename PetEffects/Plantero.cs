using Microsoft.Xna.Framework;
using PetsOverhaul.Achievements;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Plantero : PetEffect
    {
        public override int PetItemID => ItemID.MudBud;
        public int spawnChance = 15;
        public float damageMult = 0.7f;
        public float knockBack = 0.4f;
        public int flatDmg = 15;
        public int pen = 20;
        private int timerForAchievementPepper = 0;
        private int timerForAchievementNachos = 0;
        public override PetClass PetClassPrimary => PetClassID.Offensive;
        public override bool CanUseItem(Item item)
        {
            if (PetIsEquipped())
            {
                if (item.type == ItemID.SpicyPepper)
                {
                    timerForAchievementPepper = 60;
                    if (timerForAchievementNachos > 0)
                        PetUtils.DoAchievementOnPlayer<PlantaHermano>(Player.whoAmI);
                }
                if (item.type == ItemID.Nachos)
                {
                    timerForAchievementNachos = 60;
                    if (timerForAchievementPepper > 0)
                        PetUtils.DoAchievementOnPlayer<PlantaHermano>(Player.whoAmI);
                }
            }
            return base.CanUseItem(item);
        }
        public override void ExtraPreUpdate()
        {
            timerForAchievementPepper--;
            if (timerForAchievementPepper < 0)
                timerForAchievementPepper = 0;
            timerForAchievementNachos--;
            if (timerForAchievementNachos < 0)
                timerForAchievementNachos = 0;
        }
        public void SpawnGasCloud(NPC target, int damage, DamageClass dmgType)
        {
            if (PetIsEquipped())
            {
                for (int i = 0; i < PetUtils.Randomizer(spawnChance + (int)(spawnChance * Pet.abilityHaste)); i++)
                {
                    Vector2 location = new(target.Center.X + Main.rand.NextFloat(-2f, 2f), target.Center.Y + Main.rand.NextFloat(-2f, 2f));
                    Vector2 velocity = new(Main.rand.NextFloat(-1.5f, 1.5f), Main.rand.NextFloat(-1.5f, 1.5f));
                    short projId = Main.rand.Next(3) switch
                    {
                        0 => ProjectileID.SporeGas,
                        1 => ProjectileID.SporeGas2,
                        2 => ProjectileID.SporeGas3,
                        _ => ProjectileID.SporeGas,
                    };
                    ;
                    Projectile gas = Projectile.NewProjectileDirect(PetUtils.GetSource_Pet(EntitySourcePetIDs.PetProjectile), location, velocity, projId, Pet.PetDamage(damage * damageMult + flatDmg, dmgType), knockBack, Main.myPlayer);
                    gas.Resize(gas.width * 2, gas.height * 2);
                    gas.scale *= 2;
                    gas.penetrate = pen;
                    gas.DamageType = dmgType;
                    gas.CritChance = (int)Player.GetTotalCritChance(dmgType);
                }
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (proj.GetGlobalProjectile<PetGlobalProjectile>().petProj == false)
            {
                SpawnGasCloud(target, hit.SourceDamage, hit.DamageType);
            }
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            SpawnGasCloud(target, hit.SourceDamage, hit.DamageType);
        }
    }
    public sealed class MudBud : PetTooltip
    {
        public override PetEffect PetsEffect => plantero;
        public static Plantero plantero
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out Plantero pet))
                    return pet;
                else
                    return ModContent.GetInstance<Plantero>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.MudBud")
                        .Replace("<chance>", plantero.spawnChance.ToString())
                        .Replace("<dmg>", plantero.damageMult.ToString())
                        .Replace("<flatDmg>", plantero.flatDmg.ToString())
                        .Replace("<kb>", plantero.knockBack.ToString())
                        .Replace("<pen>", plantero.pen.ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.MudBud");
    }
}
