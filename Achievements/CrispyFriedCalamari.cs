using Terraria.Achievements;
using Terraria.ModLoader;
using Terraria.GameContent.Achievements;
using PetsOverhaul.Systems;
using System.Linq;
using System.Collections.Generic;
using Terraria.ID;
using Terraria;

namespace PetsOverhaul.Achievements
{
    public class CrispyFriedCalamari : ModAchievement
    {
        public override bool Hidden => true;
        public CustomFlagCondition PetFried { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Explorer);
            PetFried = AddCondition();
        }
        public override void Load()
        {
            On_Item.GetShimmered += OnShimmer;
        }
        private static void OnShimmer(On_Item.orig_GetShimmered orig, Item self)
        {
            if (self.type == ItemID.MoonLordPetItem)
            {
                ModContent.GetInstance<CrispyFriedCalamari>().PetFried.Complete();
            }
            orig(self);
        }
    }
}
