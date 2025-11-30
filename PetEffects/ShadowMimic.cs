using PetsOverhaul.Achievements;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class ShadowMimic : PetEffect
    {
        public override int PetItemID => ItemID.OrnateShadowKey;
        public override PetClass PetClassPrimary => PetClassID.Utility;
        public int chanceToRollDoubleItem = 30; //30% chance to double max/min's of stacks
        public int numeratorMult = 125; //effectively 25% increase on numerator, since denominator is also multiplied by 100 to keep the values consistent with percentages.
        public int denominatorMult = 100;
        public float lowChanceThreshold = 0.25f;
        public override void Load()
        {
            On_ItemDropResolver.ResolveRule += ShadowMimicExtraDrop;
        }

        private static ItemDropAttemptResult ShadowMimicExtraDrop(On_ItemDropResolver.orig_ResolveRule orig, ItemDropResolver self, IItemDropRule rule, DropAttemptInfo info)
        {
            ItemDropAttemptResult tempResult;
            if (rule is CommonDrop drop && ItemID.Sets.OpenableBag[drop.itemId] == false && ItemID.Sets.BossBag[drop.itemId] == false &&
                ItemID.Sets.PreHardmodeLikeBossBag[drop.itemId] == false && info.player.TryGetModPlayer(out ShadowMimic mimic) && mimic.PetIsEquipped())
            {
                if ((float)Math.Max(drop.chanceNumerator, 1) / Math.Max(drop.chanceDenominator, 1) <= mimic.lowChanceThreshold)
                {
                    drop.chanceNumerator *= mimic.numeratorMult;
                    drop.chanceDenominator *= mimic.denominatorMult;
                    tempResult = orig(self, rule, info);
                    drop.chanceNumerator /= mimic.numeratorMult; //If not reversed back, this is applied permanently until reloaded.
                    drop.chanceDenominator /= mimic.denominatorMult;
                    return tempResult;
                }
                else if (Main.rand.NextBool(mimic.chanceToRollDoubleItem, 100) && ContentSamples.ItemsByType[drop.itemId].maxStack != 1)
                {
                    drop.amountDroppedMaximum *= 2;
                    drop.amountDroppedMinimum *= 2;
                    tempResult = orig(self, rule, info);
                    drop.amountDroppedMaximum /= 2; //If not reversed back, this is applied permanently until reloaded.
                    drop.amountDroppedMinimum /= 2;
                    return tempResult;
                }
            }
            tempResult = orig(self, rule, info);

            if (info.player.CurrentPet() == ItemID.OrnateShadowKey && tempResult.State == ItemDropAttemptResultState.Success)
            {
                ModContent.GetInstance<LootChaser>().Count.Value++;
            }

            return tempResult;
        }

    }
    public sealed class OrnateShadowKey : PetTooltip
    {
        public override PetEffect PetsEffect => shadowMimic;
        public static ShadowMimic shadowMimic
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out ShadowMimic pet))
                    return pet;
                else
                    return ModContent.GetInstance<ShadowMimic>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.OrnateShadowKey")
            .Replace("<threshold>", Math.Round(shadowMimic.lowChanceThreshold * 100, 2).ToString())
            .Replace("<chanceIncrease>", (shadowMimic.numeratorMult - shadowMimic.denominatorMult).ToString())
            .Replace("<chanceToDouble>", shadowMimic.chanceToRollDoubleItem.ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.OrnateShadowKey");
    }
}
