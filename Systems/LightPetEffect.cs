using PetsOverhaul.NPCs;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace PetsOverhaul.Systems
{
    public class LightPetDetours : ModSystem
    {
        public static double multOnLightPetCombine = 1;
        public override void Load()
        {
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
    }
    public abstract class LightPetEffect : ModPlayer
    {
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        /// <summary>
        /// This field is to make this Light Pet appear on /pet light|lightpet|lightpets command(s).
        /// </summary>
        public abstract int LightPetItemID { get; }
        public virtual bool HasCustomEffect => false;
        public virtual bool CustomEffectActive { get; set; }
        public virtual void ExtraProcessTriggers(TriggersSet triggersSet) { }
        public sealed override void ProcessTriggers(TriggersSet triggersSet)
        {
            ExtraProcessTriggers(triggersSet);
            if (Main.HoverItem.type == LightPetItemID)
            {
                if (HasCustomEffect && PetKeybinds.PetCustomSwitch.JustPressed)
                {
                    CustomEffectActive = !CustomEffectActive;
                }
            }
        }
    }
    public abstract class LightPetItem : GlobalItem
    {
        public abstract int LightPetItemID { get; }
        public sealed override bool InstancePerEntity => true;
        public abstract string PetsTooltip { get; }
        public virtual string CustomPetsTooltip => string.Empty;
        public virtual bool HasCustomEffect => false;
        public virtual bool CustomEffectActive { get; set; }
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
                foreach (var globalOfNewPet in newPet.Globals) //this is 'Item 1, as the new item actually copies the Item 1 entirely with Item newPet = item1.Clone()'
                {
                    if (globalOfNewPet.GetType().IsSubclassOf(typeof(LightPetItem)))
                    {
                        foreach (var light in item2.Globals) //this is 'Item 2'
                        {
                            if (light.GetType().IsSubclassOf(typeof(LightPetItem)))
                            {
                                FieldInfo[] lightPetRolls = light.GetType().GetFields(); //all fields of Item 2's field informations (metadata) in its LightPetItem class is here
                                int changedCounter = 0;
                                float priceCounter = 0;
                                int equalCounter = 0;
                                for (int i = 0; i < lightPetRolls.Length; i++)
                                {
                                    if (lightPetRolls[i].FieldType != typeof(LightPetStat)) //fields that are not LightPetStat is skipped
                                    {
                                        continue;
                                    }
                                    LightPetStat newPetRoll = (LightPetStat)lightPetRolls[i].GetValue(globalOfNewPet); //Values of all same fields we've gotten above at light.GetType().GetFields() is from Item 1 
                                    LightPetStat secondPetRoll = (LightPetStat)lightPetRolls[i].GetValue(light); //Same is done for Item 2 now
                                    priceCounter += Math.Abs(secondPetRoll.MaxRoll * (secondPetRoll.CurrentRoll - ((newPetRoll.CurrentRoll + secondPetRoll.CurrentRoll) / 2f)) * 0.01f);
                                    if (newPetRoll.CurrentRoll < secondPetRoll.CurrentRoll)
                                    {
                                        lightPetRolls[i].SetValue(globalOfNewPet, secondPetRoll); //We set the same given field inside the object (globalOfNewPet, 1st parameter) to be the secondPetRoll (2nd parameter) here. 
                                        //For example here, it finds exact same field as the lightPetRolls[i].GetValue(light) inside the globalOfNewPet. Then sets it to have values of 'secondPetRoll'.
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
            if (context is RecipeItemCreationContext recipeResult && recipeResult.ConsumedItems.Exists(x => PetIDs.LightPetNamesAndItems.ContainsValue(x.type)))
            {
                Item oldLightPet = recipeResult.ConsumedItems.Find(x => PetIDs.LightPetNamesAndItems.ContainsValue(x.type));
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
                            newPetRoll.SetRoll(Main.LocalPlayer.luck, cap * 1.5f); //This sets roll of 'newPetRoll'
                            //The "* 1.5f" makes it so this is somewhat buffed, as otherwise, almost guaranteed you would hit bottom rolls when constantly crafted into another Pet, this way, if the rolls on material Light Pet is high, result Light Pet shouldn't be too low, but in the long term should always result in rolls decreasing.
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
            string tip = "\n" + PetsTooltip;
            if (HasCustomEffect)
            {
                if (CustomEffectActive)
                    tip = "\n" + CustomPetsTooltip + "\n" + PetUtils.LocVal("Misc.CustomLine").Replace("<switchKey>", PetUtils.KeybindText(PetKeybinds.PetCustomSwitch));
                else
                {
                    tip += "\n" + PetUtils.LocVal("Misc.NonCustomLineContributor").Replace("<switchKey>", PetUtils.KeybindText(PetKeybinds.PetCustomSwitch));
                }
            }

            if (GetRoll() <= 0)
                tip = string.Concat(tip, "\n" + PetUtils.RollMissingText());

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
        /// Sets the Roll with upper limit of given percentage amount of MaxRoll. Doesn't check whether or not quality has been rolled before or not, good to force the rolls to be done with a cap in mind.
        /// </summary>
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
            return PetUtils.LightPetRarityColorConvert(isInt ? ("+" + CurrentStatInt.ToString()) : (Math.Round(CurrentStatFloat * 100, 2).ToString() + "%"), CurrentRoll, MaxRoll) + " " +
                PetUtils.LocVal("LightPetTooltips.Quality") + " " + PetUtils.LightPetRarityColorConvert(CurrentRoll.ToString(), CurrentRoll, MaxRoll) + " " + PetUtils.LocVal("LightPetTooltips.OutOf") + " " + PetUtils.LightPetRarityColorConvert(MaxRoll.ToString(), CurrentRoll, MaxRoll);
        }
        /// <summary>
        /// Use this overload if Summary line is intended to show the current stat differently than what StatSummaryLine() does.
        /// </summary>
        public readonly string StatSummaryLine(string currentStat)
        {
            return PetUtils.LightPetRarityColorConvert(currentStat, CurrentRoll, MaxRoll) + " " + PetUtils.LocVal("LightPetTooltips.Quality") + " " +
                PetUtils.LightPetRarityColorConvert(CurrentRoll.ToString(), CurrentRoll, MaxRoll) + " " + PetUtils.LocVal("LightPetTooltips.OutOf") + " " + PetUtils.LightPetRarityColorConvert(MaxRoll.ToString(), CurrentRoll, MaxRoll);
        }
        /// <summary>
        /// Returns the stat's Base and Per Roll stats, alongside required spacings, multiplication and operators.
        /// </summary>
        public readonly string BaseAndPerQuality()
        {
            int mult = isInt ? 1 : 100;
            return (BaseStat == 0 ? "" : (Math.Round(BaseStat * mult, 2).ToString() + " + ")) + Math.Round(StatPerRoll * mult, 2).ToString()
                + (isInt ? " " : "% ") + PetUtils.LocVal("LightPetTooltips.Per");
        }
        /// <summary> 
        /// Use this overload if displayed values are intended to be displayed in a different way than BaseAndPerQuality().
        /// </summary>
        public readonly string BaseAndPerQuality(string perRoll, string baseRoll = "")
        {
            return (BaseStat == 0 ? "" : (baseRoll + " + ")) + perRoll + " " + PetUtils.LocVal("LightPetTooltips.Per");
        }
        public readonly string QualityLine()
        {
            return PetUtils.LightPetRarityColorConvert(CurrentRoll.ToString() + " " + PetUtils.LocVal("LightPetTooltips.OutOf") + " " + MaxRoll.ToString(), CurrentRoll, MaxRoll);
        }
    }
}
