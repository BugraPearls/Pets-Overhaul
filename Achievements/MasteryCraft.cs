using Terraria.Achievements;
using Terraria.ModLoader;
using Terraria.GameContent.Achievements;
using PetsOverhaul.Systems;
using System.Linq;
using System.Collections.Generic;
using Terraria.ID;

namespace PetsOverhaul.Achievements
{
    public class MasteryCraft : ModAchievement
    {
        public CustomFlagCondition PetCrafted { get; private set; }
        public override void Load()
        {
            AchievementsHelper.OnItemCraft += MasterShardCrafts;
        }

        private void MasterShardCrafts(short itemId, int count)
        {
            if (ContentSamples.ItemsByType[itemId].master && (PetIDs.PetNamesAndItems.ContainsValue(itemId) || PetIDs.LightPetNamesAndItems.ContainsValue(itemId)))
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
