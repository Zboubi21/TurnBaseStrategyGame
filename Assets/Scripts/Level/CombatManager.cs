using System;
using System.Collections.Generic;
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
        public static event Action<bool> OnCombatEnd;
        
        [Header("Player")]
        [SerializeField] private CharacterController m_CharacterController = null;

        [Header("Characters")]
        [SerializeField] private List<CharacterController> m_Characters = new List<CharacterController>();

        public CharacterController CharacterController => m_CharacterController;

        private int m_TurnCount = 1;
        private int m_CurrentCharacterTurn = 0;

        private void Start()
        {
            StartCombat();
        }

        // Add delay to transition combat states

        private void StartCombat()
        {
            // Debug.Log("OnCombatStart");
            Character.OnCharacterDie += Character_OnCharacterDie;
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
            if (m_CurrentCharacterTurn == m_Characters.Count - 1)
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

        private void Character_OnCharacterDie(Character character)
        {
            for (int i = 0, l = m_Characters.Count; i < l; ++i)
            {
                if (m_Characters[i].Character == character)
                {
                    CharacterController deadCharacter = m_Characters[i];
                    m_Characters.RemoveAt(i);
                    CheckEndCombat(deadCharacter);
                    return;
                }
            }
        }

        private void CheckEndCombat(CharacterController deadCharacter)
        {
            if (deadCharacter == m_CharacterController)
            {
                OnLose();
            }
            else if (m_Characters.Count == 1)
            {
                OnWin();
            }
        }

        private void OnWin()
        {
            // Debug.Log("WIN");
            EndCombat();
            OnCombatEnd?.Invoke(true);
        }

        private void OnLose()
        {
            // Debug.Log("Lose");
            EndCombat();
            OnCombatEnd?.Invoke(false);
        }

        private void EndCombat()
        {
            Character.OnCharacterDie -= Character_OnCharacterDie;
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