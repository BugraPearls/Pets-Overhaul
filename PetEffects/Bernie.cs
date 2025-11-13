using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Bernie : PetEffect
    {
        public override int PetItemID => ItemID.BerniePetItem;
        public int bernieRange = 1000;
        public int burnDrain = 60; //maxtimer
        public int maxBurning = 5;
        public int manaDrain = 4;
        public int healthDrain = 3;
        public int EnemiesBurning { get; internal set; }
        public override PetClass PetClassPrimary => PetClassID.Utility;
        public override PetClass PetClassSecondary => PetClassID.Defensive;
        public override int PetAbilityCooldown => burnDrain;
        public override int PetStackCurrent => EnemiesBurning;
        public override int PetStackMax => maxBurning;
        public override string PetStackText => PetUtils.LocVal("PetItemTooltips.BerniePetItemStack");
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                PetUtils.CircularDustEffect(Player.Center, DustID.Torch, bernieRange);
                Player.buffImmune[BuffID.Burning] = true;
                Player.buffImmune[BuffID.OnFire] = true;
                Player.buffImmune[BuffID.OnFire3] = true;
                Player.buffImmune[BuffID.Frostburn] = true;
                Player.buffImmune[BuffID.CursedInferno] = true;
                Player.buffImmune[BuffID.ShadowFlame] = true;
                Player.buffImmune[BuffID.Frostburn2] = true;
                EnemiesBurning = 0;
                foreach (var npc in Main.ActiveNPCs)
                {
                    if (Player.Distance(npc.Center) < bernieRange)
                    {
                        if (Main.rand.NextBool())
                        {
                            for (int a = 0; a < NPC.maxBuffs; a++)
                            {
                                if (PetIDs.BurnDebuffs[npc.buffType[a]])
                                {
                                    npc.buffTime[a]++;
                                }
                            }
                        }
                        for (int a = 0; a < NPC.maxBuffs; a++)
                        {
                            if (PetIDs.BurnDebuffs[npc.buffType[a]])
                            {
                                EnemiesBurning++;
                                break;
                            }
                            if (EnemiesBurning >= 5)
                            {
                                break;
                            }
                        }
                    }
                }
                if (Pet.timer <= 0 && EnemiesBurning > 0)
                {
                    if (EnemiesBurning > 5)
                    {
                        EnemiesBurning = 5;
                    }

                    Pet.PetRecovery(burnDrain * healthDrain * EnemiesBurning, 0.005f, isLifesteal: false);
                    Pet.PetRecovery(burnDrain * manaDrain * EnemiesBurning, 0.005f, isLifesteal: false, manaSteal: true);
                    Pet.timer = Pet.timerMax;
                }
            }
        }
    }
    public sealed class BerniePetItem : PetTooltip
    {
        public override PetEffect PetsEffect => bernie;
        public static Bernie bernie
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out Bernie pet))
                    return pet;
                else
                    return ModContent.GetInstance<Bernie>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.BerniePetItem")
                .Replace("<burnRange>", Math.Round(bernie.bernieRange / 16f, 2).ToString())
                .Replace("<burnDrainMana>", Math.Round(bernie.burnDrain * bernie.manaDrain * 0.005f, 2).ToString())
                .Replace("<burnDrainHealth>", Math.Round(bernie.burnDrain * bernie.healthDrain * 0.005f, 2).ToString())
                .Replace("<maxDrain>", bernie.maxBurning.ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.BerniePetItem");
    }
}
