using UnityEngine;
using GameDevStack.Patterns;
using System.Linq;

namespace TBSG.Combat
{
    public class CombatManager : SingletonMonoBehaviour<CombatManager>
    {
        [Header("Selected Unit"), NaughtyAttributes.ReadOnly]
        public Character m_SelectedUnit;

        [Header("Player")]
        public Character m_Player;

        [NaughtyAttributes.ReadOnly]
        public Character m_CurrentPlayerTurn = null;

        public delegate void UnitSelectedHandler(Character unit);
        public static event UnitSelectedHandler onUnitSelectedEvent;
        public static void OnUnitSelected(Character unit) { if (onUnitSelectedEvent != null) onUnitSelectedEvent(unit); }

        public delegate void UnitDeSelectedHandler();
        public static event UnitDeSelectedHandler onUnitDeSelectedEvent;
        public static void OnUnitDeSelected() { if (onUnitDeSelectedEvent != null) onUnitDeSelectedEvent(); }

        protected virtual void Start()
        {
            m_SelectedUnit = m_Player;
            TriggerNewTurn();
            TriggerSelectedUnitMovement();
        }

        public void TriggerNewTurn()
        {
            // Set the current player
            m_CurrentPlayerTurn = m_Player;

            // Reset the selected piece & units states
            // DeselectUnit();
            ResetPlayersUnits();
        }

        public void ResetPlayersUnits()
        {
            m_CurrentPlayerTurn.NewTurn();
        }

        public Character TrySelectUnitAtTile(GridTile targetTile)
        {
            var unitAtTile = GridManager.Instance.GetGridObjectAtPosition(targetTile.m_GridPosition);
            if (unitAtTile != null)
            {
                var character = unitAtTile.GetComponent<Character>();
                if (character != null)
                {
                    // if (DoesUnitBelongToCurrentPlayer(character))
                    // {
                        m_SelectedUnit = character;
                        // Trigger the unit selected event
                        OnUnitSelected(m_SelectedUnit);
                        return m_SelectedUnit;
                    // }
                }
            }

            return null;
        }

        public void DeselectUnit()
        {
            // Clear the Unit's action ranges
            if (m_SelectedUnit != null)
            {
                m_SelectedUnit.ClearRanges();
                // Set the selected unit to null
                m_SelectedUnit = null;
            }
        }

        public void SelectNextUnit()
        {
            TriggerNewTurn();
        }

        public void SelectNextUnitFromButton()
        {
            SelectNextUnit();
        }

        public void TriggerSelectedUnitAttack()
        {
            if (m_SelectedUnit == null)
                return;

            if (m_SelectedUnit.CanAttack())
            {
                m_SelectedUnit.CalculateAttackRange(true);
            }
        }

        public void TriggerSelectedUnitMovement()
        {
            if (m_SelectedUnit == null)
                return;

            if (m_SelectedUnit.CanMove())
            {
                m_SelectedUnit.CalculateMovementRange(true);
            }
        }
    }
}