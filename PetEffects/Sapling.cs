using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Sapling : PetEffect
    {
        public static List<int> PlanteraWeapon = [ItemID.VenusMagnum, ItemID.NettleBurst, ItemID.LeafBlower, ItemID.FlowerPow, ItemID.WaspGun, ItemID.Seedler, ItemID.GrenadeLauncher, ItemID.TheAxe, ItemID.Seedler];
        public static List<int> PlanteraProj = [ProjectileID.Pygmy, ProjectileID.Pygmy2, ProjectileID.Pygmy3, ProjectileID.Pygmy4, ProjectileID.FlowerPow, ProjectileID.SeedlerNut];
        public override int PetItemID => ItemID.Seedling;
        public float planteraLifesteal = 0.019f;
        public float regularLifesteal = 0.015f;
        public float damagePenalty = 0.7f;
        public override PetClass PetClassPrimary => PetClassID.Defensive;
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && PetUtils.LifestealCheck(target))
            {
                modifiers.FinalDamage *= damagePenalty;
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && PetUtils.LifestealCheck(target))
            {
                if (proj.GetGlobalProjectile<PetGlobalProjectile>().isPlanteraProjectile)
                {
                    Pet.PetRecovery(damageDone, planteraLifesteal);
                }
                else
                {
                    Pet.PetRecovery(damageDone, regularLifesteal);
                }
            }
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && PetUtils.LifestealCheck(target))
            {
                if (PlanteraWeapon.Contains(item.type))
                {
                    Pet.PetRecovery(damageDone, planteraLifesteal);
                }
                else
                {
                    Pet.PetRecovery(damageDone, regularLifesteal);
                }
            }
        }
    }
    public sealed class Seedling : PetTooltip
    {
        public override PetEffect PetsEffect => sapling;
        public static Sapling sapling
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out Sapling pet))
                    return pet;
                else
                    return ModContent.GetInstance<Sapling>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.Seedling")
                .Replace("<dmgPenalty>", sapling.damagePenalty.ToString())
                .Replace("<lifesteal>", Math.Round(sapling.regularLifesteal * 100, 2).ToString())
                .Replace("<planteraSteal>", Math.Round(sapling.planteraLifesteal * 100, 2).ToString())
            .Replace("<weapons>", PetUtils.ItemsToTooltipImages(Sapling.PlanteraWeapon));
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.Seedling");
    }
}
