using PetsOverhaul.Buffs;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class DungeonGuardian : PetEffect
    {
        public override int PetItemID => ItemID.BoneKey;
        public override PetClasses PetClassSecondary => PetClasses.Defensive;
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public int armorPen = 10;
        public int lifeRegen = 8;
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.npcTypeNoAggro[NPCID.AngryBones] = true;
                Player.npcTypeNoAggro[NPCID.AngryBonesBig] = true;
                Player.npcTypeNoAggro[NPCID.AngryBonesBigHelmet] = true;
                Player.npcTypeNoAggro[NPCID.AngryBonesBigMuscle] = true;
                Player.npcTypeNoAggro[NPCID.FromNetId(NPCID.ShortBones)] = true;
                Player.npcTypeNoAggro[NPCID.FromNetId(NPCID.BigBoned)] = true;
                Player.npcTypeNoAggro[NPCID.DarkCaster] = true;
                Player.npcTypeNoAggro[NPCID.DungeonSlime] = true;
                Player.npcTypeNoAggro[NPCID.CursedSkull] = true;
                Player.npcTypeNoAggro[NPCID.BlazingWheel] = true;
                Player.npcTypeNoAggro[NPCID.SpikeBall] = true;
                Player.npcTypeNoAggro[NPCID.WaterSphere] = true;
                if (Player.ZoneDungeon == true)
                {
                    Player.lifeRegen += lifeRegen;
                }
                Player.GetArmorPenetration<GenericDamageClass>() += armorPen;
            }
        }
        public override void Load()
        {
            On_Collision.CanTileHurt += PreventSpikesHurt;
        }
        private static bool PreventSpikesHurt(On_Collision.orig_CanTileHurt orig, ushort type, int i, int j, Player player)
        {
            bool hurt = orig(type, i, j, player);
            if (type == TileID.Spikes && player.ZoneDungeon && player.miscEquips[0].type == ItemID.BoneKey && player.HasBuff(ModContent.BuffType<ObliviousPet>()) == false)
            {
                hurt = false;
            }
            return hurt;
        }
    }
    public sealed class BoneKey : PetTooltip
    {
        public override PetEffect PetsEffect => dungeonGuardian;
        public static DungeonGuardian dungeonGuardian
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out DungeonGuardian pet))
                    return pet;
                else
                    return ModContent.GetInstance<DungeonGuardian>();
            }
        }
        public override string PetsTooltip => PetUtils.LocVal("PetItemTooltips.BoneKey")
                        .Replace("<armorPen>", dungeonGuardian.armorPen.ToString())
                        .Replace("<dungRegen>", dungeonGuardian.lifeRegen.ToString());
        public override string SimpleTooltip => PetUtils.LocVal("SimpleTooltips.BoneKey");
    }
}
