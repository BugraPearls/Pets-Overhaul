namespace PetsOverhaul.Systems
{

    public enum MessageType : byte
    {
        MultiplayerDebugText,
        ShieldFullAbsorb,
        SeaCreatureOnKill,
        HoneyBeeHeal,
        BlockPlace,
        BlockReplace,
        BlockRemove,
        PetSlow,
        NPCOnDeathEffect,
        ActivePetSlot,
        ActiveLightPetSlot,
        PetButtonPressSync,
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
        GlobalFortuneItem,
        PetProjectile,
        PetNPC,
        PetMisc,
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
