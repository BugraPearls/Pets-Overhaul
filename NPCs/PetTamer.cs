using Microsoft.Xna.Framework;
using PetsOverhaul.Items;
using PetsOverhaul.NPCs.Gores;
using PetsOverhaul.Systems;
using PetsOverhaul.Tiles;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace PetsOverhaul.NPCs
{
    //Refer to ExamplePerson if needed.
    // [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
    [AutoloadHead]
    public class PetTamer : ModNPC
    {
        public static bool openLightCombineMenu = false;
        //private static int ShimmerHeadIndex;
        private static Profiles.StackedNPCProfile NPCProfile;

        //public override void Load()
        //{
        //    // Adds our Shimmer Head to the NPCHeadLoader.
        //    ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, Texture + "_Shimmer_Head");
        //}

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 25; // The total amount of frames the NPC has

            NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
            NPCID.Sets.AttackFrameCount[Type] = 4; // The amount of frames in the attacking animation.
            NPCID.Sets.DangerDetectRange[Type] = 800; // The amount of pixels away from the center of the NPC that it tries to attack enemies.
            NPCID.Sets.AttackType[Type] = 0; // The type of attack the Town NPC performs. 0 = throwing, 1 = shooting, 2 = magic, 3 = melee
            NPCID.Sets.AttackTime[Type] = 20; // The amount of time it takes for the NPC's attack animation to be over once it starts.
            NPCID.Sets.AttackAverageChance[Type] = 10; // The denominator for the chance for a Town NPC to attack. Lower numbers make the Town NPC appear more aggressive.
            NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.
            //NPCID.Sets.ShimmerTownTransform[NPC.type] = true; // This set says that the Town NPC has a Shimmered form. Otherwise, the Town NPC will become transparent when touching Shimmer like other enemies.

            //NPCID.Sets.ShimmerTownTransform[Type] = true; // Allows for this NPC to have a different texture after touching the Shimmer liquid.

            //// Connects this NPC with a custom emote.
            //// This makes it when the NPC is in the world, other NPCs will "talk about him".
            //// By setting this you don't have to override the PickEmote method for the emote to appear.
            //NPCID.Sets.FaceEmote[Type] = ModContent.EmoteBubbleType<ExamplePersonEmote>();

            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
                Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
                              // Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
                              // If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            // Set Example Person's biome and neighbor preferences with the NPCHappiness hook. You can add happiness text and remarks with localization (See an example in ExampleMod/Localization/en-US.lang).
            // NOTE: The following code uses chaining - a style that works due to the fact that the SetXAffection methods return the same NPCHappiness instance they're called on.
            NPC.Happiness
                .SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
                .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Love)
                .SetNPCAffection(NPCID.Pirate, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Truffle, AffectionLevel.Like)
                .SetNPCAffection(NPCID.DyeTrader, AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.Painter, AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.ArmsDealer, AffectionLevel.Hate);


            //// This creates a "profile" for ExamplePerson, which allows for different textures during a party and/or while the NPC is shimmered.
            //NPCProfile = new Profiles.StackedNPCProfile(
            //    new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture), Texture + "_Party")
            //    //new Profiles.DefaultNPCProfile(Texture + "_Shimmer", ShimmerHeadIndex, Texture + "_Shimmer_Party")
            //);
        }
        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;

            AnimationType = NPCID.Guide;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.PetsOverhaul.NPCs.PetTamer.BestiaryFlavorText")),
            });
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            int num = NPC.life > 0 ? 1 : 5;

            for (int k = 0; k < num; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood);
            }

            // Create gore when the NPC is killed.
            if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
            {
                //string variant = "";
                //if (NPC.IsShimmerVariant) variant += "_Shimmer";
                //if (NPC.altTexture == 1) variant += "_Party";
                int hatGore = NPC.GetPartyHatGore();
                //int headGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Head").Type;
                //int armGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Arm").Type;
                //int legGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Leg").Type;

                int armGore = ModContent.GoreType<PetTamerArmGore>();
                int legGore = ModContent.GoreType<PetTamerLegGore>();
                int headGore = ModContent.GoreType<PetTamerHeadGore>();
                // Spawn the gores. The positions of the arms and legs are lowered for a more natural look.
                if (hatGore > 0)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, hatGore);
                }


                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, headGore, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
            }
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            return PetObtainedCondition.petIsObtained;
        }

        public override ITownNPCProfile TownNPCProfile()
        {
            return NPCProfile;
        }

        public override List<string> SetNPCNameList()
        {
            return [
                "Gumshoe", //I like ace attorney and he has a dog. 
                "Kled", //I like Kled from league of legends & has a lizard & elderly.
                "Lance", //pokemon1 & cape & he looks like a Lance??
                "Alder", //pokemon2 & elder
                "Leon", //pokemon3 & cape lol
                "Fuji", //pokemon4 & Cubone reference & elderly
                //Idk please help me with names
            ];
        }

        public override string GetChat()
        {
            WeightedRandom<string> chat = new WeightedRandom<string>();

            chat.Add(Language.GetTextValue("Mods.PetsOverhaul.NPCs.PetTamer.Quotes.Common1"), 10);
            chat.Add(Language.GetTextValue("Mods.PetsOverhaul.NPCs.PetTamer.Quotes.Common2"), 10);
            chat.Add(Language.GetTextValue("Mods.PetsOverhaul.NPCs.PetTamer.Quotes.Common3"), 10);
            chat.Add(Language.GetTextValue("Mods.PetsOverhaul.NPCs.PetTamer.Quotes.Common4"), 10);
            chat.Add(Language.GetTextValue("Mods.PetsOverhaul.NPCs.PetTamer.Quotes.Common5"), 10);
            chat.Add(Language.GetTextValue("Mods.PetsOverhaul.NPCs.PetTamer.Quotes.Common6"), 10);
            chat.Add(Language.GetTextValue("Mods.PetsOverhaul.NPCs.PetTamer.Quotes.Rare1"), 1);

            int wiz = NPC.FindFirstNPC(NPCID.Wizard);
            if (wiz != -1)
            {
                chat.Add(Language.GetTextValue("Mods.PetsOverhaul.NPCs.PetTamer.Quotes.WizardQuote").Replace("<name>", Main.npc[wiz].FullName), 15);
            }
            string chosenChat = chat; // chat is implicitly cast to a string. This is where the random choice is made.

            return chosenChat;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        { // What the chat buttons are when you open up the chat UI
            button = Language.GetTextValue("LegacyInterface.28");
            button2 = Language.GetTextValue("Mods.PetsOverhaul.NPCs.PetTamer.Combine");
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            if (firstButton)
            {
                shop = shop1; // Name of the shop tab we want to open.
            }
            else
            {
                Main.npcChatText = "";
                Main.playerInventory = true;
                openLightCombineMenu = true;
            }
        }
        private static readonly string shop1 = "Shop";
        public override void AddShops()
        {
            var npcShop = new NPCShop(Type, shop1)
            .Add<PetMonitoringTablet>()
            .Add<PetForgeItem>()
            .Add<PetFood>()
            ;
            npcShop.Register(); // Name of this shop tab
        }


        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Bonemerang>()));
        }

        // Make this Town NPC teleport to the King and/or Queen statue when triggered. Return toKingStatue for only King Statues. Return !toKingStatue for only Queen Statues. Return true for both.
        public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;
        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileID.Bone;
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 10f;
            randomOffset = 1.3f;
        }
    }
}