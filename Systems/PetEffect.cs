using PetsOverhaul.Config;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

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
        /// Checks for given PetItemID is currently in the MiscSlot[0].
        /// </summary>
        /// <param name="checkOblivious">Determines if Oblivious Pet debuff should be considered regarding the result.</param>
        /// <returns>Returns if given ID is currently in use</returns>
        public bool PetIsEquipped(bool checkOblivious = true)
        {
            if (checkOblivious)
                return Pet.PetInUseWithSwapCd(PetItemID);
            else
                return Pet.PetInUse(PetItemID);
        }
        public sealed override void PreUpdate()
        {
            if (PetIsEquipped(false))
            {
                Pet.timerMax = PetAbilityCooldown;
                Pet.currentPetStacks = PetStackCurrent;
                Pet.currentPetStacksMax = PetStackMax;
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
        public sealed override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (PreModifyPetTooltips(item, tooltips) == false)
            {
                return;
            }
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            int index = tooltips.FindLastIndex(x => x.Name == "Tooltip0");
            if (index < 0)
                index = tooltips.FindLastIndex(x => x.Name == "Equipable");
            if (index < 0)
                index = tooltips.FindLastIndex(x => x.Name == "ItemName");

            index++; //Both safety net for it to not be -1 somehow, and we want it after the given FindLastIndexes.

            tooltips.Insert(index, new(Mod, "PetTooltip0", PetsTooltip.Replace("<class>", PetTextsColors.ClassText(PetsEffect.PetClassPrimary, PetsEffect.PetClassSecondary))));
        }
        /// <summary>
        /// Defaults to true. Return false to stop default Pet Tooltip code from running, or override and return true to simply do further stuff in ModifyTooltips past the default things a pet does within in its tooltip.
        /// </summary>
        public virtual bool PreModifyPetTooltips(Item item, List<TooltipLine> tooltips)
        { return true; }
    }
}
