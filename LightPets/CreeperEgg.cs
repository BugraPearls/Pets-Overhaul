using PetsOverhaul.Systems;
using System.IO;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

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
        public LightPetStat MeleeSummonDamage = new(20, 0.004f, "Damage", 0.04f, LegacyKeysToInherit: [("FlickerwickMelee", 16), ("FlickerwickSum", 16)]);
        public LightPetStat WhipMeleeSize = new(15, 0.004f, "Size", 0.04f, LegacyKeysToInherit: ("FlickerwickAtkSpd", 20));
        public LightPetStat KnockbackIncrease = new(10, 0.004f, "KbIncrease", 0.025f);
        public LightPetStat SlowPotency = new(5, 2f, "Slow", 0.2f);
        public override int LightPetItemID => ItemID.DD2PetGhost;
        public override string BaseTooltip => PetUtils.LocVal("LightPetTooltips.CreeperEgg");
    }
}
