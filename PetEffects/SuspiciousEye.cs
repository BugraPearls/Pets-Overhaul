using Microsoft.Xna.Framework;
using PetsOverhaul.Buffs;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class SuspiciousEye : PetEffect
    {
        public override int PetItemID => ItemID.EyeOfCthulhuPetItem;
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override PetClasses PetClassSecondary => PetClasses.Utility;
        public int phaseCd = 9000;
        public int phaseTime = 1800;
        public int eocTimer = 0;
        public float critMult = 0.18f;
        public float dmgMult = 0.9f;
        public float spdMult = 0.5f;
        public int ragePoints = 0;
        public int shieldTime = 600;
        public float shieldMult = 0.5f;
        public float eocShieldMult = 1f;
        public bool eocShieldEquipped = false;
        public int dashFrameReduce = 10;
        public float forcedEnrageShield = 0.1f;
        public override int PetAbilityCooldown => phaseCd;
        public override int PetStackCurrent => ragePoints;
        public override int PetStackMax => 0;
        public override string PetStackText => PetUtils.LocVal("PetItemTooltips.EyeOfCthulhuPetItemStack");
        public override void ExtraPreUpdate()
        {
            if (eocTimer >= -1)
            {
                eocTimer--;
            }
        }
        public override void ExtraProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && PetIsEquipped())
            {
                if (Player.statLife > Player.statLifeMax2 / 2)
                {
                    int damageTaken = (int)Math.Floor((float)Player.statLife % (Player.statLifeMax2 / 2));
                    if (damageTaken == 0)
                        damageTaken = Player.statLifeMax2 / 2;
                    Player.Hurt(new Player.HurtInfo() with { Damage = damageTaken, Dodgeable = false, Knockback = 0, DamageSource = PlayerDeathReason.ByCustomReason("If you're seeing this death message, report it through our discord or steam page.") });
                    Pet.AddShield((int)(damageTaken * forcedEnrageShield), shieldTime);
                }
                else
                    Pet.inCombatTimer = Pet.inCombatTimerMax;
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                if (Pet.inCombatTimer >= 0 && Player.statLife <= Player.statLifeMax2 / 2 && Pet.timer <= 0)
                {
                    eocTimer = phaseTime;
                    Pet.timer = Pet.timerMax;
                    if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                    {
                        SoundEngine.PlaySound(SoundID.ForceRoar with { PitchVariance = 0.3f }, Player.Center);
                    }

                    Gore.NewGore(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetMisc), Player.Center, Main.rand.NextVector2Circular(2f, 2f), 8, 0.5f);
                    Gore.NewGore(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetMisc), Player.Center, Main.rand.NextVector2Circular(2f, 2f), 8, 0.5f);
                    Gore.NewGore(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetMisc), Player.Center, Main.rand.NextVector2Circular(2f, 2f), 9, 0.5f);
                    Gore.NewGore(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetMisc), Player.Center, Main.rand.NextVector2Circular(2f, 2f), 9, 0.5f);
                    PopupText.NewText(new AdvancedPopupRequest() with
                    {
                        Text = PetUtils.LocVal("PetItemTooltips.EocEnrage"),
                        DurationInFrames = 150,
                        Velocity = new Vector2(0, -10),
                        Color = Color.DarkRed
                    }, Player.Center);

                    Pet.AddShield((int)((eocShieldEquipped ? eocShieldMult : shieldMult) * (Player.statDefense + Math.Round(Player.endurance * 100))), shieldTime);
                    Player.AddBuff(ModContent.BuffType<EocPetEnrage>(), phaseTime);
                }
                if (eocTimer <= phaseTime && eocTimer >= 0)
                {
                    if (Player.statLife > Player.statLifeMax2 / 2)
                    {
                        Player.statLife = Player.statLifeMax2 / 2;
                    }
                    ragePoints = 0;
                    ragePoints += Player.statDefense;
                    ragePoints += (int)Math.Round(Player.endurance * 100);
                    Player.statDefense *= 0;
                    Player.endurance *= 0;
                    Player.GetDamage<GenericDamageClass>() += ragePoints * dmgMult / 100;
                    Player.moveSpeed += ragePoints * spdMult / 100;
                    Player.GetCritChance<GenericDamageClass>() += ragePoints * critMult;
                    if (eocShieldEquipped && Player.dashType == DashID.ShieldOfCthulhu && Player.dashDelay > dashFrameReduce)
                    {
                        Player.dashDelay = dashFrameReduce;
                    }
                }
                else if (eocTimer == 0)
                {
                    PopupText.NewText(new AdvancedPopupRequest() with
                    {
                        Text = PetUtils.LocVal("PetItemTooltips.EocCalm"),
                        DurationInFrames = 150,
                        Velocity = new Vector2(0, -10),
                        Color = Color.OrangeRed
                    }, Player.Center);
                    ragePoints = 0;
                }
                eocShieldEquipped = false;
            }
        }
        public override void UpdateDead()
        {
            eocTimer = 0;
            ragePoints = 0;
        }
    }
    public class ShieldOfCthulhuCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.EoCShield;
        }
        public override void UpdateEquip(Item item, Player player)
        {
            player.GetModPlayer<SuspiciousEye>().eocShieldEquipped = true;
        }
    }
    public sealed class EyeOfCthulhuPetItem : PetTooltip
    {
        public override PetEffect PetsEffect => suspiciousEye;
        public static SuspiciousEye suspiciousEye
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out SuspiciousEye pet))
                    return pet;
                else
                    return ModContent.GetInstance<SuspiciousEye>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.EyeOfCthulhuPetItem")
                .Replace("<keybind>", PetUtils.KeybindText(PetKeybinds.UsePetAbility))
                .Replace("<forcedEnrageMult>", Math.Round(suspiciousEye.forcedEnrageShield * 100, 2).ToString())
                .Replace("<shieldDuration>", Math.Round(suspiciousEye.shieldTime / 60f, 2).ToString())
                .Replace("<frameReduction>", suspiciousEye.dashFrameReduce.ToString())
                .Replace("<shieldMult>", suspiciousEye.shieldMult.ToString())
                .Replace("<eocShieldMult>", suspiciousEye.eocShieldMult.ToString())
                .Replace("<outOfCombat>", Math.Round(suspiciousEye.Pet.inCombatTimerMax / 60f, 2).ToString())
                .Replace("<defToDmg>", Math.Round(suspiciousEye.dmgMult * 100, 2).ToString())
                .Replace("<defToSpd>", Math.Round(suspiciousEye.spdMult * 100, 2).ToString())
                .Replace("<defToCrit>", Math.Round(suspiciousEye.critMult * 100, 2).ToString())
                .Replace("<enrageLength>", Math.Round(suspiciousEye.phaseTime / 60f, 2).ToString())
                .Replace("<enrageCd>", Math.Round(suspiciousEye.phaseCd / 60f, 2).ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.EyeOfCthulhuPetItem").Replace("<keybind>", PetUtils.KeybindText(PetKeybinds.UsePetAbility));
    }
}
