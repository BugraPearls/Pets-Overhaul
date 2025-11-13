using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.Systems
{
    /// <summary>
    /// <see langword="class"/> that contains many useful utilizable methods and fields that will assist with Pet effects development.
    /// </summary>
    public static class PetUtils
    {
        public static PetModPlayer PetPlayer(this Player player) => player.GetModPlayer<PetModPlayer>();

        public static IEntitySource GetSource_Pet(EntitySourcePetIDs typeId, string context = null)
        {
            return new EntitySource_Pet
            {
                ContextType = typeId,
                Context = context
            };
        }

        public static void PreOnPickup(Item item, Player player)
        {
            PetModPlayer PickerPet = player.PetPlayer();
            if (item.TryGetGlobalItem(out PetGlobalItem fortune) && fortune.pickedUpBefore == false && player.CanPullItem(item, player.ItemSpace(item)))
            {
                if (fortune.globalDrop)
                {
                    for (int i = 0; i < Randomizer(PickerPet.globalFortune * item.stack); i++)
                    {
                        player.QuickSpawnItem(GetSource_Pet(EntitySourcePetIDs.GlobalItem), item.type, 1);
                    }
                }

                if (fortune.harvestingDrop)
                {
                    for (int i = 0; i < Randomizer((PickerPet.globalFortune * 10 / 2 + PickerPet.harvestingFortune * 10) * item.stack, 1000); i++) //Multiplied by 10 and divided by 1000 since we divide globalFortune by 2, to get more precise numbers.
                    {
                        player.QuickSpawnItem(GetSource_Pet(EntitySourcePetIDs.HarvestingFortuneItem), item.type, 1);
                    }
                }

                if (fortune.miningDrop)
                {
                    for (int i = 0; i < Randomizer((PickerPet.globalFortune * 10 / 2 + PickerPet.miningFortune * 10) * item.stack, 1000); i++)
                    {
                        player.QuickSpawnItem(GetSource_Pet(EntitySourcePetIDs.MiningFortuneItem), item.type, 1);
                    }
                }

                if (fortune.fishingDrop)
                {
                    for (int i = 0; i < Randomizer((PickerPet.globalFortune * 10 / 2 + PickerPet.fishingFortune) * item.stack, 1000); i++)
                    {
                        player.QuickSpawnItem(GetSource_Pet(EntitySourcePetIDs.FishingFortuneItem), item.type, 1);
                    }
                }

                if (fortune.herbBoost)
                {
                    for (int i = 0; i < Randomizer((PickerPet.globalFortune + PickerPet.harvestingFortune) * 10 / 2 * item.stack, 1000); i++)
                    {
                        player.QuickSpawnItem(GetSource_Pet(EntitySourcePetIDs.HarvestingFortuneItem), item.type, 1);
                    }
                }

                if (fortune.oreBoost)
                {
                    for (int i = 0; i < Randomizer((PickerPet.globalFortune + PickerPet.miningFortune) * 10 / 2 * item.stack, 1000); i++)
                    {
                        player.QuickSpawnItem(GetSource_Pet(EntitySourcePetIDs.MiningFortuneItem), item.type, 1);
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the given enemy should not be lifestealen from, this can be used for non-lifesteal applications as some effects may not want to occur on friendly/statue/immortal etc. enemies.
        /// </summary>
        /// <param name="npc"></param>
        /// <returns></returns>
        public static bool LifestealCheck(NPC npc)
        {
            return !npc.friendly && !npc.SpawnedFromStatue && !npc.immortal && !PetIDs.EnemiesForLifestealToIgnore.Contains(npc.type) && npc.canGhostHeal;
        }
        /// <summary>
        /// Creates a Circle of dusts around the given Center with the Dust ID.
        /// </summary>
        /// <param name="dustAmount">Keep it at 0 or lower than 0 to default it to be radius divided by 10.</param>
        public static void CircularDustEffect(Vector2 Center, int dustID, int radius, int dustAmount = 0, float scale = 1f)
        {
            if (dustAmount <= 0)
            {
                dustAmount = radius / 10;
            }
            float actDustAmount = dustAmount;
            actDustAmount *= ModContent.GetInstance<PetPersonalization>().CircularDustAmount switch
            {
                ParticleAmount.None => 0,
                ParticleAmount.Lowered => 0.5f,
                ParticleAmount.Increased => 2f,
                _ => 1f,
            };
            if (actDustAmount > 0)
            {
                for (int i = 0; i < actDustAmount; i++)
                {
                    Vector2 pos = Center + Main.rand.NextVector2CircularEdge(radius, radius);
                    Point16 posCoord = Utils.ToTileCoordinates16(pos);
                    if (WorldGen.InWorld(posCoord.X, posCoord.Y))
                    {
                        if (ModContent.GetInstance<PetPersonalization>().CircularDustInsideBlocks == false && WorldGen.SolidTile(posCoord.X, posCoord.Y, true))
                        {
                            continue;
                        }

                        Dust dust = Dust.NewDustPerfect(pos, dustID, Scale: scale);
                        dust.noGravity = true;
                        dust.noLight = true;
                        dust.noLightEmittence = true;
                    }
                }
            }
        }

        /// <summary>
        /// Sets active of oldest Main.combatText to false.
        /// </summary>
        /// <returns>Index of removed combatText.</returns>
        public static int RemoveOldestCombatText()
        {
            int textLife = 6000;
            int textToRemove = 100;
            for (int i = 0; i < Main.maxCombatText; i++)
            {
                if (Main.combatText[i].lifeTime < textLife)
                {
                    textLife = Main.combatText[i].lifeTime;
                    textToRemove = i;
                }
            }
            Main.combatText[textToRemove].active = false;
            return textToRemove;
        }

        /// <summary>
        /// Randomizes the given number. numToBeRandomized / randomizeTo returns how many times its 100% chance and rolls if the leftover, non-100% amount is true. Randomizer(225) returns +2 and +1 more with 25% chance.
        /// randomizeTo is converted to positive if its negative for proper usage of Method. Negative values can be applied on numToBeRandomized to get the Method working the exact way, but to reduce. Ex: Randomizer(-225) returns -2 and -1 more with 25% chance.
        /// </summary>
        public static int Randomizer(int numToBeRandomized, int randomizeTo = 100)
        {
            if (randomizeTo < 0)
                randomizeTo *= -1;
            if (randomizeTo == 0)
                randomizeTo = 1;

            int amount = numToBeRandomized / randomizeTo;
            numToBeRandomized %= randomizeTo;

            if (numToBeRandomized < 0 && Main.rand.NextBool(numToBeRandomized * -1, randomizeTo))
            {
                amount--;
            }
            else if (Main.rand.NextBool(numToBeRandomized, randomizeTo))
            {
                amount++;
            }
            return amount;
        }

        #region Colors
        public static Color LowQuality => new(130, 130, 130);
        public static Color MidQuality => new(77, 117, 154);
        public static Color HighQuality => new(252, 194, 0);
        /// <summary>
        /// Alternates between (165, 249, 255) and (255, 207, 249) every frame.
        /// </summary>
        public static Color MaxQuality => Color.Lerp(ModContent.GetInstance<PetPersonalization>().MaxQualityColor1, ModContent.GetInstance<PetPersonalization>().MaxQualityColor2, PetModPlayer.ColorVal);
        public static Color MaxQualityColor1 => new(165, 249, 255);
        public static Color MaxQualityColor2 => new(255, 207, 249);
        public static Color MeleeClass => new(230, 145, 56);
        public static Color RangedClass => new(255, 179, 186);
        public static Color MagicClass => new(51, 153, 255);
        public static Color SummonerClass => new(138, 43, 226);
        public static Color UtilityClass => new(107, 65, 14);
        public static Color MobilityClass => new(204, 245, 245);
        public static Color HarvestingClass => new(205, 225, 0);
        public static Color MiningClass => new(150, 168, 176);
        public static Color FishingClass => new(27, 222, 255);
        public static Color OffensiveClass => new(246, 84, 106);
        public static Color DefensiveClass => new(14, 168, 14);
        public static Color SupportiveClass => new(242, 82, 169);
        public static Color RogueClass => new(255, 233, 36); //This is a temporary addition for Calamity addon, Classes will use Int rather than enum post 3.0.
        public static Color ClassEnumToColor(PetClasses Class) //Todo, will be a dictionary so easily addable from outside sources, ex. an addon.
        {
            return Class switch
            {
                PetClasses.None => new(0, 0, 0),
                PetClasses.Melee => MeleeClass,
                PetClasses.Ranged => RangedClass,
                PetClasses.Magic => MagicClass,
                PetClasses.Summoner => SummonerClass,
                PetClasses.Utility => UtilityClass,
                PetClasses.Mobility => MobilityClass,
                PetClasses.Harvesting => HarvestingClass,
                PetClasses.Mining => MiningClass,
                PetClasses.Fishing => FishingClass,
                PetClasses.Offensive => OffensiveClass,
                PetClasses.Defensive => DefensiveClass,
                PetClasses.Supportive => SupportiveClass,
                PetClasses.Rogue => RogueClass, //This is a temporary addition for Calamity addon, Classes will use Int rather than enum post 3.0.
                _ => new(0, 0, 0),
            };
        }
        #endregion

        #region Text & Tooltip related utils
        /// <summary>
        /// Shortened and easier way of retrieving Pets Overhaul's localization values.
        /// </summary>
        /// <param name="localizationKeyValue">Remainder of localization text value to come after Mods.PetsOverhaul.</param>
        /// <returns>Localization text value of "Mods.PetsOverhaul." + localizationKeyValue</returns>
        public static string LocVal(string localizationKeyValue)
        {
            return Language.GetTextValue("Mods.PetsOverhaul." + localizationKeyValue);
        }
        /// <summary>
        /// Converts given text to be corresponding color of Light Pet quality values
        /// </summary>
        /// <param name="text">Text to be converted</param>
        /// <param name="currentRoll">Current roll of the stat</param>
        /// <param name="maxRoll">Maximum roll of the stat</param>
        /// <returns>Text with its color changed depending on quality amount</returns>
        public static string LightPetRarityColorConvert(string text, int currentRoll, int maxRoll)
        {
            if (currentRoll == maxRoll)
            {
                return $"[c/{MaxQuality.Hex3()}:{text}]";
            }
            else if (currentRoll > maxRoll * 0.66f)
            {
                return $"[c/{HighQuality.Hex3()}:{text}]";
            }
            else if (currentRoll > maxRoll * 0.33f)
            {
                return $"[c/{MidQuality.Hex3()}:{text}]";
            }
            else
            {
                return $"[c/{LowQuality.Hex3()}:{text}]";
            }
        }
        /// <summary>
        /// Writes out Pet's Classes and their color mix. Works fine if only one class is given.
        /// </summary>
        public static string ClassText(PetClasses Class1, PetClasses Class2 = PetClasses.None)
        {
            if (Class1 == Class2)
            {
                Class2 = PetClasses.None;
            }

            if (Class1 == PetClasses.None && Class2 == PetClasses.None)
            {
                return "No class given.";
            }
            else if (Class1 != PetClasses.None && Class2 != PetClasses.None)
            {
                {
                    Color color = Color.Lerp(ClassEnumToColor(Class1), ClassEnumToColor(Class2), 0.5f);
                    return $"[c/{color.Hex3()}:{LocVal("Classes." + Class1)} {LocVal("Misc.And")} {LocVal("Classes." + Class2)} {LocVal("Misc.Pet")}]";
                }
            }
            else if (Class2 == PetClasses.None)
            {
                return $"[c/{ClassEnumToColor(Class1).Hex3()}:{LocVal("Classes." + Class1)} {LocVal("Misc.Pet")}]";
            }
            else
            {
                return $"[c/{ClassEnumToColor(Class2).Hex3()}:{LocVal("Classes." + Class2)} {LocVal("Misc.Pet")}]";
            }
        }
        /// <summary>
        /// Returns the 'Keybind missing' text if given Keybind is not assigned, and the Keybind's current value if it is.
        /// </summary>
        /// <param name="keybind"></param>
        /// <returns></returns>
        public static string KeybindText(ModKeybind keybind)
        {
            return keybind.GetAssignedKeys(PetModPlayer.PlayerInputMode).Count > 0 ? keybind.GetAssignedKeys(PetModPlayer.PlayerInputMode)[0] : $"[c/{LowQuality.Hex3()}:{LocVal("Misc.KeybindMissing")}]";
        }
        public static string RollMissingText()
        {
            return $"[c/{LowQuality.Hex3()}:{LocVal("LightPetTooltips.NotRolled")}]";
        }
        public static string PetClassLocalized(PetClasses petClass)
        {
            return LocVal("Classes." + petClass.ToString());
        }
        /// <summary>
        /// Converts all integers in given list to be in [i:ItemID] format, which is shown in game as the item itself within the string. Goes down below when countToGoBelow is reached. countStart is where it upstarts the count to go one line below, since there can be a text beforehand. Will also return the text 'None' if there is no items in the list.
        /// </summary>
        /// <param name="items">List of items that contains item ID'S</param>
        /// <param name="countToGoBelow"></param>
        /// <returns></returns>
        public static string ItemsToTooltipImages(List<int> items, int countToGoBelow = 15, int countStart = 5)
        {
            string result = "";
            int count = countStart;
            foreach (int item in items)
            {
                result += "[i:" + item.ToString() + "] ";
                count++;
                if (count >= countToGoBelow)
                {
                    count = 0;
                    result += "\n";
                }
            }
            if (result == "")
            {
                result = PetClassLocalized(PetClasses.None);
            }
            return result;
        }
        /// <summary>
        /// Returns: "first param" out of "second param" Seconds. Ex: 1 out of 3 Seconds. If secondVal is 0, it will just return Seconds of the firstValInFrames.
        /// </summary>
        public static string SecondsOutOfText(int firstValInFrames, int secondValInFrames)
        {
            if (firstValInFrames < 0)
            {
                firstValInFrames = 0;
            }
            if (secondValInFrames <= 0)
            {
                return $"{Math.Round(firstValInFrames / 60f, 2)} {LocVal("Misc.Secs")}";
            }
            return $"{Math.Round(firstValInFrames / 60f, 2)} {LocVal("LightPetTooltips.OutOf")} {Math.Round(secondValInFrames / 60f, 2)} {LocVal("Misc.Secs")}";
        }
        #endregion
    }
}
