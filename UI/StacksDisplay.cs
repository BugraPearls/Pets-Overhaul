using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
namespace PetsOverhaul.UI
{
    class StackCanvas : UIState
    {
        public UIText stacks;
        static int CurrentStack => Main.LocalPlayer.GetModPlayer<GlobalPet>().currentPetStacks;
        static int MaxStack => Main.LocalPlayer.GetModPlayer<GlobalPet>().currentPetStacksMax;
        static string TextStack => Main.LocalPlayer.GetModPlayer<GlobalPet>().currentPetStackText;
        static readonly string Current = Language.GetTextValue("Mods.PetsOverhaul.Misc.Current");
        public override void OnInitialize()
        {
            UIElement canvas = new();
            canvas.Height.Set(130, 0);
            canvas.HAlign = 0.6f;
            canvas.VAlign = 0.68f;
            Append(canvas);

            stacks = new("");
            canvas.Append(stacks);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Main.playerInventory == false && MaxStack >= 0)
            {
                stacks.SetText($"{Language.GetTextValue("Mods.PetsOverhaul.Misc.Current")} {TextStack}: {CurrentStack} {Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.OutOf")} {MaxStack}");
                base.Draw(spriteBatch);
            }
        }
    }
    [Autoload(Side = ModSide.Client)]
    public class StackDisplaySystem : ModSystem
    {
        internal StackCanvas stackDisplay;
        private UserInterface _stackDisplay;
        public override void Load()
        {
            stackDisplay = new StackCanvas();
            stackDisplay.Activate();
            _stackDisplay = new UserInterface();
            _stackDisplay.SetState(stackDisplay);
        }
        public override void UpdateUI(GameTime gameTime)
        {
            _stackDisplay?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "PetsOverhaul: Pet Stack Display",
                    delegate
                    {
                        _stackDisplay.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}