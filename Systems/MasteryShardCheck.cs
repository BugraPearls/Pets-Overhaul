using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.Systems
{
    public class MasteryShardCheck : ModSystem
    {
        public static bool masteryShardObtainedEoC = false;
        public static bool masteryShardObtainedWoF = false;
        public static bool masteryShardObtainedGolem = false;
        public static bool masteryShardObtainedSkeletron = false;
        public static bool masteryShardObtainedML = false;
        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("masteryshard1", masteryShardObtainedEoC);
            tag.Add("masteryshard2", masteryShardObtainedWoF);
            tag.Add("masteryshard3", masteryShardObtainedGolem);
            tag.Add("masteryshard4", masteryShardObtainedSkeletron);
            tag.Add("masteryshard5", masteryShardObtainedML);
        }
        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.TryGet("masteryshard1", out bool shard1))
            {
                masteryShardObtainedEoC = shard1;
            }

            if (tag.TryGet("masteryshard2", out bool shard2))
            {
                masteryShardObtainedWoF = shard2;
            }

            if (tag.TryGet("masteryshard3", out bool shard3))
            {
                masteryShardObtainedGolem = shard3;
            }

            if (tag.TryGet("masteryshard4", out bool shard4))
            {
                masteryShardObtainedSkeletron = shard4;
            }

            if (tag.TryGet("masteryshard5", out bool shard5))
            {
                masteryShardObtainedML = shard5;
            }
        }
    }
    public class FirstKillEoC : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return !MasteryShardCheck.masteryShardObtainedEoC;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return PetUtils.LocVal("NPCs.MasteryShard1");
        }
    }
    public class FirstKillWoF : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return !MasteryShardCheck.masteryShardObtainedWoF;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return PetUtils.LocVal("NPCs.MasteryShard2");
        }
    }
    public class FirstKillGolem : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return !MasteryShardCheck.masteryShardObtainedGolem;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return PetUtils.LocVal("NPCs.MasteryShard3");
        }
    }
    public class FirstKillSkeletron : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return !MasteryShardCheck.masteryShardObtainedSkeletron;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return PetUtils.LocVal("NPCs.MasteryShard4");
        }
    }
    public class FirstKillMoonLord : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return !MasteryShardCheck.masteryShardObtainedML;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return PetUtils.LocVal("NPCs.MasteryShard5");
        }
    }
}
