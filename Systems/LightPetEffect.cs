using PetsOverhaul.Config;
using PetsOverhaul.NPCs;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.Systems
{
    public class LightPetDetours : ModSystem
    {
        public static double multOnLightPetCombine = 1;
        public override void Load()
        {
            On_Item.CanShimmer += On_Item_CanShimmer;
            On_ShopHelper.GetShoppingSettings += On_ShopHelper_GetShoppingSettings;
        }

        private static ShoppingSettings On_ShopHelper_GetShoppingSettings(On_ShopHelper.orig_GetShoppingSettings orig, ShopHelper self, Player player, NPC npc)
        {
            ShoppingSettings settings = orig(self, player, npc);
            if (npc.type == ModContent.NPCType<PetTamer>())
            {
                multOnLightPetCombine = settings.PriceAdjustment; //This triggers upon talking to the NPC. (Opening the menu) Currently, this seems to be my greatest bet in implementing the Happiness Multiplier.
            }
            return settings;
        }

        private static bool On_Item_CanShimmer(On_Item.orig_CanShimmer orig, Item self)
        {
            if (PetItemIDs.LightPetNamesAndItems.ContainsValue(self.type))
                return false;
            return orig(self);
        }
    }
    public abstract class LightPetEffect : ModPlayer
    {
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        /// <summary>
        /// This field is to make this Light Pet appear on /pet light|lightpet|lightpets command(s).
        /// </summary>
        public abstract int LightPetItemID { get; }
    }
    public abstract class LightPetItem : GlobalItem
    {
        public abstract int LightPetItemID { get; }
        public sealed override bool InstancePerEntity => true;
        public abstract string PetsTooltip { get; }

        public sealed override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return ExtraAppliesToEntity(entity, lateInstantiation) && entity.type == LightPetItemID;
        }
        /// <summary>
        /// Defaults to true. Override this to change result of AppliesToEntity. AppliesToEntity will return true for the entities who has the type of given Pet Effect's PetItemID.
        /// </summary>
        public virtual bool ExtraAppliesToEntity(Item entity, bool lateInstantation)
        { return true; }
        /// <summary>
        /// Consumes item1 and item2, in result; returns a new Light Pet that inherits highest rolls of both Light Pets. If it fails, returns null. Also returns null if combination has 0 quality raises. See LightPetCombine.cs in UI for initiation of this mechanic.
        /// </summary>
        /// <param name="item1">First item to default its rolls to.</param>
        /// <param name="item2">Second item to inherit stats from if they are higher.</param>
        /// <returns>Returns the new item, or null if fails & was pointless.</returns>
        public static Item CombineLightPets(Item item1, Item item2)
        {
            if (item1.type == item2.type)
            {
                Item newPet = item1.Clone();
                foreach (var globalOfNewPet in newPet.Globals)
                {
                    if (globalOfNewPet.GetType().IsSubclassOf(typeof(LightPetItem)))
                    {
                        foreach (var light in item2.Globals)
                        {
                            if (light.GetType().IsSubclassOf(typeof(LightPetItem)))
                            {
                                FieldInfo[] lightPetRolls = light.GetType().GetFields();
                                int changedCounter = 0;
                                float priceCounter = 0;
                                int equalCounter = 0;
                                for (int i = 0; i < lightPetRolls.Length; i++)
                                {
                                    if (lightPetRolls[i].FieldType != typeof(LightPetStat))
                                    {
                                        continue;
                                    }
                                    LightPetStat newPetRoll = (LightPetStat)lightPetRolls[i].GetValue(globalOfNewPet);
                                    LightPetStat secondPetRoll = (LightPetStat)lightPetRolls[i].GetValue(light);
                                    priceCounter += Math.Abs(secondPetRoll.MaxRoll * (secondPetRoll.CurrentRoll - ((newPetRoll.CurrentRoll + secondPetRoll.CurrentRoll) / 2f)) * 0.01f);
                                    if (newPetRoll.CurrentRoll < secondPetRoll.CurrentRoll)
                                    {
                                        lightPetRolls[i].SetValue(globalOfNewPet, secondPetRoll);
                                        changedCounter++;
                                    }
                                    if (newPetRoll.CurrentRoll == secondPetRoll.CurrentRoll)
                                        equalCounter++;
                                }
                                if (changedCounter <= 0 || lightPetRolls.Length == changedCounter || lightPetRolls.Length == changedCounter + equalCounter)
                                {
                                    return null;
                                }
                                newPet.value = (int)(item2.value * priceCounter * LightPetDetours.multOnLightPetCombine);
                                return newPet;
                            }
                        }
                    }
                }
            }
            return null;
        }
        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is RecipeItemCreationContext recipeResult && recipeResult.ConsumedItems.Exists(x => PetItemIDs.LightPetNamesAndItems.ContainsValue(x.type)))
            {
                Item oldLightPet = recipeResult.ConsumedItems.Find(x => PetItemIDs.LightPetNamesAndItems.ContainsValue(x.type));
                float cap = 0;
                foreach (var oldGlobal in oldLightPet.Globals)
                {
                    if (oldGlobal.GetType().IsSubclassOf(typeof(LightPetItem)))
                    {
                        FieldInfo[] lightPetRolls = oldGlobal.GetType().GetFields();
                        int count = 0; //count is more reliable as we will skip non-lightpetstat fields.
                        for (int i = 0; i < lightPetRolls.Length; i++)
                        {
                            if (lightPetRolls[i].FieldType != typeof(LightPetStat))
                            {
                                continue;
                            }
                            LightPetStat oldRolls = (LightPetStat)lightPetRolls[i].GetValue(oldGlobal);
                            count++;
                            cap += (float)oldRolls.CurrentRoll / oldRolls.MaxRoll;
                        }
                        cap /= count;
                    }
                }
                foreach (var globalOfNewPet in item.Globals)
                {
                    if (globalOfNewPet.GetType().IsSubclassOf(typeof(LightPetItem)))
                    {
                        FieldInfo[] lightPetRolls = globalOfNewPet.GetType().GetFields();
                        for (int i = 0; i < lightPetRolls.Length; i++)
                        {
                            if (lightPetRolls[i].FieldType != typeof(LightPetStat))
                            {
                                continue;
                            }
                            LightPetStat newPetRoll = (LightPetStat)lightPetRolls[i].GetValue(globalOfNewPet);
                            newPetRoll.SetRoll(Main.LocalPlayer.luck, cap); //This sets roll of 'newPetRoll'
                            lightPetRolls[i].SetValue(globalOfNewPet, newPetRoll); //This actually changes the said roll to be changed on the GlobalItem.
                        }
                    }
                }
            }
            ExtraOnCreated(item, context);
        }
        public virtual void ExtraOnCreated(Item item, ItemCreationContext context)
        { }

        /// <summary>
        /// Checkd for roll missing text. Supposed to return any of LightPetStat's CurrentRoll.
        /// </summary>
        /// <returns></returns>
        public abstract int GetRoll();

        public sealed override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
                return;

            string tip = "\n" + PetsTooltip;

            if (GetRoll() <= 0)
                tip = string.Concat(tip, "\n" + PetTextsColors.RollMissingText());

            if (tooltips.Exists(x => x.Name == "Tooltip0"))
                tooltips.Find(x => x.Name == "Tooltip0").Text += tip;
            else if (tooltips.Exists(x => x.Name == "Equipable"))
                tooltips.Find(x => x.Name == "Equipable").Text += tip;
            else if (tooltips.Exists(x => x.Name == "ItemName"))
                tooltips.Find(x => x.Name == "ItemName").Text += tip;

        }
    }
    /// <summary>
    /// Struct that contains all a singular Light Pet Stat has & methods for easy tooltip.
    /// </summary>
    public struct LightPetStat
    {
        public int CurrentRoll = 0;
        public int MaxRoll = 1;
        public float StatPerRoll = 0;
        public float BaseStat = 0;
        private readonly bool isInt = false;
        public LightPetStat(int maxRoll, int statPerRoll, int baseStat = 0)
        {
            MaxRoll = maxRoll;
            StatPerRoll = statPerRoll;
            BaseStat = baseStat;
            isInt = true;
        }

        public LightPetStat(int maxRoll, float statPerRoll, float baseStat = 0)
        {
            MaxRoll = maxRoll;
            StatPerRoll = statPerRoll;
            BaseStat = baseStat;
            isInt = false;
        }
        public readonly float CurrentStatFloat => BaseStat + StatPerRoll * CurrentRoll;
        public readonly int CurrentStatInt => (int)Math.Ceiling(CurrentStatFloat);
        /// <summary>
        /// Sets the roll of this stat. Commonly used in UpdateInventory(). Player luck allows for it to be rolled again; replacing the initial roll if higher (or lower for negative luck.)
        /// </summary>
        public void SetRoll(float luck)
        {
            if (CurrentRoll <= 0)
            {
                CurrentRoll = Main.rand.Next(MaxRoll) + 1;
                int timesToRoll = GlobalPet.Randomizer((int)(luck * 100));
                if (timesToRoll > 0)
                {
                    for (int i = 0; i < timesToRoll; i++)
                    {
                        int luckRoll = Main.rand.Next(MaxRoll) + 1;
                        if (luckRoll > CurrentRoll)
                            CurrentRoll = luckRoll;

                    }
                }
                else if (timesToRoll < 0)
                {
                    for (int i = 0; i < timesToRoll * -1; i++)
                    {
                        int negativeLuckRoll = Main.rand.Next(MaxRoll) + 1;
                        if (negativeLuckRoll < CurrentRoll)
                            CurrentRoll = negativeLuckRoll;
                    }
                }
            }
        }
        /// <summary>
        /// Sets the Roll with upper limit of given percentage amount of MaxRoll. Doesn't check whether or not quality has been rolled before or not.
        /// </summary>
        /// <param name="maxRollPercent"></param>
        public void SetRoll(float luck, float maxRollPercent)
        {
            int tempMax = (int)Math.Round(Math.Clamp(maxRollPercent, 0, 1f) * MaxRoll);
            CurrentRoll = Main.rand.Next(tempMax) + 1;
            int timesToRoll = GlobalPet.Randomizer((int)(luck * 100));
            if (timesToRoll > 0)
            {
                for (int i = 0; i < timesToRoll; i++)
                {
                    int luckRoll = Main.rand.Next(tempMax) + 1;
                    if (luckRoll > CurrentRoll)
                        CurrentRoll = luckRoll;

                }
            }
            else if (timesToRoll < 0)
            {
                for (int i = 0; i < timesToRoll * -1; i++)
                {
                    int negativeLuckRoll = Main.rand.Next(tempMax) + 1;
                    if (negativeLuckRoll < CurrentRoll)
                        CurrentRoll = negativeLuckRoll;
                }
            }
        }
        /// <summary>
        /// Returns the stat's current value with its + or %, localized 'Quality' text, and its current quality, localized 'out of' text next to it and the Max roll this stat can achieve. And correctly colors them.
        /// </summary>
        public readonly string StatSummaryLine()
        {
            return PetTextsColors.LightPetRarityColorConvert(isInt ? (PetTextsColors.LocVal("Misc.+") + CurrentStatInt.ToString()) : (Math.Round(CurrentStatFloat * 100, 2).ToString() + PetTextsColors.LocVal("Misc.%")), CurrentRoll, MaxRoll) + " " +
                PetTextsColors.LocVal("LightPetTooltips.Quality") + " " + PetTextsColors.LightPetRarityColorConvert(CurrentRoll.ToString(), CurrentRoll, MaxRoll) + " " + PetTextsColors.LocVal("LightPetTooltips.OutOf") + " " + PetTextsColors.LightPetRarityColorConvert(MaxRoll.ToString(), CurrentRoll, MaxRoll);
        }
        /// <summary>
        /// Use this overload if Summary line is intended to show the current stat differently than what StatSummaryLine() does.
        /// </summary>
        public readonly string StatSummaryLine(string currentStat)
        {
            return PetTextsColors.LightPetRarityColorConvert(currentStat, CurrentRoll, MaxRoll) + " " + PetTextsColors.LocVal("LightPetTooltips.Quality") + " " +
                PetTextsColors.LightPetRarityColorConvert(CurrentRoll.ToString(), CurrentRoll, MaxRoll) + " " + PetTextsColors.LocVal("LightPetTooltips.OutOf") + " " + PetTextsColors.LightPetRarityColorConvert(MaxRoll.ToString(), CurrentRoll, MaxRoll);
        }
        /// <summary>
        /// Returns the stat's Base and Per Roll stats, alongside required spacings, multiplication and operators.
        /// </summary>
        public readonly string BaseAndPerQuality()
        {
            int mult = isInt ? 1 : 100;
            return (BaseStat == 0 ? "" : (Math.Round(BaseStat * mult, 2).ToString() + " " + PetTextsColors.LocVal("Misc.+") + " ")) + Math.Round(StatPerRoll * mult, 2).ToString()
                + (isInt ? "" : PetTextsColors.LocVal("Misc.%")) + " " + PetTextsColors.LocVal("LightPetTooltips.Per");
        }
        /// <summary>
        /// Use this overload if displayed values are intended to be displayed in a different way than BaseAndPerQuality().
        /// </summary>
        public readonly string BaseAndPerQuality(string perRoll, string baseRoll = "")
        {
            return (BaseStat == 0 ? "" : (baseRoll + " " + PetTextsColors.LocVal("Misc.+") + " ")) + perRoll + " " + PetTextsColors.LocVal("LightPetTooltips.Per");
        }
    }
}
