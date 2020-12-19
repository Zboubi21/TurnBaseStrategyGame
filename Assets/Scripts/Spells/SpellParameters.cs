using UnityEngine;

namespace TBSG.Combat
{
    [CreateAssetMenu(fileName = "NewSpell", menuName = "Data/Combat/Spell")]
    public class SpellParameters : ScriptableObject
    {
        public SpellsEnum m_Spell = 0;
        public AttackType m_AttackType = 0;
        public int m_ActionPoints = 1;
        public int m_Damages = 1;
        public int m_ThrowsPerTurnNbr = 1;
        public int m_TargetingPerOpponentNbr = 0;
        public int m_TurnsBetweenThrowsNbr = 0;
        public GameObject m_ObjectToSpawn = null;
        public RangeParameters m_Range = null;
        // public bool m_NeedLineOfSights = false;
        // public bool m_ThrowInStraightLine = false;
        [Range(0, 2)] public int m_NeedState = 0;
    }
}