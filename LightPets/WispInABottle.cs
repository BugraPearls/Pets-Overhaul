﻿using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class WispInABottleEffect : ModPlayer
    {
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].type == ItemID.WispinaBottle && Player.miscEquips[1].TryGetGlobalItem(out WispInABottle wispInABottle))
            {
                Player.GetDamage<MagicDamageClass>() += wispInABottle.CurrentMagicDmg;
                Player.GetDamage<RangedDamageClass>() += wispInABottle.CurrentRangedDmg;
            }
        }
        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (Player.miscEquips[1].type == ItemID.WispinaBottle && Player.miscEquips[1].TryGetGlobalItem(out WispInABottle wispInABottle) && (item.DamageType == DamageClass.Magic || item.DamageType == DamageClass.Ranged))
            {
                velocity *= wispInABottle.CurrentProjSpd + 1;
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Player.miscEquips[1].type == ItemID.WispinaBottle && Player.miscEquips[1].TryGetGlobalItem(out WispInABottle wispInABottle) && proj.TryGetGlobalProjectile(out ProjectileSourceChecks check) && check.petProj)
            {
                modifiers.FinalDamage *= 1 + wispInABottle.CurrentPetProjectileDamage;
            }
        }
    }
    public sealed class WispInABottle : GlobalItem
    {
        public float baseMagic = 0.04f;
        public float magicPerRoll = 0.0045f;
        public int magicMaxRoll = 20;
        public int magicRoll = 0;
        public float CurrentMagicDmg => baseMagic + magicPerRoll * magicRoll;

        public float baseRanged = 0.04f;
        public float rangedPerRoll = 0.0045f;
        public int rangedMaxRoll = 20;
        public int rangedRoll = 0;
        public float CurrentRangedDmg => baseRanged + rangedPerRoll * rangedRoll;

        public float baseProjSpd = 0.05f;
        public float projSpdPerRoll = 0.01f;
        public int projSpdMaxRoll = 12;
        public int projSpdRoll = 0;
        public float CurrentProjSpd => baseProjSpd + projSpdPerRoll * projSpdRoll;

        public float baseProjPet = 0.075f;
        public float projPetPerRoll = 0.008f;
        public int projPetMaxRoll = 25;
        public int projPetRoll = 0;
        public float CurrentPetProjectileDamage => baseProjPet + projPetPerRoll * projPetRoll;

        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.WispinaBottle;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (magicRoll <= 0)
            {
                magicRoll = Main.rand.Next(magicMaxRoll) + 1;
            }

            if (rangedRoll <= 0)
            {
                rangedRoll = Main.rand.Next(rangedMaxRoll) + 1;
            }

            if (projSpdRoll <= 0)
            {
                projSpdRoll = Main.rand.Next(projSpdMaxRoll) + 1;
            }

            if (projPetRoll <= 0)
            {
                projPetRoll = Main.rand.Next(projPetMaxRoll) + 1;
            }
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)magicRoll);
            writer.Write((byte)rangedRoll);
            writer.Write((byte)projSpdRoll);
            writer.Write((byte)projPetRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            magicRoll = reader.ReadByte();
            rangedRoll = reader.ReadByte();
            projSpdRoll = reader.ReadByte();
            projPetRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("WispMagic", magicRoll);
            tag.Add("WispRanged", rangedRoll);
            tag.Add("WispProjSpd", projSpdRoll);
            tag.Add("WispProjPet", projPetRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("WispMagic", out int magic))
            {
                magicRoll = magic;
            }

            if (tag.TryGet("WispRanged", out int ranged))
            {
                rangedRoll = ranged;
            }

            if (tag.TryGet("WispProjSpd", out int projSpd))
            {
                projSpdRoll = projSpd;
            }

            if (tag.TryGet("WispProjPet", out int petProj))
            {
                projPetRoll = petProj;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.WispInABottle")

                        .Replace("<magicBase>", Math.Round(baseMagic * 100, 2).ToString())
                        .Replace("<magicPer>", Math.Round(magicPerRoll * 100, 2).ToString())

                        .Replace("<rangedBase>", Math.Round(baseRanged * 100, 2).ToString())
                        .Replace("<rangedPer>", Math.Round(rangedPerRoll * 100, 2).ToString())

                        .Replace("<projSpdBase>", Math.Round(baseProjSpd * 100, 2).ToString())
                        .Replace("<projSpdPer>", Math.Round(projSpdPerRoll * 100, 2).ToString())

                        .Replace("<petProjBase>", Math.Round(baseProjPet * 100, 2).ToString())
                        .Replace("<petProjPer>", Math.Round(projPetPerRoll * 100, 2).ToString())

                        .Replace("<currentMagic>", PetColors.LightPetRarityColorConvert(Math.Round(CurrentMagicDmg * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), magicRoll, magicMaxRoll))
                        .Replace("<magicRoll>", PetColors.LightPetRarityColorConvert(magicRoll.ToString(), magicRoll, magicMaxRoll))
                        .Replace("<magicMaxRoll>", PetColors.LightPetRarityColorConvert(magicMaxRoll.ToString(), magicRoll, magicMaxRoll))

                        .Replace("<currentRanged>", PetColors.LightPetRarityColorConvert(Math.Round(CurrentRangedDmg * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), rangedRoll, rangedMaxRoll))
                        .Replace("<rangedRoll>", PetColors.LightPetRarityColorConvert(rangedRoll.ToString(), rangedRoll, rangedMaxRoll))
                        .Replace("<rangedMaxRoll>", PetColors.LightPetRarityColorConvert(rangedMaxRoll.ToString(), rangedRoll, rangedMaxRoll))

                        .Replace("<currentProjSpd>", PetColors.LightPetRarityColorConvert(Math.Round(CurrentProjSpd * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), projSpdRoll, projSpdMaxRoll))
                        .Replace("<projSpdRoll>", PetColors.LightPetRarityColorConvert(projSpdRoll.ToString(), projSpdRoll, projSpdMaxRoll))
                        .Replace("<projSpdMaxRoll>", PetColors.LightPetRarityColorConvert(projSpdMaxRoll.ToString(), projSpdRoll, projSpdMaxRoll))

                        .Replace("<currentPetProj>", PetColors.LightPetRarityColorConvert(Math.Round(CurrentPetProjectileDamage * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), projPetRoll, projPetMaxRoll))
                        .Replace("<petProjRoll>", PetColors.LightPetRarityColorConvert(projPetRoll.ToString(), projPetRoll, projPetMaxRoll))
                        .Replace("<petProjMaxRoll>", PetColors.LightPetRarityColorConvert(projPetMaxRoll.ToString(), projPetRoll, projPetMaxRoll))
                        ));
            if (magicRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", "[c/" + PetColors.LowQuality.Hex3() + ":" + Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.NotRolled") + "]"));
            }
        }
    }
}
