using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace PetsOverhaul.Achievements
{
    public class Purrfection : ModAchievement
    {
        public CustomFlagCondition PerfectCombined { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Collector);
            PerfectCombined = AddCondition();
        }
    }
}
