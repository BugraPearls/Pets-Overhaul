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
    public class ThisWontBeHowIDie : ModAchievement
    {
        public CustomIntCondition Kills { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            Kills = AddIntCondition(100);
        }
    }
}
