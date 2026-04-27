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
                Player.lifeRegen += toyGolem.HealthRegen.CurrentStatInt;
                Player.manaRegenBonus += toyGolem.ManaRegen.CurrentStatInt;
                Player.statLifeMax2 += (int)(Player.statLifeMax2 * toyGolem.PercentHealth.CurrentStatFloat);
            }
        }
    }
    public sealed class ToyGolem : LightPetItem
    {
        public LightPetStat HealthRegen = new(4, 1, "GolemRegen");
        public LightPetStat PercentHealth = new(35, 0.0018f, "GolemHealth", 0.022f);
        public LightPetStat ManaRegen = new(20, 5, "GolemExp", 30);
        public override int LightPetItemID => ItemID.GolemPetItem;
        public override string BaseTooltip => PetUtils.LocVal("LightPetTooltips.ToyGolem");
    }
}
