using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
namespace PetsOverhaul.UI
{

    public class ActivePetSlotCanvas : UIState
    {
        internal Item CurrentActivePet
        {
            get { return Main.LocalPlayer.GetModPlayer<ActivePetSlotPlayer>().RegularPetItemSlot[Main.LocalPlayer.CurrentLoadoutIndex]; }
            set { Main.LocalPlayer.GetModPlayer<ActivePetSlotPlayer>().RegularPetItemSlot[Main.LocalPlayer.CurrentLoadoutIndex] = value; }
        }
        internal Item CurrentActiveLightPet
        {
            get { return Main.LocalPlayer.GetModPlayer<ActivePetSlotPlayer>().LightPetItemSlot[Main.LocalPlayer.CurrentLoadoutIndex]; }
            set { Main.LocalPlayer.GetModPlayer<ActivePetSlotPlayer>().LightPetItemSlot[Main.LocalPlayer.CurrentLoadoutIndex] = value; }
        }
        internal PetItemSlot ActiveRegularUIPetSlot;
        internal PetItemSlot ActiveLightUIPetSlot;
        internal UIText HoverText;
        public override void Update(GameTime gameTime)
        {
            if (Main.LocalPlayer.GetModPlayer<ActivePetSlotPlayer>().loadedPet is not null && Main.LocalPlayer.GetModPlayer<ActivePetSlotPlayer>().loadedPet.IsAir == false)
            {
                ActiveRegularUIPetSlot.Item = Main.LocalPlayer.GetModPlayer<ActivePetSlotPlayer>().loadedPet.Clone();
                Main.LocalPlayer.GetModPlayer<ActivePetSlotPlayer>().loadedPet.TurnToAir(true);
            }
            if (ActiveRegularUIPetSlot.Item != null && ActiveRegularUIPetSlot.Item != CurrentActivePet)
            {
                CurrentActivePet = ActiveRegularUIPetSlot.Item.Clone();
            }
            if (Main.LocalPlayer.GetModPlayer<ActivePetSlotPlayer>().loadedLightPet is not null && Main.LocalPlayer.GetModPlayer<ActivePetSlotPlayer>().loadedLightPet.IsAir == false)
            {
                ActiveLightUIPetSlot.Item = Main.LocalPlayer.GetModPlayer<ActivePetSlotPlayer>().loadedLightPet.Clone();
                Main.LocalPlayer.GetModPlayer<ActivePetSlotPlayer>().loadedLightPet.TurnToAir(true);
            }
            if (ActiveLightUIPetSlot.Item != null && ActiveLightUIPetSlot.Item != CurrentActiveLightPet)
            {
                CurrentActiveLightPet = ActiveLightUIPetSlot.Item.Clone();
            }
        }
        public override void OnInitialize()
        {
            ActiveRegularUIPetSlot = new(ItemSlot.Context.EquipPet, 0.8f);
            ActiveRegularUIPetSlot.Width.Set(40, 0);
            ActiveRegularUIPetSlot.Height.Set(40, 0);
            Append(ActiveRegularUIPetSlot);

            ActiveLightUIPetSlot = new(ItemSlot.Context.EquipLight, 0.8f);
            ActiveLightUIPetSlot.Width.Set(40, 0);
            ActiveLightUIPetSlot.Height.Set(40, 0);
            Append(ActiveLightUIPetSlot);

            HoverText = new("");
            Append(HoverText);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Main.EquipPage != 2)
            {
                return;
            }
            if ((ActiveRegularUIPetSlot.IsMouseHovering && ActiveRegularUIPetSlot.Item is not null && ActiveRegularUIPetSlot.Item.IsAir) || (ActiveLightUIPetSlot.IsMouseHovering && ActiveLightUIPetSlot.Item is not null && ActiveLightUIPetSlot.Item.IsAir))
            {
                HoverText.SetText(PetUtils.LocVal("Misc.ActivePetSlotHover"));
                float endOfTextX = 0;
                if (Main.MouseScreen.X + HoverText.MinWidth.Pixels > Main.screenWidth)
                {
                    endOfTextX = Main.screenWidth - (Main.MouseScreen.X + HoverText.MinWidth.Pixels);
                }
                HoverText.Left.Set(Main.MouseScreen.X + endOfTextX, 0);
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
            ActiveRegularUIPetSlot.Left.Set(pos.X, 0);
            ActiveRegularUIPetSlot.Top.Set(pos.Y, 0);
            ActiveLightUIPetSlot.Left.Set(posLight.X, 0);
            ActiveLightUIPetSlot.Top.Set(posLight.Y, 0);
            base.Draw(spriteBatch);
        }
    }
    public class ActivePetSlotPlayer : ModPlayer
    {
        internal Item loadedPet;
        internal Item loadedLightPet;
        internal PetItemSlot ActiveRegularPetSlot;
        internal PetItemSlot ActiveLightPetSlot;
        internal List<Item> RegularPetItemSlot = [new(0), new(0), new(0)];
        internal List<Item> LightPetItemSlot = [new(0), new(0), new(0)];
        internal Item CurrentPetItem
        {
            get { return ModContent.GetInstance<ActivePetSlotSystem>().Display.ActiveRegularUIPetSlot.Item; }
            set { ModContent.GetInstance<ActivePetSlotSystem>().Display.ActiveRegularUIPetSlot.Item = value; }
        }
        internal Item CurrentLightPetItem
        {
            get { return ModContent.GetInstance<ActivePetSlotSystem>().Display.ActiveLightUIPetSlot.Item; }
            set { ModContent.GetInstance<ActivePetSlotSystem>().Display.ActiveLightUIPetSlot.Item = value; }
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
    }
}