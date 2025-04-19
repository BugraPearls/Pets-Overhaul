using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class BabyFaceMonster : PetEffect
    {
        public override int PetItemID => ItemID.BoneRattle;
        public float bonusRegenPerFrame = 0.2f; //This will work every frame, so * 60 is added every second to Life Regen Timer, which increases how fast you get your natural life regen up.
        public int stage2time = 720;
        public int stage1time = 420;
        public int stage1regen = 4;
        public int stage2regen = 15;
        public float stage2ShieldMult = 0.05f;
        public int currentRegen = 0; //Added for UI

        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override void PreUpdateBuffs() //Since inCombatTimerMax is reset in ResetEffects(), we set the desired inCombatTimerMax here.
        {
            if (PetIsEquipped(false))
            {
                Pet.inCombatTimerMax = (int)(stage2time * (1 / (1 + Pet.abilityHaste)));
            }
        }
        public override void NaturalLifeRegen(ref float regen)
        {
            if (PetIsEquipped())
            {
                if (Pet.inCombatTimer <= Pet.inCombatTimerMax - (int)(stage1time * (1 / (1 + Pet.abilityHaste))) && Player.crimsonRegen == false && Player.active)
                {
                    for (int i = 0; i < 0.05f * (Player.lifeRegen + regen); i++)
                    {
                        int num9 = Dust.NewDust(Player.position, Player.width, Player.height, 5, 0f, 0f, 175, default, 1.75f);
                        Main.dust[num9].noGravity = true;
                        Main.dust[num9].velocity *= 0.75f;
                        int num10 = Main.rand.Next(-40, 41);
                        int num11 = Main.rand.Next(-40, 41);
                        Main.dust[num9].position.X += num10;
                        Main.dust[num9].position.Y += num11;
                        Main.dust[num9].velocity.X = (-num10) * 0.075f;
                        Main.dust[num9].velocity.Y = (-num11) * 0.075f;
                    }
                }
            }
        }
        public override void UpdateLifeRegen()
        {
            if (PetIsEquipped())
            {
                Player.lifeRegenTime += bonusRegenPerFrame;
                if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                {
                    if (Pet.inCombatTimer == 1)
                    {
                        SoundEngine.PlaySound(SoundID.Zombie21 with { Pitch = -0.7f, PitchVariance = 0.3f, Volume = 0.75f }, Player.Center);
                    }
                    if (Pet.inCombatTimer == Pet.inCombatTimerMax - (int)(stage1time * (1 / (1 + Pet.abilityHaste))) + 1)
                    {
                        SoundEngine.PlaySound(new SoundStyle(SoundID.DD2_DrakinShot.SoundPath + "0") with { Pitch = -0.7f, PitchVariance = 0.3f, Volume = 0.75f }, Player.Center);
                    }
                }
                if (Pet.inCombatTimer <= 0)
                {
                    Pet.AddShield((int)(Player.statLifeMax2 * stage2ShieldMult), 1);
                    Player.lifeRegen += GlobalPet.Randomizer((int)(stage2regen * 100 * Pet.petHealMultiplier));
                }
                else if (Pet.inCombatTimer <= Pet.inCombatTimerMax - (int)(stage1time * (1 / (1 + Pet.abilityHaste))))
                {
                    Player.lifeRegen += GlobalPet.Randomizer((int)(stage1regen * 100 * Pet.petHealMultiplier));
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) //Works after GlobalPet's ModifyHitNPC()'s inCombatTimer = inCombatTimerMax to override it if needed.
        {
            if (PetIsEquipped(false) && Pet.inCombatTimer > Pet.inCombatTimerMax - (int)(stage1time * (1 / (1 + Pet.abilityHaste))))
            {
                Pet.inCombatTimer = Pet.inCombatTimerMax - (int)(stage1time * (1 / (1 + Pet.abilityHaste)));
            }
        }
    }
    public sealed class BoneRattle : PetTooltip
    {
        public override PetEffect PetsEffect => babyFaceMonster;
        public static BabyFaceMonster babyFaceMonster
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out BabyFaceMonster pet))
                    return pet;
                else
                    return ModContent.GetInstance<BabyFaceMonster>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.BoneRattle")
                .Replace("<extraRegenTime>", babyFaceMonster.bonusRegenPerFrame.ToString())
                .Replace("<stage1Time>", Math.Round((babyFaceMonster.stage2time - babyFaceMonster.stage1time) / 60f, 2).ToString())
                .Replace("<stage2Time>", Math.Round(babyFaceMonster.stage2time / 60f, 2).ToString())
                .Replace("<stage1Regen>", babyFaceMonster.stage1regen.ToString())
                .Replace("<stage2Regen>", babyFaceMonster.stage2regen.ToString())
                .Replace("<shieldAmount>", Math.Round(babyFaceMonster.stage2ShieldMult * 100, 2).ToString());
    }
}
