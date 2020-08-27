using System;
using UnityEngine;
using GameDevStack.Patterns;

namespace TBSG.Combat
{
    public class CombatManager : SingletonMonoBehaviour<CombatManager>
    {
        public static event Action OnCombatStart;
        public static event Action OnTurnStart;
        public static event Action<CharacterController> OnCharacterTurnStart;
        public static event Action<CharacterController> OnCharacterTurnEnd;
        public static event Action OnTurnEnd;
        
        [Header("Player")]
        [SerializeField] private CharacterController m_CharacterController = null;
        public CharacterController CharacterController => m_CharacterController;

        [SerializeField] private CharacterController[] m_Characters = null;

        private int m_TurnCount = 1;
        private int m_CurrentCharacterTurn = 0;

        private void Start()
        {
            StartCombat();
        }

        private void StartCombat()
        {
            // Debug.Log("OnCombatStart");
            OnCombatStart?.Invoke();
            StartTurn();
        }

        private void StartTurn()
        {
            // Debug.Log("OnTurnStart");
            OnTurnStart?.Invoke();
            StartCharacterTurn();
        }

        private void StartCharacterTurn()
        {
            CharacterController cc = m_Characters[m_CurrentCharacterTurn];
            cc.StartCharacterTurn();
            // Debug.Log("OnCharacterTurnStart: " + cc.gameObject.name);
            OnCharacterTurnStart?.Invoke(cc);
        }

        public void TriggerEndCharacterTurn() => EndCharacterTurn();

        private void EndCharacterTurn()
        {
            CharacterController cc = m_Characters[m_CurrentCharacterTurn];
            cc.EndCharacterTurn();
            // Debug.Log("OnCharacterTurnEnd: " + cc.gameObject.name);
            OnCharacterTurnEnd?.Invoke(cc);

            AddTurn();
        }

        private void AddTurn()
        {
            if (m_CurrentCharacterTurn == m_Characters.Length - 1)
            {
                m_CurrentCharacterTurn = 0;
                EndTurn();
            }
            else
            {
                m_CurrentCharacterTurn ++;
                StartCharacterTurn();
            }
        }

        private void EndTurn()
        {
            // Debug.Log("OnTurnEnd");
            OnTurnEnd?.Invoke();
            m_TurnCount ++;
            StartTurn();
        }

#region Utility
        public Entity GetEntityOnGridTile(GridTile tile)
        {
            GridObject gridObject = GridManager.Instance.GetGridObjectAtPosition(tile.m_GridPosition);
            if (gridObject && gridObject.TryGetComponent(out Entity entity))
                return entity;
            return null;
        }
#endregion
    }
}