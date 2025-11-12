using PetsOverhaul.Config;
using ReLogic.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Systems
{
    /// <summary>
    /// ModPlayer Class that includes Dictionaries and Methods used by Pets Overhaul's sound effects.
    /// </summary>
    public static class PetSounds
    {
        public static Dictionary<int, SoundStyle[]> PetItemIdToHurtSound = new() {
            {
                ItemID.Seaweed,
                    new SoundStyle[] {
                        SoundID.NPCHit24 with {
                            PitchVariance = 0.4f
                        }
                    }
            }, {
                ItemID.LunaticCultistPetItem,
                new SoundStyle[] {
                    SoundID.NPCHit55 with {
                        PitchVariance = 0.6f
                    }
                }
            }, {
                ItemID.LizardEgg,
                new SoundStyle[] {
                    SoundID.NPCHit26 with {
                        PitchVariance = 0.6f
                    }
                }
            }, {
                ItemID.BoneKey,
                new SoundStyle[] {
                    SoundID.NPCHit2 with {
                        PitchVariance = 0.3f, Pitch = 0.1f
                    }
                }
            }, {
                ItemID.SkeletronPetItem,
                new SoundStyle[] {
                    SoundID.NPCHit2 with {
                        PitchVariance = 0.3f, Pitch = -0.4f
                    }
                }
            }, {
                ItemID.ToySled,
                new SoundStyle[] {
                    SoundID.NPCHit11 with {
                        Pitch = -0.5f, PitchVariance = 0.2f
                    }
                }
            }, {
                ItemID.FullMoonSqueakyToy,
                new SoundStyle[] {
                    SoundID.NPCHit6 with {
                        PitchVariance = 0.4f
                    }
                }
            }, {
                ItemID.CursedSapling,
                new SoundStyle[] {
                    SoundID.NPCHit7 with {
                        PitchVariance = 0.4f
                    }
                }
            }, {
                ItemID.CelestialWand,
                new SoundStyle[] {
                    SoundID.NPCHit5 with {
                        PitchVariance = 0.2f, Pitch = 0.5f
                    }
                }
            }, {
                ItemID.UnluckyYarn,
                new SoundStyle[] {
                    SoundID.Meowmere with {
                        PitchVariance = 0.4f, Pitch = 0.6f
                    }
                }
            }, {
                ItemID.CompanionCube,
                new SoundStyle[] {
                    SoundID.NPCHit55 with {
                        Pitch = -0.3f, PitchVariance = 0.5f
                    }
                }
            }, {
                ItemID.ParrotCracker,
                new SoundStyle[] {
                    SoundID.NPCHit46 with {
                        PitchVariance = 0.5f
                    }
                }
            }, {
                ItemID.GlommerPetItem,
                new SoundStyle[] {
                    SoundID.NPCHit35 with {
                        PitchVariance = 0.2f, Pitch = -0.5f
                    }
                }
            }, {
                ItemID.SpiderEgg,
                new SoundStyle[] {
                    SoundID.NPCHit29 with {
                        PitchVariance = 0.3f
                    }
                }
            }, {
                ItemID.OrnateShadowKey,
                new SoundStyle[] {
                    SoundID.NPCHit4 with {
                        PitchVariance = 0.5f, Pitch = 0.3f
                    }
                }
            }, {
                ItemID.DestroyerPetItem,
                new SoundStyle[] {
                    SoundID.NPCHit4 with {
                        PitchVariance = 0.6f
                    }
                }
            }, {
                ItemID.SkeletronPrimePetItem,
                new SoundStyle[] {
                    SoundID.NPCHit4 with {
                        PitchVariance = 0.6f
                    }
                }
            }, {
                ItemID.TwinsPetItem,
                new SoundStyle[] {
                    SoundID.NPCHit4 with {
                        PitchVariance = 0.6f
                    }
                }
            }, {
                ItemID.PigPetItem,
                new SoundStyle[] {
                    SoundID.Zombie39 with {
                        PitchVariance = 0.3f
                    }
                }
            }, {
                ItemID.LightningCarrot,
                new SoundStyle[] {
                    SoundID.NPCHit34 with {
                        PitchVariance = 0.5f
                    }
                }
            }, {
                ItemID.BrainOfCthulhuPetItem,
                new SoundStyle[] {
                    SoundID.NPCHit9 with {
                        Pitch = 0.1f, PitchVariance = 0.4f
                    }
                }
            }, {
                ItemID.DD2OgrePetItem,
                new SoundStyle[] {
                    SoundID.DD2_OgreHurt with {
                        PitchVariance = 0.7f, Volume = 0.7f
                    }
                }
            }, {
                ItemID.MartianPetItem,
                new SoundStyle[] {
                    SoundID.NPCHit39 with {
                        Pitch = 0.2f, PitchVariance = 0.5f
                    }
                }
            }, {
                ItemID.DD2BetsyPetItem,
                new SoundStyle[] {
                    SoundID.DD2_BetsyHurt with {
                        Pitch = 0.3f, PitchVariance = 0.5f
                    }
                }
            }, {
                ItemID.DukeFishronPetItem,
                new SoundStyle[] {
                    SoundID.Zombie39 with {
                        PitchVariance = 0.8f
                    }
                }
            }, {
                ItemID.ChesterPetItem,
                new SoundStyle[] {
                    SoundID.ChesterOpen with {
                        PitchVariance = 0.4f, Pitch = -0.6f
                    }, SoundID.ChesterClose with {
                        PitchVariance = 0.4f, Pitch = -0.6f
                    }
                }
            }, {
                ItemID.MoonLordPetItem,
                new SoundStyle[] {
                    SoundID.Zombie100 with {
                        PitchVariance = 0.5f, Volume = 0.5f
                    }, SoundID.Zombie101 with {
                        PitchVariance = 0.5f, Volume = 0.5f
                    }, SoundID.Zombie102 with {
                        PitchVariance = 0.5f, Volume = 0.5f
                    }
                }
            }, {
                ItemID.BerniePetItem,
                    new SoundStyle[] {
                        SoundID.DSTMaleHurt with {
                            PitchVariance = 0.6f
                        }, SoundID.DSTFemaleHurt with {
                            PitchVariance = 0.6f
                        }
                    }
                }
        };
        public static Dictionary<int, SoundStyle[]> PetItemIdToAmbientSound = new() {
            {
                ItemID.LizardEgg,
                    new SoundStyle[] {
                        SoundID.Zombie37 with {
                                PitchVariance = 0.5f, Volume = 0.5f
                            },
                            SoundID.Zombie36 with {
                                PitchVariance = 0.5f, Volume = 0.5f
                            }
                    }
            }, {
                ItemID.ParrotCracker,
                new SoundStyle[] {
                    SoundID.Cockatiel with {
                            PitchVariance = 0.5f, Volume = 0.8f
                        },
                        SoundID.Macaw with {
                            PitchVariance = 0.5f, Volume = 0.8f
                        }
                }
            }, {
                ItemID.BoneRattle,
                new SoundStyle[] {
                    SoundID.Zombie8 with {
                        PitchVariance = 0.5f, Pitch = 0.5f, Volume = 0.5f
                    }
                }
            }, {
                ItemID.DukeFishronPetItem,
                new SoundStyle[] {
                    SoundID.Zombie20 with {
                        PitchVariance = 0.5f, Pitch = 0.5f, Volume = 0.5f
                    }
                }
            }, {
                ItemID.MartianPetItem,
                new SoundStyle[] {
                    SoundID.Zombie59 with {
                            PitchVariance = 0.5f, Pitch = 0.5f, Volume = 0.5f
                        },
                        SoundID.Zombie60 with {
                            PitchVariance = 0.5f, Pitch = 0.5f, Volume = 0.5f
                        }
                }
            }, {
                ItemID.QueenBeePetItem,
                new SoundStyle[] {
                    SoundID.Zombie50 with {
                            PitchVariance = 0.2f, Pitch = 0.9f, Volume = 0.5f
                        },
                        SoundID.Zombie51 with {
                            PitchVariance = 0.2f, Pitch = 0.9f, Volume = 0.5f
                        },
                        SoundID.Zombie52 with {
                            PitchVariance = 0.2f, Pitch = 0.9f, Volume = 0.5f
                        }
                }
            }
        };
        public static Dictionary<int, SoundStyle> PetItemidToKillSound = new()
        {
            {
                ItemID.Seaweed,
                SoundID.NPCDeath27 with { PitchVariance = 0.4f}
            }, {
                ItemID.LunaticCultistPetItem,
                SoundID.NPCDeath59 with { Pitch = -0.2f, PitchVariance = 0.6f }
            }, {
                ItemID.SpiderEgg,
                SoundID.NPCDeath47 with { PitchVariance = 0.6f }
            }, {
                ItemID.LightningCarrot,
                SoundID.Item94 with { PitchVariance = 0.5f }
            }, {
                ItemID.LizardEgg,
                SoundID.NPCDeath29 with { PitchVariance = 0.3f}
            }, {
                ItemID.CursedSapling,
                SoundID.NPCDeath5 with { PitchVariance = 0.5f}
            }, {
                ItemID.EverscreamPetItem,
                SoundID.NPCDeath5 with { PitchVariance = 0.5f}
            }, {
                ItemID.PigPetItem,
                SoundID.NPCDeath20 with { Pitch = 0.5f, PitchVariance = 0.3f}
            }, {
                ItemID.ParrotCracker,
                SoundID.NPCDeath48 with { PitchVariance = 0.5f}
            }, {
                ItemID.BrainOfCthulhuPetItem,
                SoundID.NPCDeath11 with { Pitch = -0.2f, PitchVariance = 0.2f}
            }, {
                ItemID.DD2OgrePetItem,
                SoundID.DD2_OgreDeath with { PitchVariance = 0.7f, Volume = 0.7f}
            }, {
                ItemID.MartianPetItem,
                SoundID.NPCDeath57 with { Pitch = -0.3f, PitchVariance = 0.5f}
            }, {
                ItemID.DD2BetsyPetItem,
                SoundID.DD2_BetsyScream with { Pitch = -0.5f, PitchVariance = 0.2f}
            }, {
                ItemID.DukeFishronPetItem,
                SoundID.NPCDeath20 with { Pitch = -0.2f, PitchVariance = 0.3f}
            }, {
                ItemID.MoonLordPetItem,
                SoundID.NPCDeath62 with { PitchVariance = 0.5f, Volume = 0.8f}
            }
        };
        public static SlotId PlayHurtSoundFromItemId(int itemId, Player player)
        {
            if (ModContent.GetInstance<PetPersonalization>().HurtSoundEnabled == false)
                return SlotId.Invalid;

            SoundStyle itemsHurtSound = SoundID.MenuClose;

            if (PetItemIdToHurtSound.TryGetValue(itemId, out SoundStyle[] value))
            {
                itemsHurtSound = value[Main.rand.Next(value.Length)];
            }
            if (itemId == ItemID.BerniePetItem)
            {
                itemsHurtSound = PetItemIdToHurtSound[itemId][player.Male == true ? 0 : 1];
            }

            return itemsHurtSound == SoundID.MenuClose ? SlotId.Invalid : SoundEngine.PlaySound(itemsHurtSound with { Type = SoundType.Sound }, player.Center);
        }

        public static SlotId PlayAmbientSoundFromItemId(int itemId, Player player)
        {
            int chance;
            switch (ModContent.GetInstance<PetPersonalization>().PassiveSoundFrequency)
            {
                case PassivePetSoundFrequency.None:
                    return SlotId.Invalid;
                case PassivePetSoundFrequency.Lowered:
                    chance = 18000;
                    break;
                case PassivePetSoundFrequency.Normal:
                    chance = 7200;
                    break;
                case PassivePetSoundFrequency.Increased:
                    chance = 3600;
                    break;
                default:
                    chance = 7200;
                    break;
            }
            if (Main.myPlayer == player.whoAmI && Main.rand.NextBool(chance))
            {
                SoundStyle petAmbientSound = SoundID.MenuClose;

                if (PetItemIdToAmbientSound.TryGetValue(itemId, out SoundStyle[] value))
                {
                    petAmbientSound = value[Main.rand.Next(value.Length)];
                }

                return petAmbientSound == SoundID.MenuClose ? SlotId.Invalid : SoundEngine.PlaySound(petAmbientSound with { Type = SoundType.Ambient, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, player.Center);
            }
            return SlotId.Invalid;

        }

        public static SlotId PlayKillSoundFromItemId(int itemId, Player player)
        {
            if (ModContent.GetInstance<PetPersonalization>().DeathSoundEnabled == false)
                return SlotId.Invalid;

            SoundStyle petKillSound = SoundID.MenuClose;

            if (PetItemidToKillSound.TryGetValue(itemId, out SoundStyle value))
            {
                petKillSound = value;
            }
            else if (itemId == ItemID.CompanionCube)
            {
                petKillSound = Main.rand.NextBool(25)
                    ? (SoundID.NPCDeath61 with { PitchVariance = 0.5f, Volume = 0.7f })
                    : (SoundID.NPCDeath59 with { PitchVariance = 0.5f });
            }

            return petKillSound == SoundID.MenuClose ? SlotId.Invalid : SoundEngine.PlaySound(petKillSound with { Type = SoundType.Sound }, player.Center);
        }
    }
}
