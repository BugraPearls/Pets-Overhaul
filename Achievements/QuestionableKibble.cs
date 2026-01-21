using PetsOverhaul.Systems;
using Terraria;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

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
                if (Main.netMode == NetmodeID.Server)
                {
                    ModPacket packet = ModContent.GetInstance<PetsOverhaul>().GetPacket();
                    packet.Write((byte)MessageType.QuestionableKibble);
                    packet.Send(toClient: self.playerIndexTheItemIsReservedFor);
                }
                else
                {
                    ModContent.GetInstance<QuestionableKibble>().PetShimmered.Complete();
                }
            }
            orig(self);
        }
    }
}
