using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class BabyImp : PetEffect
    {
        public override int PetItemID => ItemID.HellCake;
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public override PetClasses PetClassSecondary => PetClasses.Defensive;
        public int lavaImmune = 600;
        public int lavaDef = 10;
        public float lavaSpd = 0.15f;
        public int obbyDef = 8;
        public float obbySpd = 0.08f;
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.lavaMax += lavaImmune;
                if (Collision.LavaCollision(Player.position, Player.width, Player.height))
                {
                    Player.accFlipper = true;
                    Player.statDefense += lavaDef;
                    Player.moveSpeed += lavaSpd;
                }
                if (Player.HasBuff(BuffID.ObsidianSkin))
                {
                    Player.statDefense += obbyDef;
                    Player.moveSpeed -= obbySpd;
                }
            }
        }
    }
    public sealed class HellCake : PetTooltip
    {
        public override PetEffect PetsEffect => babyImp;
        public static BabyImp babyImp
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out BabyImp pet))
                    return pet;
                else
                    return ModContent.GetInstance<BabyImp>();
            }
        }
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.HellCake")
                .Replace("<immuneTime>", Math.Round(babyImp.lavaImmune / 60f, 2).ToString())
                .Replace("<lavaDef>", babyImp.lavaDef.ToString())
                .Replace("<lavaSpd>", Math.Round(babyImp.lavaSpd * 100, 2).ToString())
                .Replace("<obbyDef>", babyImp.obbyDef.ToString())
                .Replace("<obbySpd>", Math.Round(babyImp.obbySpd * 100, 2).ToString());
        public override string SimpleTooltip => PetTextsColors.LocVal("SimpleTooltips.HellCake");
    }
}
