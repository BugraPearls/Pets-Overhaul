using Microsoft.Xna.Framework.Graphics;
using PetsOverhaul.PetEffects;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Buffs
{
    public class PetGlobalBuff : GlobalBuff
    {
        public override bool PreDraw(SpriteBatch spriteBatch, int type, int buffIndex, ref BuffDrawParams drawParams)
        {
            if (type == BuffID.LunaticCultistPet)
            {
                PhantasmalDragon dragon = Main.LocalPlayer.GetModPlayer<PhantasmalDragon>();
                switch (dragon.currentAbility)
                {
                    case 0:
                        drawParams.DrawColor = Microsoft.Xna.Framework.Color.DeepSkyBlue;
                        break;
                    case 1:
                        drawParams.DrawColor = Microsoft.Xna.Framework.Color.PaleTurquoise;
                        break;
                    case 2:
                        drawParams.DrawColor = Microsoft.Xna.Framework.Color.Coral;
                        break;
                    default:
                        break;
                }

            }
            return true;
        }
    }
}
