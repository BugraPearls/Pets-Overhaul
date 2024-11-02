﻿using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class JewelOfLightEffect : LightPetEffect
    {
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out JewelOfLight empress))
            {
                Player.moveSpeed += empress.MovementSpeed.CurrentStatFloat;
                if (Player.equippedWings != null)
                {
                    Player.wingTimeMax += empress.WingTime.CurrentStatInt;
                }
            }
        }
        public override void PostUpdateRunSpeeds()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out JewelOfLight empress))
            {
                Player.runAcceleration += empress.Acceleration.CurrentStatFloat;
            }
        }
    }
    public sealed class JewelOfLight : GlobalItem
    {
        public LightPetStat MovementSpeed = new(8, 0.01f, 0.07f);
        public LightPetStat WingTime = new(15, 4, 30);
        public LightPetStat Acceleration = new(20, 0.0012f, 0.02f);
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.FairyQueenPetItem;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            MovementSpeed.SetRoll();
            WingTime.SetRoll();
            Acceleration.SetRoll();
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)MovementSpeed.CurrentRoll);
            writer.Write((byte)Acceleration.CurrentRoll);
            writer.Write((byte)WingTime.CurrentRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            MovementSpeed.CurrentRoll = reader.ReadByte();
            Acceleration.CurrentRoll = reader.ReadByte();
            WingTime.CurrentRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("EmpressMoveSpd", MovementSpeed.CurrentRoll);
            tag.Add("EmpressWing", WingTime.CurrentRoll);
            tag.Add("EmpressExp", Acceleration.CurrentRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("EmpressMoveSpd", out int spd))
            {
                MovementSpeed.CurrentRoll = spd;
            }

            if (tag.TryGet("EmpressWing", out int wing))
            {
                WingTime.CurrentRoll = wing;
            }

            if (tag.TryGet("EmpressExp", out int exp))
            {
                Acceleration.CurrentRoll = exp;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.JewelOfLight")

                        .Replace("<moveSpd>", MovementSpeed.BaseAndPerQuality())
                        .Replace("<wingTime>", WingTime.BaseAndPerQuality(Math.Round(WingTime.StatPerRoll / 60f, 2).ToString(), Math.Round(WingTime.BaseStat / 60f, 2).ToString()))
                        .Replace("<acceleration>", Acceleration.BaseAndPerQuality())

                        .Replace("<moveSpdLine>", MovementSpeed.StatSummaryLine())
                        .Replace("<wingTimeLine>", WingTime.StatSummaryLine(Math.Round(WingTime.CurrentStatInt / 60f, 2).ToString()))
                        .Replace("<accelerationLine>", Acceleration.StatSummaryLine())
                        ));
            if (WingTime.CurrentRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", PetTextsColors.RollMissingText()));
            }
        }
    }
}
