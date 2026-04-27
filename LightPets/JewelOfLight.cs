using PetsOverhaul.Systems;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class JewelOfLightEffect : LightPetEffect
    {
        public override int LightPetItemID => ItemID.FairyQueenPetItem;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out JewelOfLight empress))
            {
                Player.moveSpeed += empress.MovementSpeed.CurrentStatFloat;
                if (Player.equippedWings != null)
                {
                    Player.wingTimeMax += empress.WingTime.CurrentStatInt;
                }
            }
        }
        public override void PostUpdateRunSpeeds()
        {
            if (TryGetLightPet(out JewelOfLight empress))
            {
                Player.runAcceleration += empress.Acceleration.CurrentStatFloat;
            }
        }
    }
    public sealed class JewelOfLight : LightPetItem
    {
        public LightPetStat MovementSpeed = new(8, 0.01f, "EmpressMoveSpd", 0.07f);
        public LightPetStat WingTime = new(15, 4, "EmpressWing", 30);
        public LightPetStat Acceleration = new(20, 0.0012f, "EmpressExp", 0.02f);
        public override int LightPetItemID => ItemID.FairyQueenPetItem;
        public override string BaseTooltip => PetUtils.LocVal("LightPetTooltips.JewelOfLight");
    }
}
