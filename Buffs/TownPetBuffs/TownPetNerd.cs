using PetsOverhaul.Systems;
using PetsOverhaul.TownPets;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.Buffs.TownPetBuffs
{
    public class TownPetNerd : ModBuff
    {
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            NerdySlime nerdySlime = Main.LocalPlayer.GetModPlayer<NerdySlime>();
            foreach (var npc in Main.ActiveNPCs)
            {
                if (npc.type == NPCID.TownSlimeBlue && Main.LocalPlayer.Distance(npc.Center) < nerdySlime.auraRange)
                {
                    buffName = Lang.GetBuffName(ModContent.BuffType<TownPetNerd>()).Replace("<Name>", npc.FullName);
                    break;
                }
                else
                {
                    buffName = "Nerdy Aura";
                }
            }
            if (NPC.downedMoonlord)
            {
                tip = PetTextsColors.LocVal("Buffs.TownPetNerd.PostMoonlordDescription")
                    .Replace("<NerdPlacement>", nerdySlime.nerdBuildSpeed.ToString())
                    .Replace("<NerdShineScale>", nerdySlime.nerdLightScale.ToString());
            }
            else if (Main.hardMode)
            {
                tip = PetTextsColors.LocVal("Buffs.TownPetNerd.PostHardmodeDescription")
                    .Replace("<NerdPlacement>", nerdySlime.nerdBuildSpeed.ToString())
                    .Replace("<NerdShineScale>", nerdySlime.nerdLightScale.ToString());
            }
            else
            {
                tip = PetTextsColors.LocVal("Buffs.TownPetNerd.Description")
                    .Replace("<NerdPlacement>", nerdySlime.nerdBuildSpeed.ToString())
                    .Replace("<NerdShineScale>", nerdySlime.nerdLightScale.ToString());
            }
            rare = 0;
        }
    }
}
