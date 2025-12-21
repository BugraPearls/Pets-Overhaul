using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PetsOverhaul.Achievements;
using PetsOverhaul.NPCs;
using PetsOverhaul.Systems;
using PetsOverhaul.UI;
using ReLogic.Utilities;
using Stubble.Core.Classes;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
namespace PetsOverhaul.UI
{

    public class ActivePetSlotCanvas : UIState
    {
        internal Item CurrentActivePet
        {
            get { return Main.LocalPlayer.GetModPlayer<ActivePetSlots>().RegularPetItemSlot[Main.LocalPlayer.CurrentLoadoutIndex]; }
            set { Main.LocalPlayer.GetModPlayer<ActivePetSlots>().RegularPetItemSlot[Main.LocalPlayer.CurrentLoadoutIndex] = value; }
        }
        internal Item CurrentActiveLightPet
        {
            get { return Main.LocalPlayer.GetModPlayer<ActivePetSlots>().LightPetItemSlot[Main.LocalPlayer.CurrentLoadoutIndex]; }
            set { Main.LocalPlayer.GetModPlayer<ActivePetSlots>().LightPetItemSlot[Main.LocalPlayer.CurrentLoadoutIndex] = value; }
        }
        internal PetItemSlot ActiveRegularPetSlot;
        internal PetItemSlot ActiveLightPetSlot;
        internal UIText HoverText;
        public override void Update(GameTime gameTime)
        {
            if (Main.LocalPlayer.GetModPlayer<ActivePetSlots>().loadedPet is not null && Main.LocalPlayer.GetModPlayer<ActivePetSlots>().loadedPet.IsAir == false)
            {
                ActiveRegularPetSlot.Item = Main.LocalPlayer.GetModPlayer<ActivePetSlots>().loadedPet.Clone();
                Main.LocalPlayer.GetModPlayer<ActivePetSlots>().loadedPet.TurnToAir();
            }
            if (ActiveRegularPetSlot.Item != null && ActiveRegularPetSlot.Item.IsAir == false && ActiveRegularPetSlot.Item != CurrentActivePet)
            {
                CurrentActivePet = ActiveRegularPetSlot.Item;
            }
            if (Main.LocalPlayer.GetModPlayer<ActivePetSlots>().loadedLightPet is not null && Main.LocalPlayer.GetModPlayer<ActivePetSlots>().loadedLightPet.IsAir == false)
            {
                ActiveLightPetSlot.Item = Main.LocalPlayer.GetModPlayer<ActivePetSlots>().loadedLightPet.Clone();
                Main.LocalPlayer.GetModPlayer<ActivePetSlots>().loadedLightPet.TurnToAir();
            }
            if (ActiveLightPetSlot.Item != null && ActiveLightPetSlot.Item.IsAir == false && ActiveLightPetSlot.Item != CurrentActiveLightPet)
            {
                CurrentActiveLightPet = ActiveLightPetSlot.Item;
            }
        }
        public override void OnInitialize()
        {
            ActiveRegularPetSlot = new(ItemSlot.Context.EquipPet, 0.8f);
            ActiveRegularPetSlot.Width.Set(40, 0);
            ActiveRegularPetSlot.Height.Set(40, 0);
            Append(ActiveRegularPetSlot);

            ActiveLightPetSlot = new(ItemSlot.Context.EquipLight, 0.8f);
            ActiveLightPetSlot.Width.Set(40, 0);
            ActiveLightPetSlot.Height.Set(40, 0);
            Append(ActiveLightPetSlot);

            HoverText = new("");
            Append(HoverText);


        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Main.EquipPage != 2)
            {
                return;
            }
            if ((ActiveRegularPetSlot.IsMouseHovering && ActiveRegularPetSlot.Item is not null && ActiveRegularPetSlot.Item.IsAir) || (ActiveLightPetSlot.IsMouseHovering && ActiveLightPetSlot.Item is not null && ActiveLightPetSlot.Item.IsAir))
            {
                HoverText.SetText(PetUtils.LocVal("Misc.ActivePetSlotHover"));
                HoverText.Left.Set(Main.MouseScreen.X - (Main.screenWidth - Main.MouseScreen.X), 0);
                HoverText.Top.Set(Main.MouseScreen.Y + 50, 0);
                HoverText.TextColor = Main.MouseTextColorReal;
                HoverText.Draw(spriteBatch);
            }
            else
            {
                HoverText.SetText("");
            }
            Vector2 pos = new Vector2(Main.screenWidth - 190, Main.screenHeight / 2 - 55);
            Vector2 posLight = new Vector2(Main.screenWidth - 190, Main.screenHeight / 2 - 10);
            ActiveRegularPetSlot.Left.Set(pos.X, 0);
            ActiveRegularPetSlot.Top.Set(pos.Y, 0);
            ActiveLightPetSlot.Left.Set(posLight.X, 0);
            ActiveLightPetSlot.Top.Set(posLight.Y, 0);
            base.Draw(spriteBatch);
        }
    }
    public class ActivePetSlots : ModPlayer
    {
        internal Item loadedPet;
        internal Item loadedLightPet;
        internal PetItemSlot ActiveRegularPetSlot;
        internal PetItemSlot ActiveLightPetSlot;
        internal List<Item> RegularPetItemSlot = [new(0), new(0), new(0)];
        internal List<Item> LightPetItemSlot = [new(0), new(0), new(0)];
        internal Item CurrentPetItem
        {
            get { return ModContent.GetInstance<ActivePetSlotSystem>().Display.ActiveRegularPetSlot.Item; }
            set { ModContent.GetInstance<ActivePetSlotSystem>().Display.ActiveRegularPetSlot.Item = value; }
        }
        internal Item CurrentLightPetItem
        {
            get { return ModContent.GetInstance<ActivePetSlotSystem>().Display.ActiveLightPetSlot.Item; }
            set { ModContent.GetInstance<ActivePetSlotSystem>().Display.ActiveLightPetSlot.Item = value; }
        }
        public override void OnEquipmentLoadoutSwitched(int oldLoadoutIndex, int loadoutIndex)
        {
            if (Main.myPlayer == Player.whoAmI)
            {
                RegularPetItemSlot[oldLoadoutIndex] = CurrentPetItem;
                LightPetItemSlot[oldLoadoutIndex] = CurrentLightPetItem;
                CurrentPetItem = RegularPetItemSlot[loadoutIndex];
                CurrentLightPetItem = LightPetItemSlot[loadoutIndex];
            }
        }
        public override void SaveData(TagCompound tag)
        {
            tag.Add("RegularPet", RegularPetItemSlot);
            tag.Add("LightPet", LightPetItemSlot);
        }
        public override void LoadData(TagCompound tag)
        {
            if (tag.TryGet("RegularPet", out List<Item> pet))
            {
                RegularPetItemSlot = pet;
                loadedPet = RegularPetItemSlot[Player.CurrentLoadoutIndex];
            }
            if (tag.TryGet("LightPet", out List<Item> lightPet))
            {
                LightPetItemSlot = lightPet;
                loadedLightPet = LightPetItemSlot[Player.CurrentLoadoutIndex];
            }
        }
    }

    [Autoload(Side = ModSide.Client)]
    public class ActivePetSlotSystem : ModSystem
    {
        internal ActivePetSlotCanvas Display;
        private UserInterface _display;

        public override void OnWorldLoad()
        {
            Display = new ActivePetSlotCanvas();
            Display.Activate();
            _display = new UserInterface();
            _display.SetState(Display);
        }
        public override void UpdateUI(GameTime gameTime)
        {
            _display?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "PetsOverhaul: Active Pet and Light Pet Slots",
                    delegate
                    {
                        _display.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
        //public override void PreSaveAndQuit()
        //{
        //    if (Display is not null)
        //    {
        //        ActivePetSlots player = Main.LocalPlayer.GetModPlayer<ActivePetSlots>();
        //        player.RegularPetItemSlot = Display.ActiveRegularPetSlot.Item;
        //        player.LightPetItemSlot = Display.ActiveLightPetSlot.Item;
        //    }
        //}
    }
}