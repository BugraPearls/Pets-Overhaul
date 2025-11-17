using Terraria.Achievements;
using Terraria.ModLoader;
using Terraria.GameContent.Achievements;

namespace PetsOverhaul.Achievements
{
    public class NeedOfCompany : ModAchievement
    {
        public CustomFlagCondition PetEquipped { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Collector);
            PetEquipped = AddCondition();
        }
    }
}
