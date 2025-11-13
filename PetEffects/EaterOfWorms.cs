using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class EaterOfWorms : PetEffect
    {
        public override int PetItemID => ItemID.EaterOfWorldsPetItem;
        public override PetClass PetClassPrimary => PetClassID.Mining;

        private readonly List<(int X, int Y)> tilesToRandomize = [];
        public int tileBreakXSpread = 3;
        public int tileBreakYSpread = 3;
        public int tileBreakSpreadChance = 125;
        public float nonOreSpeed = 0.2f;
        internal int mineX = -2;
        internal int mineY = -2;
        internal int oldTileType = 0;
        internal int prevX = 0;
        internal int prevY = 0;
        public override void PostUpdate()
        {
            if (PetIsEquipped(false))
            {
                tilesToRandomize.Clear();
                Tile tile = Main.SmartCursorShowing ? Main.tile[Main.SmartCursorX, Main.SmartCursorY] : Main.tile[Player.tileTargetX, Player.tileTargetY];

                if (TileID.Sets.Ore[tile.TileType] == false && PetIDs.gemTile[tile.TileType] == false && Player.controlUseItem)
                {
                    Player.pickSpeed -= Player.pickSpeed * nonOreSpeed;
                }
                if (Player.controlUseItem && Player.HeldItem.pick > 0 && Main.tile[prevX, prevY].TileType == 0 && oldTileType != 0 && (TileID.Sets.Ore[oldTileType] || PetIDs.gemTile[oldTileType]))
                {
                    for (mineX = -tileBreakXSpread; mineX <= tileBreakXSpread; mineX++)
                    {
                        for (mineY = -tileBreakYSpread; mineY <= tileBreakYSpread; mineY++)
                        {
                            if (Main.tile[prevX + mineX, prevY + mineY].TileType == oldTileType)
                            {
                                tilesToRandomize.Add((prevX + mineX, prevY + mineY));
                            }
                        }
                        if (Main.tile[prevX + mineX, prevY + mineY].TileType == oldTileType)
                        {
                            tilesToRandomize.Add((prevX + mineX, prevY + mineY));
                        }
                    }
                    for (int i = 0; i < PetUtils.Randomizer(tileBreakSpreadChance); i++)
                    {
                        if (tilesToRandomize.Count > 0)
                        {
                            (int X, int Y) = tilesToRandomize[Main.rand.Next(tilesToRandomize.Count)];
                            if (Player.HasEnoughPickPowerToHurtTile(X, Y))
                            {
                                Player.PickTile(X, Y, 5000);
                                if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                                {
                                    SoundEngine.PlaySound(SoundID.WormDig with { PitchVariance = 0.4f }, new Vector2(X, Y));
                                }
                            }
                        }
                    }
                }
                prevX = Main.SmartCursorShowing ? Main.SmartCursorX : Player.tileTargetX;
                prevY = Main.SmartCursorShowing ? Main.SmartCursorY : Player.tileTargetY;
                oldTileType = tile.TileType;
            }
        }
    }
    public sealed class EaterOfWorldsPetItem : PetTooltip
    {
        public override PetEffect PetsEffect => eaterOfWorms;
        public static EaterOfWorms eaterOfWorms
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out EaterOfWorms pet))
                    return pet;
                else
                    return ModContent.GetInstance<EaterOfWorms>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.EaterOfWorldsPetItem")
                       .Replace("<miningSpeed>", Math.Round(eaterOfWorms.nonOreSpeed * 100, 2).ToString())
                       .Replace("<multipleBreakChance>", eaterOfWorms.tileBreakSpreadChance.ToString())
                       .Replace("<width>", eaterOfWorms.tileBreakXSpread.ToString())
                       .Replace("<length>", eaterOfWorms.tileBreakYSpread.ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.EaterOfWorldsPetItem");
    }
}
