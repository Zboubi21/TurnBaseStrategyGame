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
        StateChange,
        GodsCreation,
        MortalTouch,
    }

    public class Spells
    {
        public static SpellParameters GetDataSpellWithSpellEnum(SpellParameters[] dataSpell, SpellsEnum spellEnum)
        {
            if (dataSpell == null || dataSpell.Length == 0) return null;
            for (int i = 0, l = dataSpell.Length; i < l; ++i)
                if (dataSpell[i].m_Spell == spellEnum)
                    return dataSpell[i];
            return null;
        }
    }
}