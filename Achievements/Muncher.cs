using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace PetsOverhaul.Achievements
{
    public class Muncher : ModAchievement
    {
        public CustomIntCondition Munchies { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            Munchies = AddIntCondition(50);
        }
    }
}
