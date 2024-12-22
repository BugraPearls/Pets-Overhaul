using PetsOverhaul.PetEffects;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Projectiles
{
    /// <summary>
    /// This Class contains checks for Projectiles to help identify where Projectile is from.
    /// </summary>
    public sealed class ProjectileSourceChecks : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool isPlanteraProjectile = false;
        public bool petProj = false;
        public bool isFromSentry = false;
        public int sourceNpcId = 0;
        public Item itemProjIsFrom = null;
        public bool fromMount = false;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            isPlanteraProjectile = false;
            petProj = false;
            isFromSentry = false;
            fromMount = false;
            if (source is EntitySource_ItemUse item && Sapling.PlanteraWeapon.Contains(item.Item.type))
            {
                isPlanteraProjectile = true;
            }
            else if (source is EntitySource_Parent parent && parent.Entity is Projectile proj && Sapling.PlanteraProj.Contains(proj.type))
            {
                isPlanteraProjectile = true;
            }
            if (source is EntitySource_Pet { ContextType: EntitySourcePetIDs.PetProjectile })
            {
                petProj = true;
            }
            if (source is EntitySource_Parent parent2 && parent2.Entity is Projectile proj2 && proj2.sentry)
            {
                isFromSentry = true;
            }
            if (source is EntitySource_Parent parent3 && parent3.Entity is NPC npc)
            {
                sourceNpcId = npc.whoAmI;
            }
            if (source is EntitySource_ItemUse item2)
            {
                itemProjIsFrom = item2.Item;
            }
            else if (source is EntitySource_Parent parent4 && parent4.Entity is Projectile proj3 && proj3.TryGetGlobalProjectile(out ProjectileSourceChecks sourceChecks) && sourceChecks.itemProjIsFrom is not null)
            {
                itemProjIsFrom = sourceChecks.itemProjIsFrom;
            }
            if (source is EntitySource_Mount)
            {
                fromMount = true;
            }
        }
    }
}
