﻿using PetsOverhaul.Config;
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
    public sealed class VoltBunny : PetEffect
    {
        public override int PetItemID => ItemID.LightningCarrot;
        public float movespdFlat = 0.1f;
        public float movespdMult = 1.05f;
        public float movespdToDmg = 0.2f;
        public float staticParalysis = 3f;
        public int staticLength = 45;
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override PetClasses PetClassSecondary => PetClasses.Mobility;
        public override void PostUpdateMiscEffects()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.LightningCarrot))
            {
                Player.moveSpeed += movespdFlat;
                Player.moveSpeed *= movespdMult;
                if (Player.moveSpeed > 1f)
                {
                    Player.GetDamage<GenericDamageClass>() += (Player.moveSpeed - 1f) * movespdToDmg;
                }
            }
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.LightningCarrot) && info.DamageSource.TryGetCausingEntity(out Entity entity) && entity is NPC npc)
            {
                NpcPet.AddSlow(new NpcPet.PetSlow(staticParalysis, staticLength, PetSlowIDs.VoltBunny), npc);
            }
        }
    }
    public sealed class LightningCarrot : PetTooltip
    {
        public override PetEffect PetsEffect => voltBunny;
        public static VoltBunny voltBunny
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out VoltBunny pet))
                    return pet;
                else
                    return ModContent.GetInstance<VoltBunny>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.LightningCarrot")
                       .Replace("<flatSpd>", Math.Round(voltBunny.movespdFlat * 100, 2).ToString())
                       .Replace("<multSpd>", voltBunny.movespdMult.ToString())
                       .Replace("<spdToDmg>", Math.Round(voltBunny.movespdToDmg * 100, 2).ToString())
                       .Replace("<staticAmount>", Math.Round(voltBunny.staticParalysis * 100, 2).ToString())
                       .Replace("<staticTime>", Math.Round(voltBunny.staticLength / 60f, 2).ToString());
    }
}
