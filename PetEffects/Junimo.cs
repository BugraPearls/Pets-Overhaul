using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.Items;
using PetsOverhaul.NPCs;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.PetEffects
{
    public sealed class Junimo : PetEffect
    {
        public override int PetItemID => ItemID.JunimoPetItem;
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public string extraBosses = "";
        public int maxLvls = 25;
        public int externalLvlIncr = 0;
        public int maxXp = 2147480000;
        public float harvestingMoveSpeedPerLevel = 0.0025f;
        public float fishingDamagePerLevel = 0.0025f;
        public double miningHealthPerLevel = 1.5;
        public float harvestingCoin = 0.6f;
        public float miningCoin = 0.4f;
        public float fishingCoin = 6f;
        public int popupExpHarv = 0; //Represents current existing exp value on popup texts
        public int popupExpMining = 0;
        public int popupExpFish = 0;
        public int popupIndexHarv = -1;
        public int popupIndexMining = -1;
        public int popupIndexFish = -1;
        public bool anglerQuestDayCheck = false;
        public int CurrentExpStack
        {
            get
            {
                if (classThatGotExp == PetClasses.Harvesting)
                    return junimoHarvestingExp - junimoHarvestingLevelsToXp[junimoHarvestingLevel - 1];
                if (classThatGotExp == PetClasses.Mining)
                    return junimoMiningExp - junimoMiningLevelsToXp[junimoMiningLevel - 1];

                return junimoFishingExp - junimoFishingLevelsToXp[junimoFishingLevel - 1]; //defaults to fishing if classThatGotExp is 'invalid'
            }
        }
        public int CurrentExpRequiredStack
        {
            get
            {
                if (classThatGotExp == PetClasses.Harvesting)
                    return junimoHarvestingLevelsToXp[junimoHarvestingLevel] - junimoHarvestingLevelsToXp[junimoHarvestingLevel - 1];
                if (classThatGotExp == PetClasses.Mining)
                    return junimoMiningLevelsToXp[junimoMiningLevel] - junimoMiningLevelsToXp[junimoMiningLevel - 1];

                return junimoFishingLevelsToXp[junimoFishingLevel] - junimoFishingLevelsToXp[junimoFishingLevel - 1]; //defaults to fishing if classThatGotExp is 'invalid'
            }
        }
        public PetClasses classThatGotExp = PetClasses.Harvesting;

        public override int PetStackCurrent => CurrentExpStack;
        public override int PetStackMax => CurrentExpRequiredStack;
        public override string PetStackText => PetTextsColors.LocVal("PetItemTooltips.ExpStack").Replace("<class>", PetTextsColors.PetClassLocalized(classThatGotExp));

        /// <summary>
        /// Default exp value used by all classes.
        /// </summary>
        public int defaultExps = 100;
        public int junimoHarvestingLevel = 1;
        public int junimoHarvestingExp = 0;
        public int[] junimoHarvestingLevelsToXp =
        [
0,
75,
159,
253,
358,
475,
605,
749,
908,
1083,
1275,
1485,
1714,
1964,
2237,
2535,
2860,
3214,
3599,
4018,
4474,
4970,
5509,
6095,
6732,
7424,
8176,
8993,
9880,
10843,
11888,
13022,
14252,
15586,
17032,
18599,
20296,
22133,
24120,
26268,
28589,
31095,
33799,
36715,
39858,
43243,
46886,
50804,
55015,
59537,
64389,
69591,
75164,
81130,
87512,
94333,
101617,
109389,
117675,
126502,
135897,
145888,
156504,
167775,
179731,
192403,
205822,
220020,
235029,
250882,
267612,
285252,
303835,
323394,
343962,
365572,
388257,
412050,
436984,
463091,
490403,
518951,
548766,
579878,
612316,
646108,
681281,
717861,
755873,
795341,
836287,
878732,
922695,
968194,
1015245,
1063862,
1114058,
1165844,
1219229,
1274220,
        ];

        public int junimoMiningLevel = 1;
        public int junimoMiningExp = 0;
        public int[] junimoMiningLevelsToXp = [
0,
90,
190,
301,
424,
560,
710,
875,
1056,
1254,
1470,
1705,
1961,
2240,
2544,
2875,
3235,
3626,
4051,
4513,
5015,
5560,
6152,
6795,
7493,
8251,
9074,
9967,
10936,
11987,
13127,
14363,
15703,
17155,
18728,
20431,
22274,
24268,
26424,
28754,
31271,
33988,
36919,
40079,
43484,
47151,
51097,
55340,
59899,
64794,
70046,
75677,
81710,
88169,
95079,
102465,
110353,
118770,
127744,
137304,
147480,
158303,
169804,
182015,
194969,
208699,
223239,
238623,
254886,
272063,
290190,
309303,
329438,
350631,
372918,
396335,
420918,
446703,
473726,
502022,
531626,
562572,
594893,
628622,
663791,
700431,
738572,
778242,
819468,
862276,
906690,
952733,
1000426,
1049788,
1100837,
1153589,
1208058,
1264256,
1322193,
1381877,

        ];

        public int junimoFishingLevel = 1;
        public int junimoFishingExp = 0;
        public int[] junimoFishingLevelsToXp = [
0,
25,
53,
84,
118,
155,
195,
238,
284,
333,
385,
440,
498,
559,
623,
690,
760,
833,
909,
988,
1070,
1155,
1243,
1334,
1428,
1525,
1625,
1728,
1834,
1943,
2055,
2170,
2288,
2409,
2533,
2660,
2790,
2923,
3059,
3198,
3340,
3485,
3633,
3784,
3938,
4095,
4255,
4418,
4584,
4753,
4925,
5100,
5278,
5459,
5643,
5830,
6020,
6213,
6409,
6608,
6810,
7015,
7223,
7434,
7648,
7865,
8085,
8308,
8534,
8763,
8995,
9230,
9468,
9709,
9953,
10200,
10450,
10703,
10959,
11218,
11480,
11745,
12013,
12284,
12558,
12835,
13115,
13398,
13684,
13973,
14265,
14560,
14858,
15159,
15463,
15770,
16080,
16393,
16709,
17028,
        ];

        /// <summary>
        /// Remember to insert the expAmount as *100 from intended amount, eg. 2.5 exp should be written as 250.
        /// </summary>
        public static List<(int expAmount, int[] oreList)> MiningXpPerBlock = new()
        {
            { (90, [ItemID.Obsidian, ItemID.SiltBlock, ItemID.SlushBlock, ItemID.DesertFossil ]) },
            { (250, [ItemID.CopperOre, ItemID.TinOre ]) },
            { (325, [ItemID.IronOre, ItemID.LeadOre, ItemID.Amethyst ]) },
            { (400, [ItemID.SilverOre, ItemID.TungstenOre, ItemID.Topaz, ItemID.Sapphire, ItemID.Meteorite]) },
            { (475, [ItemID.GoldOre, ItemID.PlatinumOre, ItemID.Emerald, ItemID.Ruby, ItemID.Hellstone ]) },
            { (550, [ItemID.CrimtaneOre, ItemID.DemoniteOre, ItemID.Diamond, ItemID.Amber ]) },
            { (750, [ItemID.CobaltOre, ItemID.PalladiumOre ]) },
            { (900, [ItemID.MythrilOre, ItemID.OrichalcumOre ]) },
            { (1050, [ItemID.AdamantiteOre, ItemID.TitaniumOre, ItemID.CrystalShard ]) },
            { (1200, [ItemID.ChlorophyteOre ]) },
            { (1300, [ItemID.LunarOre ]) },
            { (2500, [ItemID.LifeCrystal ]) },
            { (12500, [ItemID.QueenSlimeCrystal ]) },
            { (100000, [ItemID.DirtiestBlock ]) }
        };
        /// <summary>
        /// Remember to insert the expAmount as *100 from intended amount, eg. 2.5 exp should be written as 250.
        /// </summary>
        public static List<(int expAmount, int[] plantList)> HarvestingXpPerGathered = new()
        {
            { (50, [ItemID.Hay]) },
            { (69, [ItemID.RottenEgg]) },
            { (110, [ItemID.Acorn ]) },
            { (125, [ItemID.AshGrassSeeds, ItemID.BlinkrootSeeds, ItemID.CorruptSeeds, ItemID.CrimsonSeeds, ItemID.DaybloomSeeds, ItemID.DeathweedSeeds, ItemID.FireblossomSeeds, ItemID.GrassSeeds, ItemID.HallowedSeeds, ItemID.JungleGrassSeeds, ItemID.MoonglowSeeds, ItemID.MushroomGrassSeeds, ItemID.ShiverthornSeeds, ItemID.WaterleafSeeds ]) },
            { (165, [ItemID.Wood, ItemID.AshWood, ItemID.BorealWood, ItemID.PalmWood, ItemID.Ebonwood, ItemID.Shadewood, ItemID.StoneBlock, ItemID.RichMahogany ]) },
            { (220, [ItemID.Daybloom, ItemID.Blinkroot, ItemID.Deathweed, ItemID.Fireblossom, ItemID.Moonglow, ItemID.Shiverthorn, ItemID.Waterleaf, ItemID.GlowingMushroom, ItemID.Pumpkin ]) },
            { (250, [ItemID.GemTreeAmberSeed, ItemID.GemTreeAmethystSeed, ItemID.GemTreeDiamondSeed, ItemID.GemTreeEmeraldSeed, ItemID.GemTreeRubySeed, ItemID.GemTreeSapphireSeed, ItemID.GemTreeTopazSeed, ItemID.Amethyst, ItemID.Topaz, ItemID.Sapphire, ItemID.Emerald, ItemID.Ruby, ItemID.Amber, ItemID.Diamond ]) },
            { (300, [ItemID.Pearlwood, ItemID.SpookyWood, ItemID.Cactus, ItemID.BambooBlock, ItemID.Mushroom, ItemID.VileMushroom, ItemID.ViciousMushroom ]) },
            { (500, [ItemID.Coral, ItemID.Seashell, ItemID.Starfish, ItemID.JungleSpores ]) },
            { (900, [ItemID.LightningWhelkShell, ItemID.TulipShell]) },
            { (1250, [ModContent.ItemType<PetFood>()]) },
            { (1750, [ItemID.SpicyPepper, ItemID.Pomegranate, ItemID.Elderberry, ItemID.BlackCurrant, ItemID.Apple, ItemID.Apricot, ItemID.Banana, ItemID.BloodOrange, ItemID.Cherry, ItemID.Coconut, ItemID.Grapefruit, ItemID.Lemon, ItemID.Mango, ItemID.Peach, ItemID.Pineapple, ItemID.Plum, ItemID.Rambutan ]) },
            { (2000, [ItemID.JunoniaShell, ModContent.ItemType<Egg>() ]) },
            { (2500, [ItemID.Dragonfruit, ItemID.Starfruit, ItemID.Grapes ]) },
            { (3500, [ItemID.GreenMushroom, ItemID.TealMushroom, ItemID.SkyBlueFlower, ItemID.YellowMarigold, ItemID.BlueBerries, ItemID.LimeKelp, ItemID.PinkPricklyPear, ItemID.OrangeBloodroot, ItemID.StrangePlant1, ItemID.StrangePlant2, ItemID.StrangePlant3, ItemID.StrangePlant4]) },
            { (5000, [ItemID.JungleRose, ItemID.ManaFlower ]) },
            { (10000, [ItemID.LifeFruit, ItemID.LeafWand, ItemID.LivingWoodWand, ItemID.LivingMahoganyWand, ItemID.LivingMahoganyLeafWand, ItemID.BlueEgg ]) },
            { (25000, [ItemID.EucaluptusSap, ItemID.MagicalPumpkinSeed ]) }
        };

        public int defaultSeaCreatureExp = 1500;
        /// <summary>
        /// Remember to insert the expAmount as *100 from intended amount, eg. 2.5 exp should be written as 250.
        /// </summary>
        public static List<(int expAmount, int[] enemyList)> FishingXpPerKill = new()
        {
            { (1500, [NPCID.EyeballFlyingFish, NPCID.ZombieMerman]) },
            { (3000, [NPCID.GoblinShark, NPCID.BloodEelBody, NPCID.BloodEelTail, NPCID.BloodEelHead ]) },
            { (5000, [NPCID.BloodNautilus ]) },
            { (50000, [NPCID.DukeFishron ]) }
        };
        public int anglerQuestExp = 4000;
        /// <summary>
        /// Remember to insert the expAmount as *100 from intended amount, eg. 2.5 exp should be written as 250.
        /// </summary>
        public static List<(int expAmount, int[] fishList)> FishingXpPerCaught = new()
        {
            { (20, [ItemID.FishingSeaweed, ItemID.OldShoe, ItemID.TinCan ]) },
            { (25, [ItemID.FrostDaggerfish ]) },
            { (35, [ItemID.BombFish ]) },
            { (100, [ItemID.Flounder, ItemID.Bass, ItemID.RockLobster, ItemID.Trout, ItemID.JojaCola ]) },
            { (175, [ItemID.AtlanticCod, ItemID.CrimsonTigerfish, ItemID.SpecularFish, ItemID.Tuna ]) },
            { (200, [ItemID.Salmon, ItemID.NeonTetra ]) },
            { (250, [ItemID.ArmoredCavefish, ItemID.Damselfish, ItemID.DoubleCod, ItemID.Ebonkoi, ItemID.FrostMinnow, ItemID.Hemopiranha, ItemID.Shrimp, ItemID.VariegatedLardfish ]) },
            { (275, [ItemID.Honeyfin, ItemID.PrincessFish, ItemID.Oyster ]) },
            { (350, [ItemID.WoodenCrate, ItemID.WoodenCrateHard ]) },
            { (375, [ItemID.Stinkfish, ItemID.BlueJellyfish, ItemID.GreenJellyfish, ItemID.PinkJellyfish, ItemID.Obsidifish, ItemID.Prismite ]) },
            { (500, [ItemID.PurpleClubberfish, ItemID.Swordfish, ItemID.ChaosFish, ItemID.FlarefinKoi, ItemID.IronCrate, ItemID.IronCrateHard ]) },
            { (600, [ItemID.SawtoothShark, ItemID.Rockfish, ItemID.ReaverShark, ItemID.AlchemyTable, ItemID.HallowedFishingCrate, ItemID.HallowedFishingCrateHard, ItemID.GoldenCarp ]) },
            { (700, [ItemID.JungleFishingCrate, ItemID.JungleFishingCrateHard, ItemID.CorruptFishingCrate, ItemID.CorruptFishingCrateHard, ItemID.CrimsonFishingCrate, ItemID.CrimsonFishingCrateHard, ItemID.DungeonFishingCrate, ItemID.DungeonFishingCrateHard, ItemID.FloatingIslandFishingCrate, ItemID.FloatingIslandFishingCrateHard, ItemID.FrozenCrate, ItemID.FrozenCrateHard, ItemID.OasisCrate, ItemID.OasisCrateHard, ItemID.OceanCrate, ItemID.OceanCrateHard ]) },
            { (900, [ItemID.GoldenCrate, ItemID.GoldenCrateHard, ItemID.LavaCrate, ItemID.LavaCrateHard ]) },
            { (1000, [ItemID.BalloonPufferfish, ItemID.FrogLeg ]) },
            { (1250, [ItemID.DreadoftheRedSea, ItemID.CombatBook, ItemID.ZephyrFish]) },
            { (1500, [ItemID.BottomlessLavaBucket, ItemID.LavaAbsorbantSponge, ItemID.DemonConch ]) },
            { (2000, [ItemID.LadyOfTheLake, ItemID.Toxikarp, ItemID.Bladetongue, ItemID.CrystalSerpent ]) },
            { (2500, [ItemID.ObsidianSwordfish, ItemID.ScalyTruffle ]) }
        };

        //public int baseRoll = 100; //1
        //public int rollChancePerLevel = 10; // 0.1
        //public static void AnglerQuestPool()
        //{
        //    GlobalPet.ItemWeight(ItemID.ApprenticeBait, 3000);
        //    GlobalPet.ItemWeight(ItemID.JourneymanBait, 300);
        //    GlobalPet.ItemWeight(ItemID.MasterBait, 50);
        //    GlobalPet.ItemWeight(ItemID.FishingPotion, 300);
        //    GlobalPet.ItemWeight(ItemID.SonarPotion, 330);
        //    GlobalPet.ItemWeight(ItemID.CratePotion, 300);
        //    GlobalPet.ItemWeight(ItemID.GoldCoin, 80);
        //    GlobalPet.ItemWeight(ItemID.PlatinumCoin, 1);

        //    GlobalPet.ItemWeight(ItemID.JojaCola, 150);
        //    GlobalPet.ItemWeight(ItemID.FrogLeg, 12);
        //    GlobalPet.ItemWeight(ItemID.BalloonPufferfish, 13);
        //    GlobalPet.ItemWeight(ItemID.PurpleClubberfish, 13);
        //    GlobalPet.ItemWeight(ItemID.ReaverShark, 10);
        //    GlobalPet.ItemWeight(ItemID.Rockfish, 10);
        //    GlobalPet.ItemWeight(ItemID.SawtoothShark, 10);
        //    GlobalPet.ItemWeight(ItemID.Swordfish, 20);
        //    GlobalPet.ItemWeight(ItemID.ZephyrFish, 4);
        //    GlobalPet.ItemWeight(ItemID.Oyster, 15);
        //    GlobalPet.ItemWeight(ItemID.WhitePearl, 10);
        //    GlobalPet.ItemWeight(ItemID.BlackPearl, 3);
        //    GlobalPet.ItemWeight(ItemID.PinkPearl, 1);
        //    GlobalPet.ItemWeight(ItemID.BottomlessLavaBucket, 3);
        //    GlobalPet.ItemWeight(ItemID.LavaAbsorbantSponge, 3);
        //    GlobalPet.ItemWeight(ItemID.DemonConch, 8);

        //    GlobalPet.ItemWeight(ItemID.PrincessFish, 500);
        //    GlobalPet.ItemWeight(ItemID.Stinkfish, 500);
        //    GlobalPet.ItemWeight(ItemID.ArmoredCavefish, 200);
        //    GlobalPet.ItemWeight(ItemID.FlarefinKoi, 75);
        //    GlobalPet.ItemWeight(ItemID.ChaosFish, 100);
        //    GlobalPet.ItemWeight(ItemID.Damselfish, 300);
        //    GlobalPet.ItemWeight(ItemID.DoubleCod, 250);
        //    GlobalPet.ItemWeight(ItemID.Ebonkoi, 200);
        //    GlobalPet.ItemWeight(ItemID.FrostMinnow, 300);
        //    GlobalPet.ItemWeight(ItemID.Hemopiranha, 200);
        //    GlobalPet.ItemWeight(ItemID.Honeyfin, 150);
        //    GlobalPet.ItemWeight(ItemID.BlueJellyfish, 50);
        //    GlobalPet.ItemWeight(ItemID.Obsidifish, 300);
        //    GlobalPet.ItemWeight(ItemID.GoldenCarp, 10);
        //    GlobalPet.ItemWeight(ItemID.SpecularFish, 550);
        //    GlobalPet.ItemWeight(ItemID.CrimsonTigerfish, 350);
        //    GlobalPet.ItemWeight(ItemID.VariegatedLardfish, 335);
        //    if (Main.hardMode)
        //    {
        //        GlobalPet.ItemWeight(ItemID.GreenJellyfish, 50);
        //        GlobalPet.ItemWeight(ItemID.Prismite, 50);
        //        GlobalPet.ItemWeight(ItemID.Toxikarp, 2);
        //        GlobalPet.ItemWeight(ItemID.Bladetongue, 2);
        //        GlobalPet.ItemWeight(ItemID.CrystalSerpent, 2);
        //        GlobalPet.ItemWeight(ItemID.ScalyTruffle, 1);
        //        GlobalPet.ItemWeight(ItemID.ObsidianSwordfish, 1);
        //    }
        //}
        /// <summary>
        /// Updates currently existing classes popup experience text or creates a new one if its nonexistent. This won't do anything if Config option to disable Exp popups are true. Returns Index of newly created Popup text, or will still return same index as before, if it already exists.
        /// </summary>
        public int PopupExp(int classIndex, int classExp, Color color)
        {
            if (Main.showItemText && classExp > 0)
            {
                Vector2 popupVelo = new(Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-6, -1));
                if (classIndex > -1)
                {
                    Main.popupText[classIndex].name = "+" + classExp.ToString() + " exp";
                    Main.popupText[classIndex].position = Player.Center;
                    Main.popupText[classIndex].velocity = popupVelo;
                    Main.popupText[classIndex].lifeTime = classExp + 180 > 700 ? 700 : classExp + 180;
                    Main.popupText[classIndex].rotation = Main.rand.NextFloat(0.2f);
                }
                else
                {
                    classIndex = PopupText.NewText(new AdvancedPopupRequest() with { Velocity = popupVelo, DurationInFrames = 180, Color = color, Text = "+" + classExp.ToString() + " exp" }, Player.Center);
                    Main.popupText[classIndex].rotation = Main.rand.NextFloat(0.2f);
                }
            }
            return classIndex;
        }
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public static void PreOnPickup(Item item, Player player)
        {
            GlobalPet PickerPet = player.GetModPlayer<GlobalPet>();
            Junimo juni = player.GetModPlayer<Junimo>();
            if (PickerPet.PickupChecks(item, juni.PetItemID, out ItemPet itemChck))
            {
                if (itemChck.harvestingDrop || itemChck.fortuneHarvestingDrop || itemChck.herbBoost)
                {
                    int index = HarvestingXpPerGathered.IndexOf(HarvestingXpPerGathered.Find(x => x.plantList.Contains(item.type)));
                    int value = index == -1
                        ? juni.defaultExps * item.stack
                        : HarvestingXpPerGathered[index].expAmount * item.stack;
                    PickerPet.GiveCoins(GlobalPet.Randomizer((int)(juni.harvestingCoin * juni.junimoHarvestingLevel * value)));
                    value = GlobalPet.Randomizer(value);
                    juni.junimoHarvestingExp += value;
                    juni.popupExpHarv += value;
                    juni.popupIndexHarv = juni.PopupExp(juni.popupIndexHarv, juni.popupExpHarv, PetTextsColors.HarvestingClass);
                    juni.classThatGotExp = PetClasses.Harvesting;

                }
                else if (itemChck.fishingDrop || itemChck.fortuneFishingDrop)
                {
                    int index = FishingXpPerCaught.IndexOf(FishingXpPerCaught.Find(x => x.fishList.Contains(item.type)));
                    int value = index == -1
                        ? juni.defaultExps * item.stack
                        : FishingXpPerCaught[index].expAmount * item.stack;
                    PickerPet.GiveCoins(GlobalPet.Randomizer((int)(juni.fishingCoin * juni.junimoFishingLevel * value)));
                    value = GlobalPet.Randomizer(value);
                    juni.junimoFishingExp += value;
                    juni.popupExpFish += value;
                    juni.popupIndexFish = juni.PopupExp(juni.popupIndexFish, juni.popupExpFish, PetTextsColors.FishingClass);
                    juni.classThatGotExp = PetClasses.Fishing;
                }
                else if (itemChck.blockNotByPlayer && (itemChck.oreBoost || itemChck.miningDrop || itemChck.fortuneMiningDrop))
                {
                    int index = MiningXpPerBlock.IndexOf(MiningXpPerBlock.Find(x => x.oreList.Contains(item.type)));
                    int value = index == -1
                        ? juni.defaultExps * item.stack
                        : MiningXpPerBlock[index].expAmount * item.stack;
                    PickerPet.GiveCoins(GlobalPet.Randomizer((int)(juni.miningCoin * juni.junimoMiningLevel * value)));
                    value = GlobalPet.Randomizer(value);
                    juni.junimoMiningExp += value;
                    juni.popupExpMining += value;
                    juni.popupIndexMining = juni.PopupExp(juni.popupIndexMining, juni.popupExpMining, PetTextsColors.MiningClass);
                    juni.classThatGotExp = PetClasses.Mining;
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.moveSpeed += junimoHarvestingLevel * harvestingMoveSpeedPerLevel;
                Player.GetDamage<GenericDamageClass>() += junimoFishingLevel * fishingDamagePerLevel;
                Player.statLifeMax2 += (int)(miningHealthPerLevel * junimoMiningLevel);
            }
        }
        public static void RunSeaCreatureOnKill(BinaryReader reader, int whoAmI, int npcId)
        {
            int player = reader.ReadByte();
            if (Main.netMode == NetmodeID.Server)
            {
                player = whoAmI;
            }
            Main.player[player].GetModPlayer<Junimo>().ExpOnSeaCreatureKill(npcId);
        }
        public void ExpOnSeaCreatureKill(int npcId)
        {
            if (Player.whoAmI == Main.myPlayer && Player.miscEquips[0].type == ItemID.JunimoPetItem)
            {
                int value;
                int index = FishingXpPerKill.IndexOf(FishingXpPerKill.Find(x => x.enemyList.Contains(npcId)));
                value = index == -1
                    ? GlobalPet.Randomizer(defaultSeaCreatureExp)
                    : GlobalPet.Randomizer(FishingXpPerKill[index].expAmount);

                junimoFishingExp += value;
                popupExpFish += value;
                popupIndexFish = PopupExp(popupIndexFish, popupExpFish, new Color(3, 130, 233));
                classThatGotExp = PetClasses.Fishing;
            }
        }
        public override void AnglerQuestReward(float rareMultiplier, List<Item> rewardItems)
        {
            if (anglerQuestDayCheck == false && PetIsEquipped(false))
            {
                int value = GlobalPet.Randomizer(anglerQuestExp);
                junimoFishingExp += value;
                popupExpFish += value;
                anglerQuestDayCheck = true;
                popupIndexFish = PopupExp(popupIndexFish, popupExpFish, new Color(3, 130, 233));
                classThatGotExp = PetClasses.Fishing;
            }
        }
        public override void ModifyCaughtFish(Item fish)
        {
            if (PetIsEquipped(false))
            {
                int index = FishingXpPerCaught.IndexOf(FishingXpPerCaught.Find(x => x.fishList.Contains(fish.type)));
                int value = index == -1
                    ? defaultExps * fish.stack
                    : FishingXpPerCaught[index].expAmount * fish.stack;
                Pet.GiveCoins(GlobalPet.Randomizer((int)(fishingCoin * junimoFishingLevel * value)));
                value = GlobalPet.Randomizer(value);
                junimoFishingExp += value;
                popupExpFish += value;
                popupIndexFish = PopupExp(popupIndexFish, popupExpFish, new Color(3, 130, 233));
                classThatGotExp = PetClasses.Fishing;
            }
        }
        public override void ExtraPreUpdateNoCheck()
        {
            if (popupIndexHarv > -1 && Main.popupText[popupIndexHarv].lifeTime <= 0)
            {
                popupIndexHarv = -1;
                popupExpHarv = 0;
            }
            if (popupIndexMining > -1 && Main.popupText[popupIndexMining].lifeTime <= 0)
            {
                popupIndexMining = -1;
                popupExpMining = 0;
            }
            if (popupIndexFish > -1 && Main.popupText[popupIndexFish].lifeTime <= 0)
            {
                popupIndexFish = -1;
                popupExpFish = 0;
            }
            if (Main.dayTime == true && Main.time == 0)
            {
                anglerQuestDayCheck = false;
            }

            maxLvls = 25;
            if (NPC.downedBoss2)
                maxLvls += 5;
            if (Main.hardMode)
                maxLvls += 5;
            if (NPC.downedMechBossAny)
                maxLvls += 5;
            if (NPC.downedPlantBoss)
                maxLvls += 5;
            if (NPC.downedAncientCultist)
                maxLvls += 5;
            maxLvls += externalLvlIncr;
            externalLvlIncr = 0;

            extraBosses = "";

            junimoHarvestingLevel = Math.Clamp(junimoHarvestingLevel, 1, maxLvls);
            junimoMiningLevel = Math.Clamp(junimoMiningLevel, 1, maxLvls);
            junimoFishingLevel = Math.Clamp(junimoFishingLevel, 1, maxLvls);

            junimoHarvestingExp = Math.Clamp(junimoHarvestingExp, 0, maxXp);
            junimoMiningExp = Math.Clamp(junimoMiningExp, 0, maxXp);
            junimoFishingExp = Math.Clamp(junimoFishingExp, 0, maxXp);

            AdvancedPopupRequest popupMessage = new()
            {
                DurationInFrames = 300,
                Velocity = new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-15, -10))
            };

            bool soundOn = ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled;
            if (junimoHarvestingLevel < maxLvls && junimoHarvestingExp >= junimoHarvestingLevelsToXp[junimoHarvestingLevel])
            {
                junimoHarvestingLevel++;
                if (soundOn)
                {
                    SoundEngine.PlaySound(SoundID.Item35 with { PitchVariance = 0.2f, Pitch = 0.5f }, Player.Center);
                }

                popupMessage.Color = PetTextsColors.HarvestingClass;
                popupMessage.Text = PetTextsColors.LocVal("PetItemTooltips.JunimoLevel")
                    .Replace("<class>", Language.GetTextValue($"Mods.PetsOverhaul.Classes.Harvesting"))
                    .Replace("<upOrMax>", junimoHarvestingLevel >= maxLvls ? PetTextsColors.LocVal("PetItemTooltips.JunimoMaxed") : PetTextsColors.LocVal("PetItemTooltips.JunimoUp"));
                PopupText.NewText(popupMessage, Player.Center);
            }
            if (junimoMiningLevel < maxLvls && junimoMiningExp >= junimoMiningLevelsToXp[junimoMiningLevel])
            {
                junimoMiningLevel++;
                if (soundOn)
                {
                    SoundEngine.PlaySound(SoundID.Item35 with
                    {
                        PitchVariance = 0.2f,
                        Pitch = 0.5f
                    }, Player.Center);
                }

                popupMessage.Color = PetTextsColors.MiningClass;
                popupMessage.Text = PetTextsColors.LocVal("PetItemTooltips.JunimoLevel")
                    .Replace("<class>", Language.GetTextValue($"Mods.PetsOverhaul.Classes.Mining"))
                    .Replace("<upOrMax>", junimoMiningLevel >= maxLvls ? PetTextsColors.LocVal("PetItemTooltips.JunimoMaxed") : PetTextsColors.LocVal("PetItemTooltips.JunimoUp"));
                PopupText.NewText(popupMessage, Player.Center);
            }

            if (junimoFishingLevel < maxLvls && junimoFishingExp >= junimoFishingLevelsToXp[junimoFishingLevel])
            {
                junimoFishingLevel++;
                if (soundOn)
                {
                    SoundEngine.PlaySound(SoundID.Item35 with
                    {
                        PitchVariance = 0.2f,
                        Pitch = 0.5f
                    }, Player.Center);
                }

                popupMessage.Color = PetTextsColors.FishingClass;
                popupMessage.Text = PetTextsColors.LocVal("PetItemTooltips.JunimoLevel")
                    .Replace("<class>", Language.GetTextValue($"Mods.PetsOverhaul.Classes.Fishing"))
                    .Replace("<upOrMax>", junimoFishingLevel >= maxLvls ? PetTextsColors.LocVal("PetItemTooltips.JunimoMaxed") : PetTextsColors.LocVal("PetItemTooltips.JunimoUp"));
                PopupText.NewText(popupMessage, Player.Center);
            }
        }
        public override void SaveData(TagCompound tag)
        {
            tag.Add("AnglerCheck", anglerQuestDayCheck);
            tag.Add("harvestlvl", junimoHarvestingLevel);
            tag.Add("harvestexp", junimoHarvestingExp);
            tag.Add("mininglvl", junimoMiningLevel);
            tag.Add("miningexp", junimoMiningExp);
            tag.Add("fishinglvl", junimoFishingLevel);
            tag.Add("fishingexp", junimoFishingExp);
        }
        public override void LoadData(TagCompound tag)
        {
            if (tag.TryGet("AnglerCheck", out bool anglerCheck))
            {
                anglerQuestDayCheck = anglerCheck;
            }

            if (tag.TryGet("harvestlvl", out int harvLvl))
            {
                junimoHarvestingLevel = harvLvl;
            }

            if (tag.TryGet("harvestexp", out int harvExp))
            {
                junimoHarvestingExp = harvExp;
            }

            if (tag.TryGet("mininglvl", out int miningLvl))
            {
                junimoMiningLevel = miningLvl;
            }

            if (tag.TryGet("miningexp", out int miningExp))
            {
                junimoMiningExp = miningExp;
            }

            if (tag.TryGet("fishinglvl", out int fishLvl))
            {
                junimoFishingLevel = fishLvl;
            }

            if (tag.TryGet("fishingexp", out int fishExp))
            {
                junimoFishingExp = fishExp;
            }
        }
    }

    public sealed class JunimoKilledSeaCreature : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override void OnKill(NPC npc)
        {
            if (npc.TryGetGlobalNPC(out NpcPet npcPet) && npcPet.seaCreature && npc.friendly == false)
            {
                int playerId = npcPet.playerThatFishedUp;
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    ModPacket packet = ModContent.GetInstance<PetsOverhaul>().GetPacket();
                    packet.Write((byte)MessageType.SeaCreatureOnKill);
                    packet.Write(npc.type);
                    packet.Write((byte)playerId);
                    packet.Send(toClient: playerId);
                }
                else
                {
                    Main.player[playerId].GetModPlayer<Junimo>().ExpOnSeaCreatureKill(npc.type);
                }
            }
        }
    }

    public sealed class JunimoPetItem : PetTooltip
    {
        public override PetEffect PetsEffect => junimo;
        public static Junimo junimo
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out Junimo pet))
                    return pet;
                else
                    return ModContent.GetInstance<Junimo>();
            }
        }
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.JunimoPetItem")
                        .Replace("<maxLvl>", junimo.maxLvls.ToString())
                        .Replace("<moreBoss>", junimo.extraBosses)
                        .Replace("<harvestingProfit>", Math.Round(junimo.harvestingCoin * junimo.junimoHarvestingLevel, 2).ToString())
                        .Replace("<flatHealth>", Math.Round(junimo.junimoMiningLevel * junimo.miningHealthPerLevel, 1).ToString())
                        .Replace("<harvestLevel>", $"[c/{PetTextsColors.ClassEnumToColor(PetClasses.Harvesting).Hex3()}:{junimo.junimoHarvestingLevel}]")
                        .Replace("<harvestNext>", $"[c/{PetTextsColors.ClassEnumToColor(PetClasses.Harvesting).Hex3()}:{(junimo.junimoHarvestingLevel >= junimo.maxLvls ? PetTextsColors.LocVal("PetItemTooltips.JunimoMaxLevelText") : (junimo.junimoHarvestingLevelsToXp[junimo.junimoHarvestingLevel] - junimo.junimoHarvestingExp).ToString())}]")
                        .Replace("<harvestCurrent>", $"[c/{PetTextsColors.ClassEnumToColor(PetClasses.Harvesting).Hex3()}:{junimo.junimoHarvestingExp}]")
                        .Replace("<miningProfit>", Math.Round(junimo.miningCoin * junimo.junimoMiningLevel, 2).ToString())
                        .Replace("<bonusMs>", Math.Round(junimo.junimoHarvestingLevel * junimo.harvestingMoveSpeedPerLevel * 100, 2).ToString())
                        .Replace("<miningLevel>", $"[c/{PetTextsColors.ClassEnumToColor(PetClasses.Mining).Hex3()}:{junimo.junimoMiningLevel}]")
                        .Replace("<miningNext>", $"[c/{PetTextsColors.ClassEnumToColor(PetClasses.Mining).Hex3()}:{(junimo.junimoMiningLevel >= junimo.maxLvls ? PetTextsColors.LocVal("PetItemTooltips.JunimoMaxLevelText") : (junimo.junimoMiningLevelsToXp[junimo.junimoMiningLevel] - junimo.junimoMiningExp).ToString())}]")
                        .Replace("<miningCurrent>", $"[c/{PetTextsColors.ClassEnumToColor(PetClasses.Mining).Hex3()}:{junimo.junimoMiningExp}]")
                        .Replace("<fishingProfit>", Math.Round(junimo.fishingCoin * junimo.junimoFishingLevel, 2).ToString())
                        .Replace("<bonusDamage>", Math.Round(junimo.junimoFishingLevel * junimo.fishingDamagePerLevel * 100, 2).ToString())
                        .Replace("<fishingLevel>", $"[c/{PetTextsColors.ClassEnumToColor(PetClasses.Fishing).Hex3()}:{junimo.junimoFishingLevel}]")
                        .Replace("<fishingNext>", $"[c/{PetTextsColors.ClassEnumToColor(PetClasses.Fishing).Hex3()}:{(junimo.junimoFishingLevel >= junimo.maxLvls ? PetTextsColors.LocVal("PetItemTooltips.JunimoMaxLevelText") : (junimo.junimoFishingLevelsToXp[junimo.junimoFishingLevel] - junimo.junimoFishingExp).ToString())}]")
                        .Replace("<fishingCurrent>", $"[c/{PetTextsColors.ClassEnumToColor(PetClasses.Fishing).Hex3()}:{junimo.junimoFishingExp}]")
                        .Replace("<harvesting>", $"[c/{PetTextsColors.ClassEnumToColor(PetClasses.Harvesting).Hex3()}:{PetTextsColors.PetClassLocalized(PetClasses.Harvesting)}]")
                        .Replace("<mining>", $"[c/{PetTextsColors.ClassEnumToColor(PetClasses.Mining).Hex3()}:{PetTextsColors.PetClassLocalized(PetClasses.Mining)}]")
                        .Replace("<fishing>", $"[c/{PetTextsColors.ClassEnumToColor(PetClasses.Fishing).Hex3()}:{PetTextsColors.PetClassLocalized(PetClasses.Fishing)}]");
        public override string SimpleTooltip => PetTextsColors.LocVal("SimpleTooltips.JunimoPetItem");
    }
}
