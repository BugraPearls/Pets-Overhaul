using PetsOverhaul.Achievements;
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
        private int achievementTracker = 0; //This is here because for whatever reason I cannot tie my Achievement Conditions to my custom Tracker to show it as 'Minutes' on Achievements, now have to do it as seconds. This way with no 'SaveData', we may lose some frames for the achievement progression.
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
                        }
                    }
                }
                if (EnemiesBurning > 0)
                {
                    achievementTracker += EnemiesBurning;
                    if (achievementTracker >= 60)
                    {
                        int modulus = achievementTracker % 60;
                        ModContent.GetInstance<Arsonist>().BurnAmplifiedFrames.Value += (achievementTracker - modulus) / 60;
                        achievementTracker = modulus;
                    }
                }
                if (Pet.timer <= 0 && EnemiesBurning > 0)
                {
                    int count = EnemiesBurning;
                    if (count > 5)
                    {
                        count = 5;
                    }

                    Pet.PetRecovery(burnDrain * healthDrain * count, 0.005f, isLifesteal: false);
                    Pet.PetRecovery(burnDrain * manaDrain * count, 0.005f, isLifesteal: false, manaSteal: true);
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
