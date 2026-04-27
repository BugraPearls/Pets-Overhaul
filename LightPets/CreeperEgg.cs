using PetsOverhaul.Systems;
using System.IO;
using Terraria;
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
                Player.GetDamage<SummonDamageClass>() += creeperEgg.SummonDamage.CurrentStatFloat;
                Player.GetDamage<MeleeDamageClass>() += creeperEgg.MeleeDamage.CurrentStatFloat;
                Player.GetAttackSpeed<MeleeDamageClass>() += creeperEgg.AttackSpeed.CurrentStatFloat;
            }
        }
    }
    public sealed class CreeperEgg : LightPetItem
    {
        public LightPetStat SummonDamage = new(16, 0.004f, "FlickerwickSum", 0.04f);
        public LightPetStat MeleeDamage = new(16, 0.004f, "FlickerwickMelee", 0.04f);
        public LightPetStat AttackSpeed = new(20, 0.004f, "FlickerwickAtkSpd", 0.025f);
        public override int LightPetItemID => ItemID.DD2PetGhost;
        public override string BaseTooltip => PetUtils.LocVal("LightPetTooltips.CreeperEgg");
    }
}
