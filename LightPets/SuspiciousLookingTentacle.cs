using PetsOverhaul.Systems;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class SuspiciousLookingTentacleEffect : LightPetEffect
    {
        public override int LightPetItemID => ItemID.SuspiciousLookingTentacle;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out SuspiciousLookingTentacle moonlord))
            {
                Player.statDefense += moonlord.Defense.CurrentStatInt;
                Player.moveSpeed += moonlord.MovementSpeed.CurrentStatFloat;
                Player.GetDamage<GenericDamageClass>() += moonlord.DamageAll.CurrentStatFloat;
                Player.GetCritChance<GenericDamageClass>() += moonlord.CritChanceAll.CurrentStatFloat * 100;
                Player.whipRangeMultiplier += moonlord.WhipRange.CurrentStatFloat;
                Player.statManaMax2 += moonlord.Mana.CurrentStatInt;
                Player.GetKnockback<MeleeDamageClass>() += moonlord.MeleeKnockback.CurrentStatFloat;
            }
        }
        public override void GetHealMana(Item item, bool quickHeal, ref int healValue)
        {
            if (TryGetLightPet(out SuspiciousLookingTentacle moonlord))
            {
                healValue += (int)(moonlord.ManaPotionIncrease.CurrentStatFloat * healValue);
            }
        }
        public override void ModifyItemScale(Item item, ref float scale)
        {
            if (TryGetLightPet(out SuspiciousLookingTentacle moonlord))
            {
                scale += moonlord.MeleeSize.CurrentStatFloat;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (TryGetLightPet(out SuspiciousLookingTentacle moonlord))
            {
                if (modifiers.DamageType == DamageClass.Ranged)
                {
                    modifiers.ScalingArmorPenetration += moonlord.RangedPercentPenetration.CurrentStatFloat;
                    modifiers.CritDamage += moonlord.RangedCritDamage.CurrentStatFloat;
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
        public LightPetStat Defense = new(5, 1, "MlDef");
        public LightPetStat MovementSpeed = new(20, 0.004f, "MlMs");
        public LightPetStat DamageAll = new(20, 0.0025f, "MlDmg");
        public LightPetStat CritChanceAll = new(20, 0.0025f, "MlCrit");
        public LightPetStat RangedPercentPenetration = new(5, 0.025f, "MlPen");
        public LightPetStat RangedCritDamage = new(5, 0.008f, "MlCrDmg");
        public LightPetStat SummonerFlatPenetration = new(5, 3, "MlMin");
        public LightPetStat WhipRange = new(5, 0.03f, "MlWhip");
        public LightPetStat ManaPotionIncrease = new(5, 0.05f, "MlPot");
        public LightPetStat Mana = new(5, 12, "MlMana");
        public LightPetStat MeleeSize = new(5, 0.04f, "MlSize");
        public LightPetStat MeleeKnockback = new(5, 0.12f,"MlHeal");
        public override int LightPetItemID => ItemID.SuspiciousLookingTentacle;
        public override string BaseTooltip => PetUtils.LocVal("LightPetTooltips.SuspiciousLookingTentacle");
    }
}
