using Terraria.ModLoader.Config;

namespace PetsOverhaul.Systems
{
    public enum MessageType : byte
    {
        ShieldFullAbsorb,
        SeaCreatureOnKill,
        HoneyBeeHeal,
        BlockPlace,
        BlockReplace,
        BlockRemove,
        PetSlow,
        NPCOnDeathEffect,
    }
    public enum EntitySourcePetIDs
    {
        GlobalItem,
        HarvestingItem,
        MiningItem,
        FishingItem,
        HarvestingFortuneItem,
        MiningFortuneItem,
        FishingFortuneItem,
        PetProjectile,
        PetNPC,
        PetMisc,
    }
    public enum PetClasses
    {
        None,
        Melee,
        Ranged,
        Magic,
        Summoner,
        Utility,
        Mobility,
        Harvesting,
        Mining,
        Fishing,
        Offensive,
        Defensive,
        Supportive,
        Rogue, //This is a temporary addition for Calamity addon, Classes will use Int rather than enum post 3.0. (Maybe not? seems unnecessary idk)
    }
    public enum ShieldPosition
    {
        [LabelKey("$Mods.PetsOverhaul.Config.PlayerLeft")]
        PlayerLeft,
        [LabelKey("$Mods.PetsOverhaul.Config.PlayerRight")]
        PlayerRight,
        [LabelKey("$Mods.PetsOverhaul.Config.HealthBarLeft")]
        HealthBarLeft,
        [LabelKey("$Mods.PetsOverhaul.Config.HealthBarRight")]
        HealthBarRight,
    }
    public enum ParticleAmount
    {
        [LabelKey("$Mods.PetsOverhaul.Config.ParticleNone")]
        None,
        [LabelKey("$Mods.PetsOverhaul.Config.ParticleLowered")]
        Lowered,
        [LabelKey("$Mods.PetsOverhaul.Config.ParticleNormal")]
        Normal,
        [LabelKey("$Mods.PetsOverhaul.Config.ParticleIncreased")]
        Increased,
    }
    public enum PassivePetSoundFrequency
    {
        [LabelKey("$Mods.PetsOverhaul.Config.PassiveSoundNever")]
        None,
        [LabelKey("$Mods.PetsOverhaul.Config.PassiveSoundRarely")]
        Lowered,
        [LabelKey("$Mods.PetsOverhaul.Config.PassiveSoundNormal")]
        Normal,
        [LabelKey("$Mods.PetsOverhaul.Config.PassiveSoundCommon")]
        Increased,

    }
}
