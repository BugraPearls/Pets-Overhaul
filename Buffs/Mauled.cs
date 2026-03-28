using Terraria;
using Terraria.ModLoader;

namespace PetsOverhaul.Buffs
{
    public class Mauled : ModBuff //DrawEffects in PetGlobalNPC class
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}
