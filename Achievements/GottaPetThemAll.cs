using PetsOverhaul.Systems;
using PetsOverhaul.TownPets;
using System.Collections.Generic;
using System.Linq;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Achievements
{
    public class GottaPetThemAll : ModAchievement
    {
        public CustomIntCondition Petted { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Collector);
            Petted = AddIntCondition(PetIDs.TownPetBuffs.Count);
        }
    }
}
