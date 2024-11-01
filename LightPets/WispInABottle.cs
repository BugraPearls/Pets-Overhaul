﻿using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.Projectiles;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class WispInABottleEffect : LightPetEffect
    {
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out WispInABottle wispInABottle))
            {
                Player.GetDamage<MagicDamageClass>() += wispInABottle.MagicDamage.CurrentStatFloat;
                Player.GetDamage<RangedDamageClass>() += wispInABottle.RangedDamage.CurrentStatFloat;
                Pet.petDirectDamageMultiplier += wispInABottle.PetDamage.CurrentStatFloat;
            }
        }
        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out WispInABottle wispInABottle) && (item.DamageType == DamageClass.Magic || item.DamageType == DamageClass.Ranged))
            {
                velocity *= wispInABottle.ProjectileVelocity.CurrentStatFloat + 1;
            }
        }
    }
    public sealed class WispInABottle : GlobalItem
    {
        public LightPetStat MagicDamage = new(20, 0.004f, 0.04f);
        public LightPetStat RangedDamage = new(20, 0.004f, 0.04f);
        public LightPetStat ProjectileVelocity = new(12, 0.01f, 0.05f);
        public LightPetStat PetDamage = new(25, 0.008f, 0.075f);
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.WispinaBottle;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            MagicDamage.SetRoll();
            RangedDamage.SetRoll();
            ProjectileVelocity.SetRoll();
            PetDamage.SetRoll();
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
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.WispInABottle")

                        .Replace("<magic>", MagicDamage.BaseAndPerQuality())
                        .Replace("<ranged>", RangedDamage.BaseAndPerQuality())
                        .Replace("<velocity>", ProjectileVelocity.BaseAndPerQuality())
                        .Replace("<petDmg>", PetDamage.BaseAndPerQuality())

                        .Replace("<magicLine>", MagicDamage.StatSummaryLine())
                        .Replace("<rangedLine>", RangedDamage.StatSummaryLine())
                        .Replace("<velocityLine>", ProjectileVelocity.StatSummaryLine())
                        .Replace("<petDmgLine>", PetDamage.StatSummaryLine())
                        ));
            if (MagicDamage.CurrentRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", PetTextsColors.RollMissingText()));
            }
        }
    }
}
