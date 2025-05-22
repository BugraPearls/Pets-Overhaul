using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class BabyPenguin : PetEffect
    {
        public override int PetItemID => ItemID.Fish;
        public override PetClasses PetClassPrimary => PetClasses.Fishing;
        public override PetClasses PetClassSecondary => PetClasses.Utility;
        internal int penguinOldChilledTime = 0;
        public int snowAndOceanPower = 25;
        public int regularFish = 20;
        public float chillingMultiplier = 0.8f;
        public int snowFishChance = 60;
        public static List<int> IceFishingDrops = [ItemID.FrostMinnow, ItemID.AtlanticCod, ItemID.FrostDaggerfish, ItemID.FrozenCrate, ItemID.FrozenCrateHard];
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped(false))
            {
                if (Player.ZoneSnow || Player.ZoneBeach)
                {
                    Player.fishingSkill += snowAndOceanPower;
                    Player.accFlipper = true;
                }
                else
                {
                    Player.fishingSkill += regularFish;
                }
            }

            if (PetIsEquipped() && Player.HasBuff(BuffID.Chilled))
            {
                if (Player.buffTime[Player.FindBuffIndex(BuffID.Chilled)] > penguinOldChilledTime)
                {
                    Player.buffTime[Player.FindBuffIndex(BuffID.Chilled)] -= (int)(Player.buffTime[Player.FindBuffIndex(BuffID.Chilled)] * chillingMultiplier);
                }
                penguinOldChilledTime = Player.buffTime[Player.FindBuffIndex(BuffID.Chilled)];
            }


        }
        public override void ModifyCaughtFish(Item fish)
        {
            if (PetIsEquipped(false) && IceFishingDrops.Contains(fish.type))
            {
                for (int i = 0; i < GlobalPet.Randomizer(snowFishChance * fish.stack); i++)
                {
                    Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.FishingItem), fish.type, 1);
                }
            }
        }
    }
    public sealed class Fish : PetTooltip
    {
        public override PetEffect PetsEffect => babyPenguin;
        public static BabyPenguin babyPenguin
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out BabyPenguin pet))
                    return pet;
                else
                    return ModContent.GetInstance<BabyPenguin>();
            }
        }
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.Fish")
                .Replace("<fp>", babyPenguin.regularFish.ToString())
                .Replace("<moreFp>", babyPenguin.snowAndOceanPower.ToString())
                .Replace("<catchChance>", babyPenguin.snowFishChance.ToString())
                .Replace("<items>", PetTextsColors.ItemsToTooltipImages(BabyPenguin.IceFishingDrops))
                .Replace("<chilledReduce>", Math.Round(babyPenguin.chillingMultiplier * 100, 2).ToString());
        public override string SimpleTooltip => PetTextsColors.LocVal("SimpleTooltips.Fish");
    }
}
