namespace PetsOverhaul.Systems
{

    public enum MessageType : byte
    {
        MultiplayerDebugText,

        PetPlayerSync,
        ShieldFullAbsorb,
        CustomEffectSwitch,
        SeaCreatureOnKill,
        HoneyBeeHeal,
        BlockPlace,
        BlockReplace,
        BlockRemove,
        PetSlow,
        NPCOnDeathEffect,
        ActivePetSlot,
        ActiveLightPetSlot,
        LootChaser,
        QuestionableKibble,
        CrispyFriedCalamari,
        AlienSkater,
        BabyRedPanda,
        BlackCat,
        Lizard,
        Moonling,
        MoonlingCurrent,
        PhantasmalDragonSpell,
        PhantasmalDragonAbilitySwap,
        PhantasmalCurrentAbility,
        SlimePrince,
        SugarGliderGlide,
        SugarGliderAbility,
        SugarGliderAbilityHit,
        SuspiciousEye,
        TinyDeerclops,
        Turtle,
        P
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
