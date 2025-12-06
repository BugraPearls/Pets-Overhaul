using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace PetsOverhaul.Achievements
{
    public class SagePet : ModAchievement
    {
        public CustomIntCondition Heal { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            Heal = AddIntCondition(15000);
        }
    }
}
