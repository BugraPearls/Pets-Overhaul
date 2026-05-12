using Microsoft.Xna.Framework;
using PetsOverhaul.Systems;
using System;
using System.IO;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class WispInABottleEffect : LightPetEffect
    {
        public override int LightPetItemID => ItemID.WispinaBottle; //Custom effect dedicated to ROH
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Main.HoverItem.type == LightPetItemID && PetKeybinds.PetCustomSwitch.JustPressed) //We want custom effect to swap when hovered over the item of same id
            {
                CustomActive = !CustomActive;
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    ModPacket packet = ModContent.GetInstance<PetsOverhaul>().GetPacket();
                    packet.Write((byte)MessageType.CustomEffectSwitch);
                    packet.Write(Index);
                    packet.Send();
                }
            }
        }
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out WispInABottle wispInABottle))
            {
                if (wispInABottle.CustomEffectActive == false)
                {
                    Player.GetDamage<MagicDamageClass>() += wispInABottle.MagicRangedDamage;
                    Player.GetDamage<RangedDamageClass>() += wispInABottle.MagicRangedDamage;
                    Pet.petDirectDamageMultiplier += wispInABottle.PetDamage.CurrentStatFloat;
                }
            }
        }
        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (TryGetLightPet(out WispInABottle wispInABottle) && (item.DamageType == DamageClass.Magic || item.DamageType == DamageClass.Ranged))
            {
                if (wispInABottle.CustomEffectActive == false)
                {
                    velocity *= wispInABottle.ProjectileVelocity.CurrentStatFloat + 1;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (TryGetLightPet(out WispInABottle wispInABottle) && wispInABottle.CustomEffectActive && Main.rand.NextBool((int)Math.Clamp(wispInABottle.ProcChance.CurrentStatFloat*100,0,100), 100))
            {
                Projectile theWisp = null;
                foreach (var projectile in Main.ActiveProjectiles)
                {
                    if (projectile is Projectile proj && proj.owner == Player.whoAmI && proj.type == ProjectileID.Wisp)
                    {
                        theWisp = projectile;
                    }
                }
                if (theWisp != null)
                {
                    SpawnGhostHurt(theWisp, (int)(wispInABottle.MultDamage.CurrentStatFloat * damageDone) + wispInABottle.FlatDamage, target);
                }
            }
        }
        /// <summary>
        /// Mostly copy-paste of vanilla Spectre Armor code.
        /// </summary>
        public void SpawnGhostHurt(Projectile wisp, int dmg, Entity Victim)
        {
            if (dmg <= 1)
            {
                return;
            }
            int num2 = -1;
            int[] array = new int[200];
            int num4 = 0;
            _ = new int[200];
            int num5 = 0;
            for (int i = 0; i < 200; i++)
            {
                if (!Main.npc[i].CanBeChasedBy(this))
                {
                    continue;
                }
                float num6 = Math.Abs(Main.npc[i].position.X + (float)(Main.npc[i].width / 2) - wisp.position.X + (float)(wisp.width / 2)) + Math.Abs(Main.npc[i].position.Y + (float)(Main.npc[i].height / 2) - wisp.position.Y + (float)(wisp.height / 2));
                if (num6 < 800f)
                {
                    if (Collision.CanHit(wisp.position, 1, 1, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height) && num6 > 50f)
                    {
                        array[num5] = i;
                        num5++;
                    }
                    else if (num5 == 0)
                    {
                        array[num4] = i;
                        num4++;
                    }
                }
            }
            if (num4 != 0 || num5 != 0)
            {
                num2 = ((num5 <= 0) ? array[Main.rand.Next(num4)] : array[Main.rand.Next(num5)]);
                float num7 = Main.rand.Next(-100, 101);
                float num8 = Main.rand.Next(-100, 101);
                float num9 = (float)Math.Sqrt(num7 * num7 + num8 * num8);
                num9 = 4f / num9;
                num7 *= num9;
                num8 *= num9;
                Projectile.NewProjectile(wisp.GetSource_OnHit(Victim), wisp.position.X, wisp.position.Y, num7, num8, ProjectileID.SpectreWrath, dmg, 0f, Player.whoAmI, num2);
            }
        }
    }
    public sealed class WispInABottle : LightPetItem //Check on 'custom effect'
    {
        public LightPetStat MagicRangedDamage = new(20, 0.004f, "Damage", 0.04f, LegacyKeysToInherit: [("WispMagic",20),("WispRanged",20)]);
        public LightPetStat ProjectileVelocity = new(12, 0.01f, "Velocity", 0.05f, LegacyKeysToInherit: ("WispProjSpd",12));
        public LightPetStat PetDamage = new(25, 0.0065f, "PetDamage", 0.0675f, LegacyKeysToInherit: ("WispProjPet",25));

        public CustomLightPetStat FlatDamage => new(MagicRangedDamage, 2, "Flat",10);
        public CustomLightPetStat MultDamage => new(PetDamage, 0.02f, "Mult", 0.1f);
        public CustomLightPetStat ProcChance => new(ProjectileVelocity, 0.02f, "Chance", 0.1f);

        public override int LightPetItemID => ItemID.WispinaBottle;
        public override bool HasCustomEffect => true;
        public override bool CustomEffectActive => Main.LocalPlayer.GetModPlayer<WispInABottleEffect>().CustomActive; //We make it so it uses the ModPlayer's CustomActive when access to the property is required.
        public override string BaseTooltip => PetUtils.LocVal("LightPetTooltips.WispInABottle");
        public override string CustomPetsTooltip => PetUtils.LocVal("CustomPetEffects.WispInABottle");
    }
}