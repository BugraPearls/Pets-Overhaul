using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace PetsOverhaul.Achievements
{
    public class ThisWontBeHowIDie : ModAchievement
    {
        public CustomIntCondition Kills { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            Kills = AddIntCondition(100);
        }
    }
}
