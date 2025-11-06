using PetsOverhaul.NPCs;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class SlimePrince : PetEffect
    {
        public override int PetItemID => ItemID.KingSlimePetItem;
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public override PetClasses PetClassSecondary => PetClasses.Offensive;
        public float knockback = 8f;
        public int baseDmg = 20; //Most values are used inside the SlimeServant's code
        public float defMult = 0.2f;
        public float hpMult = 0.5f;
        public float slowAmount = 0.35f;
        public int slowDuration = 300;
        public int slimyDuration = 1500;
        public int radius = 100;
        public int cooldown = 900;
        public const int lifetimeOfServant = 7200;
        public int howManyIsAlive = 0;
        public override int PetAbilityCooldown => cooldown;
        public override int PetStackCurrent => howManyIsAlive;
        public override int PetStackMax => 0;
        public override string PetStackText => PetUtils.LocVal("PetItemTooltips.KingSlimePetItemStack");
        public override void ExtraPreUpdate()
        {
            howManyIsAlive = 0;
            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (npc.type == ModContent.NPCType<SlimeServant>() && npc.GetGlobalNPC<SlimeServantOwner>().Owner == Player.whoAmI)
                {
                    howManyIsAlive++;
                }
            }
        }
        public override void ExtraProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && PetIsEquipped())
            {
                SoundEngine.PlaySound(SoundID.Item44 with { PitchVariance = 1.6f, Volume = 0.6f }, Player.Center);
                NPC npc = NPC.NewNPCDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetNPC), (int)Player.position.X, (int)Player.position.Y, ModContent.NPCType<SlimeServant>());
                npc.GetGlobalNPC<SlimeServantOwner>().Owner = Player.whoAmI;
                npc.defense += Player.statDefense * defMult;
                npc.lifeMax += (int)(Player.statLifeMax2 * hpMult);
                npc.life = npc.lifeMax;
                Pet.timer = Pet.timerMax;
            }
        }
    }
    public sealed class KingSlimePetItem : PetTooltip
    {
        public override PetEffect PetsEffect => slimePrince;
        public static SlimePrince slimePrince
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out SlimePrince pet))
                    return pet;
                else
                    return ModContent.GetInstance<SlimePrince>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.KingSlimePetItem")
            .Replace("<keybind>", PetUtils.KeybindText(PetKeybinds.UsePetAbility))
            .Replace("<defMult>", Math.Round(slimePrince.defMult * 100, 2).ToString())
            .Replace("<hpMult>", Math.Round(slimePrince.hpMult * 100, 2).ToString())
            .Replace("<dmg>", slimePrince.baseDmg.ToString())
            .Replace("<slowAmount>", Math.Round(slimePrince.slowAmount * 100, 2).ToString())
            .Replace("<slowDuration>", Math.Round(slimePrince.slowDuration / 60f, 2).ToString())
            .Replace("<slimyDuration>", Math.Round(slimePrince.slimyDuration / 60f, 2).ToString())
            .Replace("<radius>", Math.Round(slimePrince.radius / 16f, 2).ToString())
            .Replace("<cooldown>", Math.Round(slimePrince.cooldown / 60f, 2).ToString())
            .Replace("<lifetime>", Math.Round(SlimePrince.lifetimeOfServant / 60f, 2).ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.KingSlimePetItem").Replace("<keybind>", PetUtils.KeybindText(PetKeybinds.UsePetAbility));
    }
}
