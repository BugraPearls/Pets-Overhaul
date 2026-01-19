using PetsOverhaul.Buffs;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class ItsyBetsy : PetEffect
    {
        public override int PetItemID => ItemID.DD2BetsyPetItem;
        public int debuffTime = 720;
        public int maxStacks = 20;
        public float defReduction = 0.02f;
        public float missingHpRecover = 0.007f;
        public float maxStackBonusRecover = 0.5f;

        public override PetClass PetClassPrimary => PetClassID.Ranged;
        public override PetClass PetClassSecondary => PetClassID.Supportive;
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped(false))
            PetUtils.OldOnesAchievementHelper(Player);
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.GetGlobalNPC<PetGlobalNPC>().curseCounter > maxStacks)
            {
                target.GetGlobalNPC<PetGlobalNPC>().curseCounter = maxStacks;
            }

            if (target.HasBuff(ModContent.BuffType<QueensDamnation>()))
            {
                modifiers.Defense *= 1f - defReduction * target.GetGlobalNPC<PetGlobalNPC>().curseCounter;
            }
            else
            {
                target.GetGlobalNPC<PetGlobalNPC>().curseCounter = 0;
            }
        }
        public override void Load()
        {
            PetModPlayer.OnEnemyDeath += OnEnemyKill;
        }
        public override void Unload()
        {
            PetModPlayer.OnEnemyDeath -= OnEnemyKill;
        }
        public static void OnEnemyKill(NPC npc, Player player)
        {
            if (PetUtils.LifestealCheck(npc) && npc.TryGetGlobalNPC(out PetGlobalNPC npcPet) && npcPet.curseCounter > 0 && player.TryGetModPlayer(out ItsyBetsy betsy))
            {
                betsy.Pet.PetRecovery(player.statLifeMax2 - player.statLife, betsy.missingHpRecover * npcPet.curseCounter * (1f + (npcPet.curseCounter >= betsy.maxStacks ? betsy.maxStackBonusRecover : 0)));
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && hit.DamageType == DamageClass.Ranged)
            {
                target.AddBuff(ModContent.BuffType<QueensDamnation>(), debuffTime);
                target.GetGlobalNPC<PetGlobalNPC>().curseCounter++;
            }
        }
    }
    public sealed class DD2BetsyPetItem : PetTooltip
    {
        public override PetEffect PetsEffect => itsyBetsy;
        public static ItsyBetsy itsyBetsy
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out ItsyBetsy pet))
                    return pet;
                else
                    return ModContent.GetInstance<ItsyBetsy>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.DD2BetsyPetItem")
                        .Replace("<debuffTime>", Math.Round(itsyBetsy.debuffTime / 60f, 2).ToString())
                        .Replace("<defDecrease>", Math.Round(itsyBetsy.defReduction * 100, 2).ToString())
                        .Replace("<maxStack>", itsyBetsy.maxStacks.ToString())
                        .Replace("<missingHpSteal>", Math.Round(itsyBetsy.missingHpRecover * 100, 2).ToString())
                        .Replace("<maxStackIncr>", Math.Round(itsyBetsy.maxStackBonusRecover * 100, 2).ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.DD2BetsyPetItem");
    }
}
