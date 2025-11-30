using Terraria.Achievements;
using Terraria.ModLoader;
using Terraria.GameContent.Achievements;
using PetsOverhaul.Systems;
using System.Linq;
using System.Collections.Generic;
using Terraria.ID;
using Terraria;

namespace PetsOverhaul.Achievements
{
    public class LootChaser : ModAchievement
    {
        public CustomIntCondition Count { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            Count = AddIntCondition(50);
        }
    }
}
