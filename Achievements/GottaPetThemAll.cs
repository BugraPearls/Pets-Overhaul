using PetsOverhaul.Systems;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace PetsOverhaul.Achievements
{
    public class GottaPetThemAll : ModAchievement
    {
        public CustomIntCondition Petted { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Collector);
            Petted = AddIntCondition(PetIDs.TownPetBuffs.Count);
        }
    }
}
