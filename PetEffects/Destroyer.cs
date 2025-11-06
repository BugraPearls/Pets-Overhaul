using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Destroyer : PetEffect
    {
        public override int PetItemID => ItemID.DestroyerPetItem;
        public override PetClasses PetClassSecondary => PetClasses.Defensive;
        public override PetClasses PetClassPrimary => PetClasses.Mining;
        public int ironskinBonusDef = 8;
        public float flatDefMult = 0.15f;
        public float defItemMult = 0.35f;
        public int flatAmount = 8;
        public int miningFort = 8;
        public static bool[] IronskinEffects = BuffID.Sets.Factory.CreateNamedSet("IronskinBuffs").Description("Buffs that triggers Destroyer's effects that triggers when Ironskin buff is active. Intended for crossmod compatibility.")
        .RegisterBoolSet(false, BuffID.Ironskin);
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                foreach (var effect in Player.buffType)
                {
                    if (IronskinEffects[effect])
                    {
                        Player.statDefense += ironskinBonusDef;
                        Pet.miningFortune += miningFort;
                        break;
                    }
                }
                Player.statDefense *= 1f + flatDefMult;
            }
        }
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public static void PreOnPickup(Item item, Player player)
        {
            PetModPlayer PickerPet = player.GetModPlayer<PetModPlayer>();
            Destroyer dest = player.GetModPlayer<Destroyer>();
            if (PickerPet.PickupChecks(item, dest.PetItemID, out PetGlobalItem itemChck) && itemChck.oreBoost)
            {
                for (int i = 0; i < PetUtils.Randomizer((player.statDefense * dest.defItemMult + dest.flatAmount) * item.stack); i++)
                {
                    player.QuickSpawnItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.MiningItem), item.type, 1);
                }
            }
        }
    }
    public sealed class DestroyerPetItem : PetTooltip
    {
        public override PetEffect PetsEffect => destroyer;
        public static Destroyer destroyer
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out Destroyer pet))
                    return pet;
                else
                    return ModContent.GetInstance<Destroyer>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.DestroyerPetItem")
                        .Replace("<defMultChance>", Math.Round(destroyer.defItemMult * 100, 2).ToString())
                        .Replace("<flatAmount>", destroyer.flatAmount.ToString())
                        .Replace("<defMultIncrease>", Math.Round(destroyer.flatDefMult * 100, 2).ToString())
                        .Replace("<ironskinDef>", destroyer.ironskinBonusDef.ToString())
                        .Replace("<miningFortune>", destroyer.miningFort.ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.DestroyerPetItem");
    }
}
