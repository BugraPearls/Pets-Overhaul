using PetsOverhaul.NPCs;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class SharkPup : PetEffect
    {
        public override int PetItemID => ItemID.SharkBait;
        public float seaCreatureResist = 0.85f;
        public float seaCreatureDamage = 1.1f;
        public int shieldOnCatch = 10;
        public int shieldTime = 900;
        public int fishingPow = 10;

        public override PetClasses PetClassPrimary => PetClasses.Fishing;
        public override PetClasses PetClassSecondary => PetClasses.Offensive;
        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (PetIsEquipped() && npc.GetGlobalNPC<NpcPet>().seaCreature)
            {
                modifiers.FinalDamage *= seaCreatureResist;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && target.GetGlobalNPC<NpcPet>().seaCreature)
            {
                modifiers.FinalDamage *= seaCreatureDamage;
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped(false))
            {
                Player.fishingSkill += fishingPow;
            }
        }
        public override void ModifyCaughtFish(Item fish)
        {
            if (PetIsEquipped())
            {
                Pet.AddShield(shieldOnCatch, shieldTime);
            }
        }
    }
    public sealed class SeaCreatureProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool seaCreatureProj = false;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_Parent parent && parent.Entity is NPC npc && npc.GetGlobalNPC<NpcPet>().seaCreature)
            {
                seaCreatureProj = true;
            }
            else
            {
                seaCreatureProj = false;
            }
        }
        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
        {
            if (seaCreatureProj == true)
            {
                SharkPup player = target.GetModPlayer<SharkPup>();
                if (player.PetIsEquipped())
                {
                    modifiers.FinalDamage *= player.seaCreatureResist;
                }
            }
        }
    }
    public sealed class SharkBait : PetTooltip
    {
        public override PetEffect PetsEffect => sharkPup;
        public static SharkPup sharkPup
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out SharkPup pet))
                    return pet;
                else
                    return ModContent.GetInstance<SharkPup>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.SharkBait")
                        .Replace("<fishingPower>", sharkPup.fishingPow.ToString())
                        .Replace("<seaCreatureDmg>", sharkPup.seaCreatureDamage.ToString())
                        .Replace("<seaCreatureResist>", sharkPup.seaCreatureResist.ToString())
                        .Replace("<shield>", sharkPup.shieldOnCatch.ToString())
                        .Replace("<shieldTime>", Math.Round(sharkPup.shieldTime / 60f, 2).ToString());
    }
}
