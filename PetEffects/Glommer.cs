﻿using PetsOverhaul.Buffs;
using PetsOverhaul.Config;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Glommer : PetEffect
    {
        public int glommerSanityTime = 60;
        public int glommerSanityRecover = 2;
        public float glommerSanityAura = 0.3f;
        public int glommerSanityRange = 1040;

        public override PetClasses PetClassPrimary => PetClasses.Magic;
        public override PetClasses PetClassSecondary => PetClasses.Supportive;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.GlommerPetItem))
            {
                Pet.SetPetAbilityTimer(glommerSanityTime);
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Pet.PetInUse(ItemID.GlommerPetItem) && Main.rand.NextBool(18000))
            {
                Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.GlobalItem), ModContent.ItemType<GlommersGoop>());
            }

            if (Pet.PetInUseWithSwapCd(ItemID.GlommerPetItem))
            {
                GlobalPet.CircularDustEffect(Player.Center, DustID.ShimmerTorch, glommerSanityRange, 100);
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player plr = Main.player[i];
                    if (Player.Distance(plr.Center) < glommerSanityRange && plr.active && plr.whoAmI != 255)
                    {
                        plr.GetModPlayer<GlobalPet>().abilityHaste += glommerSanityAura;
                        plr.AddBuff(ModContent.BuffType<SanityAura>(), 1);
                    }
                }
                Player.statManaMax2 += (int)(Pet.abilityHaste * Player.statManaMax2);
                if (Pet.timer <= 0 && Player.statMana != Player.statManaMax2)
                {
                    Player.statMana += glommerSanityRecover;
                    Pet.timer = Pet.timerMax;
                }
            }
        }
    }
    public sealed class GlommerPetItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.GlommerPetItem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            Glommer glommer = Main.LocalPlayer.GetModPlayer<Glommer>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.GlommerPetItem")
                .Replace("<class>", PetTextsColors.ClassText(glommer.PetClassPrimary, glommer.PetClassSecondary))
                        .Replace("<sanityRange>", Math.Round(glommer.glommerSanityRange / 16f, 2).ToString())
                        .Replace("<sanityAmount>", Math.Round(glommer.glommerSanityAura * 100, 2).ToString())
                        .Replace("<currentHaste>", Math.Round(glommer.Pet.abilityHaste * 100, 2).ToString())
                        .Replace("<manaRecover>", glommer.glommerSanityRecover.ToString())
                        .Replace("<manaRecoverCd>", Math.Round(glommer.glommerSanityTime / 60f, 2).ToString())
                        .Replace("<goop>", ModContent.ItemType<GlommersGoop>().ToString())
                        ));
        }
    }
}
