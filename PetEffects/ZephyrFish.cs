using Microsoft.Xna.Framework;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class ZephyrFish : PetEffect
    {
        public override int PetItemID => ItemID.ZephyrFish;
        public float powerPerQuest = 0.004f;
        public float maxQuestPower = 0.4f;
        public int baseChance = 30;
        public int windChance = 120;
        public int speedMult = 20;
        public bool AmplifiedFishingChance { get; internal set; }
        public override PetClasses PetClassPrimary => PetClasses.Fishing;
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped(false))
            {
                if (Main.windSpeedCurrent < 0)
                {
                    Player.fishingSkill += (int)(Main.windSpeedCurrent * -speedMult);
                }

                if (Main.windSpeedCurrent > 0)
                {
                    Player.fishingSkill += (int)(Main.windSpeedCurrent * speedMult);
                }
            }
        }
        public override void GetFishingLevel(Item fishingRod, Item bait, ref float fishingLevel)
        {
            if (PetIsEquipped(false))
            {
                if (Player.anglerQuestsFinished * 0.004f >= maxQuestPower)
                {
                    fishingLevel *= 1f + maxQuestPower;
                }
                else
                {
                    fishingLevel *= 1f + Player.anglerQuestsFinished * 0.004f;
                }
            }
        }
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (PetIsEquipped(false) && item.fishingPole > 0)
            {
                AmplifiedFishingChance = false;
            }
            return base.Shoot(item, source, position, velocity, type, damage, knockback);
        }
        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            if (PetIsEquipped(false))
            {
                if (Main.windSpeedCurrent > 0.2f && (attempt.heightLevel == 0 || attempt.heightLevel == 1) && attempt.X > Player.Center.X / 16)
                {
                    AmplifiedFishingChance = true;
                }
                else if (Main.windSpeedCurrent < -0.2f && (attempt.heightLevel == 0 || attempt.heightLevel == 1) && attempt.X < Player.Center.X / 16)
                {
                    AmplifiedFishingChance = true;
                }
            }
        }

        public override void ModifyCaughtFish(Item fish)
        {
            if (PetIsEquipped(false) && fish.maxStack != 1)
            {
                for (int i = 0; i < GlobalPet.Randomizer((AmplifiedFishingChance ? windChance : 0 + baseChance) * fish.stack); i++)
                {
                    Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.FishingItem), fish.type, 1);
                }
            }
        }
    }
    public sealed class ZephyrFishItem : PetTooltip
    {
        public override PetEffect PetsEffect => zephyrFish;
        public static ZephyrFish zephyrFish
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out ZephyrFish pet))
                    return pet;
                else
                    return ModContent.GetInstance<ZephyrFish>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.ZephyrFish")
                        .Replace("<windFish>", Math.Round(zephyrFish.speedMult / 8f, 2).ToString())
                        .Replace("<regularChance>", zephyrFish.baseChance.ToString())
                        .Replace("<windChance>", zephyrFish.windChance.ToString())
                        .Replace("<anglerPower>", Math.Round(zephyrFish.powerPerQuest * 100, 2).ToString())
                        .Replace("<maxAnglerPower>", Math.Round(zephyrFish.maxQuestPower * 100, 2).ToString())
                        .Replace("<anglerQuests>", Main.LocalPlayer.anglerQuestsFinished.ToString())
                        .Replace("<currentAnglerPower>", Math.Round(zephyrFish.powerPerQuest * Main.LocalPlayer.anglerQuestsFinished * 100, 2).ToString());
    }
}
