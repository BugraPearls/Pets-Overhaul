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
        public int timer = 0;
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
        public override void PreUpdate()
        {
            if (timer > 0)
            {
                timer--;
            }
        }
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out WispInABottle wispInABottle))
            {
                if (wispInABottle.CustomEffectActive == false)
                {
                    Player.GetDamage<MagicDamageClass>() += wispInABottle.MagicDamage.CurrentStatFloat;
                    Player.GetDamage<RangedDamageClass>() += wispInABottle.RangedDamage.CurrentStatFloat;
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
            if (TryGetLightPet(out WispInABottle wispInABottle) && wispInABottle.CustomEffectActive && Main.rand.NextBool(wispInABottle.CustomChance, 100) && timer <= 0)
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
                    SpawnGhostHurt(theWisp, wispInABottle.CustomFlat + (int)(wispInABottle.CustomScaling * damageDone), target);
                    timer = wispInABottle.CustomCooldown;
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
                Projectile.NewProjectile(wisp.GetSource_OnHit(Victim), wisp.position.X, wisp.position.Y, num7, num8, 356, dmg, 0f, Player.whoAmI, num2);
            }
        }
    }
    public sealed class WispInABottle : LightPetItem
    {
        public LightPetStat MagicDamage = new(20, 0.004f, 0.04f);
        public LightPetStat RangedDamage = new(20, 0.004f, 0.04f);
        public LightPetStat ProjectileVelocity = new(12, 0.01f, 0.05f);
        public LightPetStat PetDamage = new(25, 0.0065f, 0.0675f);
        public int CustomFlat => MagicDamage.CurrentRoll + 10;
        public float CustomScaling => RangedDamage.CurrentRoll * 0.004f + 0.03f;
        public int CustomCooldown => ProjectileVelocity.CurrentRoll * -5 + 120;
        public int CustomChance => PetDamage.CurrentRoll + 15;
        public override int LightPetItemID => ItemID.WispinaBottle;
        public override bool HasCustomEffect => true;
        public override bool CustomEffectActive => Main.LocalPlayer.GetModPlayer<WispInABottleEffect>().CustomActive; //We make it so it uses the ModPlayer's CustomActive when access to the property is required.
        public override void UpdateInventory(Item item, Player player)
        {
            MagicDamage.SetRoll(player.luck);
            RangedDamage.SetRoll(player.luck);
            ProjectileVelocity.SetRoll(player.luck);
            PetDamage.SetRoll(player.luck);
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)MagicDamage.CurrentRoll);
            writer.Write((byte)RangedDamage.CurrentRoll);
            writer.Write((byte)ProjectileVelocity.CurrentRoll);
            writer.Write((byte)PetDamage.CurrentRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            MagicDamage.CurrentRoll = reader.ReadByte();
            RangedDamage.CurrentRoll = reader.ReadByte();
            ProjectileVelocity.CurrentRoll = reader.ReadByte();
            PetDamage.CurrentRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("WispMagic", MagicDamage.CurrentRoll);
            tag.Add("WispRanged", RangedDamage.CurrentRoll);
            tag.Add("WispProjSpd", ProjectileVelocity.CurrentRoll);
            tag.Add("WispProjPet", PetDamage.CurrentRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("WispMagic", out int magic))
            {
                MagicDamage.CurrentRoll = magic;
            }

            if (tag.TryGet("WispRanged", out int ranged))
            {
                RangedDamage.CurrentRoll = ranged;
            }

            if (tag.TryGet("WispProjSpd", out int projSpd))
            {
                ProjectileVelocity.CurrentRoll = projSpd;
            }

            if (tag.TryGet("WispProjPet", out int petProj))
            {
                PetDamage.CurrentRoll = petProj;
            }
        }
        public override int GetRoll() => MagicDamage.CurrentRoll;
        public override string PetsTooltip => PetUtils.LocVal("LightPetTooltips.WispInABottle")

                        .Replace("<magic>", MagicDamage.BaseAndPerQuality())
                        .Replace("<ranged>", RangedDamage.BaseAndPerQuality())
                        .Replace("<velocity>", ProjectileVelocity.BaseAndPerQuality())
                        .Replace("<petDmg>", PetDamage.BaseAndPerQuality())

                        .Replace("<magicLine>", MagicDamage.StatSummaryLine())
                        .Replace("<rangedLine>", RangedDamage.StatSummaryLine())
                        .Replace("<velocityLine>", ProjectileVelocity.StatSummaryLine())
                        .Replace("<petDmgLine>", PetDamage.StatSummaryLine());
        public override string CustomPetsTooltip => PetUtils.LocVal("CustomPetEffects.WispInABottle")

                        .Replace("<chance>", CustomChance.ToString())
                        .Replace("<base>", CustomFlat.ToString())
                        .Replace("<perc>", Math.Round(CustomScaling * 100, 2).ToString())
                        .Replace("<cooldown>", Math.Round(CustomCooldown / 60f, 2).ToString())

                        .Replace("<chanceLine>", PetDamage.QualityLine())
                        .Replace("<baseLine>", MagicDamage.QualityLine())
                        .Replace("<percLine>", RangedDamage.QualityLine())
                        .Replace("<cooldownLine>", ProjectileVelocity.QualityLine());
    }
}
