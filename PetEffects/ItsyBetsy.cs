using PetsOverhaul.Buffs;
using PetsOverhaul.NPCs;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
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

        public override PetClasses PetClassPrimary => PetClasses.Ranged;
        public override PetClasses PetClassSecondary => PetClasses.Supportive;
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.GetGlobalNPC<NpcPet>().curseCounter > maxStacks)
            {
                target.GetGlobalNPC<NpcPet>().curseCounter = maxStacks;
            }

            if (target.HasBuff(ModContent.BuffType<QueensDamnation>()))
            {
                modifiers.Defense *= 1f - defReduction * target.GetGlobalNPC<NpcPet>().curseCounter;
            }
            else
            {
                target.GetGlobalNPC<NpcPet>().curseCounter = 0;
            }
        }
        public override void Load()
        {
            GlobalPet.OnEnemyDeath += OnEnemyKill;
        }
        public override void Unload()
        {
            GlobalPet.OnEnemyDeath -= OnEnemyKill;
        }
        public static void OnEnemyKill(NPC npc, Player player)
        {
            if (GlobalPet.LifestealCheck(npc) && npc.TryGetGlobalNPC(out NpcPet npcPet) && npcPet.curseCounter > 0 && player.TryGetModPlayer(out ItsyBetsy betsy))
            {
                betsy.Pet.PetRecovery(player.statLifeMax2 - player.statLife, betsy.missingHpRecover * npcPet.curseCounter * (1f + (npcPet.curseCounter >= betsy.maxStacks ? betsy.maxStackBonusRecover : 0)));
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && hit.DamageType == DamageClass.Ranged)
            {
                target.AddBuff(ModContent.BuffType<QueensDamnation>(), debuffTime);
                target.GetGlobalNPC<NpcPet>().curseCounter++;
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
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.DD2BetsyPetItem")
                        .Replace("<debuffTime>", Math.Round(itsyBetsy.debuffTime / 60f, 2).ToString())
                        .Replace("<defDecrease>", Math.Round(itsyBetsy.defReduction * 100, 2).ToString())
                        .Replace("<maxStack>", itsyBetsy.maxStacks.ToString())
                        .Replace("<missingHpSteal>", Math.Round(itsyBetsy.missingHpRecover * 100, 2).ToString())
                        .Replace("<maxStackIncr>", Math.Round(itsyBetsy.maxStackBonusRecover * 100, 2).ToString());
    }
}
