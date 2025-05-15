using PetsOverhaul.Buffs;
using PetsOverhaul.Projectiles;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Turtle : PetEffect
    {
        public override int PetItemID => ItemID.Seaweed;
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public float moveSpd = 0.12f;
        public float def = 0.07f;
        public float kbResist = 0.55f;

        private int timer = 0;
        internal int currentStacks = 0;
        public int shellHardenDuration = 900;
        public int shellHardenStacks = 6;
        public int shellHardenCd = 2100;
        public float dmgReduceShellHarden = 0.07f;
        public float dmgReflect = 1.1f;
        public float dmgReflectProjectile = 0.6f;
        public override int PetAbilityCooldown => shellHardenCd;
        public override int PetStackCurrent => currentStacks;
        public override int PetStackMax => shellHardenStacks;
        public override string PetStackText => PetTextsColors.LocVal("PetItemTooltips.SeaweedStack");
        public override void ExtraPreUpdate()
        {
            timer--;
            if (timer <= 0)
            {
                currentStacks = 0;
                timer = 0;
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.statDefense *= def + 1f;
                Player.moveSpeed -= moveSpd;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (PetIsEquipped())
            {
                if (currentStacks > 0)
                {
                    modifiers.Knockback *= 0;
                    modifiers.FinalDamage *= 1f - dmgReduceShellHarden;
                    return;
                }
                modifiers.Knockback *= 1f - kbResist;
            }
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            if (PetIsEquipped() && currentStacks > 0)
            {
                if (info.DamageSource.TryGetCausingEntity(out Entity entity))
                {
                    int damageTaken = Math.Min(info.SourceDamage, Player.statLife);
                    if (entity is Projectile projectile && projectile.TryGetGlobalProjectile(out ProjectileSourceChecks proj) && Main.npc[proj.sourceNpcId].active && Main.npc[proj.sourceNpcId].dontTakeDamage == false)
                    {
                        Main.npc[proj.sourceNpcId].SimpleStrikeNPC(Main.DamageVar(Pet.PetDamage(damageTaken * dmgReflectProjectile, DamageClass.Generic), Player.luck), info.HitDirection, Main.rand.NextBool((int)Math.Min(Player.GetTotalCritChance<GenericDamageClass>(), 100), 100), 1f, DamageClass.Generic);
                    }
                    else if (entity is NPC npc && npc.active == true && npc.dontTakeDamage == false)
                    {
                        npc.SimpleStrikeNPC(Main.DamageVar(Pet.PetDamage(damageTaken * dmgReflect, DamageClass.Generic), Player.luck), info.HitDirection, Main.rand.NextBool((int)Math.Min(Player.GetTotalCritChance<GenericDamageClass>(), 100), 100), 1f, DamageClass.Generic);
                    }
                }
                currentStacks--;
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && PetIsEquipped())
            {
                timer = shellHardenDuration;
                currentStacks = shellHardenStacks;
                Pet.timer = Pet.timerMax;
                Player.AddBuff(ModContent.BuffType<HardenedShell>(), shellHardenDuration);
            }
        }
    }
    public sealed class Seaweed : PetTooltip
    {
        public override PetEffect PetsEffect => turtle;
        public static Turtle turtle
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out Turtle turtle))
                    return turtle;
                else
                    return ModContent.GetInstance<Turtle>();
            }
        }
        public override string PetsTooltip =>
            PetTextsColors.LocVal("PetItemTooltips.Seaweed")
                .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility))
                .Replace("<hitCount>", turtle.shellHardenStacks.ToString())
                .Replace("<shellDuration>", Math.Round(turtle.shellHardenDuration / 60f, 2).ToString())
                .Replace("<reducedDmg>", Math.Round(turtle.dmgReduceShellHarden * 100, 2).ToString())
                .Replace("<reflect>", Math.Round(turtle.dmgReflect * 100, 2).ToString())
                .Replace("<projReflect>", Math.Round(turtle.dmgReflectProjectile * 100, 2).ToString())
                .Replace("<def>", Math.Round(turtle.def * 100, 2).ToString())
                .Replace("<kbResist>", Math.Round(turtle.kbResist * 100, 2).ToString())
                .Replace("<moveSpd>", Math.Round(turtle.moveSpd * 100, 2).ToString());
    }
}
