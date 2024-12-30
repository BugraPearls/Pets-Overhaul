using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public class ShadowMimicEquippedCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return info.player.miscEquips[0].type == ItemID.OrnateShadowKey;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return "hello";
        }
    }
    public sealed class ShadowMimic : PetEffect
    {
        public override int PetItemID => ItemID.OrnateShadowKey;
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public int npcCoin = 15;
        public int npcItem = 8;
        public int bossCoin = 10;
        public int bossItem = 5;
        public int bagCoin = 10;
        public int bagItem = 5;
        public int chanceToRollItem = 30; //Numerator will be multiplied with this, Denominator will be multiplied by 100, effectively 30% of original chance
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
            On_ItemDropResolver.ResolveRule += ShadowMimicExtraDrop;
        }

        private static ItemDropAttemptResult ShadowMimicExtraDrop(On_ItemDropResolver.orig_ResolveRule orig, ItemDropResolver self, IItemDropRule rule, DropAttemptInfo info)
        {
            if (info.player.TryGetModPlayer(out ShadowMimic mimic) && mimic.PetIsEquipped() && rule is CommonDrop drop)
            {
                if (Math.Max(drop.chanceNumerator, 1) / Math.Max(drop.chanceDenominator, 1) <= 0.1)
                {
                    drop.chanceNumerator *= 5; //This currently maxes the droprates? Look into it!
                }
            }
            return orig(self,rule,info);
        }

        public static void PreOnPickup(Item item, Player player)
        {
            GlobalPet PickerPet = player.GetModPlayer<GlobalPet>();
            ShadowMimic mimic = player.GetModPlayer<ShadowMimic>();
            if (PickerPet.PickupChecks(item, mimic.PetItemID, out ItemPet itemChck))
            {
                mimic.chanceToRollItem = 0;
                if (itemChck.itemFromNpc == true)
                {
                    mimic.chanceToRollItem += (item.IsACoin ? mimic.npcCoin : mimic.npcItem) * item.stack;
                }
                if (itemChck.itemFromBoss == true && ItemID.Sets.BossBag[item.type] == false)
                {
                    mimic.chanceToRollItem += (item.IsACoin ? mimic.bossCoin : mimic.bossItem) * item.stack;
                }
                if (itemChck.itemFromBag == true)
                {
                    mimic.chanceToRollItem += (item.IsACoin ? mimic.bagCoin : mimic.bagItem) * item.stack;
                }
                for (int i = 0; i < GlobalPet.Randomizer(mimic.chanceToRollItem); i++)
                {
                    player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.GlobalItem), item.type, 1);
                }
            }
        }
    }
    //public sealed class ShadowMimicLoot : GlobalNPC
    //{
    //    public override void OnKill(NPC npc)
    //    {
    //        DropAttemptInfo info = new();
    //        On_ItemDropResolver.resolve
    //    }
    //    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    //    {
    //        foreach (var rule in npcLoot.Get())
    //        {
    //            if (rule is CommonDrop drop && Math.Max(drop.chanceNumerator, 1) / Math.Max(drop.chanceDenominator, 1) < 0.2)
    //            {
    //                drop.OnFailedRoll(new LeadingConditionRule(new ShadowMimicEquippedCondition()).OnSuccess(ItemDropRule.Common(drop.itemId, 2)));
    //            }
    //        }
    //    }
    //}
    public sealed class OrnateShadowKey : PetTooltip
    {
        public override PetEffect PetsEffect => shadowMimic;
        public static ShadowMimic shadowMimic
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out ShadowMimic pet))
                    return pet;
                else
                    return ModContent.GetInstance<ShadowMimic>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.OrnateShadowKey")
                        .Replace("<npcCoin>", shadowMimic.npcCoin.ToString())
                        .Replace("<npcItem>", shadowMimic.npcItem.ToString())
                        .Replace("<bossCoin>", shadowMimic.bossCoin.ToString())
                        .Replace("<bossItem>", shadowMimic.bossItem.ToString())
                        .Replace("<bagCoin>", shadowMimic.bagCoin.ToString())
                        .Replace("<bagItem>", shadowMimic.bagItem.ToString());
    }
}
