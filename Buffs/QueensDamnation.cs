using PetsOverhaul.Systems;
using Terraria;
using Terraria.ModLoader;

namespace PetsOverhaul.Buffs
{
    public class QueensDamnation : ModBuff //DrawEffects in NpcPet class
    {
        public int stacks = 0;
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            tip = Lang.GetBuffDescription(ModContent.BuffType<QueensDamnation>()).Replace("<CurseCounter>", stacks.ToString());
            rare = 0;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.TryGetGlobalNPC(out PetGlobalNPC npcAffected))
            {
                stacks = npcAffected.curseCounter;
            }
        }
    }
}
