using PetsOverhaul.Systems;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class ToyGolemEffect : LightPetEffect
    {
        public override int LightPetItemID => ItemID.GolemPetItem;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out ToyGolem toyGolem))
            {
                Player.statDefense += toyGolem.Defense.CurrentStatInt;
                Player.statLifeMax2 += (int)(Player.statLifeMax2 * toyGolem.PercentHealth.CurrentStatFloat);
                Pet.knockbackResistance += toyGolem.KnockbackResist;
                Pet.petShieldMultiplier += toyGolem.Shield;
            }
        }
    }
    public sealed class ToyGolem : LightPetItem
    {
        public LightPetStat Defense = new(5, 5, "Defense", 30, LegacyKeysToInherit: ("GolemRegen", 4));
        public LightPetStat PercentHealth = new(35, 0.0018f, "Health", 0.022f, LegacyKeysToInherit: ("GolemHealth",35));
        public LightPetStat KnockbackResist = new(20, 1, "KbResist", LegacyKeysToInherit: ("GolemExp", 20));
        public LightPetStat Shield = new(10, 4, "Shield", 2);
        public override int LightPetItemID => ItemID.GolemPetItem;
        public override string BaseTooltip => PetUtils.LocVal("LightPetTooltips.ToyGolem");
    }
}
