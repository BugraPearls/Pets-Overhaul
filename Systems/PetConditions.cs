using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.Systems
{
    public static class PetCraftingConditions
    {
        public static Condition ConsumedHead = new("Mods.PetsOverhaul.Misc.RequireHead", () => GlobalPet.pumpkingConsumed);
        public static Condition ConsumedOptic = new("Mods.PetsOverhaul.Misc.RequireOptic", () => GlobalPet.eolConsumed);
        public static Condition ConsumedWrench = new("Mods.PetsOverhaul.Misc.RequireWrench", () => GlobalPet.golemConsumed);
    }
    public class PetObtainedCondition : ModSystem
    {
        public static bool petIsObtained = false;
        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("hasPet", petIsObtained);
        }
        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.TryGet("hasPet", out bool pet))
            {
                petIsObtained = pet;
            }
        }
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(petIsObtained);
        }
        public override void NetReceive(BinaryReader reader)
        {
            petIsObtained = reader.ReadBoolean();
        }
    }
}