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
    class CooldownCanvas : UIState
    {
        public UIText displayInfo;
        public UIText cooldown;
        static int BaseCooldown => Main.LocalPlayer.GetModPlayer<GlobalPet>().timerMax;
        static int RemainingCooldown => Main.LocalPlayer.GetModPlayer<GlobalPet>().timer;
        public override void OnInitialize()
        {
            UIElement canvas = new();
            canvas.Height.Set(130, 0);
            canvas.HAlign = 0.8f;
            canvas.VAlign = 0.88f;
            Append(canvas);

            displayInfo = new("");
            canvas.Append(displayInfo);

            cooldown = new("");
            cooldown.VAlign = 0.50f;
            canvas.Append(cooldown);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ModContent.GetInstance<PetPersonalization>().ShowAbilityDisplay && Main.playerInventory == false && BaseCooldown > 0)
            {
                displayInfo.SetText(Language.GetTextValue("Mods.PetsOverhaul.Misc.BaseCd") + "\n" + (BaseCooldown == 0 ? Language.GetTextValue("Mods.PetsOverhaul.Misc.NoCd") : Math.Round((float)BaseCooldown / 60, 1).ToString() + " " + (BaseCooldown > 60 ? Language.GetTextValue("Mods.PetsOverhaul.Misc.Secs") : Language.GetTextValue("Mods.PetsOverhaul.Misc.Sec"))));
                if (RemainingCooldown > 0)
                {
                    cooldown.SetText(Language.GetTextValue("Mods.PetsOverhaul.Misc.RemainingCd") + "\n" + Math.Round((float)RemainingCooldown / 60, 1).ToString() + " " + (RemainingCooldown > 60 ? Language.GetTextValue("Mods.PetsOverhaul.Misc.Secs") : Language.GetTextValue("Mods.PetsOverhaul.Misc.Sec")));
                }
                else
                {
                    cooldown.SetText(Language.GetTextValue("Mods.PetsOverhaul.Misc.RemainingCd") + "\n" + Language.GetTextValue("Mods.PetsOverhaul.Misc.ReadyCd"));
                }
                base.Draw(spriteBatch);
            }
        }
    }
    [Autoload(Side = ModSide.Client)]
    public class AbilityCooldownDisplaySystem : ModSystem
    {
        internal CooldownCanvas cooldownDisplay;
        private UserInterface _cooldownDisplay;
        public override void Load()
        {
            cooldownDisplay = new CooldownCanvas();
            cooldownDisplay.Activate();
            _cooldownDisplay = new UserInterface();
            _cooldownDisplay.SetState(cooldownDisplay);
        }
        public override void UpdateUI(GameTime gameTime)
        {
            _cooldownDisplay?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "PetsOverhaul: Pet Ability Cooldown Display",
                    delegate
                    {
                        _cooldownDisplay.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}