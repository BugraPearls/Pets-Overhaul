using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Player;

namespace PetsOverhaul.PetEffects
{
    public sealed class SkeletronJr : PetEffect
    {
        public override int PetItemID => ItemID.SkeletronPetItem;
        public List<(int, int)> skeletronTakenDamage = new();
        private int timer = 0;
        public float enemyDamageIncrease = 1.2f;
        public float playerDamageTakenSpeed = 4f;
        public float playerTakenMult = 1.00f;
        public override int PetStackCurrent
        {
            get
            {
                int val = 0;
                skeletronTakenDamage.ForEach(x => val += x.Item1);
                return val;
            }
        }
        public override int PetStackMax => 0;
        public override string PetStackText => PetTextsColors.LocVal("PetItemTooltips.SkeletronPetItemStack");
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override void ExtraPreUpdate()
        {
            timer++;
            if (timer > 100)
            {
                timer = 100;
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (skeletronTakenDamage.Count > 0 && timer >= 60)
            {
                int totalDmg = 0;
                skeletronTakenDamage.ForEach(x => totalDmg += (int)Math.Ceiling(x.Item2 / playerDamageTakenSpeed));
                Player.statLife -= totalDmg;
                CombatText.NewText(Player.getRect(), CombatText.DamagedHostile, totalDmg);
                if (Player.statLife <= 0)
                {
                    string reason;
                    switch (Main.rand.Next(20))
                    {
                        case 0:
                            reason = PetTextsColors.LocVal("PetItemTooltips.SkeletronDeath3");
                            break;
                        case 1:
                            reason = PetTextsColors.LocVal("PetItemTooltips.SkeletronDeath4");
                            break;
                        default:
                            if (Main.rand.NextBool())
                            {
                                reason = PetTextsColors.LocVal("PetItemTooltips.SkeletronDeath1");
                            }
                            else
                            {
                                reason = PetTextsColors.LocVal("PetItemTooltips.SkeletronDeath2");
                            }
                            reason = reason.Replace("<name>", Player.name);
                            break;
                    }
                    Player.KillMe(PlayerDeathReason.ByCustomReason(reason), 1, 0);
                }

                for (int i = 0; i < skeletronTakenDamage.Count; i++)
                {
                    (int, int) point = skeletronTakenDamage[i];
                    point.Item1 -= (int)Math.Ceiling(point.Item2 / playerDamageTakenSpeed);
                    skeletronTakenDamage[i] = point;
                }
                skeletronTakenDamage.RemoveAll(x => x.Item1 <= 0);

                timer = 0;
            }
        }
        public override bool ConsumableDodge(HurtInfo info)
        {
            if (PetIsEquipped())
            {
                Player.lifeRegenTime = 0;
                PlayerLoader.OnHurt(Player, info);
                PlayerLoader.PostHurt(Player, info);
                skeletronTakenDamage.Add((info.Damage, info.Damage));
                if (info.Damage <= 1)
                {
                    Player.SetImmuneTimeForAllTypes(Player.longInvince ? 40 : 20);
                }
                else
                {
                    Player.SetImmuneTimeForAllTypes(Player.longInvince ? 80 : 40);
                }
                return true;
            }
            return base.ConsumableDodge(info);
        }
        public override void UpdateDead()
        {
            skeletronTakenDamage.Clear();
        }
    }
    public sealed class SkeletronJrEnemy : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public List<(int, int)> skeletronDealtDamage = new();
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (skeletronDealtDamage.Count > 0)
            {
                int totalDmg = 0;
                skeletronDealtDamage.ForEach(x => totalDmg += x.Item1);
                npc.lifeRegen -= totalDmg / 2;
                damage = totalDmg / 5;
            }
        }
        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            if (player.GetModPlayer<GlobalPet>().PetInUseWithSwapCd(ItemID.SkeletronPetItem))
            {
                npc.life += damageDone;
                skeletronDealtDamage.Add(((int)(damageDone * player.GetModPlayer<SkeletronJr>().enemyDamageIncrease), 240));
            }
        }
        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (Main.player[projectile.owner].GetModPlayer<GlobalPet>().PetInUseWithSwapCd(ItemID.SkeletronPetItem))
            {
                npc.life += damageDone;
                skeletronDealtDamage.Add(((int)(damageDone * Main.player[projectile.owner].GetModPlayer<SkeletronJr>().enemyDamageIncrease), 240));
            }
        }
        public override bool PreAI(NPC npc)
        {
            if (skeletronDealtDamage.Count > 0)
            {
                for (int i = 0; i < skeletronDealtDamage.Count; i++) //List'lerde struct'lar bir nevi readonly olarak çalıştığından, değeri alıp tekrar atıyoruz
                {
                    (int, int) point = skeletronDealtDamage[i];
                    point.Item2--;
                    skeletronDealtDamage[i] = point;
                }
                skeletronDealtDamage.RemoveAll(x => x.Item2 <= 0);
            }

            return base.PreAI(npc);
        }
        public override void OnKill(NPC npc)
        {
            if (skeletronDealtDamage.Count > 0)
                skeletronDealtDamage.Clear();
        }
    }
    public sealed class SkeletronPetItem : PetTooltip
    {
        public override PetEffect PetsEffect => skeletronJr;
        public static SkeletronJr skeletronJr
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out SkeletronJr pet))
                    return pet;
                else
                    return ModContent.GetInstance<SkeletronJr>();
            }
        }
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.SkeletronPetItem")
                        .Replace("<recievedMult>", skeletronJr.playerTakenMult.ToString())
                        .Replace("<recievedHowLong>", skeletronJr.playerDamageTakenSpeed.ToString())
                        .Replace("<dealtMult>", skeletronJr.enemyDamageIncrease.ToString());
        public override string SimpleTooltip => PetTextsColors.LocVal("SimpleTooltips.SkeletronPetItem");
    }
}
