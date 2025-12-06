using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace PetsOverhaul.Achievements
{
    public class WarriorPet : ModAchievement
    {
        public CustomIntCondition Damage { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            Damage = AddIntCondition(25000);
        }
    }
}
