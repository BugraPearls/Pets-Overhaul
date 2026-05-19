using PetsOverhaul.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.LightPets
{
    public sealed class CreeperEggEffect : LightPetEffect
    {
        public override int LightPetItemID => ItemID.DD2PetGhost;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out CreeperEgg creeperEgg))
            {
                PetUtils.OldOnesAchievementHelper(Player);

                Player.GetDamage<SummonDamageClass>() += creeperEgg.MeleeSummonDamage;
                Player.GetDamage<MeleeDamageClass>() += creeperEgg.MeleeSummonDamage;
                Player.whipRangeMultiplier += creeperEgg.WhipMeleeSize;
                Player.GetKnockback<GenericDamageClass>() += creeperEgg.KnockbackIncrease;

                Pet.petSlowPotency += creeperEgg.SlowPotency;
            }
        }
        public override void ModifyItemScale(Item item, ref float scale)
        {
            if (item.CountsAsClass<MeleeDamageClass>() && TryGetLightPet(out CreeperEgg creeperEgg))
            {
                scale *= 1 + creeperEgg.WhipMeleeSize;
            }
        }
    }
    public sealed class CreeperEgg : LightPetItem
    {
        public LightPetStat MeleeSummonDamage = new(20, 0.0025f, "Damage", 0.02f, LegacyKeysToInherit: [("FlickerwickMelee", 16), ("FlickerwickSum", 16)]);
        public LightPetStat WhipMeleeSize = new(15, 0.006f, "Size", 0.04f, LegacyKeysToInherit: ("FlickerwickAtkSpd", 20));
        public LightPetStat KnockbackIncrease = new(10, 0.02f, "KbIncrease", 0.1f);
        public LightPetStat SlowPotency = new(5, 0.045f, "Slow", 0.105f);
        public override int LightPetItemID => ItemID.DD2PetGhost;
        public override string BaseTooltip => PetUtils.LocVal("LightPetTooltips.CreeperEgg");
    }
}
