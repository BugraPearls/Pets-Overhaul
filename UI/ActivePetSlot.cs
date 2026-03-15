using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
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
                Main.LocalPlayer.GetModPlayer<ActivePetSlotPlayer>().loadedPet.TurnToAir();
            }
            if (ActiveRegularUIPetSlot.Item != null && ActiveRegularUIPetSlot.Item != CurrentActivePet)
            {
                CurrentActivePet = ActiveRegularUIPetSlot.Item;
            }
            if (Main.LocalPlayer.GetModPlayer<ActivePetSlotPlayer>().loadedLightPet is not null && Main.LocalPlayer.GetModPlayer<ActivePetSlotPlayer>().loadedLightPet.IsAir == false)
            {
                ActiveLightUIPetSlot.Item = Main.LocalPlayer.GetModPlayer<ActivePetSlotPlayer>().loadedLightPet.Clone();
                Main.LocalPlayer.GetModPlayer<ActivePetSlotPlayer>().loadedLightPet.TurnToAir();
            }
            if (ActiveLightUIPetSlot.Item != null && ActiveLightUIPetSlot.Item != CurrentActiveLightPet)
            {
                CurrentActiveLightPet = ActiveLightUIPetSlot.Item;
            }
        }
        public override void OnInitialize()
        {
            ActiveRegularUIPetSlot = new(ItemSlot.Context.EquipPet, 0.83f);
            ActiveRegularUIPetSlot.Width.Set(40, 0);
            ActiveRegularUIPetSlot.Height.Set(40, 0);
            Append(ActiveRegularUIPetSlot);

            ActiveLightUIPetSlot = new(ItemSlot.Context.EquipLight, 0.83f);
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
            Vector2 pos = ModContent.GetInstance<PetPersonalization>().ActivePetSlotPos;
            ActiveRegularUIPetSlot.Left.Set(0, pos.X);
            ActiveRegularUIPetSlot.Top.Set(0, pos.Y);
            ActiveLightUIPetSlot.Left.Set(0, pos.X);
            ActiveLightUIPetSlot.Top.Set(47, pos.Y);
            base.Draw(spriteBatch);
        }
    }
    public class ActivePetSlotPlayer : ModPlayer
    {
        public static void SyncActiveRegular(Item itemToBeSynced, int toWho, int fromWho) //It doesn't 'fully' sync, but current active Pet Item should suffice.
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = ModContent.GetInstance<PetsOverhaul>().GetPacket();
                packet.Write((byte)MessageType.ActivePetSlot);
                ItemIO.Send(itemToBeSynced, packet);
                packet.Send(toWho, fromWho);
            }
        }
        public static void SyncActiveLight(Item itemToBeSynced, int toWho, int fromWho)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = ModContent.GetInstance<PetsOverhaul>().GetPacket();
                packet.Write((byte)MessageType.ActiveLightPetSlot);
                ItemIO.Send(itemToBeSynced, packet);
                packet.Send(toWho, fromWho);
            }
        }
        internal Item loadedPet;
        internal Item loadedLightPet;
        internal PetItemSlot ActiveRegularPetSlot;
        internal PetItemSlot ActiveLightPetSlot;
        internal List<Item> RegularPetItemSlot = [new(0), new(0), new(0)];
        internal List<Item> LightPetItemSlot = [new(0), new(0), new(0)];
        internal Item CurrentPetItemInTheUI
        {
            get { return ModContent.GetInstance<ActivePetSlotSystem>().Display.ActiveRegularUIPetSlot.Item; }
            set { ModContent.GetInstance<ActivePetSlotSystem>().Display.ActiveRegularUIPetSlot.Item = value; }
        }
        internal Item CurrentLightPetItemInTheUI
        {
            get { return ModContent.GetInstance<ActivePetSlotSystem>().Display.ActiveLightUIPetSlot.Item; }
            set { ModContent.GetInstance<ActivePetSlotSystem>().Display.ActiveLightUIPetSlot.Item = value; }
        }
        public override void OnEquipmentLoadoutSwitched(int oldLoadoutIndex, int loadoutIndex)
        {
            if (Main.myPlayer == Player.whoAmI)
            {
                RegularPetItemSlot[oldLoadoutIndex] = CurrentPetItemInTheUI;
                LightPetItemSlot[oldLoadoutIndex] = CurrentLightPetItemInTheUI;
                CurrentPetItemInTheUI = RegularPetItemSlot[loadoutIndex];
                CurrentLightPetItemInTheUI = LightPetItemSlot[loadoutIndex];
                SyncActiveRegular(RegularPetItemSlot[loadoutIndex], -1, Player.whoAmI);
                SyncActiveLight(LightPetItemSlot[loadoutIndex], -1, Player.whoAmI);
            }
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            SyncActiveRegular(RegularPetItemSlot[Player.CurrentLoadoutIndex], toWho, fromWho);
            SyncActiveLight(LightPetItemSlot[Player.CurrentLoadoutIndex], toWho, fromWho);
        }
        public override void CopyClientState(ModPlayer targetCopy)
        {
            ActivePetSlotPlayer clone = (ActivePetSlotPlayer)targetCopy;
            clone.RegularPetItemSlot[Player.CurrentLoadoutIndex] = RegularPetItemSlot[Player.CurrentLoadoutIndex];
            clone.LightPetItemSlot[Player.CurrentLoadoutIndex] = LightPetItemSlot[Player.CurrentLoadoutIndex];
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            ActivePetSlotPlayer clone = (ActivePetSlotPlayer)clientPlayer;

            if (RegularPetItemSlot[Player.CurrentLoadoutIndex] != clone.RegularPetItemSlot[Player.CurrentLoadoutIndex])
            {
                SyncActiveRegular(RegularPetItemSlot[Player.CurrentLoadoutIndex], -1, Player.whoAmI);
            }
            if (LightPetItemSlot[Player.CurrentLoadoutIndex] != clone.LightPetItemSlot[Player.CurrentLoadoutIndex])
            {
                SyncActiveLight(LightPetItemSlot[Player.CurrentLoadoutIndex], -1, Player.whoAmI);
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