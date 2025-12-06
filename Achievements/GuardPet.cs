using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace PetsOverhaul.Achievements
{
    public class GuardPet : ModAchievement
    {
        public CustomIntCondition Shield { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            Shield = AddIntCondition(5000);
        }
    }
}
