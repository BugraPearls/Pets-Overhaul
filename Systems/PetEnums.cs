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
        PlayerLeft,
        PlayerRight,
        HealthBarLeft,
        HealthBarRight,
    }
    public enum ParticleAmount
    {
        None,
        Lowered,
        Normal,
        Increased,
    }
    public enum PassivePetSoundFrequency
    {
        None,
        Lowered,
        Normal,
        Increased,

    }
}
