using Terraria;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace PetsOverhaul.Achievements
{
    public class UnpaidLabor : ModAchievement
    {
        public CustomIntCondition CoinsGained { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            CoinsGained = AddIntCondition(Item.platinum * 5);
        }
    }
}
