using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace PetsOverhaul.Achievements
{
    public class ElusiveHunter : ModAchievement
    {
        public CustomIntCondition Hunts { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Slayer);
            Hunts = AddIntCondition(15);
        }
    }
}
