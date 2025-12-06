using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace PetsOverhaul.Achievements
{
    public class Arsonist : ModAchievement
    {
        public CustomIntCondition BurnAmplifiedFrames { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            BurnAmplifiedFrames = AddIntCondition(3600); //1 hour

        }
    }
}
