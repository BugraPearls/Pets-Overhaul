using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class TheTwins : PetEffect
    {
        public override int PetItemID => ItemID.TwinsPetItem;
        public int cooldown = 48;
        public int closeRange = 140;
        public int longRange = 560;
        public float regularEnemyHpDmg = 0.01f;
        public float bossHpDmg = 0.0008f;
        public int infernoTime = 240;
        public int percDamageCap = 35;
        public int defDrop = 2;

        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override PetClasses PetClassSecondary => PetClasses.Utility;
        public override int PetAbilityCooldown => cooldown;
        public override void ExtraPreUpdateNoCheck()
        {
            if (PetIsEquipped())
            {
                PetUtils.CircularDustEffect(Player.Center, DustID.CursedTorch, closeRange, 6);
                PetUtils.CircularDustEffect(Player.Center, DustID.RedTorch, longRange, 30);
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped())
            {
                if (Player.Distance(target.Center) > longRange && Pet.timer <= 0 && target.immortal == false)
                {

                    float bonus;
                    if (target.boss == false || NPCGlobalPet.NonBossTrueBosses.Contains(target.type) == false)
                    {
                        bonus = target.lifeMax * regularEnemyHpDmg;
                    }
                    else
                    {
                        bonus = Math.Min(target.lifeMax * bossHpDmg, percDamageCap); //modifiers.SourceDamage is the actual damage, so that has to be increased for scaling bonus damages.
                    }
                    modifiers.FlatBonusDamage += Player.GetTotalDamage(modifiers.DamageType).ApplyTo(bonus);
                    Pet.timer = Pet.timerMax;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Player.Distance(target.Center) < closeRange && PetUtils.LifestealCheck(target) && PetIsEquipped())
            {
                if (Pet.timer <= 0 && target.TryGetGlobalNPC(out TwinsPermaDefDrop defDrop))
                {
                    target.AddBuff(BuffID.CursedInferno, infernoTime);
                    defDrop.currentTwinDrop += 2;
                    Pet.timer = Pet.timerMax;
                }
            }
        }
    }
    public sealed class TwinsPermaDefDrop : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public int currentTwinDrop = 0;
        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            modifiers.Defense.Flat -= currentTwinDrop;
        }
    }
    public sealed class TwinsPetItem : PetTooltip
    {
        public override PetEffect PetsEffect => theTwins;
        public static TheTwins theTwins
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out TheTwins pet))
                    return pet;
                else
                    return ModContent.GetInstance<TheTwins>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.TwinsPetItem")
                        .Replace("<closeRange>", Math.Round(theTwins.closeRange / 16f, 2).ToString())
                        .Replace("<defReduce>", theTwins.defDrop.ToString())
                        .Replace("<cursedTime>", Math.Round(theTwins.infernoTime / 60f, 2).ToString())
                        .Replace("<longRange>", Math.Round(theTwins.longRange / 16f, 2).ToString())
                        .Replace("<hpDmg>", Math.Round(theTwins.regularEnemyHpDmg * 100, 2).ToString())
                        .Replace("<bossHpDmg>", Math.Round(theTwins.bossHpDmg * 100, 2).ToString())
                        .Replace("<bossCap>", theTwins.percDamageCap.ToString())
                        .Replace("<hpDmgCooldown>", Math.Round(theTwins.cooldown / 60f, 2).ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.TwinsPetItem");
    }
}
