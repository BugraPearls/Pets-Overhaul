using PetsOverhaul.Systems;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Squashling : PetEffect
    {
        public override int PetItemID => ItemID.MagicalPumpkinSeed;
        public override PetClass PetClassPrimary => PetClassID.Harvesting;
        public int squashlingCommonChance = 50;
        public int squashlingRareChance = 20;
        public int pumpkinArmorBonusHp = 5;
        public int pumpkinArmorBonusHarvestingFortune = 10;
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public static void PreOnPickup(Item item, Player player)
        {
            Squashling squash = player.GetModPlayer<Squashling>();
            if (player.PetPlayer().PickupChecks(item, squash.PetItemID, out PetGlobalItem itemChck))
            {
                if (itemChck.herbBoost == true)
                {
                    int count = PetUtils.Randomizer(PetIDs.HarvestingXpPerGathered.Find(x => x.plantList.Contains(item.type)).expAmount >= PetGlobalItem.MinimumExpForRarePlant ? squash.squashlingRareChance : squash.squashlingCommonChance) * item.stack;
                    squash.Pet.SpawnItemSourcingFromPet(EntitySourcePetIDs.HarvestingItem, item.type, count);
                }
            }
        }
    }
    public sealed class SquashlingPumpkinArmorChanges : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.PumpkinHelmet || entity.type == ItemID.PumpkinBreastplate || entity.type == ItemID.PumpkinLeggings;
        }
        public override void UpdateEquip(Item item, Player player)
        {
            Squashling squash = player.GetModPlayer<Squashling>();
            if (squash.PetIsEquipped(false))
            {
                player.statLifeMax2 += squash.pumpkinArmorBonusHp;
                squash.Pet.harvestingFortune += squash.pumpkinArmorBonusHarvestingFortune;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Squashling squash = Main.LocalPlayer.GetModPlayer<Squashling>();
            if (squash.PetIsEquipped(false))
            {
                int indx = tooltips.FindLastIndex(x => x.Name == "Defense"); //There is no safety net here for cases where Defense tooltip line doesn't exist, in that case these tooltips should just appear on top bc +1 & +2 on the index value
                tooltips.Insert(indx + 1, new(Mod, "PetTooltip0", PetUtils.LocVal("PetItemTooltips.SquashlingHealth").Replace("<hp>", squash.pumpkinArmorBonusHp.ToString())));
                tooltips.Insert(indx + 2, new(Mod, "PetTooltip1", PetUtils.LocVal("PetItemTooltips.SquashlingFortune").Replace("<fortune>", squash.pumpkinArmorBonusHarvestingFortune.ToString())));
            }
        }
    }
    public sealed class MagicalPumpkinSeed : PetTooltip
    {
        public override PetEffect PetsEffect => squashling;
        public static Squashling squashling
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out Squashling pet))
                    return pet;
                else
                    return ModContent.GetInstance<Squashling>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.MagicalPumpkinSeed")
                        .Replace("<plant>", squashling.squashlingCommonChance.ToString())
                        .Replace("<rarePlant>", squashling.squashlingRareChance.ToString())
                        .Replace("<health>", squashling.pumpkinArmorBonusHp.ToString())
                        .Replace("<harvFort>", squashling.pumpkinArmorBonusHarvestingFortune.ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.MagicalPumpkinSeed");
    }
}

