using PetsOverhaul.Systems;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Achievements
{
    public class MasteryCraft : ModAchievement
    {
        public CustomFlagCondition PetCrafted { get; private set; }
        public override void Load()
        {
            AchievementsHelper.OnItemCraft += MasterShardCrafts;
        }
        public override void Unload()
        {
            AchievementsHelper.OnItemCraft -= MasterShardCrafts;
        }
        private void MasterShardCrafts(short itemId, int count)
        {
            if (ContentSamples.ItemsByType[itemId].master && PetUtils.ItemIsPetItem(itemId))
            {
                PetCrafted.Complete();
            }
        }

        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Collector);
            PetCrafted = AddCondition();
        }
    }
}
