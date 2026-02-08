using Microsoft.Xna.Framework;
using PetsOverhaul.NPCs;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Lizard : PetEffect
    {
        public override int PetItemID => ItemID.LizardEgg;
        public float percentHpDmg = 0.1f;
        public int buffDurations = 120;
        private int buffTimer = 0;
        public float tailAcc = 1f;
        public float tailSpd = 0.5f;
        public int tailAggro = 10000;
        public int tailWait = 300;
        public float percentHpRecover = 0.1f;
        public float tailCdRefund = 0.5f;
        public float tailMaxHp = 0.5f; //Uses Player's maximum health.
        public int tailCooldown = 3600;

        public float kbResist = 0.6f;
        public float moveSpd = 0.7f;
        public int defense = 10;
        public float jumpMult = 0.5f;

        public override PetClass PetClassPrimary => PetClassID.Defensive;
        public override PetClass PetClassSecondary => PetClassID.Utility;
        public override int PetAbilityCooldown => tailCooldown;
        public override void ExtraPreUpdateNoCheck()
        {
            if (Pet.skinColorChanged == false)
            {
                Pet.skin = Player.skinColor;
                Pet.skinColorChanged = true;
            }
            if (PetIsEquipped(false) == false || PetIsEquipped(false) && Player.statLife > Player.statLifeMax2 * 0.55f)
            {
                Player.skinColor = Pet.skin;
                Pet.skinColorChanged = false;
            }
            if (PetIsEquipped(false))
            {
                buffTimer--;
                if (buffTimer <= 0)
                    buffTimer = 0;
                if (Player.statLife < Player.statLifeMax2 * 0.55f)
                {
                    Player.skinColor = Color.YellowGreen;
                }
            }
        }
        public override void ExtraProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && PetIsEquipped())
            {
                Decoy();
                BasicSyncMessage(MessageType.Lizard);
            }
        }
        public void Decoy()
        {
            int dmg = (int)(Player.statLifeMax2 * percentHpDmg);
            if (Player.statLife < dmg)
            {
                Player.statLife = dmg + 1;
            }
            Player.Hurt(new Player.HurtInfo() with { Damage = dmg, Dodgeable = false, Knockback = 0, DamageSource = PlayerDeathReason.ByCustomReason("If you're seeing this death message, report it through our discord or steam page.") });
            NPC.NewNPC(PetUtils.GetSource_Pet(EntitySourcePetIDs.PetNPC), (int)Player.Center.X, (int)Player.Center.Y, ModContent.NPCType<LizardTail>(), ai0: Player.statLifeMax2 * tailMaxHp, ai1: (int)(tailWait * (1 / (1 + Pet.abilityHaste))), ai2: Pet.timerMax / 2, ai3: 1); //ai[3] is only passed here from LizardTail, which will make the 'arrow' appear.
            Pet.timer = Pet.timerMax;
            buffTimer = buffDurations;
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                if (buffTimer > 0)
                {
                    Player.moveSpeed += tailSpd;
                    Player.SetImmuneTimeForAllTypes(1);
                    Player.aggro -= tailAggro;
                }
                if (Player.statLife < Player.statLifeMax2 * 0.55f)
                {
                    Player.moveSpeed += moveSpd;
                    Player.statDefense += defense;
                    Player.noKnockback = true;
                    Player.wingTimeMax = (int)(jumpMult * Player.wingTimeMax);
                    Player.jumpHeight = (int)(jumpMult * Player.jumpHeight);
                }
            }
        }
        public override void PostUpdateRunSpeeds()
        {
            if (PetIsEquipped() && buffTimer > 0)
            {
                Player.runAcceleration *= tailAcc + 1f;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (PetIsEquipped())
            {
                modifiers.Knockback *= 1f - kbResist;
            }
        }
    }
    public sealed class LizardEgg : PetTooltip
    {
        public override PetEffect PetsEffect => lizard;
        public static Lizard lizard
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out Lizard pet))
                    return pet;
                else
                    return ModContent.GetInstance<Lizard>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.LizardEgg")
                    .Replace("<keybind>", PetUtils.KeybindText(PetKeybinds.UsePetAbility))
                    .Replace("<tailDmgTaken>", Math.Round(lizard.percentHpDmg * 100, 2).ToString())
                    .Replace("<tailAcc>", Math.Round(lizard.tailAcc * 100, 2).ToString())
                    .Replace("<tailSpd>", Math.Round(lizard.tailSpd * 100, 2).ToString())
                    .Replace("<tailAggro>", lizard.tailAggro.ToString())
                    .Replace("<buffDuration>", Math.Round(lizard.buffDurations / 60f, 2).ToString())
                    .Replace("<tailWait>", Math.Round(lizard.tailWait / 60f, 2).ToString())
                    .Replace("<tailRecover>", Math.Round(lizard.percentHpRecover * 100, 2).ToString())
                    .Replace("<cdRefund>", Math.Round(lizard.tailCdRefund * 100, 2).ToString())
                    .Replace("<tailMaxHp>", Math.Round(lizard.tailMaxHp * 100, 2).ToString())
                    .Replace("<tailCooldown>", Math.Round(lizard.tailCooldown / 60f, 2).ToString())
                    .Replace("<kbResist>", Math.Round(lizard.kbResist * 100, 2).ToString())
                    .Replace("<def>", lizard.defense.ToString())
                    .Replace("<moveSpd>", Math.Round(lizard.moveSpd * 100, 2).ToString())
                    .Replace("<jumpPenalty>", Math.Round(lizard.jumpMult * 100, 2).ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.LizardEgg").Replace("<keybind>", PetUtils.KeybindText(PetKeybinds.UsePetAbility));
    }
}
