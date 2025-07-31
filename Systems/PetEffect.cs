﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.Systems
{
    /// <summary>
    /// Abstract class that contains what every Primary Pet Effect contains.
    /// </summary>
    public abstract class PetEffect : ModPlayer
    {
        /// <summary>
        /// Should be replaced with varying field that is the Pet's stack.
        /// </summary>
        public virtual int PetStackCurrent => -1;
        /// <summary>
        /// Should be replaced with the field/value that determines max stacks. Change this to be 0 if its intended for Max stack to not appear.
        /// </summary>
        public virtual int PetStackMax => -1;
        /// <summary>
        /// Should be replaced with given Pet's localization value for their stack text.
        /// </summary>
        public virtual string PetStackText => string.Empty;
        /// <summary>
        /// If this contains a value, then default setting is changed to be: "Current " + PetStackText + this.
        /// </summary>
        public virtual string PetStackSpecial => string.Empty;
        /// <summary>
        /// Sets the Pet Ability Cooldown
        /// </summary>
        public virtual int PetAbilityCooldown => 0;
        /// <summary>
        /// Accesses the GlobalPet class, which has useful methods and fields for Pet implementation.
        /// </summary>
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        /// <summary>
        /// Primary Class of Pet that will appear on its tooltip with its color.
        /// </summary>
        public abstract PetClasses PetClassPrimary { get; }
        /// <summary>
        /// Secondary Class of Pet that will appear on its tooltip, which will mix its color with the Primary Classes color. Defaults to None.
        /// </summary>
        public virtual PetClasses PetClassSecondary => PetClasses.None;
        /// <summary>
        /// Item ID of the Pet. Used by PetTooltip class, ThisPetInUse() etc. 
        /// </summary>
        public abstract int PetItemID { get; }
        /// <summary>
        /// Mostly used for Tooltip. Pets that has a custom effect thats made for Developers/Contributers have this set to true.
        /// </summary>
        public virtual bool CustomEffectIsContributor => false;
        /// <summary>
        /// Set to true if a Donor from Patreon has bought a custom effect for this Pet. This will show
        /// </summary>
        public virtual bool HasCustomEffect => false;
        /// <summary>
        /// This won't change if HasDonorEffect is false. Suggested to use this in a else if, where the prior if is PetIsEquipped. PetIsEquipped checks if HasDonorEffect & DonorEffectActive to return false.
        /// </summary>
        public virtual bool CustomEffectActive { get; set; }
        /// <summary>
        /// Custom Pet Effect's class to appear when its switched to.
        /// </summary>
        public virtual PetClasses CustomPrimaryClass => PetClasses.None;
        public virtual PetClasses CustomSecondaryClass => PetClasses.None;
        /// <summary>
        /// Checks for given PetItemID is currently in the MiscSlot[0]. Returns false if HasCustomEffect and CustomEffectActive is both true.
        /// </summary>
        /// <param name="checkOblivious">Determines if Oblivious Pet debuff should be considered regarding the result.</param>
        /// <returns>Returns if given ID is currently in use</returns>
        public bool PetIsEquipped(bool checkOblivious = true)
        {
            if (HasCustomEffect && CustomEffectActive)
                return false;
            if (checkOblivious)
                return Pet.PetInUseWithSwapCd(PetItemID);
            else
                return Pet.PetInUse(PetItemID);
        }
        /// <summary>
        /// Same as PetIsEquipped, just for Custom effects.
        /// </summary>
        /// <param name="checkOblivious"></param>
        /// <returns></returns>
        public bool PetIsEquippedForCustom(bool checkOblivious = true)
        {
            if (HasCustomEffect && CustomEffectActive)
            {
                if (checkOblivious)
                    return Pet.PetInUseWithSwapCd(PetItemID);
                else
                    return Pet.PetInUse(PetItemID);
            }
            else 
                return false;
        }
        public sealed override void PreUpdate()
        {
            if (Pet.PetInUse(PetItemID))
            {
                Pet.timerMax = PetAbilityCooldown;
                Pet.currentPetStacks = PetStackCurrent;
                Pet.currentPetStacksMax = PetStackMax;
                Pet.currentPetStackText = PetStackText;
                Pet.currentPetStackSpecialText = PetStackSpecial;
                ExtraPreUpdate();
            }
            ExtraPreUpdateNoCheck();
        }
        /// <summary>
        /// This already checks for PetIsEquipped(false), so no need to check for it twice. Ran after setting the Pet.timerMax to PetAbilityCooldown.
        /// </summary>
        public virtual void ExtraPreUpdate() { }
        /// <summary>
        /// Same as ExtraPreUpdate() but doesn't check for Pet being equipped. Ran after ExtraPreUpdate().
        /// </summary>
        public virtual void ExtraPreUpdateNoCheck() { }
        /// <summary>
        /// Runs before Custom effect switch.
        /// </summary>
        public virtual void ExtraProcessTriggers(TriggersSet triggersSet) { }
        public sealed override void ProcessTriggers(TriggersSet triggersSet)
        {
            ExtraProcessTriggers(triggersSet);
            if (Main.HoverItem.type == PetItemID)
            {
                if (HasCustomEffect && PetKeybinds.PetCustomSwitch.JustPressed)
                {
                    CustomEffectActive = !CustomEffectActive;
                }
            }
        }
    }
    public abstract class PetTooltip : GlobalItem
    {
        //One problem I've came across; While trying to determine the PetEffect instance, AppliesToEntity seems to be ran only during initializing, so it cannot find the Player Instance, therefore it crashes. Due to this, we will return ModContent.GetInstance<>(), returning a template instance of the Player for us to check the ID.
        //public static Turtle Turtle //This is ran on all Pet tooltips.
        //{
        //    get
        //    {
        //        if (Main.LocalPlayer.TryGetModPlayer(out Turtle turtle))
        //            return turtle;
        //        else
        //            return ModContent.GetInstance<Turtle>();
        //    }
        //}
        public abstract PetEffect PetsEffect { get; }
        public sealed override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return ExtraAppliesToEntity(entity, lateInstantiation) && entity.type == PetsEffect.PetItemID;
        }
        /// <summary>
        /// Defaults to true. Override this to change result of AppliesToEntity. AppliesToEntity will return true for the entities who has the type of given Pet Effect's PetItemID.
        /// </summary>
        public virtual bool ExtraAppliesToEntity(Item entity, bool lateInstantation)
        { return true; }
        /// <summary>
        /// Whole String of the Pet's Tooltip. No need to Replace the <class>, since its global for all Pets to run it, its already being handled in ModifyTooltips. Use ExtraModifyTooltips if further modification is required past the default actions done in ModifyTooltips.
        /// </summary>
        public abstract string PetsTooltip { get; }
        /// <summary>
        /// If the Pet has SimpleTooltip value assigned in their code, that will be displayed instead by default; and will show a text below the tooltip to tell the Player they can switch the tooltip with a keybind.
        /// </summary>
        public virtual string SimpleTooltip => null;

        public virtual string CustomTooltip => null;
        public virtual string CustomSimpleTooltip => null;
        public sealed override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (PreModifyPetTooltips(item, tooltips) == false)
            {
                return;
            }

            int index = tooltips.FindLastIndex(x => x.Name == "Tooltip0");
            if (index < 0)
                index = tooltips.FindLastIndex(x => x.Name == "Equipable");
            if (index < 0)
                index = tooltips.FindLastIndex(x => x.Name == "ItemName");

            index++; //Both safety net for it to not be -1 somehow, and we want it after the given FindLastIndexes.

            string Tip;
            if (PetsEffect.HasCustomEffect && PetsEffect.CustomEffectActive)
            {
                if (CustomSimpleTooltip is not null)
                {
                    if (GlobalPet.CurrentTooltipIsSimple)
                        Tip = CustomSimpleTooltip + "\n" + PetTextsColors.LocVal("Misc.CurrentSimple").Replace("<switchKey>", PetTextsColors.KeybindText(PetKeybinds.ShowDetailedTip));
                    else
                        Tip = CustomTooltip + "\n" + PetTextsColors.LocVal("Misc.CurrentDetailed").Replace("<switchKey>", PetTextsColors.KeybindText(PetKeybinds.ShowDetailedTip));
                }
                else if (CustomTooltip is not null)
                {
                    Tip = CustomTooltip;
                }
                else
                { 
                    Tip = PetsTooltip; 
                }
            }
            else if (SimpleTooltip is not null)
            {
                if (GlobalPet.CurrentTooltipIsSimple)
                    Tip = SimpleTooltip + "\n" + PetTextsColors.LocVal("Misc.CurrentSimple").Replace("<switchKey>", PetTextsColors.KeybindText(PetKeybinds.ShowDetailedTip));
                else
                    Tip = PetsTooltip + "\n" + PetTextsColors.LocVal("Misc.CurrentDetailed").Replace("<switchKey>", PetTextsColors.KeybindText(PetKeybinds.ShowDetailedTip));

            }
            else
            {
                Tip = PetsTooltip;
            }
            PetClasses petClass1;
            PetClasses petClass2;
            if (PetsEffect.HasCustomEffect && PetsEffect.CustomEffectActive)
            {
                petClass1 = PetsEffect.CustomPrimaryClass;
                petClass2 = PetsEffect.CustomSecondaryClass;
            }
            else
            {
                petClass1 = PetsEffect.PetClassPrimary;
                petClass2 = PetsEffect.PetClassSecondary;
            }
            if (Tip.Contains("<class>"))
            {
                Tip = Tip.Replace("<class>", PetTextsColors.ClassText(petClass1, petClass2)); //Legacy way of 'class text' on Pet tooltips, so old ones on addons etc. doesn't break.
            }
            else
            {
                Tip = PetTextsColors.ClassText(petClass1, petClass2) + "\n" + Tip;
            }
            
            if (PetsEffect.HasCustomEffect)
            {
                Tip += "\n";
                if (PetsEffect.CustomEffectActive)
                    Tip += PetTextsColors.LocVal("Misc.CustomLine").Replace("<switchKey>", PetTextsColors.KeybindText(PetKeybinds.PetCustomSwitch));
                else
                {
                    if (PetsEffect.CustomEffectIsContributor) 
                    {
                        Tip += PetTextsColors.LocVal("Misc.NonCustomLineContributor").Replace("<switchKey>", PetTextsColors.KeybindText(PetKeybinds.PetCustomSwitch));
                    }
                    else
                    {
                        Tip += PetTextsColors.LocVal("Misc.NonCustomLineDonator").Replace("<switchKey>", PetTextsColors.KeybindText(PetKeybinds.PetCustomSwitch));
                    }
                }
            }
            tooltips.Insert(index, new(Mod, "PetTooltip0", Tip));
        }
        /// <summary>
        /// Defaults to true. Return false to stop default Pet Tooltip code from running, or override and return true to simply do further stuff in ModifyTooltips past the default things a pet does within in its tooltip.
        /// </summary>
        public virtual bool PreModifyPetTooltips(Item item, List<TooltipLine> tooltips)
        { return true; }
    }
}
