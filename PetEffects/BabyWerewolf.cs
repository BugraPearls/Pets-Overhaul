using PetsOverhaul.Buffs;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class BabyWerewolf : PetEffect
    {
        public override int PetItemID => ItemID.FullMoonSqueakyToy;
        public float critDmgReduction = 0.35f;
        public float critChance = 1.25f;
        public float damageMultPerStack = 0.02f;
        public float maulCritDmgIncrease = 0.006f;
        public int maxStacks = 15;
        public int debuffLength = 1200;

        public override PetClass PetClassPrimary => PetClassID.Supportive;
        public override PetClass PetClassSecondary => PetClassID.Offensive;
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped(false) && Main.moonPhase == 0)
            {
                Player.wereWolf = true;
                Player.forceWerewolf = true;
                Player.npcTypeNoAggro[NPCID.Werewolf] = true;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.GetGlobalNPC<PetGlobalNPC>().maulCounter > maxStacks)
            {
                target.GetGlobalNPC<PetGlobalNPC>().maulCounter = maxStacks;
            }

            if (target.HasBuff(ModContent.BuffType<Mauled>()))
            {
                modifiers.FinalDamage *= target.GetGlobalNPC<PetGlobalNPC>().maulCounter * 0.02f + 1;
            }
            else
            {
                target.GetGlobalNPC<PetGlobalNPC>().maulCounter = 0;
            }

            if (PetIsEquipped())
            {
                modifiers.CritDamage -= critDmgReduction - target.GetGlobalNPC<PetGlobalNPC>().maulCounter * maulCritDmgIncrease;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && hit.Crit)
            {
                target.AddBuff(ModContent.BuffType<Mauled>(), debuffLength);
                target.GetGlobalNPC<PetGlobalNPC>().maulCounter++;
            }
        }
        public override void ModifyWeaponCrit(Item item, ref float crit)
        {
            if (PetIsEquipped())
            {
                crit *= critChance;
            }
        }
    }
    public sealed class FullMoonSqueakyToy : PetTooltip
    {
        public override PetEffect PetsEffect => babyWerewolf;
        public static BabyWerewolf babyWerewolf
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out BabyWerewolf pet))
                    return pet;
                else
                    return ModContent.GetInstance<BabyWerewolf>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.FullMoonSqueakyToy")
                .Replace("<critMult>", babyWerewolf.critChance.ToString())
                .Replace("<crDmgReduction>", Math.Round(babyWerewolf.critDmgReduction * 100, 2).ToString())
                .Replace("<maxStacks>", babyWerewolf.maxStacks.ToString())
                .Replace("<stackDmg>", Math.Round(babyWerewolf.damageMultPerStack * 100, 2).ToString())
                .Replace("<stackCritDmg>", Math.Round(babyWerewolf.maulCritDmgIncrease * 100, 2).ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.FullMoonSqueakyToy");
    }
}
