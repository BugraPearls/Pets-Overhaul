﻿using PetsOverhaul.Systems;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Spiffo : PetEffect
    {
        public override int PetItemID => ItemID.SpiffoPlush;
        public int ammoReserveChance = 20;
        public int zombieArmorPen = 6;
        public int penetrateChance = 75;

        public override PetClasses PetClassPrimary => PetClasses.Ranged;
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && NPCID.Sets.Zombies[target.type] && modifiers.DamageType.Type == DamageClass.Ranged.Type)
            {
                modifiers.ArmorPenetration += zombieArmorPen;
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && target.active == false && proj.CountsAsClass<RangedDamageClass>() && proj.penetrate >= 0)
            {
                proj.penetrate += GlobalPet.Randomizer(penetrateChance);
                if (proj.usesLocalNPCImmunity == false)
                {
                    proj.usesLocalNPCImmunity = true;
                    proj.localNPCHitCooldown = 10;
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
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.SpiffoPlush")
                        .Replace("<ammoReserve>", spiffo.ammoReserveChance.ToString())
                        .Replace("<armorPen>", spiffo.zombieArmorPen.ToString())
                        .Replace("<penChance>", spiffo.penetrateChance.ToString());
    }
}