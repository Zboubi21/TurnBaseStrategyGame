using UnityEngine;

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
        Headbutt,

        // Enemies
        BasicAttackCAC
    }

    [CreateAssetMenu(fileName = "NewSpell", menuName = "Data/Combat/Spell")]
    public class Spell : ScriptableObject
    {
        // Variables
        [SerializeField] private SpellsEnum m_SpellEnum = 0;
        [SerializeField] private AttackType m_AttackType = 0;
        [SerializeField, Range(0, 2)] private int m_NeedState = 0;

        [SerializeField] private SpellParameters m_SpellParameters = null;
        [SerializeField] private RangeParameters m_Range = null;

        // Getters
        public SpellsEnum SpellEnum => m_SpellEnum;
        public AttackType AttackType => m_AttackType;
        public int NeedState => m_NeedState;

        public SpellParameters SpellParameters => m_SpellParameters;
        public RangeParameters Range => m_Range;
    }
}