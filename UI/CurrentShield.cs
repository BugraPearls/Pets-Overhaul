﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using ReLogic.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;
namespace PetsOverhaul.UI
{
    class CurrentShield : UIElement
    {
        Color color => Main.MouseTextColorReal;
        int CurrentShieldVal => Main.LocalPlayer.GetModPlayer<GlobalPet>().currentShield;
        ShieldPosition ShieldSetting => ModContent.GetInstance<PetPersonalization>().ShieldLocation;
        Vector2 ShieldLoc;
        Vector2 ValueLoc;
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (CurrentShieldVal > 0)
            {
                ShieldLoc = ShieldSetting == ShieldPosition.PlayerLeft ? (new Vector2(Main.screenWidth - 70, Main.screenHeight - 110) / 2f) :
                    ShieldSetting == ShieldPosition.PlayerRight ? (new Vector2(Main.screenWidth - 10, Main.screenHeight - 110) / 2f) :
                    ShieldSetting == ShieldPosition.HealthBarLeft ? new Vector2(Main.screenWidth * 0.81f - 35, Main.screenHeight * 0.03f - 5) :
                    new Vector2(Main.screenWidth * 0.81f - 5, Main.screenHeight * 0.03f - 5);

                ValueLoc = ShieldSetting == ShieldPosition.PlayerLeft ? (new Vector2(Main.screenWidth, Main.screenHeight - 100) / 2f) :
                    ShieldSetting == ShieldPosition.PlayerRight ? (new Vector2(Main.screenWidth - 60, Main.screenHeight - 100) / 2f) :
                    ShieldSetting == ShieldPosition.HealthBarLeft ? new Vector2(Main.screenWidth * 0.81f, Main.screenHeight * 0.03f) :
                    new Vector2(Main.screenWidth * 0.81f - 30, Main.screenHeight * 0.03f);
                spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("PetsOverhaul/UI/PetShield"), ShieldLoc, color);
                spriteBatch.DrawString(FontAssets.MouseText.Value, CurrentShieldVal.ToString(), new Vector2(ValueLoc.X + 2, ValueLoc.Y), Color.Black);
                spriteBatch.DrawString(FontAssets.MouseText.Value, CurrentShieldVal.ToString(), ValueLoc, color);
            }
        }
    }
    class MenuCanvas : UIState
    {
        public CurrentShield shieldDisplay;
        public override void OnInitialize()
        {
            shieldDisplay = new CurrentShield();
            Append(shieldDisplay);
        }
    }
    [Autoload(Side = ModSide.Client)]
    public class PetShieldDisplaySystem : ModSystem
    {
        internal MenuCanvas Display;
        private UserInterface _display;
        public override void Load()
        {
            Display = new MenuCanvas();
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
                    "PetsOverhaul: Pet Shield Display",
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