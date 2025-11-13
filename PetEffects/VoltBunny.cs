using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class VoltBunny : PetEffect
    {
        public override int PetItemID => ItemID.LightningCarrot;
        public float movespdFlat = 0.1f;
        public float movespdMult = 1.05f;
        public float movespdToDmg = 0.2f;
        public float staticParalysis = 3f;
        public int staticLength = 45;
        public override PetClass PetClassPrimary => PetClassID.Offensive;
        public override PetClass PetClassSecondary => PetClassID.Mobility;
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.moveSpeed += movespdFlat;
                Player.moveSpeed *= movespdMult;
                if (Player.moveSpeed > 1f)
                {
                    Player.GetDamage<GenericDamageClass>() += (Player.moveSpeed - 1f) * movespdToDmg;
                }
            }
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            if (PetIsEquipped() && info.DamageSource.TryGetCausingEntity(out Entity entity) && entity is NPC npc)
            {
                PetGlobalNPC.AddSlow(new PetSlow(staticParalysis, staticLength, PetSlowID.VoltBunny), npc);
            }
        }
    }
    public sealed class LightningCarrot : PetTooltip
    {
        public override PetEffect PetsEffect => voltBunny;
        public static VoltBunny voltBunny
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out VoltBunny pet))
                    return pet;
                else
                    return ModContent.GetInstance<VoltBunny>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.LightningCarrot")
                       .Replace("<flatSpd>", Math.Round(voltBunny.movespdFlat * 100, 2).ToString())
                       .Replace("<multSpd>", voltBunny.movespdMult.ToString())
                       .Replace("<spdToDmg>", Math.Round(voltBunny.movespdToDmg * 100, 2).ToString())
                       .Replace("<staticAmount>", Math.Round(voltBunny.staticParalysis * 100, 2).ToString())
                       .Replace("<staticTime>", Math.Round(voltBunny.staticLength / 60f, 2).ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.LightningCarrot");
    }
}
