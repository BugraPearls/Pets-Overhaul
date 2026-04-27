using PetsOverhaul.Systems;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class JackOLanternEffect : LightPetEffect
    {
        public override int LightPetItemID => ItemID.PumpkingPetItem;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out JackOLantern jackOLantern))
            {
                Player.GetAttackSpeed<GenericDamageClass>() += jackOLantern.AttackSpeed.CurrentStatFloat;
                Pet.harvestingFortune += jackOLantern.HarvestingFortune.CurrentStatInt;
            }
        }
        public override void ModifyLuck(ref float luck)
        {
            if (TryGetLightPet(out JackOLantern jackOLantern))
            {
                luck += jackOLantern.Luck.CurrentStatFloat;
            }

        }
    }
    public sealed class JackOLantern : LightPetItem
    {
        public LightPetStat AttackSpeed = new(30, 0.003f, "PumpkinAtkSpd", 0.04f);
        public LightPetStat Luck = new(15, 0.01f, "PumpkinLuck", 0.03f);
        public LightPetStat HarvestingFortune = new(20, 1, "PumpkinExp", 10);
        public override int LightPetItemID => ItemID.PumpkingPetItem;
        public override string BaseTooltip => PetUtils.LocVal("LightPetTooltips.JackOLantern");
    }
}
