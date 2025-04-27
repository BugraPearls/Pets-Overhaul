using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class EverscreamSapling : PetEffect
    {
        public override int PetItemID => ItemID.EverscreamPetItem;
        public int cooldown = 240;
        public float critMult = 0.6f;
        public float dmgIncr = 0.3f;
        public float howMuchCrit = 10f;
        public float missingManaPercent = 0.16f;
        public int flatRecovery = 5;
        public int manaIncrease = 100;

        public override PetClasses PetClassPrimary => PetClasses.Magic;
        public override int PetAbilityCooldown => cooldown;
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.statManaMax2 += manaIncrease;
                Player.GetCritChance<MagicDamageClass>() *= critMult;
                float dmgBoost = (float)Player.statMana / Player.statManaMax2;
                Player.GetDamage<MagicDamageClass>() += dmgBoost * dmgIncr;
                Player.GetCritChance<MagicDamageClass>() += dmgBoost * howMuchCrit;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && GlobalPet.LifestealCheck(target) && hit.Crit && Pet.timer <= 0)
            {
                Pet.PetRecovery(Player.statManaMax2 - Player.statMana, missingManaPercent, flatRecovery, true, false);
                Pet.timer = Pet.timerMax;
            }
        }
    }
    public sealed class EverscreamPetItem : PetTooltip
    {
        public override PetEffect PetsEffect => everscreamSapling;
        public static EverscreamSapling everscreamSapling
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out EverscreamSapling pet))
                    return pet;
                else
                    return ModContent.GetInstance<EverscreamSapling>();
            }
        }
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.EverscreamPetItem")
                        .Replace("<magicCritNerf>", everscreamSapling.critMult.ToString())
                        .Replace("<maxMana>", everscreamSapling.manaIncrease.ToString())
                        .Replace("<missingMana>", Math.Round(everscreamSapling.missingManaPercent * 100, 2).ToString())
                        .Replace("<flatMana>", everscreamSapling.flatRecovery.ToString())
                        .Replace("<manaRecoveryCd>", (everscreamSapling.cooldown / 60f).ToString())
                        .Replace("<dmg>", Math.Round(everscreamSapling.dmgIncr * 100, 2).ToString())
                        .Replace("<crit>", everscreamSapling.howMuchCrit.ToString());
    }
}
