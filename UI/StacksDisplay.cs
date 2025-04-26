using Humanizer;
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
        static bool ShouldBeSeconds => Main.LocalPlayer.GetModPlayer<GlobalPet>().currentPetStackIsSeconds;
        static readonly string Current = PetTextsColors.LocVal("Misc.Current");
        public override void OnInitialize()
        {
            stacks = new("");
            Append(stacks);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ModContent.GetInstance<PetPersonalization>().ShowResourceDisplay && Main.playerInventory == false && MaxStack >= 0)
            {
                if (MaxStack == 0)
                {
                    stacks.SetText($"{PetTextsColors.LocVal("Misc.Current")} {TextStack}: {(ShouldBeSeconds ? (Math.Round(CurrentStack / 60f, 1).ToString() + " " + PetTextsColors.LocVal("Misc.Secs")) : CurrentStack.ToString())}");
                }
                else
                {
                    stacks.SetText($"{PetTextsColors.LocVal("Misc.Current")} {TextStack}: {(ShouldBeSeconds ? (Math.Round(CurrentStack / 60f, 1).ToString()) : CurrentStack.ToString())} {PetTextsColors.LocVal("LightPetTooltips.OutOf")} {(ShouldBeSeconds ? (Math.Round(MaxStack / 60f, 2).ToString() + " " + PetTextsColors.LocVal("Misc.Secs")) : MaxStack.ToString())}");
                }
                stacks.Top.Set(0, ModContent.GetInstance<PetPersonalization>().ResourceDisplayPos.Y);
                stacks.Left.Set(0, ModContent.GetInstance<PetPersonalization>().ResourceDisplayPos.X);
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