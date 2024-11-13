﻿using PetsOverhaul.Config;
using PetsOverhaul.NPCs;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class TinyDeerclops : PetEffect
    {
        public override int PetItemID => ItemID.DeerclopsPetItem;
        public List<(int storedDamage, int timer)> deerclopsTakenDamage = new();
        public int damageStoreTime = 300;
        public float healthTreshold = 0.4f;
        public int range = 520;
        public float slow = 0.4f;
        public int applyTime = 300;
        public int immuneTime = 180;
        public int cooldown = 1800;

        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override void OnHurt(Player.HurtInfo info)
        {
            if (PetIsEquipped(false))
            {
                deerclopsTakenDamage.Add((info.Damage, damageStoreTime));
            }
        }
        public override void PreUpdate()
        {
            if (PetIsEquipped(false))
            {
                Pet.SetPetAbilityTimer(cooldown);
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                if (deerclopsTakenDamage.Count > 0)
                {
                    for (int i = 0; i < deerclopsTakenDamage.Count; i++) //Pet will be reworked Post 3.0 update
                    {
                        (int storedDamage, int timer) value = deerclopsTakenDamage[i];
                        value.timer--;
                        deerclopsTakenDamage[i] = value;
                    }
                    deerclopsTakenDamage.RemoveAll(x => x.timer <= 0);
                    int totalDamage = 0;
                    deerclopsTakenDamage.ForEach(x => totalDamage += x.storedDamage);
                    if (totalDamage > Player.statLifeMax2 * healthTreshold && Pet.timer <= 0)
                    {
                        Pet.timer = Pet.timerMax;
                        if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                        {
                            SoundEngine.PlaySound(SoundID.DeerclopsScream with { PitchVariance = 0.4f, MaxInstances = 5 }, Player.Center);
                        }
                        GlobalPet.CircularDustEffect(Player.Center, DustID.MushroomTorch, range, 50);
                        foreach (var npc in Main.ActiveNPCs)
                        {
                            if (GlobalPet.LifestealCheck(npc) && Player.Distance(npc.Center) < range)
                            {
                                NpcPet.AddSlow(new NpcPet.PetSlow(slow, applyTime, PetSlowIDs.Deerclops), npc);
                                if (npc.boss == false || NpcPet.NonBossTrueBosses.Contains(npc.type) == false)
                                {
                                    npc.AddBuff(BuffID.Confused, applyTime);
                                }
                                    npc.AddBuff(BuffID.Frostburn, applyTime);
                            }
                        }
                        Player.SetImmuneTimeForAllTypes(immuneTime);
                    }
                }
            }
        }
    }
    public sealed class DeerclopsPetItem : PetTooltip
    {
        public override PetEffect PetsEffect => tinyDeerclops;
        public static TinyDeerclops tinyDeerclops
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out TinyDeerclops pet))
                    return pet;
                else
                    return ModContent.GetInstance<TinyDeerclops>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.DeerclopsPetItem")
                            .Replace("<treshold>", Math.Round(tinyDeerclops.healthTreshold * 100, 2).ToString())
                            .Replace("<tresholdTime>", Math.Round(tinyDeerclops.damageStoreTime / 60f, 2).ToString())
                            .Replace("<immunityTime>", Math.Round(tinyDeerclops.immuneTime / 60f, 2).ToString())
                            .Replace("<slowAmount>", Math.Round(tinyDeerclops.slow * 100, 2).ToString())
                            .Replace("<range>", Math.Round(tinyDeerclops.range / 16f, 2).ToString())
                            .Replace("<debuffTime>", Math.Round(tinyDeerclops.applyTime / 60f, 2).ToString())
                            .Replace("<cooldown>", Math.Round(tinyDeerclops.cooldown / 60f, 2).ToString());
    }
}