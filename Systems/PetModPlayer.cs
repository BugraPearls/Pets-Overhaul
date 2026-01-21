using Microsoft.Xna.Framework;
using PetsOverhaul.Achievements;
using PetsOverhaul.Buffs;
using PetsOverhaul.Config;
using PetsOverhaul.Items;
using PetsOverhaul.NPCs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.Systems
{
    /// <summary>
    /// ModPlayer class that contains many useful Methods and fields for Pet implementation, this is the central class of Pets Overhaul.
    /// </summary>
    public sealed class PetModPlayer : ModPlayer
    {
        public static InputMode PlayerInputMode => PlayerInput.CurrentProfile.InputModes.ContainsKey(InputMode.Keyboard) ? InputMode.Keyboard : InputMode.XBoxGamepad;
        public static bool eolConsumed = false;
        public static bool golemConsumed = false;
        public static bool pumpkingConsumed = false;

        public bool petObtained = false;
        /// <summary>
        /// Modify this value if you want to reduce or increase lifesteal & healing by Pets for any reason, such as a Mod applying an effect that reduces healings. Basically a modifier on heals from Pets. Used in PetRecovery().
        /// </summary>
        public float petHealMultiplier = 1f;
        /// <summary>
        /// Modify this value if you want to reduce or increase shields applied by Pets for any reason, such as increasing Shield gain with a condition. Basically a modifier on shields coming from Pets. Used in AddShield().
        /// </summary>
        public float petShieldMultiplier = 1f;
        /// <summary>
        /// Modify this value if you want to reduce or increase direct damages dealt by Pets, Pets that directly strikes NPC's with SimpleStrikeNPC and Pets that creates a Projectile uses this.
        /// </summary>
        public float petDirectDamageMultiplier = 1f;
        /// <summary>
        /// Influences the chance to increase stack of the item from your pet that doesn't fit into any other fortune category. This also increases all other fortunes with half effectiveness.
        /// </summary>
        public int globalFortune = 0;
        /// <summary>
        /// Influences the chance to increase stack of the item that your Harvesting Pet gave.
        /// </summary>
        public int harvestingFortune = 0;
        /// <summary>
        /// Influences the chance to increase stack of the item that your Mining Pet gave.
        /// </summary>
        public int miningFortune = 0;
        /// <summary>
        /// Influences the chance to increase stack of the item that your Fishing Pet gave.
        /// </summary>
        public int fishingFortune = 0;
        public Color skin;
        public bool skinColorChanged = false;

        /// <summary>
        /// Ran and is reset at end of PostUpdate().
        /// </summary>
        public static List<Point16> CoordsToRemove = [];

        /// <summary>
        /// Ran and is reset at end of PostUpdate().
        /// </summary>
        public static List<Point16> updateReplacedTile = [];

        /// <summary>
        /// shieldTimer is reduced in PostUpdate(), so if you want to do a '1 frame passive shield', make the timer 2 instead of 1.
        /// </summary>
        public List<(int shieldAmount, int shieldTimer)> petShield = [];
        public int currentShield = 0;
        public int shieldToBeReduced = 0;
        public bool jumpRegistered = false;
        public int petSwapCooldown = 600;
        internal int previousPetItem = 0;
        /// <summary>
        /// This field ticks down every frame in PreUpdate() hook. Does not go below -1. Plays 'cooldown refreshment' sound effect upon reaching 0 and displays Timer while higher than 0. Usually is recommended to use Mod's timer mechanic for timers that the Player should be aware of.
        /// </summary>
        public int timer = -1;
        /// <summary>
        /// Use this field to set how long the Pet's cooldown will be.
        /// </summary>
        public int timerMax = 0;
        /// <summary>
        /// Modified in PetEffect code, used by UI to show current stacks of a Pet
        /// </summary>
        public int currentPetStacks = -1;
        /// <summary>
        /// Modified in PetEffect code, used by UI to show how much current stacks of a Pet can go up to
        /// </summary>
        public int currentPetStacksMax = -1;
        /// <summary>
        /// Modified in PetEffect code, used by UI to 'tell' what current stack values are
        /// </summary>
        public string currentPetStackText = string.Empty;
        /// <summary>
        /// Modified in PetEffect code, used by UI to write at the end, replacing the 'current stack' if the current value to display needs to display anything except an integer.
        /// </summary>
        public string currentPetStackSpecialText = string.Empty;
        /// <summary>
        /// Used to not play the 'refresh sound' upon Pet Ability timer going below 0 more than once.
        /// </summary>
        public bool AbilityCdSoundPlayed { get; private set; }
        /// <summary>
        /// Pets that wants to initiate effects only in Combat uses this. This is triggered to be set to inCombatTimerMax upon getting directly Hurt, having negative Life Regen (being under DoT effects) or Dealing damage. Buffs on enemies does not trigger this.
        /// </summary>
        public int inCombatTimer = 0;
        /// <summary>
        /// Reset back to 300 in ResetEffects(), if Pet wants to change in combat timer to be something Lower or Higher, set it conditionally in any Updates, somewhere after ResetEffects(). Check BabyFaceMonster to how to implement different out of combat cooldowns.
        /// </summary>
        public int inCombatTimerMax = 300;
        /// <summary>
        /// Increase this value to reduce ability cooldowns. Eg. 0.1f increases how fast ability will return by 10%. Increases potency of ticks going down every frame. Ex: 200% ability haste will make it tick 3 frames every frame, while 50% will make it tick 1 or 2 frames every frame randomly with equal chance. Can be negative; affecting the tick with diminishing returns.
        /// </summary>
        public float abilityHaste = 0;
        /// <summary>
        /// Used to change alternating color of maximum Light Pet Rolls alongside colorSwitched, increases 0.01f every frame, until hitting 1f, where it decreases 0.01f every frame and so on.
        /// </summary>
        public static float ColorVal { get; internal set; }

        /// <summary>
        /// Used to change alternating color of maximum Light Pet Rolls alongside colorVal
        /// </summary>
        private static bool colorSwitched = false;

        /// <summary>
        /// Name of last Town Pet you've received buffs from.
        /// </summary>
        public static string LastTownPet = string.Empty;

        /// <summary>
        /// whoAmI of Pet Tamer who opened their Light Pet combination menu. Menu closes if Player gets away from them or this value is negative.
        /// </summary>
        public int LastPetTamer = -1;

        /// <summary>
        /// Whether or not current switched tooltip is simplified or not.
        /// </summary>
        public static bool CurrentTooltipIsSimple = true;

        /// <summary>
        /// This is instance of current active Pet effect, lots of data can be accessed here.
        /// </summary>
        public PetEffect currentActivePet = null;

        #region Achievement Fields
        public List<int> FoundPets = new(PetIDs.PetNamesAndItems.Count);
        public List<int> FoundLightPets = new(PetIDs.LightPetNamesAndItems.Count);
        public List<int> PettedTownPets = new(PetIDs.TownPetBuffs.Count);

        #endregion

        /// <summary>
        /// Is invoked at PetNpc, OnKill hook, refer to Puppy Pet's OnEnemyKill() & Load/Unload to figure how its used properly. Only called on Player that last hit the NPC, when NPC is dead.
        /// </summary>
        public static Action<NPC, Player> OnEnemyDeath;

        #region Utility related methods, that directly uses Player fields. For static methods, see PetUtils.cs.

        /// <summary>
        /// Runs all of the standart OnPickup's checks for the Pet to work with no problems. Try to always use this for Gathering Pets that works on-pickup.
        /// </summary>
        public bool PickupChecks(Item item, int petitemid, out PetGlobalItem itemPet)
        {
            if (PetInUse(petitemid) && Player.CanPullItem(item, Player.ItemSpace(item)) && item.TryGetGlobalItem(out PetGlobalItem petItemCheck) && petItemCheck.pickedUpBefore == false && petItemCheck.doNotBenefitFromThisItem == false)
            {
                itemPet = petItemCheck;
                return true;
            }
            itemPet = null;
            return false;
        }
        /// <summary>
        /// Spawns coins accordingly to the given value and converts it to a higher coin tier if possible. Source of spawned coins will be globalItem. Recommended use is with GlobalPet.Randomizer() to achieve more precise and 'natural' values. (100x the intended coin value)
        /// </summary>
        public void GiveCoins(int coinAmount)
        {
            if (coinAmount > 1000000)
            {
                Player.QuickSpawnItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.GlobalItem), ItemID.PlatinumCoin, coinAmount / 1000000);
                coinAmount %= 1000000;
            }
            if (coinAmount > 10000)
            {
                Player.QuickSpawnItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.GlobalItem), ItemID.GoldCoin, coinAmount / 10000);
                coinAmount %= 10000;
            }
            if (coinAmount > 100)
            {
                Player.QuickSpawnItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.GlobalItem), ItemID.SilverCoin, coinAmount / 100);
                coinAmount %= 100;
            }
            if (coinAmount > 0)
            {
                Player.QuickSpawnItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.GlobalItem), ItemID.CopperCoin, coinAmount);
            }
        }

        /// <summary>
        /// Spawns an item on the Player instance with given <see cref="EntitySourcePetIDs"/>, and with the given stack. More than once is spawned if item is unstackable. Try to always use <see cref="PetUtils.Randomizer(int, int)"/> for <paramref name="howMany"/> parameter!! Will safely skip the item spawning if howMany is 0.
        /// </summary>
        /// <param name="sourceID"><see cref="EntitySourcePetIDs"/> value to be passed as source when spawning the item.</param>
        /// <param name="ItemID">ID of the item.</param>
        /// <param name="howMany">How many times to do it. Always use the <see cref="PetUtils.Randomizer(int, int)"/> method to pass through a more consistent amount.</param>
        public int SpawnItemSourcingFromPet(EntitySourcePetIDs sourceID, int ItemId, int howMany = 1)
        {
            if (howMany <= 0)
            {
                return 0;
            }
            if (ContentSamples.ItemsByType[ItemId].maxStack > 1)
            {
                return Player.QuickSpawnItem(PetUtils.GetSource_Pet(sourceID), ItemId, howMany);
            }
            else
            {
                int id = 0;
                for (int i = 0; i < howMany; i++)
                {
                    id = Player.QuickSpawnItem(PetUtils.GetSource_Pet(sourceID), ItemId, 1);
                }
                return id;
            }
        }
        public static void PreOnPickup(Item item, Player player)
        {
            PetModPlayer PickerPet = player.PetPlayer();
            if (item.TryGetGlobalItem(out PetGlobalItem fortune) && fortune.pickedUpBefore == false && player.CanPullItem(item, player.ItemSpace(item)) && fortune.doNotBenefitFromThisItem == false)
            {
                if (fortune.globalDropFromPet)
                {
                    int count = PetUtils.Randomizer(PickerPet.globalFortune * item.stack);
                    PickerPet.SpawnItemSourcingFromPet(EntitySourcePetIDs.GlobalFortuneItem, item.type, count);
                    PetUtils.DoAchievementOnPlayer<UnpaidLabor>(player.whoAmI, item.value * (item.stack + count)); //So we increment the achievement for both the normal item drop (item.stack) from the Pet, and from Fortune stats thats buffing up the Pet drops.
                }

                if (fortune.harvestingDropFromPet)
                {
                    int count = PetUtils.Randomizer((PickerPet.globalFortune * 10 / 2 + PickerPet.harvestingFortune * 10) * item.stack, 1000);
                    PickerPet.SpawnItemSourcingFromPet(EntitySourcePetIDs.HarvestingFortuneItem, item.type, count);
                    PetUtils.DoAchievementOnPlayer<UnpaidLabor>(player.whoAmI, item.value * (item.stack + count));
                }

                if (fortune.miningDropFromPet)
                {
                    int count = PetUtils.Randomizer((PickerPet.globalFortune * 10 / 2 + PickerPet.miningFortune * 10) * item.stack, 1000);
                    PickerPet.SpawnItemSourcingFromPet(EntitySourcePetIDs.MiningFortuneItem, item.type, count);
                    PetUtils.DoAchievementOnPlayer<UnpaidLabor>(player.whoAmI, item.value * (item.stack + count));
                }

                if (fortune.fishingDropFromPet)
                {
                    int count = PetUtils.Randomizer((PickerPet.globalFortune * 10 / 2 + PickerPet.fishingFortune) * item.stack, 1000);
                    PickerPet.SpawnItemSourcingFromPet(EntitySourcePetIDs.FishingFortuneItem, item.type, count);
                    PetUtils.DoAchievementOnPlayer<UnpaidLabor>(player.whoAmI, item.value * (item.stack + count));
                }

                //These are extra increases of Fortune stats. Ones above directly increases drops by Pets. Does not increment achievement. Fishing is down at ModifyCaughtFish.
                if (fortune.herbBoost)
                {
                    PickerPet.SpawnItemSourcingFromPet(EntitySourcePetIDs.HarvestingFortuneItem, item.type, PetUtils.Randomizer((PickerPet.globalFortune + PickerPet.harvestingFortune) * 10 / 2 * item.stack, 1000));
                }

                if (fortune.oreBoost)
                {
                    PickerPet.SpawnItemSourcingFromPet(EntitySourcePetIDs.MiningFortuneItem, item.type, PetUtils.Randomizer((PickerPet.globalFortune + PickerPet.miningFortune) * 10 / 2 * item.stack, 1000));
                }
            }
        }

        /// <summary>
        /// Checks if the given Pet Item is in use and checks if pet has been lately swapped or not.
        /// </summary>
        public bool PetInUseWithSwapCd(int petItemID)
        {
            return PetInUse(petItemID) && Player.HasBuff(ModContent.BuffType<ObliviousPet>()) == false;
        }
        /// <summary>
        /// Checks if the given Pet Item is in use without being affected by swapping cooldown.
        /// </summary>
        public bool PetInUse(int petItemID)
        {
            return Player.CurrentPet() == petItemID;
        }

        public bool AbilityPressCheck()
        {
            return Player.dead == false && timer <= 0 && PetKeybinds.UsePetAbility.JustPressed;
        }

        /// <summary>
        /// Adds to petShield list and applies petShieldMultiplier. Does not allow for values lower than 1 to be added.
        /// </summary>
        /// <param name="shieldAmount">Pet Shield to be added to Player.</param>
        /// <param name="shieldDuration">Duration of this individual Shield on the Player.</param>
        /// <param name="applyPetShieldMult">Set to false to prevent from petShieldMultiplier from being applied.</param>
        /// <returns>Value of the added shield, -1 if failed to add.</returns>
        public int AddShield(int shieldAmount, int shieldDuration, bool applyPetShieldMult = true)
        {
            int shield = shieldAmount;
            if (applyPetShieldMult)
            {
                shield = (int)Math.Round(shield * petShieldMultiplier);
            }
            if (shield > 0 && shieldDuration > 0)
            {
                petShield.Add((shield, shieldDuration));
                return shield;
            }
            return -1;
        }
        /// <summary>
        /// Use this to properly scale Pet sourced damage values with the given damage class, Pet damage multiplier and a check to prevent this being lower than 1.
        /// </summary>
        /// <param name="damage">Base damage value</param>
        /// <param name="damageClass">Class that this damage should get damage bonuses from</param>
        public int PetDamage(float damage, DamageClass damageClass)
        {
            damage = Player.GetTotalDamage(damageClass).ApplyTo(damage);
            damage *= petDirectDamageMultiplier;
            return (int)Math.Max(damage, 1);

        }
        /// <summary>
        /// Used for Healing and Mana recovery purposes. Non converted amount can still grant +1, depending on a roll. Example: PetRecovery(215, 0.05f) will heal you for 10 health and 75% chance to heal +1 more, resulting in 11 health recovery.
        /// </summary>
        /// <param name="baseAmount">Base amount of value to be recovered</param>
        /// <param name="percentageAmount">Percentage of baseAmount to be converted to recovery.</param>
        /// <param name="flatIncrease">Amount to be included that will not go through any calculations and complications.</param>
        /// <param name="manaSteal">Whether or not if it will recover health or mana. petHealMultiplier and Moon Leech debuff will be disabled if set to True.</param>
        /// <param name="isLifesteal">Should be set to false if this is not a Life Steal, it won't use vanilla Life steal cap, won't be affected by Moon Leech debuff and won't modify player.lifeSteal if set to false.</param>
        /// <param name="doHeal">Should be set to false if intended to simply return a value but not do anything at all. Such as if its intended to check how much this method will be healing.</param>
        /// <returns>Returns amount calculated, irrelevant to Player's health cap, or the lifeSteal cap etc.</returns>
        public int PetRecovery(double baseAmount, float percentageAmount, int flatIncrease = 0, bool manaSteal = false, bool isLifesteal = true, bool doHeal = true)
        {
            double num;
            if (manaSteal)
            {
                num = baseAmount * percentageAmount;
            }
            else
            {
                num = baseAmount * petHealMultiplier * percentageAmount;
                if (isLifesteal && Player.HasBuff(BuffID.MoonLeech))
                {
                    num *= 0.33f;
                }
            }

            int calculatedAmount = (int)num;
            if (Main.rand.NextFloat(0, 1) < num % 1)
            {
                calculatedAmount++;
            }

            calculatedAmount += flatIncrease;
            num = calculatedAmount;
            if (doHeal == true && calculatedAmount > 0 && Main.myPlayer == Player.whoAmI)
            {
                if (manaSteal == false)
                {
                    int healEff = calculatedAmount;
                    if (calculatedAmount > Player.statLifeMax2 - Player.statLife)
                    {
                        calculatedAmount = Player.statLifeMax2 - Player.statLife;
                    }

                    if (isLifesteal == true)
                    {
                        if (calculatedAmount > Player.lifeSteal)
                        {
                            calculatedAmount = (int)Player.lifeSteal;
                        }

                        if (calculatedAmount > 0)
                        {
                            Player.statLife += calculatedAmount;
                            Player.lifeSteal -= calculatedAmount;
                            Player.HealEffect(calculatedAmount);
                        }
                    }
                    else
                    {
                        Player.statLife += calculatedAmount;
                        Player.HealEffect(healEff);
                    }

                    PetUtils.DoAchievementOnPlayer<SagePet>(Player.whoAmI, calculatedAmount);
                }
                else
                {
                    Player.ManaEffect(calculatedAmount);
                    if (calculatedAmount > Player.statManaMax2 - Player.statMana)
                    {
                        calculatedAmount = Player.statManaMax2 - Player.statMana;
                    }
                    Player.statMana += calculatedAmount;
                }
            }

            return (int)num;
        }
        public static void HandleShieldBlockMessage(BinaryReader reader, int whoAmI, int damageAmount)
        {
            int player = reader.ReadByte();
            if (Main.netMode == NetmodeID.Server)
            {
                player = whoAmI;
            }

            Main.player[player].PetPlayer().ShieldFullBlockEffect(damageAmount);

            if (Main.netMode == NetmodeID.Server)
            {
                SendShieldBlockToServer(player, damageAmount);
            }
        }
        public static void SendShieldBlockToServer(int whoAmI, int dmgAmount)
        {
            ModPacket packet = ModContent.GetInstance<PetsOverhaul>().GetPacket();
            packet.Write((byte)MessageType.ShieldFullAbsorb);
            packet.Write(dmgAmount);
            packet.Write((byte)whoAmI);
            packet.Send(ignoreClient: whoAmI);
        }
        public void ShieldFullBlockEffect(int damage)
        {
            CombatText.NewText(Player.Hitbox, Color.Cyan, -damage, true);
            if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath43 with { PitchVariance = 0.4f, Pitch = -0.8f, Volume = 0.2f }, Player.Center);
            }
            if (damage <= 1)
            {
                Player.SetImmuneTimeForAllTypes(Player.longInvince ? 40 : 20);
            }
            else
            {
                Player.SetImmuneTimeForAllTypes(Player.longInvince ? 80 : 40);
            }
            if (Player.whoAmI == Main.myPlayer)
            {
                shieldToBeReduced += damage;
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    SendShieldBlockToServer(Player.whoAmI, damage);
                }
            }
        }
        #endregion

        #region ModPlayer Overrides
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
            On_Player.DoBootsEffect_PlaceFlowersOnTile += GrassIsPlacedByPlayer;
            On_Player.ItemCheck_CutTiles += CutGrassIsRemovedFromList;
        }

        private static void CutGrassIsRemovedFromList(On_Player.orig_ItemCheck_CutTiles orig, Player self, Item sItem, Rectangle itemRectangle, bool[] shouldIgnore) //This is bugged for multiplayer idk cant fix
        {
            int minX = itemRectangle.X / 16;
            int maxX = (itemRectangle.X + itemRectangle.Width) / 16 + 1;
            int minY = itemRectangle.Y / 16;
            int maxY = (itemRectangle.Y + itemRectangle.Height) / 16 + 1;
            Utils.ClampWithinWorld(ref minX, ref minY, ref maxX, ref maxY);
            for (int i = minX; i < maxX; i++)
            {
                for (int j = minY; j < maxY; j++)
                {
                    if (Main.tile[i, j] == null || !Main.tileCut[Main.tile[i, j].TileType] || shouldIgnore[Main.tile[i, j].TileType] || !WorldGen.CanCutTile(i, j, TileCuttingContext.AttackMelee))
                    {
                        continue;
                    }
                    if (sItem.type == ItemID.Sickle)
                    {
                        ushort type = Main.tile[i, j].TileType;
                        WorldGen.KillTile(i, j);
                        if (!Main.tile[i, j].HasTile)
                        {
                            int num = 0;
                            switch (type)
                            {
                                case 3:
                                case 24:
                                case 61:
                                case 110:
                                case 201:
                                case 529:
                                case 637:
                                    num = Main.rand.Next(1, 3);
                                    break;
                                case 73:
                                case 74:
                                case 113:
                                    num = Main.rand.Next(2, 5);
                                    break;
                            }
                            if (num > 0)
                            {
                                num *= self.miscEquips[0].type == ItemID.GlowTulip ? 2 : 1; //Double gathered Hay don't matter if its 'placed by player' or not.
                                int number = Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 1727, num);
                                if (Main.netMode == NetmodeID.MultiplayerClient)
                                {
                                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 1f);
                                }
                                PetGlobalTile.RemoveFromList(i, j);
                            }
                        }
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j);
                        }
                    }
                    else
                    {
                        WorldGen.KillTile(i, j);
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j);
                        }
                        PetGlobalTile.RemoveFromList(i, j);
                    }
                }
            }
        }
        public static bool GrassIsPlacedByPlayer(On_Player.orig_DoBootsEffect_PlaceFlowersOnTile orig, Player self, int X, int Y)
        {
            bool PlacedFlower = orig(self, X, Y);
            if (PlacedFlower)
            {
                PetGlobalTile.AddToList(X, Y);
            }
            return PlacedFlower;
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("CurrentTooltip", CurrentTooltipIsSimple);
            tag.Add("LastTownPet", LastTownPet);
            tag.Add("SkinColor", skin);
            tag.Add("SkinColorChanged", skinColorChanged);
            tag.Add("eolConsume", eolConsumed);
            tag.Add("golemConsume", golemConsumed);
            tag.Add("pumpkingConsume", pumpkingConsumed);
            tag.Add("hasPet", petObtained);
            tag.Add("foundPets", FoundPets);
            tag.Add("foundLightPets", FoundLightPets);
            tag.Add("pettedTownPets", PettedTownPets);
        }
        public override void LoadData(TagCompound tag)
        {
            if (tag.TryGet("CurrentTooltip", out bool stateOfTooltip))
            {
                CurrentTooltipIsSimple = stateOfTooltip;
            }

            if (tag.TryGet("LastTownPet", out string townPetName))
            {
                LastTownPet = townPetName;
            }

            if (tag.TryGet("SkinColor", out Color skinColor))
            {
                skin = skinColor;
            }

            if (tag.TryGet("SkinColorChanged", out bool skinChanged))
            {
                skinColorChanged = skinChanged;
            }

            if (tag.TryGet("eolConsume", out bool eol))
            {
                eolConsumed = eol;
            }

            if (tag.TryGet("golemConsume", out bool golem))
            {
                golemConsumed = golem;
            }

            if (tag.TryGet("pumpkingConsume", out bool pumpking))
            {
                pumpkingConsumed = pumpking;
            }

            if (tag.TryGet("hasPet", out bool pet))
            {
                petObtained = pet;
            }

            if (tag.TryGet("foundPets", out List<int> found))
            {
                FoundPets = found;
            }

            if (tag.TryGet("foundLightPets", out List<int> foundLight))
            {
                FoundLightPets = foundLight;
            }

            if (tag.TryGet("pettedTownPets", out List<int> petted))
            {
                PettedTownPets = petted;
            }
        }
        public override void NaturalLifeRegen(ref float regen)
        {
            if (Player.lifeRegen + regen < 0)
            {
                inCombatTimer = inCombatTimerMax;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (ModContent.GetInstance<PetPersonalization>().DifficultAmount != 0 && petObtained)
            {
                modifiers.FinalDamage *= 1f - ModContent.GetInstance<PetPersonalization>().DifficultAmount * 0.01f;
            }
            inCombatTimer = inCombatTimerMax;
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (ModContent.GetInstance<PetPersonalization>().DifficultAmount != 0 && petObtained)
            {
                modifiers.FinalDamage *= 1f + ModContent.GetInstance<PetPersonalization>().DifficultAmount * 0.01f;
            }
            modifiers.ModifyHurtInfo += (ref Player.HurtInfo info) =>
            {
                if (info.Damage > currentShield && currentShield > 0)
                {
                    CombatText.NewText(Player.Hitbox, Color.Cyan, -currentShield, true);
                    if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                    {
                        SoundEngine.PlaySound(SoundID.NPCDeath43 with { PitchVariance = 0.4f, Pitch = -0.8f, Volume = 0.2f }, Player.Center);
                    }

                    info.Damage -= currentShield;
                    shieldToBeReduced += currentShield;
                }
            };
            if (ModContent.GetInstance<PetPersonalization>().HurtSoundEnabled && PetSounds.PetItemIdToHurtSound.ContainsKey(Player.CurrentPet()))
            {
                modifiers.DisableSound();
            }

            inCombatTimer = inCombatTimerMax;
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            PetSounds.PlayHurtSoundFromItemId(Player.CurrentPet(), Player);
        }
        public override bool ConsumableDodge(Player.HurtInfo info)
        {
            if (currentShield > 0 && info.Damage <= currentShield - shieldToBeReduced)
            {
                info.SoundDisabled = true;
                ShieldFullBlockEffect(info.Damage);
                return true;
            }
            return base.ConsumableDodge(info);
        }
        public override void ResetEffects() //ResetEffects runs AFTER PreUpdate().
        {
            petSwapCooldown = 600;
            inCombatTimerMax = 300;

            fishingFortune = 0;
            harvestingFortune = 0;
            miningFortune = 0;
            globalFortune = 0;

            abilityHaste = 0;
            petHealMultiplier = 1f;
            petShieldMultiplier = 1f;
            petDirectDamageMultiplier = 1f;
        }
        public override void PreUpdate()
        {
            if (Player.CurrentPet() == ItemID.None)
            {
                currentActivePet = null;
            }

            if (Main.mouseItem.TryGetGlobalItem(out PetGlobalItem item) && item.pickedUpBefore == false) //Player's hand slot is not being reckognized as 'inventory' in UpdateInventory() of GlobalItem, so manually updating the Hand slot
            {
                item.pickedUpBefore = true;
            }

            if (Player.trashItem.TryGetGlobalItem(out PetGlobalItem trash) && trash.pickedUpBefore == false) //same thing as mouseItem above, this one is extremely situational and I doubt its possible to put an item into Trash Slot without picking it up with mouse, but I suppose some mods could make it happen?
            {
                trash.pickedUpBefore = true;
            }

            if (ColorVal >= 1f)
            {
                colorSwitched = true;
            }
            else if (ColorVal <= 0f)
            {
                colorSwitched = false;
            }

            ColorVal += colorSwitched ? -0.01f : 0.01f;

            if (Player.jump == 0)
            {
                jumpRegistered = false;
            }

            inCombatTimer--;
            if (inCombatTimer < 0)
            {
                inCombatTimer = 0;
            }

            if (timer >= 0)
            {
                float mult = 1f;
                if (abilityHaste >= 0)
                {
                    mult += abilityHaste;
                }
                else
                {
                    mult += -1 + -1 * (1 / (-1 + abilityHaste));
                }
                timer -= PetUtils.Randomizer((int)(1000 * mult), 1000);
                AbilityCdSoundPlayed = false;
            }
            if (timer < -1)
            {
                timer = -1;
            }

            if (timer <= 0 && AbilityCdSoundPlayed == false)
            {
                AbilityCdSoundPlayed = true;
                if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled && (ModContent.GetInstance<PetPersonalization>().LowCooldownSoundEnabled == false && timerMax > ModContent.GetInstance<PetPersonalization>().LowCooldownThreshold || ModContent.GetInstance<PetPersonalization>().LowCooldownSoundEnabled))
                {
                    SoundEngine.PlaySound(SoundID.MaxMana with { PitchVariance = 0.3f }, Player.Center);
                }
            }

            if (Main.myPlayer == Player.whoAmI && (Player.talkNPC == -1 || Player.talkNPC != LastPetTamer)) //If currently not talking to an NPC, or, if current talked NPC is not Pet Tamer we've prompted to open Combination menu with (So if any other NPC is talked to while menu is open, it will close)
            { //If it isn't for Main.myPlayer == Player.whoAmI, this code runs on other clients too in the name of the original client. This causes Light Pet menu to insta close.
                PetTamer.openLightCombineMenu = false;
            }
        }
        public override void PostUpdate()
        {
            if (petObtained == false && (Player.CurrentPet() != ItemID.None || Player.CurrentLightPet() != ItemID.None))
            {
                petObtained = true;
            }

            if (petObtained)
            {
                PetUtils.DoAchievementOnPlayer<NeedOfCompany>(Player.whoAmI);
            }
            if (petShield.Count > 0)
            {
                int addToAchievement = 0;
                while (shieldToBeReduced > 0 && petShield.Count > 0)
                {
                    (int shieldAmount, int shieldTimer) value = petShield.Find(x => x.shieldTimer == petShield.Min(x => x.shieldTimer));
                    int index = petShield.IndexOf(value);
                    if (index != -1 && value.shieldAmount <= shieldToBeReduced)
                    {
                        shieldToBeReduced -= value.shieldAmount;
                        addToAchievement += value.shieldAmount;
                        petShield.RemoveAt(index);
                    }
                    else if (index != -1 && value.shieldAmount > shieldToBeReduced)
                    {
                        value.shieldAmount -= shieldToBeReduced;
                        addToAchievement += shieldToBeReduced;
                        shieldToBeReduced = 0;
                        petShield[index] = value;
                    }
                }
                PetUtils.DoAchievementOnPlayer<GuardPet>(Player.whoAmI,addToAchievement);

                shieldToBeReduced = 0;
                currentShield = 0;

                petShield.RemoveAll(x => x.shieldTimer < 1 || x.shieldAmount <= 0);
                petShield.ForEach(x => currentShield += x.shieldAmount);

                for (int i = 0; i < petShield.Count; i++)
                {
                    (int shieldAmount, int shieldTimer) shieldValue = petShield[i];
                    shieldValue.shieldTimer--;
                    petShield[i] = shieldValue;
                }
            }

            if (CoordsToRemove.Count > 0)
            {
                PlayerPlacedBlockList.placedBlocksByPlayer.RemoveAll(CoordsToRemove.Contains);
                CoordsToRemove.Clear();
            }
            if (updateReplacedTile.Count > 0)
            {
                PlayerPlacedBlockList.placedBlocksByPlayer.AddRange(updateReplacedTile);
                updateReplacedTile.Clear();
            }
        }
        public override void PostUpdateMiscEffects()
        {
            Player.buffImmune[ModContent.BuffType<ObliviousPet>()] = !ModContent.GetInstance<PetPersonalization>().SwapCooldown; //If Swap cooldown is turned off, player will be 'immune' to the debuff.
        }
        public override void OnEnterWorld()
        {
            previousPetItem = Player.CurrentPet();
            if (ModContent.GetInstance<PetPersonalization>().EnableNotice)
            {
                Main.NewText(PetUtils.LocVal("Misc.Notice"));
            }
            if (ModContent.GetInstance<PetPersonalization>().EnableModNotice)
            {
                if (ModLoader.TryGetMod("PetsOverhaulCalamityAddon", out _) == false && ModLoader.TryGetMod("CalamityMod", out _) == true)
                    Main.NewText(PetUtils.LocVal("Misc.CalamityDetected"));
            }
        }
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            playSound = PetSounds.PlayKillSoundFromItemId(Player.CurrentPet(), Player) == ReLogic.Utilities.SlotId.Invalid;

            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }
        public override void UpdateEquips()
        {
            if (Player.CurrentPet() == ItemID.None)
            {
                timerMax = 0;
                currentPetStacksMax = -1;
                currentPetStackSpecialText = string.Empty;
                return;
            }

            if (previousPetItem != Player.CurrentPet())
            {
                timerMax = 0;
                currentPetStacksMax = -1;
                currentPetStackSpecialText = string.Empty;
                if (ModContent.GetInstance<PetPersonalization>().SwapCooldown)
                {
                    Player.AddBuff(ModContent.BuffType<ObliviousPet>(), petSwapCooldown);
                }


                previousPetItem = Player.CurrentPet();
            }

            if (Player.CurrentPet() != ItemID.None && FoundPets.Contains(Player.CurrentPet()) == false)
            {
                FoundPets.Add(Player.CurrentPet());
                PetUtils.DoAchievementOnPlayer<TheCollector>(Player.whoAmI);
            }

            if (Player.CurrentLightPet() != ItemID.None && FoundLightPets.Contains(Player.CurrentLightPet()) == false)
            {
                FoundLightPets.Add(Player.CurrentLightPet());
                PetUtils.DoAchievementOnPlayer<GleamingCollection>(Player.whoAmI);
            }

            PetSounds.PlayAmbientSoundFromItemId(Player.CurrentPet(), Player);
        }
        public override void UpdateDead()
        {
            timer = -1;
            petShield.Clear();
            inCombatTimer = 0;
        }
        public override void ModifyCaughtFish(Item fish) //this is where fish is actually caught/reeled out.
        {
            SpawnItemSourcingFromPet(EntitySourcePetIDs.FishingFortuneItem, fish.type, PetUtils.Randomizer((globalFortune + fishingFortune) * 10 / 2 * fish.stack, 1000));
            if (Main.rand.NextBool(5))
            {
                Player.QuickSpawnItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.GlobalItem), ModContent.ItemType<PetFood>(), Main.rand.Next(1, 3)); //Next(1,3) IS 1 or 2, not 1, 2 and 3.
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (triggersSet.Inventory)
            {
                PetTamer.openLightCombineMenu = false;
            }

            if (PetIDs.PetNamesAndItems.ContainsValue(Main.HoverItem.type) && PetKeybinds.ShowDetailedTip.JustPressed)
            {
                CurrentTooltipIsSimple = !CurrentTooltipIsSimple;
            }
        }
        public override void AnglerQuestReward(float rareMultiplier, List<Item> rewardItems)
        {
            if (Main.rand.NextBool(3))
            {
                rewardItems.Add(new Item(ModContent.ItemType<PetFood>(), Main.rand.Next(5, 11))); //11 since we want 10 to be possible
            }
        }
        #endregion
    }
}
