using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace PetsOverhaul.Achievements
{
    public class PeakGenetics : ModAchievement
    {
        public CustomFlagCondition flag { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Collector);
            flag = AddCondition();
        }
    }
}
