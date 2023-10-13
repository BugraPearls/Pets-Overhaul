﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace PetsOverhaul.Systems
{
    public class MasteryShardCheck : ModSystem
    {
        internal static bool masteryShardObtained1 = false;
        internal static bool masteryShardObtained2 = false;
        internal static bool masteryShardObtained3 = false;
        public override void OnWorldLoad()
        {
            masteryShardObtained1 = false;
            masteryShardObtained2 = false;
            masteryShardObtained3 = false;
        }
        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("masteryshard1",masteryShardObtained1);
            tag.Add("masteryshard2",masteryShardObtained2);
            tag.Add("masteryshard3",masteryShardObtained3);
        }
        public override void LoadWorldData(TagCompound tag)
        {
            masteryShardObtained1 = tag.GetBool("masteryshard1");
            masteryShardObtained2 = tag.GetBool("masteryshard2");
            masteryShardObtained3 = tag.GetBool("masteryshard3");
        }
    }
}
