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
    public class ISeeWhatYoureDoing : ModAchievement
    {
        public override bool Hidden => true;
        public CustomFlagCondition flag { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            flag = AddCondition();
        }
    }
}
