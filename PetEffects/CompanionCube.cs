using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class CompanionCube : PetEffect
    {
        public override int PetItemID => ItemID.CompanionCube;
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Magic;
        public float manaToHealth = 0.11f;
        /// <summary>
        /// This is base amount for mana to health recovery before the Potion Sickness reductions.
        /// </summary>
        public float manaToHealthUpdate = 0.11f;
        public float healthToMana = 0.25f;
        public float manaPotionNerf = 0.17f;
        public float manaToHealthNerf = 0.03f;
        public override void ExtraPreUpdate()
        {
            manaToHealth = manaToHealthUpdate;
        }
        public override void PostUpdateBuffs()
        {
            if (PetIsEquipped() && Player.manaSick)
            {
                if (Player.manaSickReduction > Player.manaSickLessDmg)
                {
                    manaToHealth -= manaToHealthNerf * 2;
                }
                else
                {
                    manaToHealth -= manaToHealthNerf;
                }
            }
        }
        public override void GetHealMana(Item item, bool quickHeal, ref int healValue)
        {
            if (PetIsEquipped() && Player.manaSick)
            {
                if (Player.manaSickReduction > Player.manaSickLessDmg)
                {
                    healValue -= (int)(healValue * manaPotionNerf * 2);
                }
                else
                {
                    healValue -= (int)(healValue * manaPotionNerf);
                }
            }
        }
        public override void OnConsumeMana(Item item, int manaConsumed)
        {
            if (PetIsEquipped())
            {
                Pet.PetRecovery(manaConsumed, manaToHealth, isLifesteal: false);
            }
        }
        public override void PostHurt(Player.HurtInfo info)
        {
            if (PetIsEquipped())
            {
                Pet.PetRecovery(info.Damage, healthToMana, manaSteal: true, isLifesteal: false);
            }
        }
    }
    public sealed class CompanionCubeItem : PetTooltip
    {
        public override PetEffect PetsEffect => companionCube;
        public static CompanionCube companionCube
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out CompanionCube pet))
                    return pet;
                else
                    return ModContent.GetInstance<CompanionCube>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.CompanionCube")
                        .Replace("<manaToHealth>", Math.Round(companionCube.manaToHealth * 100, 2).ToString())
                        .Replace("<healthToMana>", Math.Round(companionCube.healthToMana * 100, 2).ToString())
                        .Replace("<manaPotionNerf>", Math.Round(companionCube.manaPotionNerf * 100, 2).ToString())
                        .Replace("<manaToHealthNerf>", Math.Round(companionCube.manaToHealthNerf * 100, 2).ToString())
                        .Replace("<halfOfSickness>", Math.Round(Player.manaSickLessDmg * 100, 2).ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.CompanionCube");
    }
}
