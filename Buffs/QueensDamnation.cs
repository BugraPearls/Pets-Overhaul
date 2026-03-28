using Terraria;
using Terraria.ModLoader;

namespace PetsOverhaul.Buffs
{
    public class QueensDamnation : ModBuff //DrawEffects in PetGlobalNPC class
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}
