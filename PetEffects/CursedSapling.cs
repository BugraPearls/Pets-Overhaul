using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class CursedSapling : PetEffect
    {
        public override int PetItemID => ItemID.CursedSapling;
        public override PetClasses PetClassPrimary => PetClasses.Summoner;
        public float whipSpeed = 0.0075f;
        public float whipRange = 0.01f;
        public float pumpkinWeaponDmg = 0.1f;
        public float ravenDmg = 0.075f;
        public int maxMinion = 1;
        public float darkHarvestMult = 1.5f;
        public static List<int> PumpkinMoonWeapons = [ItemID.StakeLauncher, ItemID.TheHorsemansBlade, ItemID.BatScepter, ItemID.CandyCornRifle, ItemID.ScytheWhip, ItemID.JackOLanternLauncher, ItemID.RavenStaff];
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && PumpkinMoonWeapons.Contains(item.type))
            {
                damage += pumpkinWeaponDmg;
                if (item.type == ItemID.RavenStaff)
                {
                    damage += ravenDmg;
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.maxMinions += maxMinion;
                if (Player.HeldItem.type == ItemID.ScytheWhip)
                {
                    Player.GetAttackSpeed<SummonMeleeSpeedDamageClass>() += Player.maxMinions * whipSpeed * darkHarvestMult;
                    Player.whipRangeMultiplier += Player.maxMinions * whipRange * darkHarvestMult;
                }
                else if (Player.HeldItem.CountsAsClass<SummonMeleeSpeedDamageClass>())
                {
                    Player.GetAttackSpeed<SummonMeleeSpeedDamageClass>() += Player.maxMinions * whipSpeed;
                    Player.whipRangeMultiplier += Player.maxMinions * whipRange;
                }
            }
        }
    }
    public sealed class CursedSaplingItem : PetTooltip
    {
        public override PetEffect PetsEffect => cursedSapling;
        public static CursedSapling cursedSapling
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out CursedSapling pet))
                    return pet;
                else
                    return ModContent.GetInstance<CursedSapling>();
            }
        }
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.CursedSapling")
                        .Replace("<minionSlot>", cursedSapling.maxMinion.ToString())
                        .Replace("<dmg>", Math.Round(cursedSapling.pumpkinWeaponDmg * 100, 2).ToString())
                        .Replace("<weapons>", PetTextsColors.ItemsToTooltipImages(CursedSapling.PumpkinMoonWeapons))
                        .Replace("<ravenDmg>", Math.Round(cursedSapling.ravenDmg * 100, 2).ToString())
                        .Replace("<whipRange>", Math.Round(cursedSapling.whipRange * 100, 2).ToString())
                        .Replace("<whipSpeed>", Math.Round(cursedSapling.whipSpeed * 100, 2).ToString())
                        .Replace("<darkHarvestMult>", cursedSapling.darkHarvestMult.ToString());
        public override string SimpleTooltip => PetTextsColors.LocVal("SimpleTooltips.CursedSapling");
    }
}
