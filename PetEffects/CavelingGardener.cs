using Microsoft.Xna.Framework;
using PetsOverhaul.Systems;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class CavelingGardener : PetEffect
    {
        public override int PetItemID => ItemID.GlowTulip;
        public override PetClass PetClassPrimary => PetClassID.Harvesting;
        public override PetClass PetClassSecondary => PetClassID.Utility;
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
            CavelingGardener caveling = player.GetModPlayer<CavelingGardener>();
            if (player.PetPlayer().PickupChecks(item, caveling.PetItemID, out PetGlobalItem itemChck))
            {
                if (itemChck.herbBoost && (player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight || player.ZoneUnderworldHeight))
                {
                    int count = PetUtils.Randomizer((PetIDs.HarvestingXpPerGathered.Find(x => x.plantList.Contains(item.type)).expAmount >= PetGlobalItem.MinimumExpForRarePlant ?
                        caveling.cavelingRarePlantChance : caveling.cavelingRegularPlantChance + (PetIDs.gemstoneTreeItem[item.type] ? caveling.cavelingGemTreeChance : 0)) * item.stack);
                    caveling.Pet.SpawnItemSourcingFromPet(EntitySourcePetIDs.HarvestingItem, item.type, count);
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
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.GlowTulip")
                .Replace("<harvestChance>", cavelingGardener.cavelingRegularPlantChance.ToString())
                .Replace("<rarePlantChance>", cavelingGardener.cavelingRarePlantChance.ToString())
                .Replace("<gemstoneTreeChance>", cavelingGardener.cavelingGemTreeChance.ToString())
                .Replace("<shineMult>", cavelingGardener.shineMult.ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.GlowTulip");
    }
}

