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
    class CooldownCanvas : UIState
    {
        public UIText displayInfo;
        static int BaseCooldown => Main.LocalPlayer.GetModPlayer<GlobalPet>().timerMax;
        static int RemainingCooldown => Main.LocalPlayer.GetModPlayer<GlobalPet>().timer;
        public override void OnInitialize()
        {
            displayInfo = new("");
            Append(displayInfo);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ModContent.GetInstance<PetPersonalization>().ShowAbilityDisplay && Main.playerInventory == false && BaseCooldown > 0)
            {
                string theText = PetUtils.LocVal("Misc.BaseCd") + "\n" + (BaseCooldown == 0 ? PetUtils.LocVal("Misc.NoCd") : Math.Round((float)BaseCooldown / 60, 1).ToString() + " " +
                    (BaseCooldown > 60 ? PetUtils.LocVal("Misc.Secs") : PetUtils.LocVal("Misc.Sec"))) + "\n" + PetUtils.LocVal("Misc.RemainingCd") + "\n";
                if (RemainingCooldown > 0)
                {
                    theText += Math.Round((float)RemainingCooldown / 60, 1).ToString() + " " + (RemainingCooldown > 60 ? PetUtils.LocVal("Misc.Secs") : PetUtils.LocVal("Misc.Sec"));
                }
                else
                {
                    theText += PetUtils.LocVal("Misc.ReadyCd");
                }
                displayInfo.SetText(theText);
                displayInfo.Top.Set(0, ModContent.GetInstance<PetPersonalization>().AbilityDisplayPos.Y);
                displayInfo.Left.Set(0, ModContent.GetInstance<PetPersonalization>().AbilityDisplayPos.X);
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