using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class PlanteraSeedling : PetEffect
    {
        public override int PetItemID => ItemID.PlanteraPetItem;
        public float secondMultiplier = 0.1f;

        public override PetClasses PetClassPrimary => PetClasses.Ranged;
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && modifiers.DamageType.CountsAsClass<RangedDamageClass>())
            {
                float multiplyDamage = 2 / ((float)target.life / target.lifeMax + 1);
                multiplyDamage += (2 / ((float)target.life / target.lifeMax + 1) - 1) * secondMultiplier;
                modifiers.FinalDamage *= multiplyDamage;
            }
        }
    }
    public sealed class PlanteraPetItem : PetTooltip
    {
        public override PetEffect PetsEffect => planteraSeedling;
        public static PlanteraSeedling planteraSeedling
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out PlanteraSeedling pet))
                    return pet;
                else
                    return ModContent.GetInstance<PlanteraSeedling>();
            }
        }
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.PlanteraPetItem")
                        .Replace("<maxAmount>", Math.Round(planteraSeedling.secondMultiplier * 100 + 100, 2).ToString());
    }
}
