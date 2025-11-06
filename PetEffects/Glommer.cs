using PetsOverhaul.Buffs;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Glommer : PetEffect
    {
        public override int PetItemID => ItemID.GlommerPetItem;
        public int glommerSanityTime = 60;
        public int glommerSanityRecover = 2;
        public float glommerSanityAura = 0.3f;
        public int glommerSanityRange = 1040;

        public override PetClasses PetClassPrimary => PetClasses.Magic;
        public override PetClasses PetClassSecondary => PetClasses.Supportive;
        public override int PetAbilityCooldown => glommerSanityTime;
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped(false) && Main.rand.NextBool(18000))
            {
                Player.QuickSpawnItem(PetUtils.GetSource_Pet(EntitySourcePetIDs.GlobalItem), ModContent.ItemType<GlommersGoop>());
            }

            if (PetIsEquipped())
            {
                PetUtils.CircularDustEffect(Player.Center, DustID.ShimmerTorch, glommerSanityRange);
                foreach (var player in Main.ActivePlayers)
                {
                    if (Player.Distance(player.Center) < glommerSanityRange)
                    {
                        player.GetModPlayer<GlobalPet>().abilityHaste += glommerSanityAura;
                        player.AddBuff(ModContent.BuffType<SanityAura>(), 1);
                    }
                }
                Player.statManaMax2 += (int)(Pet.abilityHaste * Player.statManaMax2);
                if (Pet.timer <= 0 && Player.statMana != Player.statManaMax2)
                {
                    Player.statMana += glommerSanityRecover;
                    Pet.timer = Pet.timerMax;
                }
            }
        }
    }
    public sealed class GlommerPetItem : PetTooltip
    {
        public override PetEffect PetsEffect => glommer;
        public static Glommer glommer
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out Glommer pet))
                    return pet;
                else
                    return ModContent.GetInstance<Glommer>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.GlommerPetItem")
                        .Replace("<sanityRange>", Math.Round(glommer.glommerSanityRange / 16f, 2).ToString())
                        .Replace("<sanityAmount>", Math.Round(glommer.glommerSanityAura * 100, 2).ToString())
                        .Replace("<currentHaste>", Math.Round(glommer.Pet.abilityHaste * 100, 2).ToString())
                        .Replace("<manaRecover>", glommer.glommerSanityRecover.ToString())
                        .Replace("<manaRecoverCd>", Math.Round(glommer.glommerSanityTime / 60f, 2).ToString())
                        .Replace("<goop>", ModContent.ItemType<GlommersGoop>().ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.GlommerPetItem");
    }
}
