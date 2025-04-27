using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.Systems
{
    public class RecipeGroupsForPets : ModSystem
    {
        public static RecipeGroup Silts;
        public static List<int> silts = [ItemID.SiltBlock, ItemID.SlushBlock];
        public static RecipeGroup ShadowHelmet;
        public static List<int> shadowHelmet = [ItemID.ShadowHelmet, ItemID.AncientShadowHelmet];
        public static RecipeGroup ShadowScalemail;
        public static List<int> shadowScalemail = [ItemID.ShadowScalemail, ItemID.AncientShadowScalemail];
        public static RecipeGroup ShadowGreaves;
        public static List<int> shadowGreaves = [ItemID.ShadowGreaves, ItemID.AncientShadowGreaves];
        public static RecipeGroup IceBlocks;
        public static List<int> iceBlocks = [ItemID.IceBlock, ItemID.PinkIceBlock, ItemID.PurpleIceBlock, ItemID.RedIceBlock];
        public static RecipeGroup Chests;
        public static List<int> chests = [ItemID.Chest, ItemID.GoldenChest, ItemID.IceChest, ItemID.IvyChest, ItemID.LihzahrdChest, ItemID.LivingWoodChest, ItemID.MushroomChest, ItemID.RichMahoganyChest,
                ItemID.DesertChest, ItemID.SkywareChest, ItemID.WaterChest, ItemID.WebCoveredChest, ItemID.GraniteChest, ItemID.MarbleChest, ItemID.ShadowChest, ItemID.GoldenChest, ItemID.CorruptionChest, ItemID.CrimsonChest, ItemID.HallowedChest, ItemID.FrozenChest, ItemID.JungleChest,
                ItemID.DungeonDesertChest, ItemID.DeadMansChest, ItemID.NebulaChest, ItemID.SolarChest, ItemID.StardustChest, ItemID.VortexChest, ItemID.BoneChest, ItemID.LesionChest, ItemID.FleshChest, ItemID.GlassChest, ItemID.HoneyChest, ItemID.SlimeChest, ItemID.SteampunkChest,
                ItemID.AshWoodChest, ItemID.BalloonChest, ItemID.BambooChest, ItemID.BlueDungeonChest, ItemID.BorealWoodChest, ItemID.CactusChest, ItemID.CrystalChest, ItemID.DynastyChest, ItemID.EbonwoodChest, ItemID.GreenDungeonChest, ItemID.MartianChest, ItemID.MeteoriteChest,
                ItemID.PalmWoodChest, ItemID.PearlwoodChest, ItemID.PinkDungeonChest, ItemID.PumpkinChest, ItemID.CoralChest, ItemID.ShadewoodChest, ItemID.SpiderChest, ItemID.SpookyChest];
        public static RecipeGroup Copper;
        public static List<int> copper = [ItemID.CopperBar, ItemID.TinBar];
        public static RecipeGroup Silver;
        public static List<int> silver = [ItemID.SilverBar, ItemID.TungstenBar];
        public static RecipeGroup Gold;
        public static List<int> gold = [ItemID.GoldBar, ItemID.PlatinumBar];
        public static RecipeGroup Crowns; //WHY in Vanilla there is no Crown recipe group?????
        public static List<int> crowns = [ItemID.GoldCrown, ItemID.PlatinumCrown];
        public static RecipeGroup AllBugs;
        public static List<int> allBugs = [ItemID.Buggy, ItemID.EnchantedNightcrawler, ItemID.Firefly, ItemID.Lavafly, ItemID.LightningBug, ItemID.Grasshopper, ItemID.Grubby, ItemID.LadyBug, ItemID.Maggot, ItemID.Scorpion, ItemID.BlackScorpion, ItemID.Sluggy, ItemID.Stinkbug, ItemID.WaterStrider,
                ItemID.BlackDragonfly, ItemID.BlueDragonfly, ItemID.GreenDragonfly, ItemID.OrangeDragonfly, ItemID.RedDragonfly, ItemID.YellowDragonfly];
        public static RecipeGroup DemoniteBar;
        public static List<int> demoniteBar = [ItemID.DemoniteBar, ItemID.CrimtaneBar];
        public static RecipeGroup Fairies;
        public static List<int> fairies = [ItemID.FairyCritterBlue, ItemID.FairyCritterGreen, ItemID.FairyCritterPink];
        public static RecipeGroup Herbs;
        public static List<int> herbs = [ItemID.Daybloom, ItemID.Moonglow, ItemID.Blinkroot, ItemID.Deathweed, ItemID.Waterleaf, ItemID.Fireblossom, ItemID.Shiverthorn];
        public static RecipeGroup Seeds;
        public static List<int> seeds = [ItemID.DaybloomSeeds, ItemID.MoonglowSeeds, ItemID.BlinkrootSeeds, ItemID.DeathweedSeeds, ItemID.WaterleafSeeds, ItemID.FireblossomSeeds, ItemID.ShiverthornSeeds];
        public static RecipeGroup Animals;
        public static List<int> animals = [ItemID.Bird, ItemID.BlueJay, ItemID.Buggy, ItemID.Bunny, ItemID.ExplosiveBunny, ItemID.GemBunnyAmber, ItemID.GemBunnyAmethyst, ItemID.GemBunnyDiamond, ItemID.GemBunnyEmerald, ItemID.GemBunnyRuby, ItemID.GemBunnySapphire, ItemID.GemBunnyTopaz, ItemID.Cardinal,
                ItemID.GrayCockatiel, ItemID.YellowCockatiel, ItemID.Duck, ItemID.MallardDuck, ItemID.EnchantedNightcrawler, ItemID.Firefly, ItemID.Lavafly, ItemID.LightningBug, ItemID.Frog, ItemID.Goldfish, ItemID.Grasshopper, ItemID.Grebe, ItemID.Grubby, ItemID.LadyBug, ItemID.BlueMacaw, ItemID.ScarletMacaw,
                ItemID.Maggot, ItemID.Mouse, ItemID.Owl, ItemID.Penguin, ItemID.Pupfish, ItemID.Rat, ItemID.Scorpion,ItemID.BlackScorpion, ItemID.Seagull, ItemID.Seahorse, ItemID.Sluggy, ItemID.Snail, ItemID.GlowingSnail, ItemID.MagmaSnail, ItemID.Squirrel, ItemID.SquirrelRed, ItemID.GemSquirrelAmber,
                ItemID.GemSquirrelAmethyst, ItemID.GemSquirrelDiamond, ItemID.GemSquirrelEmerald, ItemID.GemSquirrelRuby, ItemID.GemSquirrelSapphire, ItemID.GemSquirrelTopaz, ItemID.Stinkbug, ItemID.Toucan, ItemID.Turtle, ItemID.TurtleJungle, ItemID.WaterStrider, ItemID.Worm, ItemID.BlackDragonfly,
                ItemID.BlueDragonfly, ItemID.GreenDragonfly, ItemID.OrangeDragonfly, ItemID.RedDragonfly, ItemID.YellowDragonfly];
        public static RecipeGroup GoldenAnimals; //Vanilla recipe group is unused & unregistered sadly
        public static List<int> goldenAnimals = [ItemID.GoldBird, ItemID.GoldBunny, ItemID.GoldButterfly, ItemID.GoldDragonfly, ItemID.GoldFrog, ItemID.GoldGoldfish, ItemID.GoldGrasshopper, ItemID.GoldLadyBug, ItemID.GoldMouse, ItemID.GoldSeahorse, ItemID.SquirrelGold, ItemID.GoldWaterStrider, ItemID.GoldWorm];

        public override void AddRecipeGroups()
        {
            Silts = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.SiltBlock)}", [.. silts]);
            RecipeGroup.RegisterGroup(nameof(ItemID.SiltBlock), Silts);
            ShadowHelmet = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.ShadowHelmet)}", [.. shadowHelmet]);
            RecipeGroup.RegisterGroup(nameof(ItemID.ShadowHelmet), ShadowHelmet);
            ShadowScalemail = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.ShadowScalemail)}", [.. shadowScalemail]);
            RecipeGroup.RegisterGroup(nameof(ItemID.ShadowScalemail), ShadowScalemail);
            ShadowGreaves = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.ShadowGreaves)}", [.. shadowGreaves]);
            RecipeGroup.RegisterGroup(nameof(ItemID.ShadowGreaves), ShadowGreaves);
            IceBlocks = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.IceBlock)}", [.. iceBlocks]);
            RecipeGroup.RegisterGroup(nameof(ItemID.IceBlock), IceBlocks);
            Chests = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.Chest)}", [.. chests]);
            RecipeGroup.RegisterGroup(nameof(ItemID.Chest), Chests);
            Copper = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.CopperBar)}", [.. copper]);
            RecipeGroup.RegisterGroup(nameof(ItemID.CopperBar), Copper);
            Silver = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.SilverBar)}", [.. silver]);
            RecipeGroup.RegisterGroup(nameof(ItemID.SilverBar), Silver);
            Gold = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.GoldBar)}", [.. gold]);
            RecipeGroup.RegisterGroup(nameof(ItemID.GoldBar), Gold);
            Crowns = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.GoldCrown)}", [.. crowns]);
            RecipeGroup.RegisterGroup(nameof(ItemID.GoldCrown), Crowns);
            AllBugs = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("RandomWorldName_Noun.Bugs")}", [.. allBugs]);
            RecipeGroup.RegisterGroup(nameof(ItemID.Buggy), AllBugs);
            DemoniteBar = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.DemoniteBar)}", [.. demoniteBar]);
            RecipeGroup.RegisterGroup(nameof(ItemID.DemoniteBar), DemoniteBar);
            Fairies = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("RandomWorldName_Noun.Fairies")}", [.. fairies]);
            RecipeGroup.RegisterGroup(nameof(ItemID.FairyCritterBlue), Fairies);
            Herbs = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {PetTextsColors.LocVal("Misc.Herb")}", [.. herbs]);
            RecipeGroup.RegisterGroup(nameof(ItemID.Daybloom), Herbs);
            Seeds = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {PetTextsColors.LocVal("Misc.Herb")} {Language.GetTextValue("RandomWorldName_Noun.Seeds")}", [.. seeds]);
            RecipeGroup.RegisterGroup(nameof(ItemID.DaybloomSeeds), Seeds);
            Animals = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {PetTextsColors.LocVal("Misc.Animal")}", [.. animals]);
            RecipeGroup.RegisterGroup(nameof(ItemID.Bunny), Animals);
            GoldenAnimals = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("RandomWorldName_Adjective.Golden")} {PetTextsColors.LocVal("Misc.Animal")}", [.. goldenAnimals]);
            RecipeGroup.RegisterGroup(nameof(ItemID.GoldBunny), GoldenAnimals);
        }
    }
}
