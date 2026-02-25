using Microsoft.Xna.Framework;
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

        [LabelKey("$Mods.PetsOverhaul.Config.ActivePetSlotPosLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.ActivePetSlotPosTooltip")]
        [DefaultValue(typeof(Vector2), "0.89, 0.485")]
        [BackgroundColor(35, 120, 54, 190)]
        public Vector2 ActivePetSlotPos { get; set; }

        [DefaultValue(ShieldPosition.HealthBarRight)]
        [LabelKey("$Mods.PetsOverhaul.Config.ShieldLocationLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.ShieldLocationTooltip")]
        [JsonConverter(typeof(StringEnumConverter))]
        [BackgroundColor(35, 120, 54, 190)]
        [SliderColor(120, 35, 54, 125)]
        public ShieldPosition ShieldLocation { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.CooldownDisplayLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.CooldownDisplayTooltip")]
        [DefaultValue(true)]
        [BackgroundColor(35, 120, 54, 190)]
        public bool ShowAbilityDisplay { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.CooldownDisplayLocationLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.CooldownDisplayLocationTooltip")]
        [DefaultValue(typeof(Vector2), "0.76, 0.73")]
        [BackgroundColor(35, 120, 54, 190)]
        public Vector2 AbilityDisplayPos { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.ResourceDisplayLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.ResourceDisplayTooltip")]
        [DefaultValue(true)]
        [BackgroundColor(35, 120, 54, 190)]
        public bool ShowResourceDisplay { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.ResourceDisplayLocationLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.ResourceDisplayLocationTooltip")]
        [DefaultValue(typeof(Vector2), "0.76, 0.67")]
        [BackgroundColor(35, 120, 54, 190)]
        public Vector2 ResourceDisplayPos { get; set; }

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

        [DefaultValue(PassivePetSoundFrequency.Normal)]
        [LabelKey("$Mods.PetsOverhaul.Config.PassiveSoundLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.PassiveSoundTooltip")]
        [JsonConverter(typeof(StringEnumConverter))]
        [BackgroundColor(35, 120, 54, 190)]
        [SliderColor(58, 28, 36, 125)]
        public PassivePetSoundFrequency PassiveSoundFrequency { get; set; }

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

        [LabelKey("$Mods.PetsOverhaul.Config.LowCooldownThresholdLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.LowCooldownThresholdTooltip")]
        [Range(30, 600)]
        [DefaultValue(150)]
        [BackgroundColor(35, 120, 54, 190)]
        public int LowCooldownThreshold { get; set; }
        #endregion
    }
}
