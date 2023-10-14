﻿using Terraria;
using Terraria.ID;
using PetsOverhaul.Systems;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using static Terraria.Player;

namespace PetsOverhaul.PetEffects.Vanilla
{
    sealed public class SkeletronJr : ModPlayer
    {
        public List<(int, int)> skeletronTakenDamage = new(100000);
        private int timer = 0;
        public float enemyDamageIncrease = 1.2f;
        public int playerDamageTakenSpeed = 4;
        public float playerTakenMult = 1f;
        GlobalPet Pet { get => Player.GetModPlayer<GlobalPet>(); }
        public override void PreUpdate()
        {
            timer++;
            if (timer > 10000)
                timer = 10000;
        }
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.SkeletronPetItem))
            {
                if (skeletronTakenDamage.Count > 0 && timer >= 60)
                {
                    int totalDmg = 0;
                    skeletronTakenDamage.ForEach(x => totalDmg += x.Item2 / playerDamageTakenSpeed);
                    Player.statLife -= totalDmg;
                    CombatText.NewText(Player.getRect(), CombatText.DamagedHostile, totalDmg);
                    if (Player.statLife <= 0)
                        Player.KillMe(PlayerDeathReason.ByCustomReason(Player.name + " could not contain Skeletron's curse."), 1, 0);
                    for (int i = 0; i < skeletronTakenDamage.Count; i++) //List'lerde struct'lar bir nevi readonly olarak çalıştığından, değeri alıp tekrar atıyoruz
                    {
                        var point = skeletronTakenDamage[i];
                        point.Item1 -= point.Item2 / playerDamageTakenSpeed;
                        skeletronTakenDamage[i] = point;
                    }
                    skeletronTakenDamage.RemoveAll(x => x.Item1 <= 0);

                    timer = 0;
                }
            }
        }
        public override bool ConsumableDodge(HurtInfo info)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.SkeletronPetItem))
            {
                SoundEngine.PlaySound(SoundID.PlayerHit with { PitchVariance = 0.2f }, Player.position);
                skeletronTakenDamage.Add((info.Damage, info.Damage));
                if (info.Damage <= 1)
                    Player.SetImmuneTimeForAllTypes(Player.longInvince ? 40 : 20);
                else
                    Player.SetImmuneTimeForAllTypes(Player.longInvince ? 80 : 40);
                return true;
            }
            return false;
        }
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            skeletronTakenDamage.Clear();
        }
    }
    sealed public class SkeletronJrEnemy : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public List<(int, int)> skeletronDealtDamage = new(100000);
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
                    var point = skeletronDealtDamage[i];
                    point.Item2--;
                    skeletronDealtDamage[i] = point;
                }
                skeletronDealtDamage.RemoveAll(x => x.Item2 <= 0);
            }

            return true;
        }
        public override void OnKill(NPC npc)
        {
            skeletronDealtDamage.Clear();
        }
    }
}
