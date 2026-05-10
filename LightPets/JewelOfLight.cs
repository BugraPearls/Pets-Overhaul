using Microsoft.Xna.Framework;
using PetsOverhaul.Systems;
using System;
using System.IO;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public class JewelOfLightJump : ExtraJump
    {
        public override Position GetDefaultPosition() => BeforeBottleJumps;
        public override float GetDurationMultiplier(Player player)
        {
            float baseAmount = 1f;
            if (player.GetModPlayer<JewelOfLightEffect>().TryGetLightPet(out JewelOfLight empress))
            {
                baseAmount += empress.JumpDuration;
            }
            return baseAmount;
        }
        public override void OnStarted(Player player, ref bool playSound)
        {
            Vector2 center = player.Center;
            Vector2 vector2 = new(50f, 20f);
            float num10 = (float)Math.PI * 2f * Main.rand.NextFloat();
            for (int m = 0; m < 5; m++)
            {
                for (float num11 = 0f; num11 < 14f; num11 += 1f)
                {
                    Dust obj = Main.dust[Dust.NewDust(center, 0, 0, DustID.GemAmethyst)];
                    Vector2 vector3 = Vector2.UnitY.RotatedBy(num11 * ((float)Math.PI * 2f) / 14f + num10);
                    vector3 *= 0.2f * m;
                    obj.position = center + vector3 * vector2;
                    obj.velocity = vector3 + new Vector2(0f, player.gravDir * 4f);
                    obj.noGravity = true;
                    obj.scale = 1f + Main.rand.NextFloat() * 0.8f;
                    obj.fadeIn = Main.rand.NextFloat() * 2f;
                    obj.shader = GameShaders.Armor.GetSecondaryShader(player.cLight, player);
                }
            }
        }

        public override void ShowVisuals(Player player)
        {
            int num3 = player.height;
            if (player.gravDir == -1f)
                num3 = -6;

            float num4 = (player.jump / 75f + 1f) / 2f;

            int num5 = Dust.NewDust(new Vector2(player.position.X, player.position.Y + num3 / 2), player.width, 32, DustID.UndergroundHallowedEnemies, player.velocity.X * 0.3f, player.velocity.Y * -0.7f, 150, default, 0.15f * num4);
            Main.dust[num5].shader = GameShaders.Armor.GetSecondaryShader(player.cLight, player);
        }

        public override void UpdateHorizontalSpeeds(Player player)
        {
            float acc = 1f;
            float maxSpd = 1f;

            if (player.GetModPlayer<JewelOfLightEffect>().TryGetLightPet(out JewelOfLight empress))
            {
                acc += empress.Acceleration;
                maxSpd += empress.MaxRunSpeed;
            }

            player.runAcceleration *= acc;
            player.maxRunSpeed *= maxSpd;
        }
    }
    public sealed class JewelOfLightEffect : LightPetEffect
    {
        public override int LightPetItemID => ItemID.FairyQueenPetItem;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out JewelOfLight empress))
            {
                Player.GetJumpState<JewelOfLightJump>().Enable();
                Pet.abilityHaste += empress.AbilityHaste;
            }
        }
    }
    public sealed class JewelOfLight : LightPetItem
    {
        public LightPetStat AbilityHaste = new(10, 0.01f, "Haste", 0.07f, LegacyKeysToInherit: ("EmpressMoveSpd", 8));
        public LightPetStat JumpDuration = new(20, 4, "JumpDuration", 30, LegacyKeysToInherit: ("EmpressWing", 15));
        public LightPetStat Acceleration = new(5, 0.0012f, "Acceleration", 0.02f, LegacyKeysToInherit: ("EmpressExp", 20));
        public LightPetStat MaxRunSpeed = new(5, 0.0012f, "MaxSpeed", 0.02f);
        public override int LightPetItemID => ItemID.FairyQueenPetItem;
        public override string BaseTooltip => PetUtils.LocVal("LightPetTooltips.JewelOfLight");
    }
}
