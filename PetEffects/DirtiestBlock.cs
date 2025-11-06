using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace PetsOverhaul.PetEffects
{
    public sealed class DirtiestBlock : PetEffect
    {
        public override int PetItemID => ItemID.DirtiestBlock;
        public override PetClasses PetClassPrimary => PetClasses.Mining;
        public int dirtCoin = 850;
        public int soilCoin = 600;
        public int everythingCoin = 300;
        public static List<int> CommonBlock = [ItemID.SiltBlock, ItemID.SlushBlock, ItemID.DesertFossil, ItemID.MudBlock, ItemID.AshBlock, ItemID.ClayBlock, ItemID.Marble, ItemID.Granite, ItemID.EbonstoneBlock, ItemID.CrimstoneBlock, ItemID.PearlstoneBlock, ItemID.SandBlock, ItemID.EbonsandBlock, ItemID.CrimsandBlock, ItemID.PearlsandBlock, ItemID.Sandstone, ItemID.CorruptSandstone, ItemID.CrimsonSandstone, ItemID.HallowSandstone, ItemID.HardenedSand, ItemID.CorruptHardenedSand, ItemID.CrimsonHardenedSand, ItemID.HallowHardenedSand, ItemID.SnowBlock, ItemID.IceBlock, ItemID.PurpleIceBlock, ItemID.RedIceBlock, ItemID.PinkIceBlock, ItemID.StoneBlock];
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public static void PreOnPickup(Item item, Player player)
        {
            GlobalPet PickerPet = player.GetModPlayer<GlobalPet>();
            DirtiestBlock dirt = player.GetModPlayer<DirtiestBlock>();
            if (PickerPet.PickupChecks(item, dirt.PetItemID, out PetGlobalItem itemChck) && itemChck.blockNotByPlayer == true)
            {
                if (item.type == ItemID.DirtBlock)
                {
                    PickerPet.GiveCoins(GlobalPet.Randomizer(item.stack * dirt.dirtCoin));
                }
                else if (CommonBlock.Contains(item.type))
                {
                    PickerPet.GiveCoins(GlobalPet.Randomizer(item.stack * dirt.soilCoin));
                }
                else
                {
                    PickerPet.GiveCoins(GlobalPet.Randomizer(item.stack * dirt.everythingCoin));
                }
            }

        }
    }
    public sealed class DirtiestBlockItem : PetTooltip
    {
        public override PetEffect PetsEffect => dirtiestBlock;
        public static DirtiestBlock dirtiestBlock
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out DirtiestBlock pet))
                    return pet;
                else
                    return ModContent.GetInstance<DirtiestBlock>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.DirtiestBlock")
                        .Replace("<any>", Math.Round(dirtiestBlock.everythingCoin / 100f, 2).ToString())
                        .Replace("<soil>", Math.Round(dirtiestBlock.soilCoin / 100f, 2).ToString())
                        .Replace("<soilItems>", PetUtils.ItemsToTooltipImages(DirtiestBlock.CommonBlock, 12))
                        .Replace("<dirt>", Math.Round(dirtiestBlock.dirtCoin / 100f, 2).ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.DirtiestBlock");
    }
}
