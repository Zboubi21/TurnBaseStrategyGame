namespace TBSG.Combat
{
    public enum AttackType
    {
        None,
        Damage,
        Invocation,
        StateChange,
        Teleportation,
    }
    public enum SpellsEnum
    {
        None,

        // Player
        StateChange,
        GodsCreation,
        MortalTouch,
        Jump,

        // Enemies
        BasicAttackCAC
    }
}