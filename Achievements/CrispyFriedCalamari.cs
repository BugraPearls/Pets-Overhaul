using Terraria;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

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
