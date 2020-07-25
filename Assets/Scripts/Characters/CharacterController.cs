using UnityEngine;

namespace TBSG.Combat
{
    public enum PlayerControllerState { SelectingUnit, SelectedUnit, WaitingForUnitCallback, GameOver }

    public class CharacterController : MonoBehaviour
    {
        [NaughtyAttributes.ReadOnly, Header("Controller State")]
    public PlayerControllerState m_CurrentState = PlayerControllerState.SelectingUnit;

    private void OnEnable()
    {
        // TacticsGameManager.onNewTurnEvent += OnNewTurn;
        // TacticsGameManager.onUnitSelectedEvent += OnSelectedUnit;
        // TacticsGameManager.onUnitDeSelectedEvent += OnDeSelectedUnit;
        // TacticsGameManager.onGameOverEvent += OnGameOver;
    }
    private void OnDisable()
    {
        // TacticsGameManager.onNewTurnEvent -= OnNewTurn;
        // TacticsGameManager.onUnitSelectedEvent -= OnSelectedUnit;
        // TacticsGameManager.onUnitDeSelectedEvent -= OnDeSelectedUnit;
        // TacticsGameManager.onGameOverEvent -= OnGameOver;
    }

    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // store the hovered tile for usage in all states
            var hoveredTile = GridManager.Instance.m_HoveredGridTile;
            // Selecting Unit State
            if (m_CurrentState == PlayerControllerState.SelectingUnit && hoveredTile != null)
            {
                var selectedUnitAtTile = CombatManager.Instance.TrySelectUnitAtTile(hoveredTile);
                if (selectedUnitAtTile != null)
                {
                    EnterSelectedUnitState(selectedUnitAtTile);
                }
            }
            else if (m_CurrentState == PlayerControllerState.SelectedUnit && hoveredTile != null) // Unit selected state
            {
                var selectedUnit = CombatManager.Instance.m_SelectedUnit;
                if (selectedUnit.CanAttackTile(hoveredTile))
                {
                    // Switch states
                    EnterWaitingForUnitCallbackState();
                    // Attack the target tile
                    selectedUnit.AttackTile(hoveredTile, () => { EnterSelectedUnitState(selectedUnit); });
                }
                else if (selectedUnit.CanMoveToTile(hoveredTile))
                {
                    // Switch states
                    EnterWaitingForUnitCallbackState();
                    // Move to the target tile
                    selectedUnit.MoveToTile(hoveredTile, () => { EnterSelectedUnitState(selectedUnit); });
                }
            }
        }
    }

    public void SetControllerState(PlayerControllerState state)
    {
        if (m_CurrentState == PlayerControllerState.GameOver)
            return;

        var previousState = m_CurrentState;
        m_CurrentState = state;
    }

    public void EnterSelectingUnitState()
    {
        SetControllerState(PlayerControllerState.SelectingUnit);
    }

    public void EnterSelectedUnitState(Character unit)
    {
        // Unit selected event
        CombatManager.OnUnitSelected(unit);
    }

    public void EnterWaitingForUnitCallbackState()
    {
        // Unit deselected event
        CombatManager.OnUnitDeSelected();
        // Wait for callback state
        SetControllerState(PlayerControllerState.WaitingForUnitCallback);
    }

    // private void OnNewTurn(TacticsPlayer player)
    // {
    //     var nextUnit = CombatManager.Instance.SelectNextUnit();
    // }

    private void OnDeSelectedUnit()
    {
        HighlightManager.Instance.UnHighlightTiles();
    }

    private void OnSelectedUnit(Character unit)
    {
        // Selectedunit state
        SetControllerState(PlayerControllerState.SelectedUnit);
        if (unit.CanMove())
        {
            unit.CalculateMovementRange(true);
            if (unit.CanAttack())
            {
                unit.CalculateAttackRangeWithVictimsOnly(true);
            }
        }
        else if (unit.CanAttack())
        {
            unit.CalculateAttackRange(true);
        }
        else
        {
            CombatManager.Instance.SelectNextUnit();
        }
    }

    private void OnGameOver(TacticsPlayer winner)
    {
        SetControllerState(PlayerControllerState.GameOver);
    }
    }
}