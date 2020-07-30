using UnityEngine;
using GameDevStack.Patterns;

namespace TBSG.Combat
{
    public class CombatManager : SingletonMonoBehaviour<CombatManager>
    {
        [Header("Player")]
        public CharacterController m_CharacterController;

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
            m_CharacterController.NewTurn();
        }
    }
}