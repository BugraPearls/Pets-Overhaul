using PetsOverhaul.Projectiles;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class PropellerGato : PetEffect
    {
        public override int PetItemID => ItemID.DD2PetGato;
        public int bonusCritChance = 15;
        public int turretIncrease = 1;

        public override PetClasses PetClassPrimary => PetClasses.Summoner;
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.maxTurrets++;
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && proj.GetGlobalProjectile<ProjectileSourceChecks>().isFromSentry)
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
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.DD2PetGato")
                        .Replace("<crit>", propellerGato.bonusCritChance.ToString())
                        .Replace("<maxSentry>", propellerGato.turretIncrease.ToString());
    }
}
