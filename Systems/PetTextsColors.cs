using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.Systems
{

    /// <summary>
    /// <see langword="class"/> that contains many useful <see langword="method"/>'s for better tooltip and string usage, alongside including colors used by Pets Overhaul.
    /// </summary>
    public class PetTextsColors
    {
        /// <summary>
        /// Shortened and easier form of PetTextsColors.LocVal("<paramref name="localizationKeyValue"/>") to retrieve Pets Overhaul's localization values.
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
        public static Color LowQuality => new(130, 130, 130);
        public static Color MidQuality => new(77, 117, 154);
        public static Color HighQuality => new(252, 194, 0);
        /// <summary>
        /// Alternates between (165, 249, 255) and (255, 207, 249) every frame.
        /// </summary>
        public static Color MaxQuality => Color.Lerp(ModContent.GetInstance<PetPersonalization>().MaxQualityColor1, ModContent.GetInstance<PetPersonalization>().MaxQualityColor2, GlobalPet.ColorVal);
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
                    return $"[c/{color.Hex3()}:{PetTextsColors.LocVal("Classes." + Class1)} {PetTextsColors.LocVal("Misc.And")} {PetTextsColors.LocVal("Classes." + Class2)} {PetTextsColors.LocVal("Misc.Pet")}]";
                }
            }
            else if (Class2 == PetClasses.None)
            {
                return $"[c/{ClassEnumToColor(Class1).Hex3()}:{PetTextsColors.LocVal("Classes." + Class1)} {PetTextsColors.LocVal("Misc.Pet")}]";
            }
            else
            {
                return $"[c/{ClassEnumToColor(Class2).Hex3()}:{PetTextsColors.LocVal("Classes." + Class2)} {PetTextsColors.LocVal("Misc.Pet")}]";
            }
        }
        /// <summary>
        /// Returns the 'Keybind missing' text if given Keybind is not assigned, and the Keybind's current value if it is.
        /// </summary>
        /// <param name="keybind"></param>
        /// <returns></returns>
        public static string KeybindText(ModKeybind keybind)
        {
            return keybind.GetAssignedKeys(GlobalPet.PlayerInputMode).Count > 0 ? keybind.GetAssignedKeys(GlobalPet.PlayerInputMode)[0] : $"[c/{LowQuality.Hex3()}:{PetTextsColors.LocVal("Misc.KeybindMissing")}]";
        }
        public static string RollMissingText()
        {
            return "[c/" + LowQuality.Hex3() + ":" + PetTextsColors.LocVal("LightPetTooltips.NotRolled") + "]";
        }
        public static string PetClassLocalized(PetClasses petClass)
        {
            return PetTextsColors.LocVal("Classes." + petClass.ToString());
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
    }
}
