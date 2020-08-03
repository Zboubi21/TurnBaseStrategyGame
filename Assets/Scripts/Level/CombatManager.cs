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

        protected virtual void Start()
        {
            OnCombatStart?.Invoke();
            TriggerNewTurn();
        }

        public void TriggerNewTurn()
        {
            m_CharacterController.NewTurn();
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