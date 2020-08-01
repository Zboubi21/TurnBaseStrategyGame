using UnityEngine;

namespace TBSG.Combat
{
    [CreateAssetMenu(fileName = "New Spell", menuName = "Data/Combat/Spell")]
    public class SpellParameters : ScriptableObject
    {
        // public string m_Name = "";
        public SpellsEnum m_Spell = 0;
        public int m_ActionPoints = 1;
        public int m_Damages = 1;
        public int m_ThrowsPerTurnNbr = 1;
        public int m_TargetingPerOpponentNbr = 0;
        public int m_TurnsBetweenThrowsNbr = 0;
        public GameObject m_ObjectToSpawn = null;
        public RangeParameters m_Range = null;
        public bool m_NeedLineOfSights = false;
        public bool m_ThrowInStraightLine = false;
    }
}