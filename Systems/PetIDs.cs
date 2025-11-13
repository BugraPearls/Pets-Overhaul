using PetsOverhaul.Items;
using ReLogic.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Systems
{
    /// <summary>
    /// Class that contains ID Bool Sets, Dictionaries and Lists that is related to Pets Overhaul.
    /// </summary>
    [ReinitializeDuringResizeArrays]
    public static class PetIDs
    {
        #region BuffID Sets
        /// <summary>
        /// List of Town Pet Buffs added by Pets Overhaul. Buffs here will be removed upon obtaining a new Town Pet Buff.
        /// </summary>
        public static bool[] TownPetBuffIDs = BuffID.Sets.Factory.CreateBoolSet(false); //We are not assigning here, as ALL TownPetBuffs has this set to true for their type by default.

        /// <summary>
        /// Contains list of debuffs that are related to burning.
        /// </summary>
        public static bool[] BurnDebuffs = BuffID.Sets.Factory.CreateBoolSet(false, BuffID.Burning, BuffID.OnFire, BuffID.OnFire3, BuffID.Frostburn, BuffID.CursedInferno, BuffID.ShadowFlame, BuffID.Frostburn2);

        #endregion

        #region NPCID Sets
        /// <summary>
        /// List of enemies that should not get health recovered off of.
        /// </summary>
        public static List<int> EnemiesForLifestealToIgnore = []; //LifestealCheck() method already covers vanilla cases of this. This is usually for other mods.

        /// <summary>
        /// Contains all Vanilla bosses that does not return npc.boss = true
        /// </summary>
        public static List<int> NonBossTrueBosses = [NPCID.TheDestroyer, NPCID.TheDestroyerBody, NPCID.TheDestroyerTail, NPCID.EaterofWorldsBody, NPCID.EaterofWorldsTail, NPCID.EaterofWorldsHead, NPCID.LunarTowerSolar, NPCID.LunarTowerNebula, NPCID.LunarTowerStardust, NPCID.LunarTowerVortex, NPCID.TorchGod, NPCID.Retinazer, NPCID.Spazmatism];

        /// <summary>
        /// Contains list of enemies that are associated with Corruption biome.
        /// </summary>
        public static List<int> CorruptEnemies = [NPCID.EaterofSouls, NPCID.LittleEater, NPCID.BigEater, NPCID.DevourerHead, NPCID.DevourerBody, NPCID.DevourerTail, NPCID.EaterofWorldsHead, NPCID.EaterofWorldsBody, NPCID.EaterofWorldsTail, NPCID.Corruptor, NPCID.CorruptSlime, NPCID.Slimeling, NPCID.Slimer, NPCID.Slimer2, NPCID.SeekerHead, NPCID.SeekerBody, NPCID.SeekerTail, NPCID.DarkMummy, NPCID.CursedHammer, NPCID.Clinger, NPCID.BigMimicCorruption, NPCID.DesertGhoulCorruption, NPCID.SandsharkCorrupt, NPCID.PigronCorruption, NPCID.CorruptGoldfish, NPCID.CorruptBunny, NPCID.CorruptPenguin, NPCID.DesertDjinn];

        /// <summary>
        /// Contains list of enemies that are associated with Crimson biome.
        /// </summary>
        public static List<int> CrimsonEnemies = [NPCID.BloodCrawler, NPCID.BloodCrawlerWall, NPCID.FaceMonster, NPCID.Crimera, NPCID.LittleCrimera, NPCID.BigCrimera, NPCID.BrainofCthulhu, NPCID.Creeper, NPCID.Herpling, NPCID.Crimslime, NPCID.BigCrimslime, NPCID.LittleCrimslime, NPCID.BloodJelly, NPCID.BloodFeeder, NPCID.BloodMummy, NPCID.CrimsonAxe, NPCID.IchorSticker, NPCID.FloatyGross, NPCID.BigMimicCrimson, NPCID.DesertGhoulCrimson, NPCID.SandsharkCrimson, NPCID.PigronCrimson, NPCID.CrimsonGoldfish, NPCID.CrimsonBunny, NPCID.CrimsonPenguin, NPCID.DesertDjinn];

        /// <summary>
        /// Contains list of enemies that are associated with Hallow biome. 
        /// </summary>
        public static List<int> HallowEnemies = [NPCID.Pixie, NPCID.Unicorn, NPCID.RainbowSlime, NPCID.Gastropod, NPCID.LightMummy, NPCID.QueenSlimeBoss, NPCID.QueenSlimeMinionBlue, NPCID.QueenSlimeMinionPink, NPCID.QueenSlimeMinionPurple, NPCID.HallowBoss, NPCID.IlluminantSlime, NPCID.IlluminantBat, NPCID.ChaosElemental, NPCID.EnchantedSword, NPCID.BigMimicHallow, NPCID.DesertGhoulHallow, NPCID.PigronHallow, NPCID.SandsharkHallow];

        #endregion

        #region TileID Sets
        /// <summary>
        /// Includes Gem tiles.
        /// </summary>
        public static bool[] gemTile = TileID.Sets.Factory.CreateBoolSet(false, TileID.Amethyst, TileID.Topaz, TileID.Sapphire, TileID.Emerald, TileID.Ruby, TileID.AmberStoneBlock, TileID.Diamond, TileID.ExposedGems, TileID.Crystals);

        /// <summary>
        /// Includes tiles that are extractable by an Extractinator and a few other stuff that aren't recognized as ores such as Obsidian and Luminite
        /// </summary>
        public static bool[] extractableAndOthers = TileID.Sets.Factory.CreateBoolSet(false, TileID.DesertFossil, TileID.Slush, TileID.Silt, TileID.Obsidian, TileID.LunarOre);

        /// <summary>
        /// Includes tiles that counts as trees.
        /// </summary>
        public static bool[] treeTile = TileID.Sets.Factory.CreateBoolSet(false, TileID.TreeAmber, TileID.TreeAmethyst, TileID.TreeAsh, TileID.TreeDiamond, TileID.TreeEmerald, TileID.TreeRuby, TileID.Trees, TileID.TreeSapphire, TileID.TreeTopaz, TileID.MushroomTrees, TileID.PalmTree, TileID.VanityTreeSakura, TileID.VanityTreeYellowWillow, TileID.Bamboo, TileID.Cactus);

        #endregion

        #region ItemID Sets
        /// <summary>
        /// Contains items dropped by gemstone trees. Current only use is Caveling Gardener and checking for the Gemstone Tree
        /// </summary>
        public static bool[] gemstoneTreeItem = ItemID.Sets.Factory.CreateBoolSet(false, ItemID.GemTreeAmberSeed, ItemID.GemTreeAmethystSeed, ItemID.GemTreeDiamondSeed, ItemID.GemTreeEmeraldSeed, ItemID.GemTreeRubySeed, ItemID.GemTreeSapphireSeed, ItemID.GemTreeTopazSeed, ItemID.Amethyst, ItemID.Topaz, ItemID.Sapphire, ItemID.Emerald, ItemID.Ruby, ItemID.Amber, ItemID.Diamond, ItemID.StoneBlock);

        /// <summary>
        /// Contains items dropped by trees. Only used by Blue Chicken.
        /// </summary>
        public static bool[] treeItem = ItemID.Sets.Factory.CreateBoolSet(false, ItemID.Acorn, ItemID.BambooBlock, ItemID.Cactus, ItemID.Wood, ItemID.AshWood, ItemID.BorealWood, ItemID.PalmWood, ItemID.Ebonwood, ItemID.Shadewood, ItemID.RichMahogany, ItemID.Pearlwood, ItemID.SpookyWood);

        /// <summary>
        /// Contains forageable items on Ocean biomes that counts as herb item for Harvesting Pet purposes.
        /// </summary>
        public static bool[] seaPlantItem = ItemID.Sets.Factory.CreateBoolSet(false, ItemID.Coral, ItemID.Seashell, ItemID.Starfish, ItemID.LightningWhelkShell, ItemID.TulipShell, ItemID.JunoniaShell);

        /// <summary>
        /// Contains plants that cannot be planted by using a Seed.
        /// </summary>
        public static bool[] plantsWithNoSeeds = ItemID.Sets.Factory.CreateBoolSet(false, ItemID.Hay, ItemID.Mushroom, ItemID.GlowingMushroom, ItemID.VileMushroom, ItemID.ViciousMushroom, ItemID.GreenMushroom, ItemID.TealMushroom, ItemID.SkyBlueFlower, ItemID.YellowMarigold, ItemID.BlueBerries, ItemID.LimeKelp, ItemID.PinkPricklyPear, ItemID.OrangeBloodroot, ItemID.StrangePlant1, ItemID.StrangePlant2, ItemID.StrangePlant3, ItemID.StrangePlant4, ItemID.LifeFruit);

        #endregion

        #region Mining, Harvesting, Fishing Items & Enemies and Exp values
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

        //public int defaultSeaCreatureExp = 1500;
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
        #endregion

        #region Pet Item ID's
        public static Dictionary<string, int> LightPetNamesAndItems = new()
        {
            {"Flickerwick",ItemID.DD2PetGhost },
            {"Crimson Heart",ItemID.CrimsonHeart },
            {"Fairy",ItemID.FairyBell},
            {"Jack 'O Lantern",ItemID.PumpkingPetItem },
            {"Fairy Princess",ItemID.FairyQueenPetItem },
            {"Magic Lantern",ItemID.MagicLantern },
            {"Shadow Orb",ItemID.ShadowOrb },
            {"Suspicious Looking Eye",ItemID.SuspiciousLookingTentacle },
            {"Toy Golem",ItemID.GolemPetItem},
            {"Wisp",ItemID.WispinaBottle },
        };

        public static Dictionary<string, int> PetNamesAndItems = new()
        {
            {"Turtle", ItemID.Seaweed},
            {"Baby Dinosaur", ItemID.AmberMosquito},
            {"Baby Eater", ItemID.EatersBone},
            {"Baby Face Monster", ItemID.BoneRattle},
            {"Baby Grinch", ItemID.BabyGrinchMischiefWhistle},
            {"Baby Hornet", ItemID.Nectar},
            {"Baby Imp", ItemID.HellCake},
            {"Baby Penguin", ItemID.Fish},
            {"Baby Red Panda", ItemID.BambooLeaf},
            {"Dungeon Guardian", ItemID.BoneKey},
            {"Baby Snowman", ItemID.ToySled},
            {"Baby Truffle", ItemID.StrangeGlowingMushroom},
            {"Baby Werewolf", ItemID.FullMoonSqueakyToy},
            {"Bernie", ItemID.BerniePetItem},
            {"Black Cat", ItemID.UnluckyYarn},
            {"Blue Chicken", ItemID.BlueEgg},
            {"Bunny", ItemID.Carrot},
            {"Caveling Gardener", ItemID.GlowTulip},
            {"Chester", ItemID.ChesterPetItem},
            {"Companion Cube", ItemID.CompanionCube},
            {"Cursed Sapling", ItemID.CursedSapling},
            {"Dirtiest Block", ItemID.DirtiestBlock},
            {"Dynamite Kitten", ItemID.BallOfFuseWire},
            {"Estee", ItemID.CelestialWand},
            {"Eyeball Spring", ItemID.EyeSpring},
            {"Fennec Fox", ItemID.ExoticEasternChewToy},
            {"Glittery Butterfly", ItemID.BedazzledNectar},
            {"Glommer", ItemID.GlommerPetItem},
            {"Hoardagron", ItemID.DD2PetDragon},
            {"Junimo", ItemID.JunimoPetItem},
            {"Lil' Harpy", ItemID.BirdieRattle},
            {"Lizard", ItemID.LizardEgg},
            {"Mini Minotaur", ItemID.TartarSauce},
            {"Parrot", ItemID.ParrotCracker},
            {"Pig Man", ItemID.PigPetItem},
            {"Plantero", ItemID.MudBud},
            {"Propeller Gato", ItemID.DD2PetGato},
            {"Puppy", ItemID.DogWhistle},
            {"Sapling", ItemID.Seedling},
            {"Spider", ItemID.SpiderEgg},
            {"Shadow Mimic", ItemID.OrnateShadowKey},
            {"SharkPup", ItemID.SharkBait},
            {"Spiffo", ItemID.SpiffoPlush},
            {"Squashling", ItemID.MagicalPumpkinSeed},
            {"Sugar Glider", ItemID.EucaluptusSap},
            {"Tiki Spirit", ItemID.TikiTotem},
            {"Volt Bunny", ItemID.LightningCarrot},
            {"Zephyr Fish", ItemID.ZephyrFish},
            {"Suspicious Eye", ItemID.EyeOfCthulhuPetItem},
            {"Spider Brain", ItemID.BrainOfCthulhuPetItem},
            {"Eater of Worms", ItemID.EaterOfWorldsPetItem},
            {"Slime Prince", ItemID.KingSlimePetItem},
            {"HoneyBee", ItemID.QueenBeePetItem},
            {"Tiny Deerclops", ItemID.DeerclopsPetItem},
            {"Skeletron Jr.", ItemID.SkeletronPetItem},
            {"Slime Princess", ItemID.QueenSlimePetItem},
            {"Mini Prime", ItemID.SkeletronPrimePetItem},
            {"Destroyer", ItemID.DestroyerPetItem},
            {"Rez and Spaz", ItemID.TwinsPetItem},
            {"Everscream Sapling", ItemID.EverscreamPetItem},
            {"Alien Skater", ItemID.MartianPetItem},
            {"Baby Ogre", ItemID.DD2OgrePetItem},
            {"Tiny Fishron", ItemID.DukeFishronPetItem},
            {"Phantasmal Dragon", ItemID.LunaticCultistPetItem},
            {"Itsy Betsy", ItemID.DD2BetsyPetItem},
            {"Ice Queen", ItemID.IceQueenPetItem},
            {"Plantera Seedling", ItemID.PlanteraPetItem},
            {"Moonling", ItemID.MoonLordPetItem},
            {"Slime Royals", ItemID.ResplendentDessert},
        };
        #endregion
    }
    /// <summary>
    /// Class that contains PetSlowID's, where same slow ID does not overlap with itself, and a slow with greater slow & better remaining time will override the obsolete one.
    /// </summary>
    public class PetSlowID
    {
        /// <summary>
        /// Assign Sets only in SetStaticDefaults() or with use of <see cref="ReinitializeDuringResizeArraysAttribute"/>. Example:
        /// <code>
        /// internal static int MyCustomSlowID; //This needs to be assigned in a Load() with usage of <see cref="RegisterSlowID(string, ref int)"/>.
        /// public override SetStaticDefaults()
        /// {
        ///     PetSlowID.Sets.ElectricBasedSlows[MyCustomSlowID] = true;
        /// } 
        /// </code>
        /// </summary>
        [ReinitializeDuringResizeArrays]
        public static class Sets
        {
            public static SetFactory Factory = new SetFactory(SlowIDCount, "PetsOverhaul/PetSlowIDs", Search);

            public static bool[] ElectricBasedSlows = Factory.CreateNamedSet("ElectricSlows")
            .Description("Enemies with this type of slow will emit Electric dusts.")
            .RegisterBoolSet(false, VoltBunny, PhantasmalLightning);

            public static bool[] ColdBasedSlows = Factory.CreateNamedSet("ColdSlows")
            .Description("Enemies with this type of slow will emit Icy dusts.")
            .RegisterBoolSet(false, Any, Snowman, Deerclops, IceQueen, PhantasmalIce, Grinch);

            public static bool[] SicknessBasedSlows = Factory.CreateNamedSet("SicknessSlows")
            .Description("Enemies with this type of slow will emit poison dusts.")
            .RegisterBoolSet(false, PrincessSlime, PrinceSlime);
        }
        internal static int SlowIDCount { get; set; } = 10; //This is the 'base' amount of ID's we have. It goes up from here as more Slow ID's gets added.

        public static IdDictionary Search = IdDictionary.Create<PetSlowID, int>();

        public const int Any = 0;
        public const int Snowman = 1;
        public const int PrincessSlime = 2;
        public const int Deerclops = 3;
        public const int IceQueen = 4;
        public const int VoltBunny = 5;
        public const int PhantasmalIce = 6;
        public const int PhantasmalLightning = 7;
        public const int PrinceSlime = 8;
        public const int Grinch = 9;

        /// <summary>
        /// This should be called in Load(). If needed, assign Bool Factories in SetStaticDefaults().
        /// 
        /// Example use:
        /// <code>
        /// internal static int MyCustomSlowID;
        /// 
        /// public override Load()
        /// {
        ///     PetSlowID.RegisterSlowID(nameof(MyCustomSlowID), ref MyCustomSlowID);
        /// }
        /// </code>
        /// </summary>
        /// <param name="nameofID">Name of the field to be added to the PetSlowID's. Usage: nameof("your field here")</param>
        /// <param name="fieldOfID">Field that will have its ID assigned to.</param>
        public static void RegisterSlowID(string nameofID, ref int fieldOfID)
        {
            Search.Add(nameofID, SlowIDCount);
            fieldOfID = SlowIDCount;
            SlowIDCount++;
        }
    }
}
