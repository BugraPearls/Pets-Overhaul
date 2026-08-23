using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace PetsOverhaul.Systems
{
    public class PetOnWorldSystems : ModSystem
    {
        public override void PostUpdateNPCs()
        {
            PetModPlayer.anyAliveBosses = false;
            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (npc.boss || PetIDs.NonBossTrueBosses.Contains(npc.type))
                {
                    PetModPlayer.anyAliveBosses = true;
                }
            }
        }
    }
}
