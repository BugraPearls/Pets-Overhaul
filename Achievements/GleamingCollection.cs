using PetsOverhaul.Systems;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace PetsOverhaul.Achievements
{
    public class GleamingCollection : ModAchievement
    {
        public CustomIntCondition CollectedPets { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Collector);
            CollectedPets = AddIntCondition(PetIDs.LightPetNamesAndItems.Count);
        }
    }
}
