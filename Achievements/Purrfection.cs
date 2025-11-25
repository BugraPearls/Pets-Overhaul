using Terraria.Achievements;
using Terraria.ModLoader;
using Terraria.GameContent.Achievements;
using PetsOverhaul.Systems;
using System.Linq;
using System.Collections.Generic;
using Terraria.ID;

namespace PetsOverhaul.Achievements
{
    public class Purrfection : ModAchievement
    {
        public CustomFlagCondition PerfectCombined { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Collector);
            PerfectCombined = AddCondition();
        }
    }
}
