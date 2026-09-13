using PetsOverhaul.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Items
{
    public class Bonemerang : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.CombatWrench);
            Item.height = 32;
            Item.width = 32;
            Item.shoot = ModContent.ProjectileType<BonemerangProj>();
            Item.damage = 19;
        }
    }
}
