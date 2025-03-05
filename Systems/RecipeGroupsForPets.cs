using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.Systems
{
    public class RecipeGroupsForPets : ModSystem
    {
        public static RecipeGroup Silts;
        public static RecipeGroup ShadowHelmet;
        public static RecipeGroup ShadowScalemail;
        public static RecipeGroup ShadowGreaves;
        public static RecipeGroup IceBlocks;
        public static RecipeGroup Chests;
        public static RecipeGroup Copper;
        public static RecipeGroup Silver;
        public static RecipeGroup Gold;
        public static RecipeGroup Crowns; //WHY in Vanilla there is no Crown recipe group?????
        public static RecipeGroup AllBugs;
        public static RecipeGroup DemoniteBar;
        public static RecipeGroup Fairies;
        public static RecipeGroup Herbs;
        public static RecipeGroup Seeds;
        public static RecipeGroup Animals;
        public static RecipeGroup GoldenAnimals; //Vanilla recipe group is unused & unregistered sadly

        public override void AddRecipeGroups()
        {
            Silts = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.SiltBlock)}", ItemID.SiltBlock, ItemID.SlushBlock);
            RecipeGroup.RegisterGroup(nameof(ItemID.SiltBlock), Silts);
            ShadowHelmet = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.ShadowHelmet)}", ItemID.ShadowHelmet, ItemID.AncientShadowHelmet);
            RecipeGroup.RegisterGroup(nameof(ItemID.ShadowHelmet), ShadowHelmet);
            ShadowScalemail = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.ShadowScalemail)}", ItemID.ShadowScalemail, ItemID.AncientShadowScalemail);
            RecipeGroup.RegisterGroup(nameof(ItemID.ShadowScalemail), ShadowScalemail);
            ShadowGreaves = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.ShadowGreaves)}", ItemID.ShadowGreaves, ItemID.AncientShadowGreaves);
            RecipeGroup.RegisterGroup(nameof(ItemID.ShadowGreaves), ShadowGreaves);
            IceBlocks = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.IceBlock)}", ItemID.IceBlock, ItemID.PinkIceBlock, ItemID.PurpleIceBlock, ItemID.RedIceBlock);
            RecipeGroup.RegisterGroup(nameof(ItemID.IceBlock), IceBlocks);
            Chests = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.Chest)}", ItemID.Chest, ItemID.GoldenChest, ItemID.IceChest, ItemID.IvyChest, ItemID.LihzahrdChest, ItemID.LivingWoodChest, ItemID.MushroomChest, ItemID.RichMahoganyChest,
                ItemID.DesertChest, ItemID.SkywareChest, ItemID.WaterChest, ItemID.WebCoveredChest, ItemID.GraniteChest, ItemID.MarbleChest, ItemID.ShadowChest, ItemID.GoldenChest, ItemID.CorruptionChest, ItemID.CrimsonChest, ItemID.HallowedChest, ItemID.FrozenChest, ItemID.JungleChest,
                ItemID.DungeonDesertChest, ItemID.DeadMansChest, ItemID.NebulaChest, ItemID.SolarChest, ItemID.StardustChest, ItemID.VortexChest, ItemID.BoneChest, ItemID.LesionChest, ItemID.FleshChest, ItemID.GlassChest, ItemID.HoneyChest, ItemID.SlimeChest, ItemID.SteampunkChest,
                ItemID.AshWoodChest, ItemID.BalloonChest, ItemID.BambooChest, ItemID.BlueDungeonChest, ItemID.BorealWoodChest, ItemID.CactusChest, ItemID.CrystalChest, ItemID.DynastyChest, ItemID.EbonwoodChest, ItemID.GreenDungeonChest, ItemID.MartianChest, ItemID.MeteoriteChest,
                ItemID.PalmWoodChest, ItemID.PearlwoodChest, ItemID.PinkDungeonChest, ItemID.PumpkinChest, ItemID.CoralChest, ItemID.ShadewoodChest, ItemID.SpiderChest, ItemID.SpookyChest);
            RecipeGroup.RegisterGroup(nameof(ItemID.Chest), Chests);
            Copper = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.CopperBar)}", ItemID.CopperBar, ItemID.TinBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.CopperBar), Copper);
            Silver = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.SilverBar)}", ItemID.SilverBar, ItemID.TungstenBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.SilverBar), Silver);
            Gold = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.GoldBar)}", ItemID.GoldBar, ItemID.PlatinumBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.GoldBar), Gold);
            Crowns = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.GoldCrown)}", ItemID.GoldCrown, ItemID.PlatinumCrown);
            RecipeGroup.RegisterGroup(nameof(ItemID.GoldCrown), Crowns);
            AllBugs = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("RandomWorldName_Noun.Bugs")}", ItemID.Buggy, ItemID.EnchantedNightcrawler, ItemID.Firefly, ItemID.Lavafly, ItemID.LightningBug, ItemID.Grasshopper, ItemID.Grubby, ItemID.LadyBug, ItemID.Maggot, ItemID.Scorpion, ItemID.BlackScorpion, ItemID.Sluggy, ItemID.Stinkbug, ItemID.WaterStrider);
            RecipeGroup.RegisterGroup(nameof(ItemID.Buggy), AllBugs);
            DemoniteBar = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.DemoniteBar)}", ItemID.DemoniteBar, ItemID.CrimtaneBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.DemoniteBar), DemoniteBar);
            Fairies = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("RandomWorldName_Noun.Fairies")}", ItemID.FairyCritterBlue, ItemID.FairyCritterGreen, ItemID.FairyCritterPink);
            RecipeGroup.RegisterGroup(nameof(ItemID.FairyCritterBlue), Fairies);
            Herbs = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.PetsOverhaul.Herb")}", ItemID.Daybloom, ItemID.Moonglow, ItemID.Blinkroot, ItemID.Deathweed, ItemID.Waterleaf, ItemID.Fireblossom, ItemID.Shiverthorn);
            RecipeGroup.RegisterGroup(nameof(ItemID.Daybloom), Herbs);
            Seeds = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.PetsOverhaul.Herb")} {Language.GetTextValue("RandomWorldName_Noun.Seeds")}", ItemID.DaybloomSeeds, ItemID.MoonglowSeeds, ItemID.BlinkrootSeeds, ItemID.DeathweedSeeds, ItemID.WaterleafSeeds, ItemID.FireblossomSeeds, ItemID.ShiverthornSeeds);
            RecipeGroup.RegisterGroup(nameof(ItemID.DaybloomSeeds), Seeds);
            Animals = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.PetsOverhaul.Animal")}", ItemID.Bird, ItemID.BlueJay, ItemID.Buggy, ItemID.Bunny, ItemID.ExplosiveBunny, ItemID.GemBunnyAmber, ItemID.GemBunnyAmethyst, ItemID.GemBunnyDiamond, ItemID.GemBunnyEmerald, ItemID.GemBunnyRuby, ItemID.GemBunnySapphire, ItemID.GemBunnyTopaz, ItemID.Cardinal,
                ItemID.GrayCockatiel, ItemID.YellowCockatiel, ItemID.Duck, ItemID.MallardDuck, ItemID.EnchantedNightcrawler, ItemID.Firefly, ItemID.Lavafly, ItemID.LightningBug, ItemID.Frog, ItemID.Goldfish, ItemID.Grasshopper, ItemID.Grebe, ItemID.Grubby, ItemID.LadyBug, ItemID.BlueMacaw, ItemID.ScarletMacaw, ItemID.Maggot, ItemID.Mouse, ItemID.Owl, ItemID.Penguin, ItemID.Pupfish, ItemID.Rat, ItemID.Scorpion,
                ItemID.BlackScorpion, ItemID.Seagull, ItemID.Seahorse, ItemID.Sluggy, ItemID.Snail, ItemID.GlowingSnail, ItemID.MagmaSnail, ItemID.Squirrel, ItemID.SquirrelRed, ItemID.GemSquirrelAmber, ItemID.GemSquirrelAmethyst, ItemID.GemSquirrelDiamond, ItemID.GemSquirrelEmerald, ItemID.GemSquirrelRuby, ItemID.GemSquirrelSapphire, ItemID.GemSquirrelTopaz, ItemID.Stinkbug, ItemID.Toucan, ItemID.Turtle,
                ItemID.TurtleJungle, ItemID.WaterStrider, ItemID.Worm, ItemID.BlackDragonfly, ItemID.BlueDragonfly, ItemID.GreenDragonfly, ItemID.OrangeDragonfly, ItemID.RedDragonfly, ItemID.YellowDragonfly);
            RecipeGroup.RegisterGroup(nameof(ItemID.Bunny), Animals);
            GoldenAnimals = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("RandomWorldName_Adjective.Golden")} {Language.GetTextValue("Mods.PetsOverhaul.Animal")}", ItemID.GoldBird, ItemID.GoldBunny, ItemID.GoldButterfly, ItemID.GoldDragonfly, ItemID.GoldFrog, ItemID.GoldGoldfish, ItemID.GoldGrasshopper, ItemID.GoldLadyBug, ItemID.GoldMouse, ItemID.GoldSeahorse, ItemID.SquirrelGold, ItemID.GoldWaterStrider, ItemID.GoldWorm);
            RecipeGroup.RegisterGroup(nameof(ItemID.GoldBunny), GoldenAnimals);
        }
    }
}
