using PetsOverhaul.Systems;
using Terraria;
using Terraria.ModLoader;

namespace PetsOverhaul.Buffs
{
    public class Mauled : ModBuff //DrawEffects in NpcPet class
    {
        public int stacks = 0;
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            rare = 0;
            tip = Lang.GetBuffDescription(ModContent.BuffType<Mauled>()).Replace("<MaulCounter>", stacks.ToString());
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.TryGetGlobalNPC(out NPCGlobalPet npcAffected))
            {
                stacks = npcAffected.maulCounter;
            }
        }
    }
}
