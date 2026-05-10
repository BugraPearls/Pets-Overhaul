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
                Pet.knockbackResistance += creeperEgg.KnockbackResistance;
            }
        }
        public override void ModifyItemScale(Item item, ref float scale)
        {
            if (TryGetLightPet(out CreeperEgg creeperEgg))
            {
                scale += creeperEgg.WhipMeleeSize;
            }
        }
    }
    public sealed class CreeperEgg : LightPetItem
    {
        public LightPetStat MeleeSummonDamage = new(20, 0.004f, "Damage", 0.04f, LegacyKeysToInherit: [("FlickerwickMelee", 16), ("FlickerwickSum", 16)]);
        public LightPetStat WhipMeleeSize = new(15, 0.004f, "Size", 0.04f, LegacyKeysToInherit: ("FlickerwickAtkSpd", 20));
        public LightPetStat KnockbackResistance = new(10, 0.004f, "KbResist", 0.025f);
        public override int LightPetItemID => ItemID.DD2PetGhost;
        public override string BaseTooltip => PetUtils.LocVal("LightPetTooltips.CreeperEgg");
    }
}
