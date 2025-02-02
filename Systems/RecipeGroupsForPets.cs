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
        }
    }
}
