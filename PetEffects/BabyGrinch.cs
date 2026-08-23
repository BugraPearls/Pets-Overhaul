using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class BabyGrinch : PetEffect
    {
        public override int PetItemID => ItemID.BabyGrinchMischiefWhistle;
        public float winterDmg = 0.15f;
        public int winterCrit = 10;
        public float grinchSlow = 0.3f;
        public int grinchRange = 480;
        public int maxSlowRange = 80;
        public static List<int> FrostMoonWeapons = [ItemID.ChristmasTreeSword, ItemID.Razorpine, ItemID.ElfMelter, ItemID.ChainGun, ItemID.BlizzardStaff, ItemID.SnowmanCannon, ItemID.NorthPole];
        public override PetClass PetClassPrimary => PetClassID.Utility;
        public override PetClass PetClassSecondary => PetClassID.Offensive;
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && FrostMoonWeapons.Contains(item.type))
            {
                damage += winterDmg;
            }
        }
        public override void ModifyWeaponCrit(Item item, ref float crit)
        {
            if (PetIsEquipped() && FrostMoonWeapons.Contains(item.type))
            {
                crit += winterCrit;
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                PetUtils.CircularDustEffect(Player.Center, DustID.Snow, grinchRange);
                Player.resistCold = true;

                foreach (var npc in Main.ActiveNPCs)
                {
                    float dist = Player.Distance(npc.Center);
                    if (dist < grinchRange)
                    {
                        dist -= maxSlowRange;
                        float slow = grinchSlow;
                        if (dist <= 0)
                        {
                            slow *= 3;
                        }
                        else
                        {
                            slow *= 3 - 1f / (grinchRange - maxSlowRange) * 2f * dist; 
                            //Basically 1 / (grinchRange - maxSlowRange) gives us 0.0025 here, which is then multiplied by how many points of distance there is, which scales up to 1 (100%), which is then doubled so we can subtract the 3 up to 2 at max range so we do a 1x at the end.
                        }
                        PetGlobalNPC.AddSlow(new PetSlow(slow, 1, PetSlowID.Grinch), npc, Player);
                    }
                }
            }
        }
    }
    public sealed class BabyGrinchMischiefWhistle : PetTooltip
    {
        public override PetEffect PetsEffect => babyGrinch;
        public static BabyGrinch babyGrinch
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out BabyGrinch pet))
                    return pet;
                else
                    return ModContent.GetInstance<BabyGrinch>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.BabyGrinchMischiefWhistle")
                .Replace("<slowAmount>", Math.Round(babyGrinch.grinchSlow * 100, 2).ToString())
                .Replace("<slowRange>", Math.Round(babyGrinch.grinchRange / 16f, 2).ToString())
                .Replace("<maxSlowRange>", Math.Round(babyGrinch.maxSlowRange / 16f, 2).ToString())
                .Replace("<dmg>", Math.Round(babyGrinch.winterDmg * 100, 2).ToString())
                .Replace("<crit>", babyGrinch.winterCrit.ToString())
                .Replace("<weapons>", PetUtils.ItemsToTooltipImages(BabyGrinch.FrostMoonWeapons));
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.BabyGrinchMischiefWhistle");
    }
}

