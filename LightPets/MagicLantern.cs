using PetsOverhaul.Systems;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class MagicLanternEffect : LightPetEffect
    {
        public override int LightPetItemID => ItemID.MagicLantern;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out MagicLantern magicLantern))
            {
                Pet.knockbackResistance += magicLantern.KnockbackResistance;
                Pet.petShieldMultiplier += magicLantern.PetShielding;
                Player.endurance += magicLantern.DamageReduction;
                Pet.miningFortune += magicLantern.MiningFortune;
            }
        }
    }
    public sealed class MagicLantern : LightPetItem
    {
        public LightPetStat KnockbackResistance = new(5, 0.04f, "KbResist", 0.4f, LegacyKeysToInherit: ("LanternMult",20));
        public LightPetStat PetShielding = new(6, 0.013f,"Shield",0.052f, LegacyKeysToInherit: ("LanternDef", 3));
        public LightPetStat DamageReduction = new(25, 0.002f, "DamageReduction", 0.01f, LegacyKeysToInherit: ("LanternExp",15));
        public LightPetStat MiningFortune = new(15, 1, "Fortune", 5, LegacyKeysToInherit: ("LanternFort", 15));
        public override int LightPetItemID => ItemID.MagicLantern;
        public override string BaseTooltip => PetUtils.LocVal("LightPetTooltips.MagicLantern");
    }
}
