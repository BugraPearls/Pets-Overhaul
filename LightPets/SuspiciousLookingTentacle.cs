using PetsOverhaul.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.LightPets
{
    public sealed class SuspiciousLookingTentacleEffect : LightPetEffect
    {
        public override int LightPetItemID => ItemID.SuspiciousLookingTentacle;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out SuspiciousLookingTentacle moonlord))
            {
                Pet.petSlowPotency += moonlord.PetSlow;
                Pet.petDirectDamageMultiplier += moonlord.PetDamage;
                Pet.petShieldMultiplier += moonlord.PetShield;
                Pet.petHealMultiplier += moonlord.PetHeal;
                Player.statManaMax2 += moonlord.Mana.CurrentStatInt;
            }
        }
        public override void ModifyItemScale(Item item, ref float scale)
        {
            if (item.CountsAsClass<MeleeDamageClass>() && TryGetLightPet(out SuspiciousLookingTentacle moonlord))
            {
                scale *= 1 + moonlord.MeleeSize;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (TryGetLightPet(out SuspiciousLookingTentacle moonlord))
            {
                if (modifiers.DamageType == DamageClass.Ranged)
                {
                    modifiers.CritDamage += moonlord.RangedCritDamage;
                }
                if (modifiers.DamageType == DamageClass.Summon)
                {
                    modifiers.ArmorPenetration += moonlord.SummonerFlatPenetration.CurrentStatInt;
                }
            }
        }
    }
    public sealed class SuspiciousLookingTentacle : LightPetItem
    {
        public LightPetStat Haste = new(5, 0.035f, "Haste");
        public LightPetStat PetSlow = new(5, 0.035f, "Slow", LegacyKeysToInherit: ("MlMs", 20));
        public LightPetStat PetDamage = new(5, 0.035f, "Damage", LegacyKeysToInherit: ("MlDmg", 20));
        public LightPetStat PetShield = new(5, 0.035f, "Shield", LegacyKeysToInherit: ("MlDef", 5));
        public LightPetStat PetHeal = new(5, 0.035f, "Heal", LegacyKeysToInherit: ("MlCrit", 20));
        public LightPetStat RangedCritDamage = new(5, 0.01f, "Crit", LegacyKeysToInherit: [("MlCrDmg", 5), ("MlPen", 5)]);
        public LightPetStat SummonerFlatPenetration = new(5, 4, "Pen", LegacyKeysToInherit: [("MlMin", 5), ("MlWhip", 5)]);
        public LightPetStat Mana = new(5, 15, "Mana", LegacyKeysToInherit: [("MlPot", 5), ("MlMana", 5)]);
        public LightPetStat MeleeSize = new(5, 0.05f, "Size", LegacyKeysToInherit: [("MlSize", 5), ("MlHeal", 5)]);
        public override int LightPetItemID => ItemID.SuspiciousLookingTentacle;
        public override string BaseTooltip => PetUtils.LocVal("LightPetTooltips.SuspiciousLookingTentacle");
    }
}
