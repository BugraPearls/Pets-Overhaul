using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
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
        static string StackSpecial => Main.LocalPlayer.GetModPlayer<GlobalPet>().currentPetStackSpecialText;
        static readonly string Current = PetTextsColors.LocVal("Misc.Current");
        public override void OnInitialize()
        {
            stacks = new("");
            Append(stacks);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ModContent.GetInstance<PetPersonalization>().ShowResourceDisplay && Main.playerInventory == false && (MaxStack >= 0 || StackSpecial != string.Empty))
            {
                string currTxt = $"{PetTextsColors.LocVal("Misc.Current")} {TextStack}:";
                if (StackSpecial != string.Empty) 
                {
                    stacks.SetText($"{currTxt} {StackSpecial}");
                }
                else if (MaxStack == 0)
                {
                    stacks.SetText($"{currTxt} {CurrentStack}");
                }
                else
                {
                    stacks.SetText($"{currTxt} {CurrentStack} {PetTextsColors.LocVal("LightPetTooltips.OutOf")} {MaxStack}");
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