using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

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
