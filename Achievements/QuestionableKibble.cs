using Terraria.Achievements;
using Terraria.ModLoader;
using Terraria.GameContent.Achievements;
using PetsOverhaul.Systems;
using System.Linq;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.GameContent;
using Terraria;

namespace PetsOverhaul.Achievements
{
    public class QuestionableKibble : ModAchievement
    {
        public CustomFlagCondition PetShimmered { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Explorer);
            PetShimmered = AddCondition();
        }
        public override void Load()
        {
            On_Item.GetShimmered += OnShimmer;
        }
        private static void OnShimmer(On_Item.orig_GetShimmered orig, Item self)
        {
            if (PetUtils.ItemIsPetItem(self.type))
            {
                ModContent.GetInstance<QuestionableKibble>().PetShimmered.Complete();
            }
            orig(self);
        }
    }
}
