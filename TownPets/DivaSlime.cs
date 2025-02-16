using PetsOverhaul.Buffs.TownPetBuffs;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets
{
    public class DivaSlime : TownPet
    {
        public float divaDisc = 0.005f;
        public override void PreUpdate()
        {
            if (NPC.downedMoonlord)
            {
                divaDisc = 0.015f;
            }
            else if (Main.hardMode)
            {
                divaDisc = 0.01f;
            }
            else
            {
                divaDisc = 0.005f;
            }
        }
        public override void PostUpdateBuffs()
        {
            foreach (var npc in Main.ActiveNPCs)
            {
                if (Player.Distance(npc.Center) < auraRange && npc.type == NPCID.TownSlimeRainbow)
                {
                    Player.AddBuff(ModContent.BuffType<TownPetDiva>(), 2);
                    break;
                }
            }
        }
        public override bool CanBuyItem(NPC vendor, Item[] shopInventory, Item item)
        {
            bool didBuy = base.CanBuyItem(vendor, shopInventory, item);
            if (didBuy && item.buyOnce == false && Player.HasBuff(ModContent.BuffType<TownPetDiva>()))
            {
                Player.GetModPlayer<GlobalPet>().GiveCoins(GlobalPet.Randomizer((int)(item.GetStoreValue() * divaDisc * 1000), 1000));
            }
            return didBuy;
        }
    }
}