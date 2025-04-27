using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace PetsOverhaul.Tiles
{
    public class PetForge : ModTile
    {
        public override void SetStaticDefaults()
        {

            // Properties
            Main.tileNoAttach[Type] = true;
            Main.tileFrameImportant[Type] = true;
            //TileID.Sets.DisableSmartCursor[Type] = true;

            DustType = DustID.Stone;
            // Placement
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.CoordinateHeights = [16, 16];
            TileObjectData.addTile(Type);

            // Etc
            AddMapEntry(new Color(200, 150, 200), Language.GetText("Mods.PetsOverhaul.Items.PetForgeItem.DisplayName"));
        }

        public override void NumDust(int x, int y, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }

}