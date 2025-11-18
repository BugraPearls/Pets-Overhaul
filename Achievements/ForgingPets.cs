using Terraria.Achievements;
using Terraria.ModLoader;
using Terraria.GameContent.Achievements;
using PetsOverhaul.Systems;
using System.Linq;
using System.Collections.Generic;
using Terraria.ID;

namespace PetsOverhaul.Achievements
{
    public class ForgingPets : ModAchievement
    {
        public ItemCraftCondition PetCrafted { get; private set; }
        public override void SetStaticDefaults()
        {
            List<int> list = [.. PetIDs.LightPetNamesAndItems.Values];
            list.AddRange(PetIDs.PetNamesAndItems.Values);
            
            Achievement.SetCategory(AchievementCategory.Collector);
            PetCrafted = AddItemCraftCondition([.. list]);
        }
    }
}
