using UnityEngine;
using TBSG.UI;

namespace TBSG.Combat
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private GameObject m_MountainPrefab = null;

        private Character m_Character; 
        private bool m_InMovementState = true;

        private void Awake()
        {
            m_Character = GetComponent<Character>();
            CharacterCanvas.OnClickSpell += SetCurrentPlayerSpell;
        }

        public void NewTurn()
        {
            m_InMovementState = true;
            m_Character.NewTurn();
        }

        private void Update()
        {
            DetectMouse(); 
        }

        private void DetectMouse()
        {
            if (GridManager.Instance.m_HoveredGridTile == null) return;

            if (Input.GetMouseButtonDown(0))
                DoMainAction();
                // MovePlayer();
            // if (Input.GetMouseButtonDown(1))
                // SpawnMountainOnTile(GridManager.Instance.m_HoveredGridTile);
            if (Input.GetMouseButtonDown(2))
                DestroyMountainOnTile(GridManager.Instance.m_HoveredGridTile);
        }

        private void DoMainAction()
        {
            if (m_InMovementState)
                MovePlayer();
            else
                if (m_Character.CanAttackTile(GridManager.Instance.m_HoveredGridTile))
                    SpawnMountainOnTile(GridManager.Instance.m_HoveredGridTile);
        }

        private void MovePlayer()
        {
            if (m_Character.CanMoveToTile(GridManager.Instance.m_HoveredGridTile))
                m_Character.MoveToTile(GridManager.Instance.m_HoveredGridTile, OnCharacterReachedTargetPos); 
        }

        private void SetCurrentPlayerSpell()
        {
            m_Character.CalculateAttackRange(true);
            m_InMovementState = false;
        }

        private void SpawnMountainOnTile(GridTile gridTile)
        {
            if (CanSpawnMountainOnTile(gridTile))
            {
                GridObject mountainGridObject = m_MountainPrefab.GetComponent<GridObject>();
                GridManager.Instance.InstantiateGridObject(mountainGridObject, gridTile.m_GridPosition);

                m_InMovementState = true;
                m_Character.ClearAttackRange();
                m_Character.CalculateMovementRange(true);
            }
        }

        private bool CanSpawnMountainOnTile(GridTile gridTile)
        {
            return GridManager.Instance.GetGridObjectAtPosition(gridTile.m_GridPosition) == null && !gridTile.m_IsTileFlyable;
        }

        private void DestroyMountainOnTile(GridTile gridTile)
        {
            GridObject go = GridManager.Instance.GetGridObjectAtPosition(gridTile.m_GridPosition);
            if (go == null) return;
            if (go.TryGetComponent(out Mountain mountain))
            {
                GridManager.Instance.EraseGridObjectAtPosition(gridTile.m_GridPosition);
            }
        }

        private void OnCharacterReachedTargetPos()
        {
            m_Character.CalculateMovementRange(true);
        }
    }
}