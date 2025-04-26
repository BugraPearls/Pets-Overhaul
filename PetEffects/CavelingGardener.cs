using Microsoft.Xna.Framework;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class CavelingGardener : PetEffect
    {
        public override int PetItemID => ItemID.GlowTulip;
        public override PetClasses PetClassPrimary => PetClasses.Harvesting;
        public override PetClasses PetClassSecondary => PetClasses.Utility;
        public int cavelingRegularPlantChance = 30;
        public int cavelingGemTreeChance = 100;
        public int cavelingRarePlantChance = 15;
        public float shineMult = 0.5f;
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public static void PreOnPickup(Item item, Player player) //ALSO Directly increases ALL Hay gathered inside GlobalPet
        {
            GlobalPet PickerPet = player.GetModPlayer<GlobalPet>();
            CavelingGardener caveling = player.GetModPlayer<CavelingGardener>();
            if (PickerPet.PickupChecks(item, caveling.PetItemID, out ItemPet itemChck))
            {
                if (itemChck.herbBoost && (player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight || player.ZoneUnderworldHeight))
                {
                    for (int i = 0; i < GlobalPet.Randomizer((Junimo.HarvestingXpPerGathered.Find(x => x.plantList.Contains(item.type)).expAmount >= ItemPet.MinimumExpForRarePlant ? caveling.cavelingRarePlantChance : caveling.cavelingRegularPlantChance + (ItemPet.gemstoneTreeItem[item.type] ? caveling.cavelingGemTreeChance : 0)) * item.stack); i++)
                    {
                        player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.HarvestingItem), item.type, 1);
                    }
                }
            }
        }
        public override void UpdateEquips()
        {
            if (PetIsEquipped(false))
            {
                Lighting.AddLight(Player.Center, new Vector3(0.0013f * Main.mouseTextColor, 0.0064f * Main.mouseTextColor, 0.0115f * Main.mouseTextColor) * shineMult);
            }
        }
    }
    public sealed class GlowTulip : PetTooltip
    {
        public override PetEffect PetsEffect => cavelingGardener;
        public static CavelingGardener cavelingGardener
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out CavelingGardener pet))
                    return pet;
                else
                    return ModContent.GetInstance<CavelingGardener>();
            }
        }
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.GlowTulip")
                .Replace("<harvestChance>", cavelingGardener.cavelingRegularPlantChance.ToString())
                .Replace("<rarePlantChance>", cavelingGardener.cavelingRarePlantChance.ToString())
                .Replace("<gemstoneTreeChance>", cavelingGardener.cavelingGemTreeChance.ToString())
                .Replace("<shineMult>", cavelingGardener.shineMult.ToString());
    }
}

