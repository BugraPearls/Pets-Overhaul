using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.PetEffects
{
    public sealed class PropellerGato : PetEffect
    {
        public override int PetItemID => ItemID.DD2PetGato;
        public int bonusCritChance = 15;
        public int turretIncrease = 1;

        public override PetClass PetClassPrimary => PetClassID.Summoner;
        public override bool CustomEffectIsContributor => true;
        public override bool HasCustomEffect => true; //Dedicated to Furqi
        public override PetClass CustomPrimaryClass => PetClassID.Mobility;
        public int wingTime = 90;
        public override void SaveData(TagCompound tag)
        {
            tag.Add("CustomActive", CustomEffectActive);
        }
        public override void LoadData(TagCompound tag)
        {
            if (tag.TryGet("CustomActive", out bool custom))
            {
                CustomEffectActive = custom;
            }
        }
        public override void PostUpdateMiscEffects()
        {
            PetUtils.OldOnesAchievementHelper(Player);

            if (PetIsEquipped())
            {
                Player.maxTurrets++;
            }
            else if (PetIsEquippedForCustom())
            {
                Player.wingTimeMax += wingTime;
                Player.statDefense += 2;
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && proj.GetGlobalProjectile<PetGlobalProjectile>().isFromSentry)
            {
                int playersCrit = (int)Player.GetTotalCritChance<GenericDamageClass>();
                if (playersCrit + bonusCritChance >= 100)
                {
                    modifiers.SetCrit();
                }
                else if (Main.rand.NextBool(playersCrit + bonusCritChance, 100))
                {
                    modifiers.SetCrit();
                }
                else
                {
                    modifiers.DisableCrit();
                }
            }
        }

    }
    public sealed class DD2PetGato : PetTooltip
    {
        public override PetEffect PetsEffect => propellerGato;
        public static PropellerGato propellerGato
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out PropellerGato pet))
                    return pet;
                else
                    return ModContent.GetInstance<PropellerGato>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.DD2PetGato")
                        .Replace("<crit>", propellerGato.bonusCritChance.ToString())
                        .Replace("<maxSentry>", propellerGato.turretIncrease.ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.DD2PetGato");
        public override string CustomTooltip => PetUtils.LocVal("CustomPetEffects.DD2PetGato")
            .Replace("<wingTime>", Math.Round(propellerGato.wingTime / 60f, 2).ToString());
        public override string CustomSimpleTooltip => PetUtils.LocVal("SimpleCustomPetEffects.DD2PetGato");
    }
}
