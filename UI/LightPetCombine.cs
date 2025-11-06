using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PetsOverhaul.NPCs;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
namespace PetsOverhaul.UI
{   // This class wraps the vanilla ItemSlot class into a UIElement. The ItemSlot class was made before the UI system was made, so it can't be used normally with UIState. 
    // By wrapping the vanilla ItemSlot class, we can easily use ItemSlot.
    // ItemSlot isn't very modder friendly and operates based on a "Context" number that dictates how the slot behaves when left, right, or shift clicked and the background used when drawn. 
    // If you want more control, you might need to write your own UIElement.
    // I've added basic functionality for validating the item attempting to be placed in the slot via the validItem Func. 
    // See ExamplePersonUI for usage and use the Awesomify chat option of Example Person to see in action.
    // Took from VanillaItemSlotWrapper.cs of ExampleMod
    internal class LightPetSlot : UIElement
    {
        internal Item Item;
        private readonly int _context;
        private readonly float _scale;
        internal Func<Item, bool> ValidItemFunc;

        public LightPetSlot(int context = ItemSlot.Context.BankItem, float scale = 1f)
        {
            _context = context;
            _scale = scale;
            Item = new Item();
            Item.SetDefaults(0);

            Width.Set(40 * scale, 0f);
            Height.Set(40 * scale, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float oldScale = Main.inventoryScale;
            Main.inventoryScale = _scale;
            Rectangle rectangle = GetDimensions().ToRectangle();

            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
            {
                Main.LocalPlayer.mouseInterface = true;
                if (ValidItemFunc == null || ValidItemFunc(Main.mouseItem))
                {
                    // Handle handles all the click and hover actions based on the context.
                    ItemSlot.Handle(ref Item, _context);
                }
            }
            // Draw draws the slot itself and Item. Depending on context, the color will change, as will drawing other things like stack counts.
            ItemSlot.Draw(spriteBatch, ref Item, _context, rectangle.TopLeft());
            Main.inventoryScale = oldScale;
        }
    }

    //public class LightPetSlot : UIPanel //first custom slot prototype
    //{
    //    public Item itemInTheSlot = new();
    //    public override void LeftClick(UIMouseEvent evt)
    //    {
    //        if (Main.mouseItem.IsAir && itemInTheSlot.IsAir)
    //            return;

    //        (Main.mouseItem, itemInTheSlot) = (itemInTheSlot, Main.mouseItem); //Apparently you can swap values using Tuples without typing out a temporary variable
    //        SoundEngine.PlaySound(SoundID.Grab);
    //    }
    //}

    public class ClickPreventedPanel : UIPanel
    {
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            if (ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
        }
    }
    public class LightPetCombineCanvas : UIState
    {
        internal Item NewItem => LightPetItem.CombineLightPets(slot1.Item, slot2.Item);
        internal LightPetSlot slot1;
        internal LightPetSlot slot2;
        internal UIText price;
        internal UIText infoRegardingState;
        internal int cost => NewItem.value;
        public override void OnInitialize()
        {
            ClickPreventedPanel panel = new();
            panel.Width.Set(550, 0);
            panel.Height.Set(240, 0);
            panel.HAlign = 0.5f;
            panel.VAlign = 0.5f;
            Append(panel);

            UIText header = new(PetUtils.LocVal("LightPetCombineUI.Header"));
            header.HAlign = 0.5f;
            header.Top.Set(15, 0);
            panel.Append(header);

            UIPanel button = new();
            button.Width.Set(80, 0);
            button.Height.Set(30, 0);
            button.Top.Set(65, 0);
            button.Left.Set(160, 0);
            button.OnLeftClick += OnButtonClick;
            panel.Append(button);

            UIText text = new(PetUtils.LocVal("LightPetCombineUI.Combine"));
            text.HAlign = text.VAlign = 0.5f;
            button.Append(text);

            UIText infoText = new(PetUtils.LocVal("LightPetCombineUI.InfoText"));
            infoText.Top.Set(125, 0);
            panel.Append(infoText);

            slot1 = new();
            slot1.Top.Set(55, 0);
            slot1.Left.Set(20, 0);
            slot1.Width.Set(40, 0);
            slot1.Height.Set(40, 0);
            panel.Append(slot1);

            slot2 = new();
            slot2.Top.Set(55, 0);
            slot2.Left.Set(95, 0);
            slot2.Width.Set(40, 0);
            slot2.Height.Set(40, 0);
            panel.Append(slot2);

            price = new(PetUtils.LocVal("LightPetCombineUI.Price"));
            price.Top.Set(55, 0);
            price.Left.Set(250, 0);
            panel.Append(price);

            infoRegardingState = new(PetUtils.LocVal("LightPetCombineUI.State1"));
            infoRegardingState.Top.Set(85, 0);
            infoRegardingState.Left.Set(250, 0);
            panel.Append(infoRegardingState);
        }
        public void OnButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (NewItem is not null && Main.LocalPlayer.BuyItem(cost))
            {
                Item item = Main.LocalPlayer.QuickSpawnItemDirect(PetUtils.GetSource_Pet(EntitySourcePetIDs.PetMisc, "Light Pet Combine"), NewItem);
                item.value = slot1.Item.value;
                slot1.Item.TurnToAir();
                slot2.Item.TurnToAir();
                SoundEngine.PlaySound(SoundID.Item37);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (PetTamer.openLightCombineMenu == false)
            {
                if (slot1.Item is not null && slot1.Item.IsAir == false)
                {
                    Main.LocalPlayer.QuickSpawnItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.PetMisc, "Light Pet Combine Close"), slot1.Item);
                    slot1.Item.TurnToAir();
                }
                if (slot2.Item is not null && slot2.Item.IsAir == false)
                {
                    Main.LocalPlayer.QuickSpawnItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.PetMisc, "Light Pet Combine Close"), slot2.Item);
                    slot2.Item.TurnToAir();
                }
                return;
            }
            if (NewItem is not null)
            {
                price.SetText("Cost: [i:74]" + (cost / 1000000).ToString() + " [i:73]" + (cost % 1000000 / 10000).ToString() + " [i:72]" + (cost % 10000 / 100).ToString() + " [i:71]" + (cost % 100).ToString());
                if (Main.LocalPlayer.CanAfford(cost))
                    infoRegardingState.SetText(PetUtils.LocVal("LightPetCombineUI.State2"));
                else
                    infoRegardingState.SetText(PetUtils.LocVal("LightPetCombineUI.State3"));
            }
            else
            {
                price.SetText("Cost: N/A");
                if (slot1.Item is not null && slot1.Item.IsAir == false && slot2.Item is not null && slot2.Item.IsAir == false)
                {
                    bool flag1 = PetIDs.LightPetNamesAndItems.ContainsValue(slot1.Item.type);
                    bool flag2 = PetIDs.LightPetNamesAndItems.ContainsValue(slot2.Item.type);
                    if (!flag1 && !flag2)
                    {
                        infoRegardingState.SetText(PetUtils.LocVal("LightPetCombineUI.State4"));
                    }
                    else if (!flag1)
                    {
                        infoRegardingState.SetText(PetUtils.LocVal("LightPetCombineUI.State5"));
                    }
                    else if (!flag2)
                    {
                        infoRegardingState.SetText(PetUtils.LocVal("LightPetCombineUI.State6"));
                    }
                }
                else
                {

                    infoRegardingState.SetText(PetUtils.LocVal("LightPetCombineUI.State1"));
                }
            }

            base.Draw(spriteBatch);
        }
    }
    public class StashSystem : ModPlayer
    {
        public static List<Item> Stash = [];
        public override void OnEnterWorld()
        {
            if (Stash.Count > 0)
            {
                for (int i = 0; i < Stash.Count; i++)
                {
                    Player.QuickSpawnItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.PetMisc, "Stash"), Stash[i]);
                }
                Stash.Clear();
            }
        }
    }
    [Autoload(Side = ModSide.Client)]
    public class LightPetCombineDisplaySystem : ModSystem
    {
        internal LightPetCombineCanvas Display;
        private UserInterface _display;
        public override void OnWorldLoad() //Was originally in Load(), but in order to reset contents of the UI, its now in OnWorldLoad. Also added OpenLightPetCombineMenu = false so it won't have the menu open upon world load.
        {
            PetTamer.openLightCombineMenu = false;
            Display = new LightPetCombineCanvas();
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
                    "PetsOverhaul: Light Pet Combine Menu",
                    delegate
                    {
                        _display.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
        public override void SaveWorldData(TagCompound tag)
        {
            if (Display is not null)
            {
                if (Display.slot1.Item is not null && Display.slot1.Item.IsAir == false)
                    tag.Add("slot1", Display.slot1.Item);
                if (Display.slot2.Item is not null && Display.slot2.Item.IsAir == false)
                    tag.Add("slot2", Display.slot2.Item);
            }
        }
        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.TryGet("slot1", out Item item))
            {
                StashSystem.Stash.Add(item);
                tag.Remove("slot1");
            }
            if (tag.TryGet("slot2", out Item item2))
            {
                StashSystem.Stash.Add(item2);
                tag.Remove("slot2");
            }
        }
    }
}