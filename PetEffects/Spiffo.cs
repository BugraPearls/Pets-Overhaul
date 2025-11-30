using PetsOverhaul.Achievements;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Spiffo : PetEffect
    {
        public override int PetItemID => ItemID.SpiffoPlush;
        public int ammoReserveChance = 20;
        public int zombieArmorPen = 6;
        public int penetrateChance = 75;

        public override PetClass PetClassPrimary => PetClassID.Ranged;
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && NPCID.Sets.Zombies[target.type] && modifiers.DamageType.Type == DamageClass.Ranged.Type)
            {
                modifiers.ArmorPenetration += zombieArmorPen;
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && target.active == false)
            {
                if (proj.CountsAsClass<RangedDamageClass>() && proj.penetrate >= 0)
                {
                    proj.penetrate += PetUtils.Randomizer(penetrateChance);
                    if (proj.usesLocalNPCImmunity == false)
                    {
                        proj.usesLocalNPCImmunity = true;
                        proj.localNPCHitCooldown = 10;
                    }
                }
                if (NPCID.Sets.Zombies[target.type])
                {
                    ModContent.GetInstance<ThisWontBeHowIDie>().Kills.Value++;
                }
            }
        }
        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            return (!PetIsEquipped() || !Main.rand.NextBool(ammoReserveChance, 100)) && base.CanConsumeAmmo(weapon, ammo);
        }
    }
    public sealed class SpiffoPlush : PetTooltip
    {
        public override PetEffect PetsEffect => spiffo;
        public static Spiffo spiffo
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out Spiffo pet))
                    return pet;
                else
                    return ModContent.GetInstance<Spiffo>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.SpiffoPlush")
                        .Replace("<ammoReserve>", spiffo.ammoReserveChance.ToString())
                        .Replace("<armorPen>", spiffo.zombieArmorPen.ToString())
                        .Replace("<penChance>", spiffo.penetrateChance.ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.SpiffoPlush");
    }
}