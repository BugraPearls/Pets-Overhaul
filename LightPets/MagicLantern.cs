using PetsOverhaul.Systems;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class MagicLanternEffect : LightPetEffect
    {
        public override int LightPetItemID => ItemID.MagicLantern;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out MagicLantern magicLantern))
            {
                Player.statDefense += magicLantern.Defense.CurrentStatInt;
                Player.statDefense *= magicLantern.DefensePercent.CurrentStatFloat + 1f;
                Player.endurance += magicLantern.DamageReduction.CurrentStatFloat;
                Pet.miningFortune += magicLantern.MiningFortune.CurrentStatInt;
            }
        }
    }
    public sealed class MagicLantern : LightPetItem
    {
        public LightPetStat Defense = new(3, 1,"LanternDef");
        public LightPetStat DefensePercent = new(20, 0.002f, "LanternMult", 0.01f);
        public LightPetStat DamageReduction = new(15, 0.002f, "LanternExp", 0.01f);
        public LightPetStat MiningFortune = new(15, 1, "LanternFort", 5);
        public override int LightPetItemID => ItemID.MagicLantern;
        public override string BaseTooltip => PetUtils.LocVal("LightPetTooltips.MagicLantern");
    }
}
