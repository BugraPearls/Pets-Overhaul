using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class MiniPrime : PetEffect
    {
        public override int PetItemID => ItemID.SkeletronPrimePetItem;
        public int shieldRecovery = 7500; //all 5 shields timer combined
        public float dmgIncrease = 0.07f;
        public int critIncrease = 7;
        public float defIncrease = 1.08f;
        public float shieldMult = 0.05f;
        public int shieldTime = 300;
        private int lastShield = 0;
        private int shieldIndex = 0;
        private int oldShieldCount = 0;
        public bool shieldedStatBoostActive = false;

        public int howManyShieldsAvailable = 0; //Purely added for UI display

        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Offensive;
        public override int PetAbilityCooldown => shieldRecovery;
        public override int PetStackCurrent => howManyShieldsAvailable;
        public override int PetStackMax => 5;
        public override string PetStackText => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.SkeletronPrimePetItemStack");
        public override void ExtraPreUpdate()
        {
            shieldedStatBoostActive = false;
        }
        private void AddShield() //This Pet gives me headaches.
        {
            if (oldShieldCount > shieldIndex && Pet.petShield[shieldIndex].shieldAmount < lastShield)
            {
                Pet.timer += Pet.timerMax / 5;
                (int shieldAmount, int shieldTimer) shield = Pet.petShield[shieldIndex];
                shield.shieldTimer = shieldTime;
                Pet.petShield[shieldIndex] = shield;
            }
            else
            {
                shieldIndex = Pet.petShield.Count - 1;
                Pet.petShield[shieldIndex] = ((int)Math.Round(Player.statLifeMax2 * shieldMult * Pet.petShieldMultiplier), 2);
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                if (Pet.petShield.Count <= 0)
                {
                    Pet.petShield.Add((0, 0));
                }
                if (Pet.timer <= Pet.timerMax * 0.8f && Player.statLife <= Player.statLifeMax2 * 0.25f)
                {
                    AddShield();
                }
                else if (Pet.timer <= Pet.timerMax * 0.6f && Player.statLife <= Player.statLifeMax2 * 0.5f)
                {
                    AddShield();
                }
                else if (Pet.timer <= Pet.timerMax * 0.4f && Player.statLife <= Player.statLifeMax2 * 0.75f)
                {
                    AddShield();
                }
                else if (Pet.timer <= Pet.timerMax * 0.2f && Player.statLife < Player.statLifeMax2)
                {
                    AddShield();
                }
                else if (Pet.timer <= 0 && Player.statLife <= Player.statLifeMax2)
                {
                    AddShield();
                }

                howManyShieldsAvailable = (int)((Pet.timerMax - Pet.timer) / (Pet.timerMax * 0.2f));

                if (oldShieldCount > shieldIndex)
                {
                    lastShield = Pet.petShield[shieldIndex].shieldAmount;
                }

                if (Pet.currentShield > 0)
                {
                    AddShieldedStatBoosts();
                }
                oldShieldCount = Pet.petShield.Count;
            }
        }
        public void AddShieldedStatBoosts() //This works with Calamity shields. (See in Calamity Addon code)
        {
            if (shieldedStatBoostActive == false)
            {
                Player.GetDamage<GenericDamageClass>() += dmgIncrease;
                Player.GetCritChance<GenericDamageClass>() += critIncrease;
                Player.statDefense *= defIncrease;
                shieldedStatBoostActive = true;
            }
        }
    }
    public sealed class SkeletronPrimePetItem : PetTooltip
    {
        public override PetEffect PetsEffect => miniPrime;
        public static MiniPrime miniPrime
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out MiniPrime pet))
                    return pet;
                else
                    return ModContent.GetInstance<MiniPrime>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.SkeletronPrimePetItem")
                        .Replace("<shieldMaxHealthAmount>", Math.Round(miniPrime.shieldMult * 100, 2).ToString())
                        .Replace("<shieldCooldown>", Math.Round(miniPrime.shieldRecovery / 300f, 2).ToString())
                        .Replace("<dmg>", Math.Round(miniPrime.dmgIncrease * 100, 2).ToString())
                        .Replace("<crit>", miniPrime.critIncrease.ToString())
                        .Replace("<def>", miniPrime.defIncrease.ToString())
                        .Replace("<shieldLifetime>", Math.Round(miniPrime.shieldTime / 60f, 2).ToString());
    }
}
