using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace PetsOverhaul.PetEffects
{
    public sealed class BabyDinosaur : PetEffect
    {
        public override int PetItemID => ItemID.AmberMosquito;
        public int chance = 175; // 17.5% because its with 1000
        public override PetClasses PetClassPrimary => PetClasses.Mining;
        public static int RandomizeItemDrop()
        {
            WeightedRandom<int> itemsToDrop = new();

            itemsToDrop.Add(ItemID.TinOre, 10);
            itemsToDrop.Add(ItemID.CopperOre, 10);
            itemsToDrop.Add(ItemID.Amethyst, 9);
            itemsToDrop.Add(ItemID.IronOre, 9);
            itemsToDrop.Add(ItemID.LeadOre, 9);
            itemsToDrop.Add(ItemID.Topaz, 8);
            itemsToDrop.Add(ItemID.Sapphire, 8);
            itemsToDrop.Add(ItemID.SilverOre, 8);
            itemsToDrop.Add(ItemID.TungstenOre, 8);
            itemsToDrop.Add(ItemID.GoldOre, 7);
            itemsToDrop.Add(ItemID.PlatinumOre, 7);
            itemsToDrop.Add(ItemID.Emerald, 7);
            itemsToDrop.Add(ItemID.Ruby, 7);
            itemsToDrop.Add(ItemID.Diamond, 6);
            itemsToDrop.Add(ItemID.Amber, 6);

            int result = itemsToDrop;
            return result;
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
                for (int i = 0; i < PetUtils.Randomizer(dino.chance * item.stack, 1000); i++)
                {
                    player.QuickSpawnItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.MiningItem), RandomizeItemDrop(), 1);
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
}
