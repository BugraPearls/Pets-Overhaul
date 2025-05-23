using Microsoft.Xna.Framework;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public class FennecFoxJump : ExtraJump
    {
        public override Position GetDefaultPosition() => BeforeBottleJumps;
        public override float GetDurationMultiplier(Player player) => 1.25f;
        public override void OnStarted(Player player, ref bool playSound)
        {
            Vector2 center = player.Center;
            Vector2 vector2 = new(50f, 20f);
            float num10 = (float)Math.PI * 2f * Main.rand.NextFloat();
            for (int m = 0; m < 5; m++)
            {
                for (float num11 = 0f; num11 < 14f; num11 += 1f)
                {
                    Dust obj = Main.dust[Dust.NewDust(center, 0, 0, DustID.Cloud)];
                    Vector2 vector3 = Vector2.UnitY.RotatedBy(num11 * ((float)Math.PI * 2f) / 14f + num10);
                    vector3 *= 0.2f * m;
                    obj.position = center + vector3 * vector2;
                    obj.velocity = vector3 + new Vector2(0f, player.gravDir * 4f);
                    obj.noGravity = true;
                    obj.scale = 1f + Main.rand.NextFloat() * 0.8f;
                    obj.fadeIn = Main.rand.NextFloat() * 2f;
                    obj.shader = GameShaders.Armor.GetSecondaryShader(player.cPet, player);
                }
            }
        }

        public override void ShowVisuals(Player player)
        {
            int num3 = player.height;
            if (player.gravDir == -1f)
                num3 = -6;

            float num4 = (player.jump / 75f + 1f) / 2f;
            for (int i = 0; i < 3; i++)
            {
                int num5 = Dust.NewDust(new Vector2(player.position.X, player.position.Y + num3 / 2), player.width, 32, DustID.Cloud, player.velocity.X * 0.3f, player.velocity.Y * 0.3f, 150, default, 1f * num4);
                Main.dust[num5].velocity *= 0.5f * num4;
                Main.dust[num5].fadeIn = 1.5f * num4;
                Main.dust[num5].shader = GameShaders.Armor.GetSecondaryShader(player.cPet, player);
            }
        }

        public override void UpdateHorizontalSpeeds(Player player)
        {
            player.runAcceleration *= 2.5f;
            player.maxRunSpeed *= 1.5f;
        }
    }
    public sealed class FennecFox : PetEffect
    {
        public override int PetItemID => ItemID.ExoticEasternChewToy;
        public override PetClasses PetClassPrimary => PetClasses.Mobility;
        public override PetClasses PetClassSecondary => PetClasses.Melee;
        public float sizeDecrease = 0.9f;
        public float speedIncrease = 0.04f;
        public float meleeSpdIncrease = 0.1f;
        public float meleeDmg = 0.05f;
        public override void ModifyItemScale(Item item, ref float scale)
        {
            if (PetIsEquipped())
            {
                scale *= sizeDecrease;
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.GetAttackSpeed<MeleeDamageClass>() += meleeSpdIncrease;
                Player.moveSpeed += speedIncrease;
                Player.GetDamage<MeleeDamageClass>() += meleeDmg;
                Player.GetJumpState<FennecFoxJump>().Enable();
            }
        }
    }
    public sealed class ExoticEasternChewToy : PetTooltip
    {
        public override PetEffect PetsEffect => fennecFox;
        public static FennecFox fennecFox
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out FennecFox pet))
                    return pet;
                else
                    return ModContent.GetInstance<FennecFox>();
            }
        }
        public override string PetsTooltip => PetTextsColors.LocVal("PetItemTooltips.ExoticEasternChewToy")
                        .Replace("<meleeSpd>", Math.Round(fennecFox.meleeSpdIncrease * 100, 2).ToString())
                        .Replace("<moveSpd>", Math.Round(fennecFox.speedIncrease * 100, 2).ToString())
                        .Replace("<sizeNerf>", fennecFox.sizeDecrease.ToString())
                        .Replace("<dmg>", Math.Round(fennecFox.meleeDmg * 100, 2).ToString());
        public override string SimpleTooltip => PetTextsColors.LocVal("SimpleTooltips.ExoticEasternChewToy");
    }
}
