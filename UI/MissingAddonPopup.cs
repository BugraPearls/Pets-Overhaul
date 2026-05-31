using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PetsOverhaul.Achievements;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using ReLogic.Utilities;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;
namespace PetsOverhaul.UI
{
    class MissingAddonPopupCanvas : UIState
    {
        public UIPanel panel = new();
        public override void OnInitialize()
        {
            panel.Width.Set(800, 0);
            panel.Height.Set(275, 0);
            panel.HAlign = 0.5f;
            panel.VAlign = 0.5f;
            Append(panel);

            UIText message = new(Language.GetText("Mods.PetsOverhaul.MissingAddonUI.CalamityMessage"));
            message.HAlign = 0.5f;
            message.Top.Set(15, 0);
            panel.Append(message);

            UIButton<LocalizedText> closeButton = new(Language.GetText("Mods.PetsOverhaul.MissingAddonUI.Close"));
            closeButton.Width.Set(225, 0);
            closeButton.Height.Set(40, 0);
            closeButton.HAlign = 0.8f;
            closeButton.VAlign = 0.91f;
            closeButton.OnLeftClick += OnCloseButtonClick;
            closeButton.HoverSound = SoundID.MenuTick;
            closeButton.ClickSound = SoundID.MenuClose;
            panel.Append(closeButton);

            UIButton<LocalizedText> neverButton = new(Language.GetText("Mods.PetsOverhaul.MissingAddonUI.Never"));
            neverButton.Width.Set(225, 0);
            neverButton.Height.Set(40, 0);
            neverButton.HAlign = 0.2f;
            neverButton.VAlign = 0.91f;
            neverButton.OnLeftClick += OnNeverButtonClick;
            neverButton.HoverSound = SoundID.MenuTick;
            neverButton.ClickSound = SoundID.MenuClose;
            panel.Append(neverButton);

        }
        public void OnCloseButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.LocalPlayer.PetPlayer().activateCalamityAddonPopup = false;
        }

        public void OnNeverButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.LocalPlayer.PetPlayer().NeverShowCalamityAddonPopup = true;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            // If this code is in the panel or container element, check it directly
            if (panel.ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
        }
    }

    [Autoload(Side = ModSide.Client)]
    public class MissingAddonPopupDisplaySystem : ModSystem
    {
        internal MissingAddonPopupCanvas Display;
        private UserInterface _display;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                Display = new MissingAddonPopupCanvas();
                Display.Activate();
                _display = new UserInterface();
            }
        }
        public override void UpdateUI(GameTime gameTime)
        {
            _display?.Update(gameTime);
            if (Main.LocalPlayer.PetPlayer().ShowCalamityAddonPopup)
            {
                _display?.SetState(Display);
            }
            else
            {
                _display?.SetState(null);
            }

        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "PetsOverhaul: Missing Addon Popup",
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