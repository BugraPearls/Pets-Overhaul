﻿using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PetsOverhaul.Systems;
using System;
using System.ComponentModel;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace PetsOverhaul.Config
{
    [BackgroundColor(35, 54, 42, 220)]
    public class PetPersonalization : ModConfig
    {
        public override LocalizedText DisplayName => Language.GetText("Mods.PetsOverhaul.Config.Personalization");
        public override ConfigScope Mode => ConfigScope.ClientSide;

        #region Gameplay
        [Header("$Mods.PetsOverhaul.Config.HeaderGameplay")]

        [Slider()]
        [Range(0, 20)]
        [LabelKey("$Mods.PetsOverhaul.Config.MoreDifficultLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.MoreDifficultTooltip")]
        [DefaultValue(0)]
        [BackgroundColor(35, 120, 54, 190)]
        [SliderColor(54, 35, 120, 125)]
        public int DifficultAmount { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.SwapCooldownLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.SwapCooldownTooltip")]
        [DefaultValue(true)]
        [BackgroundColor(35, 120, 54, 190)]
        public bool SwapCooldown { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.PhantasmalDragonShootLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.PhantasmalDragonShootTooltip")]
        [DefaultValue(false)]
        [BackgroundColor(35, 120, 54, 190)]
        public bool PhantasmalDragonVolleyFromMouth { get; set; }
        #endregion

        #region Display
        [Header("$Mods.PetsOverhaul.Config.HeaderDisplay")]

        [LabelKey("$Mods.PetsOverhaul.Config.NoticeLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.NoticeTooltip")]
        [DefaultValue(true)]
        [BackgroundColor(35, 120, 54, 190)]
        public bool EnableNotice { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.ModNoticeLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.ModNoticeTooltip")]
        [DefaultValue(true)]
        [BackgroundColor(35, 120, 54, 190)]
        public bool EnableModNotice { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.DisableTooltipToggleLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.DisableTooltipToggleTooltip")]
        [DefaultValue(false)]
        [BackgroundColor(35, 120, 54, 190)]
        public bool EnableTooltipToggle { get; set; }

        [DefaultValue(ShieldPosition.HealthBarRight)]
        [LabelKey("$Mods.PetsOverhaul.Config.ShieldLocationLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.ShieldLocationTooltip")]
        [JsonConverter(typeof(StringEnumConverter))]
        [BackgroundColor(35, 120, 54, 190)]
        [SliderColor(120, 35, 54, 125)]
        public ShieldPosition ShieldLocation { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.DisableAbilityUnusedLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.DisableAbilityUnusedTooltip")]
        [DefaultValue(false)]
        [BackgroundColor(35, 120, 54, 190)]
        public bool AbilityDisplayUnused { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.AbilityDisplayInfoLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.AbilityDisplayInfoTooltip")]
        [DefaultValue(true)]
        [BackgroundColor(35, 120, 54, 190)]
        public bool AbilityDisplayInfo { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.AbilityDisplayDisableLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.AbilityDisplayDisableTooltip")]
        [DefaultValue(true)]
        [BackgroundColor(35, 120, 54, 190)]
        public bool ShowAbilityDisplay { get; set; }

        [DefaultValue(ParticleAmount.Normal)]
        [LabelKey("$Mods.PetsOverhaul.Config.DustAmountLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.DustAmountTooltip")]
        [JsonConverter(typeof(StringEnumConverter))]
        [BackgroundColor(35, 120, 54, 190)]
        [SliderColor(68, 108, 92, 125)]
        public ParticleAmount CircularDustAmount { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.DustInsideBlocksLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.DustInsideBlocksTooltip")]
        [DefaultValue(true)]
        [BackgroundColor(35, 120, 54, 190)]
        public bool CircularDustInsideBlocks { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.MaxQualityColor1Label")]
        [TooltipKey("$Mods.PetsOverhaul.Config.MaxQualityColor1Tooltip")]
        [BackgroundColor(35, 120, 54, 190)]
        [DefaultValue(typeof(Color), "165, 249, 255, 255"), ColorNoAlpha] //ColorsAndTexts.MaxQualityColor1
        public Color MaxQualityColor1 { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.MaxQualityColor2Label")]
        [TooltipKey("$Mods.PetsOverhaul.Config.MaxQualityColor2Tooltip")]
        [BackgroundColor(35, 120, 54, 190)]
        [DefaultValue(typeof(Color), "255, 207, 249, 255"), ColorNoAlpha] //ColorsAndTexts.MaxQualityColor2
        public Color MaxQualityColor2 { get; set; }
        #endregion

        #region Sounds
        [Header("$Mods.PetsOverhaul.Config.HeaderSound")]

        [LabelKey("$Mods.PetsOverhaul.Config.HurtSoundLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.HurtSoundTooltip")]
        [DefaultValue(true)]
        [BackgroundColor(35, 120, 54, 190)]
        public bool HurtSoundEnabled { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.DeathSoundLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.DeathSoundTooltip")]
        [DefaultValue(true)]
        [BackgroundColor(35, 120, 54, 190)]
        public bool DeathSoundEnabled { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.PassiveSoundLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.PassiveSoundTooltip")]
        [DefaultValue(true)]
        [BackgroundColor(35, 120, 54, 190)]
        public bool PassiveSoundEnabled { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.AbilitySoundLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.AbilitySoundTooltip")]
        [DefaultValue(true)]
        [BackgroundColor(35, 120, 54, 190)]
        public bool AbilitySoundEnabled { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.LowCooldownSoundLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.LowCooldownSoundTooltip")]
        [DefaultValue(false)]
        [BackgroundColor(35, 120, 54, 190)]
        public bool LowCooldownSoundEnabled { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.LowCooldownTresholdLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.LowCooldownTresholdTooltip")]
        [Range(30, 600)]
        [DefaultValue(150)]
        [BackgroundColor(35, 120, 54, 190)]
        public int LowCooldownTreshold { get; set; }
        #endregion
    }
}
