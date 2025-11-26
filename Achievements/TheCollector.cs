using Terraria.Achievements;
using Terraria.ModLoader;
using Terraria.GameContent.Achievements;
using PetsOverhaul.Systems;
using System.Linq;
using System.Collections.Generic;
using Terraria.ID;

namespace PetsOverhaul.Achievements
{
    public class TheCollector : ModAchievement
    {
        public CustomIntCondition CollectedPets { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Collector);
            CollectedPets = AddIntCondition(PetIDs.PetNamesAndItems.Count);
        }
    }
}
