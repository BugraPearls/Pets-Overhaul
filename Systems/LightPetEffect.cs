using PetsOverhaul.NPCs;
using PetsOverhaul.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

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
        public bool TryGetLightPet<T>(out T result) where T : LightPetItem
        {
            if (Player.GetModPlayer<ActivePetSlotPlayer>().LightPetItemSlot[Player.CurrentLoadoutIndex].type > ItemID.None && PetIDs.LightPetNamesAndItems.ContainsValue(Player.GetModPlayer<ActivePetSlotPlayer>().LightPetItemSlot[Player.CurrentLoadoutIndex].type) && Player.GetModPlayer<ActivePetSlotPlayer>().LightPetItemSlot[Player.CurrentLoadoutIndex].TryGetGlobalItem(out T result1))
            {
                result = result1;
                Pet.currentActiveLightPet = result1;
                return true;
            }
            else if (Player.miscEquips[1].TryGetGlobalItem(out T result2))
            {
                result = result2;
                Pet.currentActiveLightPet = result2;
                return true;
            }
            result = null;
            return false;
        }
        public PetModPlayer Pet => Player.PetPlayer();
        /// <summary>
        /// This field is to make this Light Pet appear on /pet light|lightpet|lightpets command(s).
        /// </summary>
        public abstract int LightPetItemID { get; }
        public bool CustomActive = false; //This needs to be on the Player instance, as we both want it to trigger on all same items of same type, and allows for much, much easier access to the field. See WispInABottle.cs for implementation.
    }
    /// <summary>
    /// Core of Light Pet Quality System.
    /// </summary>
    public abstract class LightPetItem : GlobalItem
    {
        public bool hasRolled = false;
        public abstract int LightPetItemID { get; }
        public sealed override bool InstancePerEntity => true;
        public abstract string BaseTooltip { get; }
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
        public virtual void ExtraOnCreated(Item item, ItemCreationContext context)
        { }
        public sealed override void OnCreated(Item item, ItemCreationContext context)
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
        /// <summary>
        /// Runs after all ModifyTooltips does is thing of creating the Light Pet tooltip. Good usage is for Custom entries.
        /// </summary>
        public virtual void ModifyLightPetTooltip(ref string tooltip)
        { }
        /// <summary>
        /// Runs before Pets Overhaul's Light Pet tooltip code
        /// </summary>
        public virtual void ExtraModifyTooltips(Item item, List<TooltipLine> tooltips)
        { }
        public sealed override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            ExtraModifyTooltips(item, tooltips);

            int unrolledCount = 0;
            int totalStatCount = 0;
            string tip = "\n" + BaseTooltip;
            foreach (var stat in GetAllLightPetStats())
            {
                LightPetStat tempStat = stat; //Apparently ModifyTooltips' item is a clone of the original, so thats good. But besides that, this temp assignment gives a lot of freedom, as we cant modify 'stat' due to it being the variable of foreach.


                if (tempStat.CurrentRoll <= -1)
                {
                    tempStat.CurrentRoll = tempStat.MaxRoll;
                    unrolledCount++;
                }

                tip = tip.Replace($"<{tempStat.DataKey}>", PetUtils.LightPetRarityColorConvert(PetUtils.LocVal("LightPetTooltips.StatTooltipLine").Replace("<3>", tempStat.CurrentRoll.ToString()).Replace("<4>", tempStat.MaxRoll.ToString()), tempStat.CurrentRoll, tempStat.MaxRoll));

                if (tempStat.CustomDisplay == false) //Disabling key <0> <1> and <2> if custom display is desired, which is to be handled at ModifyLightPetTooltip()
                {
                    if (tempStat.isInt)
                    {
                        tip = tip.Replace("<0>", "+" + tempStat.CurrentStatInt.ToString()).Replace("<1>", tempStat.BaseStat.ToString()).Replace("<2>", tempStat.StatPerRoll.ToString());
                    }
                    else
                    {
                        tip = tip.Replace("<0>", PetUtils.Percentize(tempStat.CurrentStatFloat) + "%").Replace("<1>", PetUtils.Percentize(tempStat.BaseStat)).Replace("<2>", PetUtils.Percentize(tempStat.StatPerRoll) + "%");
                    }
                }
                totalStatCount++;
            }

            if (HasCustomEffect)
            {
                if (CustomEffectActive)
                    tip = "\n" + CustomPetsTooltip + "\n" + PetUtils.LocVal("Misc.CustomLine").Replace("<switchKey>", PetUtils.KeybindText(PetKeybinds.PetCustomSwitch));
                else
                {
                    tip += "\n" + PetUtils.LocVal("Misc.NonCustomLineContributor").Replace("<switchKey>", PetUtils.KeybindText(PetKeybinds.PetCustomSwitch));
                }
            }

            if (unrolledCount > 0)
            {
                tip += "\n";

                if (unrolledCount == totalStatCount)
                {
                    tip += PetUtils.LocVal("LightPetTooltips.NotRolled");
                }
                else
                {
                    tip += PetUtils.LocVal("LightPetTooltips.SomeStatsAreNotRolled");
                }
            }

            ModifyLightPetTooltip(ref tip);

            if (tooltips.Exists(x => x.Name == "Tooltip0"))
                tooltips.Find(x => x.Name == "Tooltip0").Text += tip;
            else if (tooltips.Exists(x => x.Name == "Equipable"))
                tooltips.Find(x => x.Name == "Equipable").Text += tip;
            else if (tooltips.Exists(x => x.Name == "ItemName"))
                tooltips.Find(x => x.Name == "ItemName").Text += tip;

        }

        /// <summary>
        /// Returns a list containing info from all the Light Pet Stats in this instance. This is not the actual fields, but rather clones of the actual LightPetStat fields! Only use this to read data.
        /// </summary>
        public List<LightPetStat> GetAllLightPetStats()
        {
            List<LightPetStat> lightPetStats = new();
            foreach (var field in this.GetType().GetFields())
            {
                if (field.FieldType == typeof(LightPetStat))
                {
                    LightPetStat stat = (LightPetStat)field.GetValue(this);
                    lightPetStats.Add(stat);
                }
            }
            return lightPetStats;
        }
        public void ApplyQualities(Player player)
        {
            if (hasRolled == false) //This exists only in sake of performance, so ApplyQualities runs only once in a regular gameplay. I don't want to save this as data, I think its unnecessary, its okay for this to run once in a while. Ex. if a Pet gets a new Stat, this can help triggering it.
            {
                foreach (var field in this.GetType().GetFields())
                {
                    if (field.FieldType == typeof(LightPetStat))
                    {
                        LightPetStat stat = (LightPetStat)field.GetValue(this);

                        stat.SetRoll(player.luck);

                        field.SetValue(this, stat);
                    }
                }
                hasRolled = true;
            }
        }
        /// <summary>
        /// Return false to prevent Pets Overhaul's <see cref="ApplyQualities(Player)"/> from running and to run own Quality logic if desired
        /// </summary>
        public virtual bool ExtraUpdateInventory(Item item, Player player) { return true; }
        public sealed override void UpdateInventory(Item item, Player player)
        {
            if (ExtraUpdateInventory(item, player))
            {
                ApplyQualities(player);
            }
        }
        /// <summary>
        /// This works after Pets Overhaul's code for syncing Qualities for all clients. Pets Overhaul's code WILL NOT send data if a CurrentRoll is not within the <see cref="sbyte"/> limits, so send them here.
        /// </summary>
        public virtual void ExtraNetSend(Item item, BinaryWriter writer)
        { }
        public sealed override void NetSend(Item item, BinaryWriter writer)
        {
            foreach (var stat in GetAllLightPetStats())
            {
                if (stat.CurrentRoll >= sbyte.MinValue && stat.CurrentRoll <= sbyte.MaxValue)
                    writer.Write((sbyte)stat.CurrentRoll);
            }
            ExtraNetSend(item, writer);
        }
        /// <summary>
        /// This works after Pets Overhaul's code for syncing Qualities for all clients. Pets Overhaul's code WILL NOT send data if a CurrentRoll is not within the <see cref="sbyte"/> limits during the <see cref="NetSend(Item, BinaryWriter)"/>, so make sure to send at <see cref="ExtraNetSend(Item, BinaryWriter)"/> and receive them here afterwards.
        /// </summary>
        public virtual void ExtraNetReceive(Item item, BinaryReader reader)
        { }
        public sealed override void NetReceive(Item item, BinaryReader reader)
        {
            foreach (var field in this.GetType().GetFields()) //These are always in a order so it should work fine.
            {
                if (field.FieldType == typeof(LightPetStat))
                {
                    LightPetStat stat = (LightPetStat)field.GetValue(this);

                    stat.CurrentRoll = reader.ReadSByte();
                    field.SetValue(this, stat);
                }
            }
            ExtraNetReceive(item, reader);
        }
        public virtual void ExtraSaveData(Item item, TagCompound tag)
        { }
        public sealed override void SaveData(Item item, TagCompound tag)
        {
            foreach (var stat in GetAllLightPetStats())
            {
                tag.Add(stat.DataKey, stat.CurrentRoll);
            }
            ExtraSaveData(item, tag);
        }
        public virtual void ExtraLoadData(Item item, TagCompound tag)
        { }
        public sealed override void LoadData(Item item, TagCompound tag)
        {
            foreach (var field in this.GetType().GetFields())
            {
                if (field.FieldType == typeof(LightPetStat))
                {
                    LightPetStat stat = (LightPetStat)field.GetValue(this);

                    if (tag.TryGet(stat.DataKey, out int tagResult))
                    {
                        stat.CurrentRoll = tagResult;
                    }

                    field.SetValue(this, stat);
                }
            }
            ExtraLoadData(item, tag);
        }
    }
    /// <summary>
    /// Struct that contains all important things for a singular Light Pet Stat. IMPORTANT: Make sure the key in localization files is exact name you put for <see cref="DataKey"/>! Example: If its "Health", make the localization key <![CDATA[<Health>]]>. <see cref="CustomDisplay"/> will disable normal Tooltip code from running for this stat.
    /// </summary>
    public struct LightPetStat
    {
        public bool CustomDisplay = false;
        public string DataKey = "";
        public int CurrentRoll = -1;
        public int MaxRoll = 1;
        public float StatPerRoll = 0;
        public float BaseStat = 0;
        internal readonly bool isInt = false;
        public LightPetStat(int maxRoll, int statPerRoll, string dataKey, int baseStat = 0, bool customStatDisplay = false)
        {
            MaxRoll = maxRoll;
            StatPerRoll = statPerRoll;
            BaseStat = baseStat;
            isInt = true;
            DataKey = dataKey;
            CustomDisplay = customStatDisplay;
        }

        public LightPetStat(int maxRoll, float statPerRoll, string dataKey, float baseStat = 0, bool customStatDisplay = false)
        {
            MaxRoll = maxRoll;
            StatPerRoll = statPerRoll;
            BaseStat = baseStat;
            isInt = false;
            DataKey = dataKey;
            CustomDisplay = customStatDisplay;
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
                int timesToRoll = PetUtils.Randomizer((int)(luck * 100));
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

            int timesToRoll = PetUtils.Randomizer((int)(luck * 100));
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
