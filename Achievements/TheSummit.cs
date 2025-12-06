using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace PetsOverhaul.Achievements
{
    public class TheSummit : ModAchievement
    {
        public CustomFlagCondition HarvestingLvl { get; private set; }
        public CustomFlagCondition MiningLvl { get; private set; }
        public CustomFlagCondition FishingLvl { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            HarvestingLvl = AddCondition("HarvestingCondition");
            MiningLvl = AddCondition("MiningCondition");
            FishingLvl = AddCondition("FishingCondition");
            //These are set to 'complete' in Junimo's code when they reach lvl 50.
        }
    }
}
