using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace PetsOverhaul.Systems
{
    public static class PetConditions
    {
        public static Condition ConsumedHead = new("Mods.PetsOverhaul.Misc.RequireHead", () => PetModPlayer.pumpkingConsumed);
        public static Condition ConsumedOptic = new("Mods.PetsOverhaul.Misc.RequireOptic", () => PetModPlayer.eolConsumed);
        public static Condition ConsumedWrench = new("Mods.PetsOverhaul.Misc.RequireWrench", () => PetModPlayer.golemConsumed);
    }
    public class NotABossCondition : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info) => !info.npc.boss;
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => null;
    }
}