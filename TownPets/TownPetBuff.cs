using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets
{
    public abstract class TownPetBuff : ModBuff
    {
        public sealed override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.TimeLeftDoesNotDecrease[Type] = true;
        }
        /// <summary>
        /// Update (player) with GlobalPet also added as a parameter.
        /// </summary>
        /// <param name="player">Player instance</param>
        /// <param name="pet">Global Pet instance</param>
        /// <param name="buffIndex">buff's index</param>
        public virtual void UpdateEffects(Player player, PetModPlayer pet, ref int buffIndex)
        { }
        /// <summary>
        /// Return false to stop normal code from running.
        /// </summary>
        /// <param name="buffName">Name of the buff</param>
        /// <param name="tip">Tooltip of the buff</param>
        /// <param name="rare">Rarity of the buff</param>
        /// <returns>If PetsOverhaul's usual ModifyBuffText for this TownPet buff should run.</returns>
        public virtual bool ExtraModifyBuffText(ref string buffName, ref string tip, ref int rare)
        { return true; }
        /// <summary>
        /// What the buff tooltip will be. By default its Description.Value. Will have a line added at the end, mentioning only 1 of them can be active.
        /// </summary>
        public virtual string BuffTooltip => Description.Value;
        public sealed override void Update(Player player, ref int buffIndex)
        {
            UpdateEffects(player, player.PetPlayer(), ref buffIndex);
        }
        public sealed override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            if (ExtraModifyBuffText(ref buffName, ref tip, ref rare) == false)
            {
                return;
            }
            buffName = DisplayName.Value.Replace("<Name>", PetModPlayer.LastTownPet);
            tip = BuffTooltip + "\n" + PetUtils.LocVal("Misc.TownPetBuffTip");
            rare = 0;
        }
    }
}
