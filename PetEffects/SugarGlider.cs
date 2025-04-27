using PetsOverhaul.Systems;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class SugarGlider : PetEffect
    {
        public override int PetItemID => ItemID.EucaluptusSap;
        public float glideSpeedMult = 0.3f;
        public override PetClasses PetClassPrimary => PetClasses.Mobility;
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Player.velocity.Y > 0 && PetIsEquipped() && triggersSet.Jump && Player.dead == false)
            {
                Player.maxFallSpeed *= glideSpeedMult;
                Player.fallStart = (int)((double)Player.position.Y / 16.0);
            }
        }
    }
    public sealed class EucaluptusSap : PetTooltip
    {
        public override PetEffect PetsEffect => sugarGlider;
        public static SugarGlider sugarGlider
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out SugarGlider pet))
                    return pet;
                else
                    return ModContent.GetInstance<SugarGlider>();
            }
        }
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.EucaluptusSap")
                .Replace("<glide>", sugarGlider.glideSpeedMult.ToString());
    }
}
