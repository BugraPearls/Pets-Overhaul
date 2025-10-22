using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Systems
{
    /// <summary>
    /// Class used by Pets Overhaul to do something to all Pet Items with ease.
    /// </summary>
    [ReinitializeDuringResizeArrays]
    public static class PetItemIDs
    {
        public static bool[] TownPetBuffIDs = BuffID.Sets.Factory.CreateNamedSet("TownPetBuffs").Description("List of Town Pet Buffs added by Pets Overhaul. Buffs here will be removed upon obtaining a new Town Pet Buff.")
            .RegisterBoolSet(false); //We are not assigning here, as ALL TownPetBuffs has this set to true for their type by default.

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
    }
}
