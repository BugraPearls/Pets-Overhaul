﻿using PetsOverhaul.Buffs;
using PetsOverhaul.Config;
using PetsOverhaul.NPCs;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class BabyWerewolf : PetEffect
    {
        public float critDmgReduction = 0.35f;
        public float critChance = 1.2f;
        public float damageMultPerStack = 0.02f;
        public float maulCritDmgIncrease = 0.006f;
        public int maxStacks = 15;
        public int debuffLength = 1200;

        public override PetClasses PetClassPrimary => PetClasses.Supportive;
        public override PetClasses PetClassSecondary => PetClasses.Offensive;
        public override void PostUpdateMiscEffects()
        {
            if (Pet.PetInUse(ItemID.FullMoonSqueakyToy) && Main.moonPhase == 0)
            {
                Player.wereWolf = true;
                Player.forceWerewolf = true;
                Player.npcTypeNoAggro[NPCID.Werewolf] = true;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.GetGlobalNPC<NpcPet>().maulCounter > maxStacks)
            {
                target.GetGlobalNPC<NpcPet>().maulCounter = maxStacks;
            }

            if (target.HasBuff(ModContent.BuffType<Mauled>()))
            {
                modifiers.FinalDamage *= target.GetGlobalNPC<NpcPet>().maulCounter * 0.02f + 1;
            }
            else
            {
                target.GetGlobalNPC<NpcPet>().maulCounter = 0;
            }

            if (Pet.PetInUseWithSwapCd(ItemID.FullMoonSqueakyToy))
            {
                modifiers.CritDamage -= critDmgReduction - target.GetGlobalNPC<NpcPet>().maulCounter * maulCritDmgIncrease;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.FullMoonSqueakyToy) && hit.Crit)
            {
                target.AddBuff(ModContent.BuffType<Mauled>(), debuffLength);
                target.GetGlobalNPC<NpcPet>().maulCounter++;
            }
        }
        public override void ModifyWeaponCrit(Item item, ref float crit)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.FullMoonSqueakyToy))
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
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.FullMoonSqueakyToy")
                .Replace("<critMult>", babyWerewolf.critChance.ToString())
                .Replace("<crDmgReduction>", Math.Round(babyWerewolf.critDmgReduction * 100, 2).ToString())
                .Replace("<maxStacks>", babyWerewolf.maxStacks.ToString())
                .Replace("<stackDmg>", Math.Round(babyWerewolf.damageMultPerStack * 100, 2).ToString())
                .Replace("<stackCritDmg>", Math.Round(babyWerewolf.maulCritDmgIncrease * 100, 2).ToString());
    }
}
