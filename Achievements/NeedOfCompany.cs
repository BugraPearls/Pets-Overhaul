using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace PetsOverhaul.Achievements
{
    public class NeedOfCompany : ModAchievement
    {
        public CustomFlagCondition PetEquipped { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            PetEquipped = AddCondition();
        }
    }
}
