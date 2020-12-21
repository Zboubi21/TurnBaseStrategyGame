using UnityEngine;

namespace TBSG.Combat
{
    [CreateAssetMenu(fileName = "NewSpell", menuName = "Data/Combat/Spell")]
    public class SpellParameters : ScriptableObject
    {
        // Variables
        [SerializeField] private SpellsEnum m_Spell = 0;
        [SerializeField] private AttackType m_AttackType = 0;
        [SerializeField] private int m_ActionPoints = 1;
        [SerializeField] private int m_Damages = 1;
        [SerializeField] private int m_ThrowsPerTurnNbr = 1;
        [SerializeField] private int m_TargetingPerOpponentNbr = 0;
        [SerializeField] private int m_TurnsBetweenThrowsNbr = 0;
        [SerializeField] private GameObject m_ObjectToSpawn = null;
        [SerializeField] private RangeParameters m_Range = null;
        [SerializeField, Range(0, 2)] private int m_NeedState = 0;

        // Getters
        public SpellsEnum Spell => m_Spell;
        public AttackType AttackType => m_AttackType;
        public int ActionPoints => m_ActionPoints;
        public int Damages => m_Damages;
        public int ThrowsPerTurnNbr => m_ThrowsPerTurnNbr;
        public int TargetingPerOpponentNbr => m_TargetingPerOpponentNbr;
        public int TurnsBetweenThrowsNbr => m_TurnsBetweenThrowsNbr;
        public GameObject ObjectToSpawn => m_ObjectToSpawn;
        public RangeParameters Range => m_Range;
        public int NeedState => m_NeedState;
    }
}