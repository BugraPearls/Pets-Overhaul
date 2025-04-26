using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Pigman : PetEffect
    {
        public override int PetItemID => ItemID.PigPetItem;
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public int foodChance = 15;
        public int potionChance = 10;
        public int shieldCooldown = 420;
        public int tier1Shield = 10;
        public int tier2Shield = 20;
        public int tier3Shield = 30;
        public int shieldTime = 1650;
        public override int PetAbilityCooldown => shieldCooldown;
    }
    public sealed class PigmanEat : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool ConsumeItem(Item item, Player player)
        {
            GlobalPet Pet = player.GetModPlayer<GlobalPet>();
            Pigman pig = player.GetModPlayer<Pigman>();
            if (pig.PetIsEquipped(false))
            {
                if (BuffID.Sets.IsWellFed[item.buffType])
                {
                    if (Pet.timer <= 0 && pig.PetIsEquipped())
                    {
                        int shieldAmount = 0;
                        if (item.buffType == BuffID.WellFed)
                        {
                            shieldAmount = pig.tier1Shield;
                        }
                        else if (item.buffType == BuffID.WellFed2)
                        {
                            shieldAmount = pig.tier2Shield;
                        }
                        else if (item.buffType == BuffID.WellFed3)
                        {
                            shieldAmount = pig.tier3Shield;
                        }
                        Pet.AddShield(shieldAmount, pig.shieldTime + item.buffTime / 60);
                        Pet.timer = Pet.timerMax;
                    }
                    if (Main.rand.NextBool(pig.foodChance, 100))
                    {
                        return false;
                    }
                }
                else if (item.buffType != 0 && Main.debuff[item.buffType] == false && Main.rand.NextBool(pig.potionChance, 100))
                {
                    return false;
                }
            }
            return base.ConsumeItem(item, player);
        }
    }
    public sealed class PigPetItem : PetTooltip
    {
        public override PetEffect PetsEffect => pigman;
        public static Pigman pigman
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out Pigman pet))
                    return pet;
                else
                    return ModContent.GetInstance<Pigman>();
            }
        }
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.PigPetItem")
                       .Replace("<foodChance>", pigman.foodChance.ToString())
                       .Replace("<potionChance>", pigman.potionChance.ToString())
                       .Replace("<shield1>", pigman.tier1Shield.ToString())
                       .Replace("<shield2>", pigman.tier2Shield.ToString())
                       .Replace("<shield3>", pigman.tier3Shield.ToString())
                       .Replace("<shieldTime>", Math.Round(pigman.shieldTime / 60f, 2).ToString())
                       .Replace("<cooldown>", Math.Round(pigman.shieldCooldown / 60f, 2).ToString());
    }
}
