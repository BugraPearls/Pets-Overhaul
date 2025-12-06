using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace PetsOverhaul.Achievements
{
    public class HoverboardSkater : ModAchievement
    {
        public CustomIntCondition TimeSaved { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            TimeSaved = AddIntCondition(600);
        }
    }
}
