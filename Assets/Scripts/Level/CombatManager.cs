using System;
using UnityEngine;
using GameDevStack.Patterns;

namespace TBSG.Combat
{
    public class CombatManager : SingletonMonoBehaviour<CombatManager>
    {
        public static event Action OnCombatStart;
        public static event Action OnTurnChanged;
        
        [Header("Player")]
        [SerializeField] private CharacterController m_CharacterController = null;
        public CharacterController CharacterController => m_CharacterController;

        [SerializeField] private CharacterController[] m_Characters = null;

        private bool m_IsFirstCharacterTurn = true;
        private int m_CurrentCharacterTurn = 0;

        protected virtual void Start()
        {
            OnCombatStart?.Invoke();
            NewCharacterTurn();
        }

        public void TriggerNextTurn()
        {
            NewCharacterTurn();
        }
        private void NewCharacterTurn()
        {
            if (!m_IsFirstCharacterTurn)
            {
                if (m_CurrentCharacterTurn - 1 >= 0)
                    m_Characters[m_CurrentCharacterTurn - 1].OnTurnEnd();
                else
                    m_Characters[m_Characters.Length - 1].OnTurnEnd();
            }

            m_Characters[m_CurrentCharacterTurn].NewTurn();
            m_IsFirstCharacterTurn = false;

            if (m_CurrentCharacterTurn == m_Characters.Length - 1)
            {
                m_CurrentCharacterTurn = 0;
                NewCompleteTurn();
            }
            else
                m_CurrentCharacterTurn ++;
        }
        private void NewCompleteTurn()
        {
            OnTurnChanged?.Invoke();
        }

        public Entity GetEntityOnGridTile(GridTile tile)
        {
            GridObject gridObject = GridManager.Instance.GetGridObjectAtPosition(tile.m_GridPosition);
            if (gridObject && gridObject.TryGetComponent(out Entity entity))
                return entity;
            return null;
        }
    }
}