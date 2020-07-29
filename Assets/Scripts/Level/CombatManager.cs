using UnityEngine;
using GameDevStack.Patterns;

namespace TBSG.Combat
{
    public class CombatManager : SingletonMonoBehaviour<CombatManager>
    {
        [Header("Player")]
        public Character m_Player;

        protected virtual void Start()
        {
            TriggerNewTurn();
        }

        public void TriggerNewTurn()
        {
            // Reset the selected piece & units states
            ResetPlayersUnits();
        }

        private void ResetPlayersUnits()
        {
            m_Player.NewTurn();
        }
    }
}