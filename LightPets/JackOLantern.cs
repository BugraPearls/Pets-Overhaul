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
                Pet.harvestingFortune += jackOLantern.HarvestingFortune;
                Pet.petDirectDamageMultiplier += jackOLantern.PetDamage;
            }
        }
        public override void ModifyLuck(ref float luck)
        {
            if (TryGetLightPet(out JackOLantern jackOLantern))
            {
                luck += jackOLantern.Luck;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (TryGetLightPet(out JackOLantern jackOLantern) && Main.rand.NextBool(Math.Clamp((int)(jackOLantern.BurnChance.CurrentStatFloat * 100), 1, 100), 100))
            {
                target.AddBuff(BuffID.OnFire, 120);
            }
        }
    }
    public sealed class JackOLantern : LightPetItem
    {
        public LightPetStat PetDamage = new(15, 0.003f, "Damage", 0.04f, LegacyKeysToInherit: ("PumpkinAtkSpd", 30));
        public LightPetStat Luck = new(12, 0.01f, "Luck", 0.03f, true, ("PumpkinLuck", 15));
        public LightPetStat HarvestingFortune = new(20, 1, "Fortune", 10, LegacyKeysToInherit: ("PumpkinExp", 20));
        public LightPetStat BurnChance = new(20, 0.01f, "Burn", 0.1f);
        public override int LightPetItemID => ItemID.PumpkingPetItem;
        public override string BaseTooltip => PetUtils.LocVal("LightPetTooltips.JackOLantern");
        public override void ModifyLightPetTooltip(ref string tooltip)
        {
            tooltip = tooltip.Replace("<0Luck>", Math.Round(Luck.CurrentStatFloat, 2).ToString()).Replace("<1Luck>", Luck.BaseStat.ToString()).Replace("<2Luck>", Luck.StatPerRoll.ToString());
        }
    }
}
