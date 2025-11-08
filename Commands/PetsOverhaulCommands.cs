using Microsoft.Xna.Framework;
using PetsOverhaul.PetEffects;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Commands
{
    public class PetsOverhaulCommands : ModCommand
    {
        public struct TopPlayer
        {
            public string PlayerName { get; set; }
            public int PlayerLevel { get; set; }
            public int PlayerExp { get; set; }
        }
        public override string Command => "pet";
        public override CommandType Type => CommandType.Chat;
        public override string Description => "Pets Overhaul commands";
        public override string Usage => "/pet <option>\n"
            + "Only use /pet to see options.";
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    caller.Reply(PetUtils.LocVal("Commands.Help"), Color.Gray);
                    break;
                case 1:
                    switch (args[0].ToLower())
                    {
                        case "fortune" or "fortunestat" or "fortunestats":
                            PetModPlayer Pet = caller.Player.PetPlayer();
                            caller.Reply(PetUtils.LocVal("Commands.FortuneInfo"), Color.Gray);
                            caller.Reply(PetUtils.LocVal("Commands.FortuneCurrent").Replace("<global>", Pet.globalFortune.ToString())
                                .Replace("<mining>", Pet.miningFortune.ToString()).Replace("<fishing>", Pet.fishingFortune.ToString()).Replace("<harvesting>", Pet.harvestingFortune.ToString()));
                            break;

                        case "vanity" or "vanitypet":
                            caller.Reply(PetUtils.LocVal("Commands.VanityPet"));
                            break;

                        case "junimo" or "junimoscoreboard" or "junimoleaderboard":
                            if (Main.netMode == NetmodeID.SinglePlayer)
                            {
                                Junimo junimoLvls = caller.Player.GetModPlayer<Junimo>();
                                caller.Reply(PetUtils.LocVal("Commands.SinglePlayerJunimo").Replace("<color>", PetUtils.ClassEnumToColor(PetClasses.Mining).Hex3())
                                    .Replace("<class>", PetUtils.PetClassLocalized(PetClasses.Mining)).Replace("<level>", junimoLvls.junimoMiningLevel.ToString()).Replace("<exp>", junimoLvls.junimoMiningExp.ToString()));

                                caller.Reply(PetUtils.LocVal("Commands.SinglePlayerJunimo").Replace("<color>", PetUtils.ClassEnumToColor(PetClasses.Fishing).Hex3())
                                    .Replace("<class>", PetUtils.PetClassLocalized(PetClasses.Fishing)).Replace("<level>", junimoLvls.junimoFishingLevel.ToString()).Replace("<exp>", junimoLvls.junimoFishingExp.ToString()));

                                caller.Reply(PetUtils.LocVal("Commands.SinglePlayerJunimo").Replace("<color>", PetUtils.ClassEnumToColor(PetClasses.Harvesting).Hex3())
                                    .Replace("<class>", PetUtils.PetClassLocalized(PetClasses.Harvesting)).Replace("<level>", junimoLvls.junimoHarvestingLevel.ToString()).Replace("<exp>", junimoLvls.junimoHarvestingExp.ToString()));
                            }
                            else
                            {
                                List<TopPlayer> topMining = [];
                                List<TopPlayer> topFishing = [];
                                List<TopPlayer> topHarvesting = [];
                                foreach (var player in Main.ActivePlayers)
                                {
                                    Junimo juni = player.GetModPlayer<Junimo>();
                                    topMining.Add(new TopPlayer() with { PlayerExp = juni.junimoMiningExp, PlayerLevel = juni.junimoMiningLevel, PlayerName = player.name });
                                    topFishing.Add(new TopPlayer() with { PlayerExp = juni.junimoFishingExp, PlayerLevel = juni.junimoFishingLevel, PlayerName = player.name });
                                    topHarvesting.Add(new TopPlayer() with { PlayerExp = juni.junimoHarvestingExp, PlayerLevel = juni.junimoHarvestingLevel, PlayerName = player.name });
                                }
                                int displayCounter = 0;

                                caller.Reply(PetUtils.LocVal("Commands.LeaderboardList")
                                    .Replace("<color>", PetUtils.ClassEnumToColor(PetClasses.Mining).Hex3()).Replace("<class>", PetUtils.PetClassLocalized(PetClasses.Mining)));
                                for (int i = topMining.Count; i > 0 && displayCounter < 3; i--, displayCounter++)
                                {
                                    TopPlayer topPlayer = topMining.Find(x => x.PlayerExp == topMining.Max(x => x.PlayerExp));
                                    caller.Reply(PetUtils.LocVal("Commands.PlayerRankings").Replace("<player>", topPlayer.PlayerName)
                                        .Replace("<class>", PetUtils.PetClassLocalized(PetClasses.Mining)).Replace("<level>", topPlayer.PlayerLevel.ToString()).Replace("<exp>", topPlayer.PlayerExp.ToString()),
                                        displayCounter == 0 ? Color.Lavender : displayCounter == 1 ? PetUtils.HighQuality : displayCounter == 2 ? PetUtils.MidQuality : PetUtils.LowQuality);
                                    topMining.Remove(topPlayer);
                                }

                                displayCounter = 0;
                                caller.Reply(PetUtils.LocVal("Commands.LeaderboardList")
                                    .Replace("<color>", PetUtils.ClassEnumToColor(PetClasses.Fishing).Hex3()).Replace("<class>", PetUtils.PetClassLocalized(PetClasses.Fishing)));
                                for (int i = topFishing.Count; i > 0 && displayCounter < 3; i--, displayCounter++)
                                {
                                    TopPlayer topPlayer = topFishing.Find(x => x.PlayerExp == topFishing.Max(x => x.PlayerExp));
                                    caller.Reply(PetUtils.LocVal("Commands.PlayerRankings").Replace("<player>", topPlayer.PlayerName)
                                        .Replace("<class>", PetUtils.PetClassLocalized(PetClasses.Fishing)).Replace("<level>", topPlayer.PlayerLevel.ToString()).Replace("<exp>", topPlayer.PlayerExp.ToString()),
                                        displayCounter == 0 ? Color.Lavender : displayCounter == 1 ? PetUtils.HighQuality : displayCounter == 2 ? PetUtils.MidQuality : PetUtils.LowQuality);
                                }

                                displayCounter = 0;
                                caller.Reply(PetUtils.LocVal("Commands.LeaderboardList")
                                    .Replace("<color>", PetUtils.ClassEnumToColor(PetClasses.Harvesting).Hex3()).Replace("<class>", PetUtils.PetClassLocalized(PetClasses.Harvesting)));
                                for (int i = topHarvesting.Count; i > 0 && displayCounter < 3; i--, displayCounter++)
                                {
                                    TopPlayer topPlayer = topHarvesting.Find(x => x.PlayerExp == topHarvesting.Max(x => x.PlayerExp));
                                    caller.Reply(PetUtils.LocVal("Commands.PlayerRankings").Replace("<player>", topPlayer.PlayerName)
                                        .Replace("<class>", PetUtils.PetClassLocalized(PetClasses.Harvesting)).Replace("<level>", topPlayer.PlayerLevel.ToString()).Replace("<exp>", topPlayer.PlayerExp.ToString()),
                                        displayCounter == 0 ? Color.Lavender : displayCounter == 1 ? PetUtils.HighQuality : displayCounter == 2 ? PetUtils.MidQuality : PetUtils.LowQuality);
                                    topHarvesting.Remove(topPlayer);
                                }
                            }
                            break;

                        case "miningscoreboard" or "miningleaderboard":
                            if (Main.netMode == NetmodeID.SinglePlayer)
                            {
                                caller.Reply(PetUtils.LocVal("Commands.UseInMultiplayer"));
                            }
                            else
                            {
                                List<TopPlayer> topMining = [];
                                foreach (var player in Main.ActivePlayers)
                                {
                                    Junimo juni = player.GetModPlayer<Junimo>();
                                    topMining.Add(new TopPlayer() with { PlayerExp = juni.junimoMiningExp, PlayerLevel = juni.junimoMiningLevel, PlayerName = player.name });
                                }
                                int displayCounter = 0;

                                caller.Reply(PetUtils.LocVal("Commands.LeaderboardList")
                                    .Replace("<color>", PetUtils.ClassEnumToColor(PetClasses.Mining).Hex3()).Replace("<class>", PetUtils.PetClassLocalized(PetClasses.Mining)));
                                for (int i = topMining.Count; i > 0 && displayCounter < Main.maxPlayers; i--, displayCounter++)
                                {
                                    TopPlayer topPlayer = topMining.Find(x => x.PlayerExp == topMining.Max(x => x.PlayerExp));
                                    caller.Reply(PetUtils.LocVal("Commands.PlayerRankings").Replace("<player>", topPlayer.PlayerName)
                                        .Replace("<class>", PetUtils.PetClassLocalized(PetClasses.Mining)).Replace("<level>", topPlayer.PlayerLevel.ToString()).Replace("<exp>", topPlayer.PlayerExp.ToString()),
                                        displayCounter == 0 ? Color.Lavender : displayCounter == 1 ? PetUtils.HighQuality : displayCounter == 2 ? PetUtils.MidQuality : PetUtils.LowQuality);
                                    topMining.Remove(topPlayer);
                                }
                            }
                            break;

                        case "fishingscoreboard" or "fishingleaderboard":
                            if (Main.netMode == NetmodeID.SinglePlayer)
                            {
                                caller.Reply(PetUtils.LocVal("Commands.UseInMultiplayer"));
                            }
                            else
                            {
                                List<TopPlayer> topFishing = [];
                                foreach (var player in Main.ActivePlayers)
                                {
                                    Junimo juni = player.GetModPlayer<Junimo>();
                                    topFishing.Add(new TopPlayer() with { PlayerExp = juni.junimoFishingExp, PlayerLevel = juni.junimoFishingLevel, PlayerName = player.name });
                                }
                                int displayCounter = 0;

                                caller.Reply(PetUtils.LocVal("Commands.LeaderboardList")
                                    .Replace("<color>", PetUtils.ClassEnumToColor(PetClasses.Fishing).Hex3()).Replace("<class>", PetUtils.PetClassLocalized(PetClasses.Fishing)));
                                for (int i = topFishing.Count; i > 0 && displayCounter < Main.maxPlayers; i--, displayCounter++)
                                {
                                    TopPlayer topPlayer = topFishing.Find(x => x.PlayerExp == topFishing.Max(x => x.PlayerExp));
                                    caller.Reply(PetUtils.LocVal("Commands.PlayerRankings").Replace("<player>", topPlayer.PlayerName)
                                        .Replace("<class>", PetUtils.PetClassLocalized(PetClasses.Fishing)).Replace("<level>", topPlayer.PlayerLevel.ToString()).Replace("<exp>", topPlayer.PlayerExp.ToString()),
                                        displayCounter == 0 ? Color.Lavender : displayCounter == 1 ? PetUtils.HighQuality : displayCounter == 2 ? PetUtils.MidQuality : PetUtils.LowQuality);
                                    topFishing.Remove(topPlayer);
                                }
                            }
                            break;

                        case "harvestingscoreboard" or "harvestingleaderboard":
                            if (Main.netMode == NetmodeID.SinglePlayer)
                            {
                                caller.Reply(PetUtils.LocVal("Commands.UseInMultiplayer"));
                            }
                            else
                            {
                                List<TopPlayer> topHarvesting = [];
                                foreach (var player in Main.ActivePlayers)
                                {
                                    Junimo juni = player.GetModPlayer<Junimo>();
                                    topHarvesting.Add(new TopPlayer() with { PlayerExp = juni.junimoHarvestingExp, PlayerLevel = juni.junimoHarvestingLevel, PlayerName = player.name });
                                }
                                int displayCounter = 0;

                                caller.Reply(PetUtils.LocVal("Commands.LeaderboardList")
                                    .Replace("<color>", PetUtils.ClassEnumToColor(PetClasses.Harvesting).Hex3()).Replace("<class>", PetUtils.PetClassLocalized(PetClasses.Harvesting)));
                                for (int i = topHarvesting.Count; i > 0 && displayCounter < Main.maxPlayers; i--, displayCounter++)
                                {
                                    TopPlayer topPlayer = topHarvesting.Find(x => x.PlayerExp == topHarvesting.Max(x => x.PlayerExp));
                                    caller.Reply(PetUtils.LocVal("Commands.PlayerRankings").Replace("<player>", topPlayer.PlayerName)
                                        .Replace("<class>", PetUtils.PetClassLocalized(PetClasses.Harvesting)).Replace("<level>", topPlayer.PlayerLevel.ToString()).Replace("<exp>", topPlayer.PlayerExp.ToString()),
                                        displayCounter == 0 ? Color.Lavender : displayCounter == 1 ? PetUtils.HighQuality : displayCounter == 2 ? PetUtils.MidQuality : PetUtils.LowQuality);
                                    topHarvesting.Remove(topPlayer);
                                }
                            }
                            break;

                        case "faq" or "question":
                            caller.Reply(PetUtils.LocVal("Commands.FAQ"));
                            break;

                        case "light" or "lightpet" or "lightpets":
                            string reply = PetUtils.LocVal("Commands.LightPetList") + " ";
                            int counterToGoDown = 5;
                            foreach (ModPlayer player in caller.Player.ModPlayers)
                            {
                                if (player is LightPetEffect light)
                                {
                                    counterToGoDown++;
                                    reply += "[i:" + light.LightPetItemID + "] ";
                                    if (counterToGoDown >= 20)
                                    {
                                        reply += "\n";
                                        counterToGoDown = 0;
                                    }
                                }
                            }
                            caller.Reply(reply);
                            break;
                        default:
                            caller.Reply(PetUtils.LocVal("Commands.ArgumentInvalid"), Color.Red);
                            caller.Reply(PetUtils.LocVal("Commands.Help"), Color.Gray);
                            break;

                    }
                    break;
                case 2:
                    switch (args[0].ToLower())
                    {
                        case "class" or "pets":
                            string reply = PetUtils.LocVal("Commands.PetList") + " ";

                            void iterate(PetClasses petClass, bool abilityPets = false)
                            {
                                int counterToGoDown = 5;
                                bool found = false;
                                foreach (ModPlayer player in caller.Player.ModPlayers)
                                {
                                    if (player is PetEffect pet && (petClass is PetClasses.None || pet.PetClassPrimary == petClass || pet.PetClassSecondary == petClass))
                                    {
                                        if (abilityPets == true && pet.PetAbilityCooldown <= 0)
                                        {
                                            continue;
                                        }
                                        counterToGoDown++;
                                        found = true;
                                        reply += "[i:" + pet.PetItemID + "] ";
                                        if (counterToGoDown >= 20)
                                        {
                                            reply += "\n";
                                            counterToGoDown = 0;
                                        }
                                    }
                                }
                                if (found == false)
                                    reply += PetUtils.LocVal("Commands.NoPets");
                                caller.Reply(reply);
                            }

                            switch (args[1].ToLower())
                            {
                                case "all":
                                    reply = reply.Replace("<class>", PetUtils.LocVal("Commands.All"));
                                    iterate(PetClasses.None);
                                    break;
                                case "melee":
                                    reply = reply.Replace("<class>", PetUtils.LocVal("Classes.Melee"));
                                    iterate(PetClasses.Melee);
                                    break;
                                case "ranged":
                                    reply = reply.Replace("<class>", PetUtils.LocVal("Classes.Ranged"));
                                    iterate(PetClasses.Ranged);
                                    break;
                                case "magic":
                                    reply = reply.Replace("<class>", PetUtils.LocVal("Classes.Magic"));
                                    iterate(PetClasses.Magic);
                                    break;
                                case "summoner":
                                    reply = reply.Replace("<class>", PetUtils.LocVal("Classes.Summoner"));
                                    iterate(PetClasses.Summoner);
                                    break;
                                case "utility":
                                    reply = reply.Replace("<class>", PetUtils.LocVal("Classes.Utility"));
                                    iterate(PetClasses.Utility);
                                    break;
                                case "mobility":
                                    reply = reply.Replace("<class>", PetUtils.LocVal("Classes.Mobility"));
                                    iterate(PetClasses.Mobility);
                                    break;
                                case "harvesting":
                                    reply = reply.Replace("<class>", PetUtils.LocVal("Classes.Harvesting"));
                                    iterate(PetClasses.Harvesting);
                                    break;
                                case "mining":
                                    reply = reply.Replace("<class>", PetUtils.LocVal("Classes.Mining"));
                                    iterate(PetClasses.Mining);
                                    break;
                                case "fishing":
                                    reply = reply.Replace("<class>", PetUtils.LocVal("Classes.Fishing"));
                                    iterate(PetClasses.Fishing);
                                    break;
                                case "offensive":
                                    reply = reply.Replace("<class>", PetUtils.LocVal("Classes.Offensive"));
                                    iterate(PetClasses.Offensive);
                                    break;
                                case "defensive":
                                    reply = reply.Replace("<class>", PetUtils.LocVal("Classes.Defensive"));
                                    iterate(PetClasses.Defensive);
                                    break;
                                case "supportive":
                                    reply = reply.Replace("<class>", PetUtils.LocVal("Classes.Supportive"));
                                    iterate(PetClasses.Supportive);
                                    break;
                                case "rogue":
                                    reply = reply.Replace("<class>", PetUtils.LocVal("Classes.Rogue"));
                                    iterate(PetClasses.Rogue);
                                    break;
                                case "ability" or "cooldown":
                                    reply = reply.Replace("<class>", PetUtils.LocVal("Misc.Ability"));
                                    iterate(PetClasses.None, true);
                                    break;
                                default:
                                    caller.Reply(PetUtils.LocVal("Commands.ClassArgumentInvalid"), Color.Red);
                                    break;
                            }
                            break;
                        case "item" or "items":
                            string itemReply = PetUtils.LocVal("Commands.Items");
                            switch (args[1].ToLower())
                            {
                                case "harvesting":
                                    List<int> harv = [];
                                    List<int> rareHarv = [];
                                    foreach (var (expAmount, plantList) in PetIDs.HarvestingXpPerGathered)
                                    {
                                        if (expAmount >= 1000)
                                        {
                                            rareHarv.AddRange(plantList);
                                        }
                                        else
                                        {
                                            harv.AddRange(plantList);
                                        }
                                    }
                                    itemReply = itemReply.Replace("<class>", PetUtils.PetClassLocalized(PetClasses.Harvesting)) + "\n" + PetUtils.ItemsToTooltipImages(harv, 22, 0) + " " + PetUtils.LocVal("Misc.RarePlant") + ": " + PetUtils.ItemsToTooltipImages(rareHarv, 22, 10);

                                    break;
                                case "mining":
                                    List<int> mining = [];
                                    foreach (var (expAmount, oreList) in PetIDs.MiningXpPerBlock)
                                    {
                                        mining.AddRange(oreList);
                                    }
                                    itemReply = itemReply.Replace("<class>", PetUtils.PetClassLocalized(PetClasses.Mining)) + "\n" + PetUtils.ItemsToTooltipImages(mining, 22, 0);
                                    break;
                                case "fishing":
                                    List<int> fish = [];
                                    foreach (var (expAmount, fishList) in PetIDs.FishingXpPerCaught)
                                    {
                                        fish.AddRange(fishList);
                                    }
                                    itemReply = itemReply.Replace("<class>", PetUtils.PetClassLocalized(PetClasses.Fishing)) + "\n" + PetUtils.ItemsToTooltipImages(fish, 22, 0);
                                    break;
                                default:
                                    caller.Reply(PetUtils.LocVal("Commands.ClassArgumentInvalidItems"), Color.Red);
                                    break;
                            }
                            caller.Reply(itemReply);
                            break;
                        default:
                            caller.Reply(PetUtils.LocVal("Commands.ArgumentInvalid"), Color.Red);
                            caller.Reply(PetUtils.LocVal("Commands.Help"), Color.Gray);
                            break;
                    }
                    break;
                default:
                    caller.Reply(PetUtils.LocVal("Commands.ArgumentInvalid"), Color.Red);
                    caller.Reply(PetUtils.LocVal("Commands.Help"), Color.Gray);
                    break;
            }
        }
    }
}
