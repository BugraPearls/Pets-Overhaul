using Microsoft.Xna.Framework;
using PetsOverhaul.NPCs;
using PetsOverhaul.Projectiles;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.PetEffects
{
    public sealed class SugarGlider : PetEffect
    {
        public override int PetItemID => ItemID.EucaluptusSap; //Eucalyptus Sap is EucaluptusSap???
        public float glideSpeedMult = 0.3f;
        public float customGlideWeak = 0.5f;
        public int shuricornCooldown = 1200;
        public int shuricornDamage = 20;
        public const int shuricornDuration = 300;
        public float shuricornKb = 7f;
        public override PetClasses PetClassPrimary => PetClasses.Mobility;
        public override bool HasCustomEffect => true; //Dedicated to sskToji
        public override PetClasses CustomPrimaryClass => PetClasses.Mobility;
        public override bool CustomEffectIsContributor => true;
        public override int PetAbilityCooldown => CustomEffectActive ? shuricornCooldown : base.PetAbilityCooldown;
        public override void ExtraProcessTriggers(TriggersSet triggersSet)
        {
            if (Player.velocity.Y > 0 && triggersSet.Jump && Player.dead == false)
            {
                if (PetIsEquipped())
                {
                    Player.maxFallSpeed *= glideSpeedMult;
                    Player.fallStart = (int)(Player.position.Y / 16.0);
                }
                else if (PetIsEquippedForCustom())
                {
                    Player.maxFallSpeed *= customGlideWeak;
                    Player.fallStart = (int)(Player.position.Y / 16.0);
                }
            }
            if (PetIsEquippedForCustom() && Pet.AbilityPressCheck())
            {
                Projectile.NewProjectile(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), Player.Center, new Vector2(20 * Player.direction, 0), ModContent.ProjectileType<Shuricorn>(), 20, shuricornKb, Player.whoAmI);
                Player.velocity.X = 10 * Player.direction * -1;
                Pet.timer = Pet.timerMax;
            }
            if (PetIsEquippedForCustom() && Player.dead == false && PetKeybinds.UsePetAbility.JustPressed)
            {
                foreach (var npc in Main.ActiveNPCs)
                {
                    if (npc.TryGetGlobalNPC(out NpcPet enemy) && enemy.shuricornMark > 0 && WorldGen.SolidTile(Utils.ToTileCoordinates(npc.Center)) == false)
                    {
                        Player.SetImmuneTimeForAllTypes(15);
                        Player.Center = npc.Center;
                        npc.SimpleStrikeNPC(shuricornDamage * 3, Player.position.X < npc.position.X ? 1 : -1, knockBack: shuricornKb * 3);
                        enemy.shuricornMark = 0;
                        break;
                    }
                }
            }
        }
        public override void SaveData(TagCompound tag)
        {
            tag.Add("CustomActive", CustomEffectActive);
        }
        public override void LoadData(TagCompound tag)
        {
            if (tag.TryGet("CustomActive", out bool custom))
            {
                CustomEffectActive = custom;
            }
        }
    }
    public sealed class EucaluptusSap : PetTooltip
    {
        public override PetEffect PetsEffect => sugarGlider;
        public static SugarGlider sugarGlider
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out SugarGlider pet))
                    return pet;
                else
                    return ModContent.GetInstance<SugarGlider>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.EucaluptusSap")
                .Replace("<glide>", sugarGlider.glideSpeedMult.ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.EucaluptusSap");
        public override string CustomTooltip => PetUtils.LocVal("CustomPetEffects.EucaluptusSap")
            .Replace("<keybind>", PetUtils.KeybindText(PetKeybinds.UsePetAbility))
            .Replace("<glide>", sugarGlider.customGlideWeak.ToString())
            .Replace("<dmg>", sugarGlider.shuricornDamage.ToString())
            .Replace("<kb>", sugarGlider.shuricornKb.ToString())
            .Replace("<cooldown>", Math.Round(sugarGlider.shuricornCooldown / 60f, 2).ToString())
            .Replace("<mark>", Math.Round(SugarGlider.shuricornDuration / 60f, 2).ToString());
        public override string CustomSimpleTooltip => PetUtils.LocVal("CustomSimplePetEffects.EucaluptusSap");
    }
}
