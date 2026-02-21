using PetsOverhaul.Items;
using PetsOverhaul.Tiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
namespace PetsOverhaul.Systems
{
    public class PetRecipes : ModSystem
    {
        /// <summary>
        /// Pet Recipes all have Pet Food in their recipes, and crafted on Pet Forge.
        /// </summary>
        /// <param name="recipe">Recipe that will get registered, to be added Pet Food and to be crafted on a Pet Forge.</param>
        public static void PetRecipe(Recipe recipe, int petFoodAmount = 1)
        {
            recipe.AddIngredient(ModContent.ItemType<PetFood>(), Math.Max(petFoodAmount, 1))
                .AddTile(ModContent.TileType<PetForge>())
                .AddCustomShimmerResult(ModContent.ItemType<PetFood>(), petFoodAmount) //This makes it so the Shimmering ALWAYS results in how much Pet Food used in the recipes. 
                .Register();
        }
        /// <summary>
        /// Creates a Pet Recipe that also has a Mastery Shard as one of its ingredients.
        /// </summary>
        public static void MasterModePetRecipe(Recipe recipe, int petFoodAmount = 1, int masteryShardAmount = 1) => PetRecipe(recipe.AddIngredient<MasteryShard>(Math.Max(masteryShardAmount, 1)), petFoodAmount);

        public static void PetFoodRecipe(Recipe recipe)
        {
            recipe.AddTile(ModContent.TileType<PetForge>())
                .DisableDecraft() //De-shimmer here would mean you can get so many materials you shouldn't get so we disable
                .Register();
        }

        public override void AddRecipes()
        {
            MasterModePetRecipe(Recipe.Create(ItemID.MartianPetItem).AddIngredient(ItemID.MartianConduitPlating, 750).AddIngredient(ItemID.Hoverboard), 2500); //Example: This is 750 Martian Conduit Plating, 1 Hoverboard, 1.25~ Platinum Coins (2500 Pet Food) and 1 Mastery Shard.
            PetRecipe(Recipe.Create(ItemID.AmberMosquito).AddRecipeGroup(RecipeGroupsForPets.Silts, 500).AddIngredient(ItemID.DesertFossil, 200).AddIngredient(ItemID.FossilOre, 25).AddIngredient(ItemID.Amber, 15).AddIngredient(ItemID.Amethyst, 3).AddIngredient(ItemID.Topaz, 3).AddIngredient(ItemID.Sapphire, 3).AddIngredient(ItemID.Emerald, 3).AddIngredient(ItemID.Ruby, 3).AddIngredient(ItemID.Diamond, 3));
            PetRecipe(Recipe.Create(ItemID.EatersBone).AddRecipeGroup(RecipeGroupsForPets.ShadowHelmet).AddRecipeGroup(RecipeGroupsForPets.ShadowScalemail).AddRecipeGroup(RecipeGroupsForPets.ShadowGreaves).AddIngredient(ItemID.EbonstoneBlock, 40).AddIngredient(ItemID.VileMushroom, 12), 75);
            PetRecipe(Recipe.Create(ItemID.BoneRattle).AddIngredient(ItemID.CrimsonHelmet).AddIngredient(ItemID.CrimsonScalemail).AddIngredient(ItemID.CrimsonGreaves).AddIngredient(ItemID.CrimstoneBlock, 40).AddIngredient(ItemID.ViciousMushroom, 12), 75);
            PetRecipe(Recipe.Create(ItemID.BabyGrinchMischiefWhistle).AddRecipeGroup(RecipeGroupsForPets.IceBlocks, 1000).AddIngredient(ItemID.Snowball, 1000).AddIngredient(ItemID.Shiverthorn, 500).AddIngredient(ItemID.FlinxFur, 20).AddCondition(Condition.DownedIceQueen), 2000);
            PetRecipe(Recipe.Create(ItemID.Nectar).AddIngredient(ItemID.BeeWax, 100).AddIngredient(ItemID.Stinger, 8).AddIngredient(ItemID.Hive, 40), 30);
            PetRecipe(Recipe.Create(ItemID.HellCake).AddIngredient(ItemID.Obsidian, 25).AddIngredient(ItemID.AshBlock, 300).AddIngredient(ItemID.LavaBucket).AddIngredient(ItemID.Fireblossom, 30), 8);
            MasterModePetRecipe(Recipe.Create(ItemID.DD2OgrePetItem).AddIngredient(ItemID.DefenderMedal, 100).AddRecipeGroup(RecipeGroupID.Wood, 5000).AddIngredient(ItemID.Gel, 100).AddCondition(Condition.DownedOldOnesArmyT2), 1000);
            PetRecipe(Recipe.Create(ItemID.Fish).AddIngredient(ItemID.Feather, 10).AddIngredient(ItemID.AtlanticCod, 25).AddIngredient(ItemID.FrostMinnow, 10).AddIngredient(ItemID.Shrimp, 4).AddIngredient(ItemID.FrostDaggerfish, 150).AddIngredient(ItemID.Penguin, 3).AddIngredient(ItemID.Flipper), 50);
            PetRecipe(Recipe.Create(ItemID.BambooLeaf).AddIngredient(ItemID.BambooBlock, 1100), 350);
            PetRecipe(Recipe.Create(ItemID.BoneKey).AddIngredient(ItemID.Ectoplasm, 999).AddIngredient(ItemID.Bone, 9999), 9999);
            PetRecipe(Recipe.Create(ItemID.ToySled).AddIngredient(ItemID.FrostCore, 3).AddIngredient(ItemID.SnowBlock, 3500).AddIngredient(ItemID.BorealWood, 500), 700);
            PetRecipe(Recipe.Create(ItemID.StrangeGlowingMushroom).AddIngredient(ItemID.GlowingMushroom, 500).AddIngredient(ItemID.GlowingSnail).AddIngredient(ItemID.MushroomGrassSeeds, 10).AddCondition(Condition.Hardmode, Condition.InGlowshroom), 100);
            PetRecipe(Recipe.Create(ItemID.FullMoonSqueakyToy).AddIngredient(ItemID.MoonCharm).AddIngredient(ItemID.TatteredCloth, 3), 300);
            PetRecipe(Recipe.Create(ItemID.BerniePetItem).AddIngredient(ItemID.LivingFireBlock, 50).AddIngredient(ItemID.Fireblossom, 12).AddIngredient(ItemID.Silk, 20).AddCondition(Condition.DownedPlantera), 100);
            PetRecipe(Recipe.Create(ItemID.UnluckyYarn).AddIngredient(ItemID.LuckPotionLesser).AddIngredient(ItemID.LadyBug).AddIngredient(ItemID.WizardHat).AddIngredient(ItemID.Wood).AddIngredient(ItemID.Hay).AddIngredient(ItemID.SilkRopeCoil, 13), 13);
            PetRecipe(Recipe.Create(ItemID.BlueEgg).AddIngredient(ItemID.BlinkrootSeeds, 20).AddIngredient(ItemID.DaybloomSeeds, 20).AddIngredient(ItemID.DeathweedSeeds, 20).AddIngredient(ItemID.FireblossomSeeds, 20).AddIngredient(ItemID.MoonglowSeeds, 20).AddIngredient(ItemID.ShiverthornSeeds, 20).AddIngredient(ItemID.WaterleafSeeds, 20).AddIngredient(ItemID.Acorn, 100).AddRecipeGroup(RecipeGroupID.Fruit, 5), 50);
            PetRecipe(Recipe.Create(ItemID.GlowTulip).AddIngredient(ItemID.Moonglow, 30).AddIngredient(ItemID.Blinkroot, 30).AddIngredient(ItemID.Glowstick, 100).AddIngredient(ItemID.Hay, 200), 20);
            PetRecipe(Recipe.Create(ItemID.ChesterPetItem).AddIngredient(ItemID.BuilderPotion, 3).AddRecipeGroup(RecipeGroupsForPets.Chests, 50));
            PetRecipe(Recipe.Create(ItemID.CompanionCube).AddIngredient(ItemID.LifeCrystal, 10).AddIngredient(ItemID.ManaCrystal, 10).AddRecipeGroup(RecipeGroupsForPets.Copper, 10).AddRecipeGroup(RecipeGroupID.IronBar, 10).AddRecipeGroup(RecipeGroupsForPets.Silver, 10).AddRecipeGroup(RecipeGroupsForPets.Gold, 10), 5000);
            PetRecipe(Recipe.Create(ItemID.CursedSapling).AddIngredient(ItemID.SpookyWood, 2500), 2000);
            MasterModePetRecipe(Recipe.Create(ItemID.DestroyerPetItem).AddIngredient(ItemID.Wire, 10).AddRecipeGroup(RecipeGroupsForPets.Copper, 40).AddRecipeGroup(RecipeGroupID.IronBar, 15).AddRecipeGroup(RecipeGroupsForPets.Silver, 15).AddRecipeGroup(RecipeGroupsForPets.Gold, 15).AddIngredient(ItemID.HallowedBar, 4).AddIngredient(ItemID.SoulofMight, 10), 900);
            PetRecipe(Recipe.Create(ItemID.DirtiestBlock).AddIngredient(ItemID.DirtBlock, 25000));
            MasterModePetRecipe(Recipe.Create(ItemID.ResplendentDessert).AddRecipeGroup(RecipeGroupsForPets.Crowns, 2).AddIngredient(ItemID.Gel, 850).AddIngredient(ItemID.PinkGel, 100).AddIngredient(ItemID.GelBalloon, 100).AddIngredient(ItemID.Bubble, 25), 250, 2); //Is basically both Prince & Princess but slightly 'cheaper'
            PetRecipe(Recipe.Create(ItemID.BallOfFuseWire).AddIngredient(ItemID.Dynamite, 99).AddIngredient(ItemID.Wire, 99).AddCondition(Condition.BestiaryFilledPercent(70)), 300);
            MasterModePetRecipe(Recipe.Create(ItemID.EaterOfWorldsPetItem).AddIngredient(ItemID.MiningPotion).AddIngredient(ItemID.WormTooth, 20).AddIngredient(ItemID.WormFood).AddIngredient(ItemID.RottenChunk, 25), 100);
            PetRecipe(Recipe.Create(ItemID.CelestialWand).AddIngredient(ItemID.FallenStar, 99).AddRecipeGroup(RecipeGroupsForPets.Gold, 10), 1800);
            MasterModePetRecipe(Recipe.Create(ItemID.EverscreamPetItem).AddIngredient(ItemID.StarCloak).AddRecipeGroup(RecipeGroupID.Wood, 2500).AddIngredient(ItemID.FallenStar, 50).AddIngredient(ItemID.LeafWand).AddCondition(Condition.DownedEverscream), 2000);
            PetRecipe(Recipe.Create(ItemID.EyeSpring).AddIngredient(ItemID.BloodMoonStarter).AddIngredient(ItemID.ChumBucket, 35).AddIngredient(ItemID.BlackLens, 2).AddIngredient(ItemID.Lens, 30).AddCondition(Condition.Eclipse), 3000);
            PetRecipe(Recipe.Create(ItemID.ExoticEasternChewToy).AddIngredient(ItemID.AntlionMandible, 15).AddRecipeGroup(RecipeGroupID.Sand, 600).AddRecipeGroup(RecipeGroupsForPets.AllBugs, 7).AddRecipeGroup(RecipeGroupID.Birds, 3), 1500);
            PetRecipe(Recipe.Create(ItemID.BedazzledNectar).AddIngredient(ItemID.MonarchButterfly).AddIngredient(ItemID.SulphurButterfly).AddIngredient(ItemID.ZebraSwallowtailButterfly).AddIngredient(ItemID.UlyssesButterfly).AddIngredient(ItemID.HellButterfly).AddIngredient(ItemID.JuliaButterfly).AddIngredient(ItemID.RedAdmiralButterfly).AddIngredient(ItemID.PurpleEmperorButterfly).AddIngredient(ItemID.TreeNymphButterfly).AddIngredient(ItemID.GoldButterfly), 1750);
            PetRecipe(Recipe.Create(ItemID.GlommerPetItem).AddIngredient(ItemID.Marble, 500).AddIngredient(ItemID.PoopBlock, 250).AddIngredient(ItemID.PinkGel, 50).AddIngredient(ItemID.ManaFlower).AddIngredient(ItemID.SkyBlueFlower).AddIngredient(ItemID.AbigailsFlower).AddCondition(Condition.Hardmode), 750);
            PetRecipe(Recipe.Create(ItemID.DD2PetDragon).AddIngredient(ItemID.DefenderMedal, 10).AddRecipeGroup(RecipeGroupsForPets.Chests).AddIngredient(ItemID.HellstoneBar, 20).AddIngredient(ItemID.AshWood, 100).AddCondition(Condition.DownedOldOnesArmyAny), 200);
            MasterModePetRecipe(Recipe.Create(ItemID.QueenBeePetItem).AddIngredient(ItemID.ClayPot).AddIngredient(ItemID.BottledHoney, 100).AddIngredient(ItemID.Honeyfin, 10).AddIngredient(ItemID.BeeWax, 18).AddIngredient(ItemID.HoneyBlock, 40).AddIngredient(ItemID.CrispyHoneyBlock, 30).AddIngredient(ItemID.HoneyBucket), 100);
            MasterModePetRecipe(Recipe.Create(ItemID.IceQueenPetItem).AddRecipeGroup(RecipeGroupsForPets.IceBlocks, 2500).AddRecipeGroup(RecipeGroupsForPets.Crowns).AddIngredient(ItemID.Ectoplasm, 10).AddIngredient(ItemID.IceRod).AddCondition(Condition.DownedIceQueen), 1000);
            MasterModePetRecipe(Recipe.Create(ItemID.DD2BetsyPetItem).AddIngredient(ItemID.MeteoriteBar, 35).AddIngredient(ItemID.Fireblossom, 200).AddIngredient(ItemID.BetsyWings), 500);
            PetRecipe(Recipe.Create(ItemID.JunimoPetItem).AddIngredient(ItemID.StoneBlock, 100).AddIngredient(ItemID.Hay, 100).AddIngredient(ItemID.Bass, 10).AddIngredient(ItemID.JojaCola).AddCondition(Condition.DownedEarlygameBoss), 25);
            PetRecipe(Recipe.Create(ItemID.BirdieRattle).AddIngredient(ItemID.Feather, 45).AddIngredient(ItemID.Cloud, 100).AddIngredient(ItemID.RainCloud, 10), 1750);
            PetRecipe(Recipe.Create(ItemID.LizardEgg).AddIngredient(ItemID.BeetleHusk, 20).AddIngredient(ItemID.LunarTabletFragment, 15).AddIngredient(ItemID.LihzahrdBrick, 200).AddIngredient(ItemID.LizardEars).AddIngredient(ItemID.LizardTail), 5000);
            PetRecipe(Recipe.Create(ItemID.TartarSauce).AddRecipeGroup(RecipeGroupID.IronBar, 80).AddIngredient(ItemID.Chain, 30).AddIngredient(ItemID.Seashell, 2), 100);
            MasterModePetRecipe(Recipe.Create(ItemID.SkeletronPrimePetItem).AddIngredient(ItemID.Wire, 20).AddRecipeGroup(RecipeGroupsForPets.Copper, 5).AddRecipeGroup(RecipeGroupID.IronBar, 10).AddRecipeGroup(RecipeGroupsForPets.Silver, 5).AddRecipeGroup(RecipeGroupsForPets.Gold, 10).AddIngredient(ItemID.HallowedBar, 10).AddIngredient(ItemID.SoulofFright, 10), 900);
            MasterModePetRecipe(Recipe.Create(ItemID.MoonLordPetItem).AddIngredient(ItemID.LunarBar, 30).AddIngredient(ItemID.FragmentNebula, 20).AddIngredient(ItemID.FragmentSolar, 20).AddIngredient(ItemID.FragmentStardust, 20).AddIngredient(ItemID.FragmentVortex, 20), 5000);
            PetRecipe(Recipe.Create(ItemID.ParrotCracker).AddIngredient(ItemID.Feather, 99).AddIngredient(ItemID.EyePatch), 7000);
            MasterModePetRecipe(Recipe.Create(ItemID.LunaticCultistPetItem).AddIngredient(ItemID.SpellTome, 3).AddRecipeGroup(RecipeGroupsForPets.IceBlocks, 250).AddIngredient(ItemID.LivingFireBlock, 100).AddIngredient(ItemID.MartianConduitPlating, 100).AddIngredient(ItemID.Ectoplasm, 10).AddCondition(Condition.DownedCultist), 4000);
            PetRecipe(Recipe.Create(ItemID.PigPetItem).AddIngredient(ItemID.MonsterLasagna, 12).AddIngredient(ItemID.PiggyBank).AddIngredient(ItemID.GoldenDelight), 1000);
            MasterModePetRecipe(Recipe.Create(ItemID.PlanteraPetItem).AddIngredient(ItemID.Vine, 20).AddIngredient(ItemID.Stinger, 30).AddIngredient(ItemID.ChlorophyteBar, 30).AddIngredient(ItemID.JungleRose).AddCondition(Condition.DownedPlantera), 2000);
            PetRecipe(Recipe.Create(ItemID.MudBud).AddIngredient(ItemID.JungleSpores, 99).AddIngredient(ItemID.SpicyPepper).AddIngredient(ItemID.Hay, 10).AddCondition(Condition.DownedPlantera), 500);
            PetRecipe(Recipe.Create(ItemID.DD2PetGato).AddIngredient(ItemID.DefenderMedal, 10).AddIngredient(ItemID.Bass, 20).AddIngredient(ItemID.Trout, 12).AddIngredient(ItemID.Salmon, 8).AddIngredient(ItemID.RedSnapper, 6).AddCondition(Condition.DownedOldOnesArmyAny), 200);
            PetRecipe(Recipe.Create(ItemID.DogWhistle).AddIngredient(ItemID.Bone, 99).AddIngredient(ItemID.HunterPotion), 100);
            PetRecipe(Recipe.Create(ItemID.Seedling).AddIngredient(ItemID.ChlorophyteBar, 100).AddIngredient(ItemID.LifeFruit, 5).AddIngredient(ItemID.Acorn).AddCondition(Condition.DownedPlantera), 500);
            PetRecipe(Recipe.Create(ItemID.OrnateShadowKey).AddIngredient(ItemID.ShadowChest).AddIngredient(ItemID.ShadowKey).AddIngredient(ItemID.Obsidian, 50).AddIngredient(ItemID.GoldenKey).AddRecipeGroup(RecipeGroupsForPets.DemoniteBar, 12), 100);
            PetRecipe(Recipe.Create(ItemID.SharkBait).AddIngredient(ItemID.SharkFin, 10).AddIngredient(ItemID.WaterBucket).AddIngredient(ItemID.Seashell, 5), 10);
            MasterModePetRecipe(Recipe.Create(ItemID.SkeletronPetItem).AddIngredient(ItemID.Bone, 250).AddIngredient(ItemID.WaterCandle).AddIngredient(ItemID.MeteoriteBar, 15).AddIngredient(ItemID.Hook, 2), 600);
            MasterModePetRecipe(Recipe.Create(ItemID.KingSlimePetItem).AddRecipeGroup(RecipeGroupsForPets.Crowns).AddIngredient(ItemID.Gel, 550).AddIngredient(ItemID.SlimeBlock, 100), 50);
            MasterModePetRecipe(Recipe.Create(ItemID.QueenSlimePetItem).AddRecipeGroup(RecipeGroupsForPets.Crowns).AddIngredient(ItemID.Gel, 300).AddIngredient(ItemID.PinkGel, 115).AddIngredient(ItemID.GelBalloon, 100).AddIngredient(ItemID.Bubble, 25), 250);
            PetRecipe(Recipe.Create(ItemID.SpiderEgg).AddIngredient(ItemID.Cobweb, 400).AddIngredient(ItemID.SpiderFang, 20).AddIngredient(ItemID.VialofVenom, 20).AddCondition(Condition.DownedPumpking), 500);
            MasterModePetRecipe(Recipe.Create(ItemID.BrainOfCthulhuPetItem).AddIngredient(ItemID.RegenerationPotion).AddIngredient(ItemID.Glass, 30).AddIngredient(ItemID.LifeCrystal, 2).AddIngredient(ItemID.BloodySpine).AddIngredient(ItemID.Vertebrae, 25), 100);
            PetRecipe(Recipe.Create(ItemID.SpiffoPlush).AddIngredient(ItemID.ZombieArm).AddIngredient(ItemID.MusketBall, 25).AddIngredient(ItemID.AmmoReservationPotion).AddIngredient(ItemID.ZombieBanner).AddIngredient(ItemID.Shackle, 4), 400);
            PetRecipe(Recipe.Create(ItemID.MagicalPumpkinSeed).AddIngredient(ItemID.Pumpkin, 500).AddIngredient(ItemID.HerbBag), 300);
            PetRecipe(Recipe.Create(ItemID.EucaluptusSap).AddIngredient(ItemID.Umbrella).AddRecipeGroup(RecipeGroupID.Wood, 500).AddRecipeGroup(RecipeGroupID.Fruit, 3), 300);
            MasterModePetRecipe(Recipe.Create(ItemID.EyeOfCthulhuPetItem).AddIngredient(ItemID.Lens, 35).AddRecipeGroup(RecipeGroupsForPets.DemoniteBar, 15), 140);
            MasterModePetRecipe(Recipe.Create(ItemID.TwinsPetItem).AddIngredient(ItemID.Wire, 100).AddRecipeGroup(RecipeGroupsForPets.Copper, 10).AddRecipeGroup(RecipeGroupID.IronBar, 5).AddRecipeGroup(RecipeGroupsForPets.Silver, 10).AddRecipeGroup(RecipeGroupsForPets.Gold, 5).AddIngredient(ItemID.HallowedBar, 7).AddIngredient(ItemID.SoulofSight, 10), 900);
            PetRecipe(Recipe.Create(ItemID.TikiTotem).AddIngredient(ItemID.RichMahogany, 225).AddIngredient(ItemID.JungleSpores, 22).AddIngredient(ItemID.Vine, 3).AddIngredient(ItemID.SoulofLight, 12).AddIngredient(ItemID.SoulofNight, 12), 3000);
            MasterModePetRecipe(Recipe.Create(ItemID.DeerclopsPetItem).AddIngredient(ItemID.FlinxFur, 16).AddIngredient(ItemID.IceTorch, 99).AddIngredient(ItemID.SnowBlock, 250), 300);
            MasterModePetRecipe(Recipe.Create(ItemID.DukeFishronPetItem).AddIngredient(ItemID.Bacon, 6).AddIngredient(ItemID.TruffleWorm, 6).AddIngredient(ItemID.SharkFin, 6), 666);
            PetRecipe(Recipe.Create(ItemID.Seaweed).AddIngredient(ItemID.FishingSeaweed).AddIngredient(ItemID.Seashell, 10).AddIngredient(ItemID.ShellPileBlock, 4).AddIngredient(ItemID.ThornsPotion).AddIngredient(ItemID.JungleSpores, 12).AddRecipeGroup(RecipeGroupID.Sand, 250), 150);
            PetRecipe(Recipe.Create(ItemID.LightningCarrot).AddIngredient(ItemID.Bunny).AddIngredient(ItemID.Wire, 25).AddIngredient(ItemID.FallenStar, 25).AddIngredient(ItemID.RainCloud, 25).AddCondition(Condition.BestiaryFilledPercent(50)), 700);
            PetRecipe(Recipe.Create(ItemID.ZephyrFish).AddIngredient(ItemID.Cloud, 250).AddIngredient(ItemID.ApprenticeBait, 25).AddRecipeGroup(RecipeGroupsForPets.AllBugs, 5).AddCondition(Condition.AnglerQuestsFinishedOver(30)), 500);

            PetRecipe(Recipe.Create(ItemID.DD2PetGhost).AddIngredient(ItemID.DefenderMedal, 5).AddIngredient(ItemID.WaterCandle).AddIngredient(ItemID.SoulofNight, 4).AddIngredient(ItemID.SoulofLight, 4).AddIngredient(ItemID.Silk, 6), 70);
            PetRecipe(Recipe.Create(ItemID.CrimsonHeart).AddIngredient(ItemID.TissueSample, 4).AddIngredient(ItemID.CrimtaneBar, 3).AddIngredient(ItemID.ViciousMushroom, 2), 10);
            PetRecipe(Recipe.Create(ItemID.FairyBell).AddIngredient(ItemID.Bell).AddRecipeGroup(RecipeGroupsForPets.Fairies).AddIngredient(ItemID.SoulofSight, 5).AddIngredient(ItemID.PixieDust, 8), 80);
            PetRecipe(Recipe.Create(ItemID.PumpkingPetItem).AddIngredient(ItemID.SpookyCandle).AddIngredient(ItemID.SpookyWood, 50).AddIngredient(ItemID.Pumpkin, 30).AddCondition(PetConditions.ConsumedHead), 75);
            PetRecipe(Recipe.Create(ItemID.FairyQueenPetItem).AddIngredient(ItemID.SoulofFlight, 8).AddIngredient(ItemID.CrystalShard, 14).AddCondition(PetConditions.ConsumedOptic), 125);
            PetRecipe(Recipe.Create(ItemID.MagicLantern).AddIngredient(ItemID.Glass, 10).AddIngredient(ItemID.Blinkroot).AddIngredient(ItemID.Torch, 10).AddRecipeGroup(RecipeGroupsForPets.Copper, 2).AddRecipeGroup(RecipeGroupID.IronBar, 2).AddRecipeGroup(RecipeGroupsForPets.Silver, 2).AddRecipeGroup(RecipeGroupsForPets.Gold, 2).AddCondition(Condition.MoonPhaseFull).AddCondition(Condition.TimeNight), 175);
            PetRecipe(Recipe.Create(ItemID.ShadowOrb).AddIngredient(ItemID.ShadowScale, 4).AddIngredient(ItemID.DemoniteBar, 3).AddIngredient(ItemID.VileMushroom, 2), 10);
            PetRecipe(Recipe.Create(ItemID.SuspiciousLookingTentacle).AddIngredient(ItemID.FragmentNebula, 6).AddIngredient(ItemID.FragmentSolar, 6).AddIngredient(ItemID.FragmentStardust, 6).AddIngredient(ItemID.FragmentVortex, 6).AddIngredient(ItemID.SoulofFright, 3).AddIngredient(ItemID.SoulofMight, 3).AddIngredient(ItemID.SoulofSight, 3).AddCondition(Condition.DownedMoonLord), 400);
            PetRecipe(Recipe.Create(ItemID.GolemPetItem).AddIngredient(ItemID.LihzahrdBrick, 15).AddIngredient(ItemID.LunarTabletFragment).AddIngredient(ItemID.BeetleHusk, 3).AddIngredient(ItemID.Wire, 25).AddCondition(PetConditions.ConsumedWrench), 125);
            PetRecipe(Recipe.Create(ItemID.WispinaBottle).AddIngredient(ItemID.Ectoplasm, 8).AddIngredient(ItemID.Bottle).AddIngredient(ItemID.SoulofLight).AddIngredient(ItemID.SoulofNight), 150);

            MasterModePetRecipe(Recipe.Create(ModContent.ItemType<LihzahrdWrench>()).AddIngredient(ItemID.LihzahrdPowerCell).AddIngredient(ItemID.SoulofFright, 5).AddIngredient(ItemID.SoulofMight, 5).AddIngredient(ItemID.SoulofSight, 5).AddIngredient(ItemID.LihzahrdFurnace).AddCondition(Condition.DownedGolem), 50);
            MasterModePetRecipe(Recipe.Create(ModContent.ItemType<PrismaticOptic>()).AddIngredient(ItemID.Lens, 5).AddIngredient(ItemID.QueenSlimeCrystal).AddIngredient(ItemID.CrystalShard, 20).AddIngredient(ItemID.EmpressButterfly).AddIngredient(ItemID.PearlstoneBlock, 65).AddCondition(Condition.DownedEmpressOfLight), 50);
            MasterModePetRecipe(Recipe.Create(ModContent.ItemType<PumpkingsHead>()).AddIngredient(ItemID.SpookyWood, 50).AddIngredient(ItemID.Pumpkin, 50).AddIngredient(ItemID.PumpkingTrophy).AddIngredient(ItemID.Ectoplasm, 15).AddCondition(Condition.DownedPumpking), 50);

            PetFoodRecipe(Recipe.Create(ModContent.ItemType<PetFood>(), 40).AddIngredient(ItemID.LifeFruit));
            PetFoodRecipe(Recipe.Create(ModContent.ItemType<PetFood>()).AddRecipeGroup(RecipeGroupsForPets.Seeds, 16));
            PetFoodRecipe(Recipe.Create(ModContent.ItemType<PetFood>()).AddIngredient(ItemID.Hay, 75));
            PetFoodRecipe(Recipe.Create(ModContent.ItemType<PetFood>()).AddIngredient(ItemID.BambooBlock, 30));
            PetFoodRecipe(Recipe.Create(ModContent.ItemType<PetFood>()).AddRecipeGroup(RecipeGroupsForPets.Herbs, 7).AddIngredient(ItemID.Mushroom, 3));
            PetFoodRecipe(Recipe.Create(ModContent.ItemType<PetFood>()).AddIngredient(ItemID.Bone, 15));
            PetFoodRecipe(Recipe.Create(ModContent.ItemType<PetFood>()).AddRecipeGroup(RecipeGroupsForPets.Herbs).AddRecipeGroup(RecipeGroupsForPets.Seeds).AddIngredient(ItemID.GlowingMushroom, 10));
            PetFoodRecipe(Recipe.Create(ModContent.ItemType<PetFood>(), 300).AddRecipeGroup(RecipeGroupsForPets.GoldenAnimals));
            PetFoodRecipe(Recipe.Create(ModContent.ItemType<PetFood>(), 250).AddIngredient(ItemID.TruffleWorm));
            PetFoodRecipe(Recipe.Create(ModContent.ItemType<PetFood>(), 5).AddRecipeGroup(RecipeGroupsForPets.Animals));
            PetFoodRecipe(Recipe.Create(ModContent.ItemType<PetFood>(), 5).AddRecipeGroup(RecipeGroupID.Fruit));
            PetFoodRecipe(Recipe.Create(ModContent.ItemType<PetFood>(), 5).AddIngredient(ItemID.CookedFish, 4));
            PetFoodRecipe(Recipe.Create(ModContent.ItemType<PetFood>(), 4).AddIngredient(ItemID.CookedShrimp));
            PetFoodRecipe(Recipe.Create(ModContent.ItemType<PetFood>(), 10).AddIngredient(ItemID.Sashimi, 7));
            PetFoodRecipe(Recipe.Create(ModContent.ItemType<PetFood>(), 5).AddIngredient(ItemID.SeafoodDinner));
            PetFoodRecipe(Recipe.Create(ModContent.ItemType<PetFood>(), 5).AddIngredient(ItemID.LobsterTail));
            PetFoodRecipe(Recipe.Create(ModContent.ItemType<PetFood>(), 5).AddIngredient(ItemID.ShuckedOyster));
        }
    }

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
            RecipeGroup.RegisterGroup("AnySilverBar", Silver); //For whatever reason recipe group "SilverBar" exists, but vanilla doesn't seem to actually have it? So we have to name it AnySilverBar.
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
            Herbs = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {PetUtils.LocVal("Misc.Herb")}", [.. herbs]);
            RecipeGroup.RegisterGroup(nameof(ItemID.Daybloom), Herbs);
            Seeds = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {PetUtils.LocVal("Misc.Herb")} {Language.GetTextValue("RandomWorldName_Noun.Seeds")}", [.. seeds]);
            RecipeGroup.RegisterGroup(nameof(ItemID.DaybloomSeeds), Seeds);
            Animals = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {PetUtils.LocVal("Misc.Animal")}", [.. animals]);
            RecipeGroup.RegisterGroup(nameof(ItemID.Bunny), Animals);
            GoldenAnimals = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("RandomWorldName_Adjective.Golden")} {PetUtils.LocVal("Misc.Animal")}", [.. goldenAnimals]);
            RecipeGroup.RegisterGroup(nameof(ItemID.GoldBunny), GoldenAnimals);
        }
    }
}
