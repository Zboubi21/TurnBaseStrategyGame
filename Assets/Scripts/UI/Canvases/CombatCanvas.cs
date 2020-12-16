using UnityEngine;
using TBSG.Combat;

namespace TBSG.UI
{
    public class CombatCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject m_WinCanvas = null;
        [SerializeField] private GameObject m_LoseCanvas = null;

        private void Awake()
        {
            CombatManager.OnCombatEnd += CombatManager_OnCombatEnd;
        }

        private void CombatManager_OnCombatEnd(bool win)
        {
            if (win)
                OnWinCombat();
            else
                OnLoseCombat();
        }

        private void OnWinCombat()
        {
            m_WinCanvas.SetActive(true);
        }

        private void OnLoseCombat()
        {
            m_LoseCanvas.SetActive(true);
        }

        private void OnDestroy()
        {
            CombatManager.OnCombatEnd -= CombatManager_OnCombatEnd;
        }
    }
}