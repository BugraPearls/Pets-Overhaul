using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class BabyDinosaur : PetEffect
    {
        public override int PetItemID => ItemID.AmberMosquito;
        public int chance = 175; // 17.5% because its with 1000
        public override PetClasses PetClassPrimary => PetClasses.Mining;
        public static void AddItemsToPool()
        {
            PetModPlayer.ItemWeight(ItemID.TinOre, 10);
            PetModPlayer.ItemWeight(ItemID.CopperOre, 10);
            PetModPlayer.ItemWeight(ItemID.Amethyst, 9);
            PetModPlayer.ItemWeight(ItemID.IronOre, 9);
            PetModPlayer.ItemWeight(ItemID.LeadOre, 9);
            PetModPlayer.ItemWeight(ItemID.Topaz, 8);
            PetModPlayer.ItemWeight(ItemID.Sapphire, 8);
            PetModPlayer.ItemWeight(ItemID.SilverOre, 8);
            PetModPlayer.ItemWeight(ItemID.TungstenOre, 8);
            PetModPlayer.ItemWeight(ItemID.GoldOre, 7);
            PetModPlayer.ItemWeight(ItemID.PlatinumOre, 7);
            PetModPlayer.ItemWeight(ItemID.Emerald, 7);
            PetModPlayer.ItemWeight(ItemID.Ruby, 7);
            PetModPlayer.ItemWeight(ItemID.Diamond, 6);
            PetModPlayer.ItemWeight(ItemID.Amber, 6);
        }
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public static void PreOnPickup(Item item, Player player)
        {
            BabyDinosaur dino = player.GetModPlayer<BabyDinosaur>();
            if (player.PetPlayer().PickupChecks(item, dino.PetItemID, out PetGlobalItem itemChck) && itemChck.oreBoost)
            {
                AddItemsToPool();
                if (PetModPlayer.ItemPool.Count > 0)
                {
                    for (int i = 0; i < PetUtils.Randomizer(dino.chance * item.stack, 1000); i++)
                    {
                        player.QuickSpawnItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.MiningItem), PetModPlayer.ItemPool[Main.rand.Next(PetModPlayer.ItemPool.Count)], 1);
                    }
                }
            }
        }
    }
    public sealed class AmberMosquito : PetTooltip
    {
        public override PetEffect PetsEffect => babyDinosaur;
        public static BabyDinosaur babyDinosaur
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out BabyDinosaur pet))
                    return pet;
                else
                    return ModContent.GetInstance<BabyDinosaur>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.AmberMosquito")
                .Replace("<oreChance>", Math.Round(babyDinosaur.chance / 10f, 2).ToString());

        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.AmberMosquito");

    }
}
