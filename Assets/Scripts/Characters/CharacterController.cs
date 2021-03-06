﻿using UnityEngine;
using TBSG.UI;

namespace TBSG.Combat
{
    public class CharacterController : MonoBehaviour
    {
        [Header("Attacks")]
        [SerializeField] private SpellParameters[] m_Attacks = null;

        private Character m_Character; 
        private bool m_InMovementState = true;
        private SpellParameters m_CurrentSpell;

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
            if (!GridManager.Instance.m_HoveredGridTile) return;

            if (Input.GetMouseButtonDown(0))
                DoMainAction();
            if (Input.GetMouseButtonDown(2))
                DestroyMountainOnTile(GridManager.Instance.m_HoveredGridTile);
        }

        private void DoMainAction()
        {
            if (m_InMovementState)
                MovePlayer();
            else
                LaunchSpell(m_CurrentSpell, GridManager.Instance.m_HoveredGridTile);
        }

        private void MovePlayer()
        {
            if (m_Character.CanMoveToTile(GridManager.Instance.m_HoveredGridTile))
                m_Character.MoveToTile(GridManager.Instance.m_HoveredGridTile, OnCharacterReachedTargetPos); 
        }

        private void SetCurrentPlayerSpell(SpellsEnum spellEnum)
        {
            SpellParameters spell = GetSpellWithEnum(spellEnum);
            if (!m_Character.HasEnoughActionPoints(spell)) return;
            m_CurrentSpell = spell;
            m_Character.CalculateAttackRange(spell.m_Range, true);
            m_InMovementState = false;
        }

        private void LaunchSpell(SpellParameters spell, GridTile gridTile)
        {
            if (!m_Character.CanAttackTile(GridManager.Instance.m_HoveredGridTile)) return;

            if (spell.m_ObjectToSpawn)
            {
                SpawnObjectOnTile(spell, spell.m_ObjectToSpawn, gridTile);
            }
            else
            {
                GridObject gridObject = GridManager.Instance.GetGridObjectAtPosition(gridTile.m_GridPosition);
                if (gridObject && gridObject.TryGetComponent(out Entity entity))
                    entity.TakeDamage(spell.m_Damages);
                OnLaunchedSpell(spell);
            }
        }

        private void SpawnObjectOnTile(SpellParameters spell, GameObject obj, GridTile gridTile)
        {
            if (!gridTile.m_IsTileFlyable)
            {
                GridObject targetPosGridObject = GridManager.Instance.GetGridObjectAtPosition(gridTile.m_GridPosition);
                if (!targetPosGridObject)
                {
                    GridObject gridObject = obj.GetComponent<GridObject>();
                    GridManager.Instance.InstantiateGridObject(gridObject, gridTile.m_GridPosition);
                }
                else
                {
                    GridObject instantiatedGridObject = Instantiate(obj, targetPosGridObject.transform.position, Quaternion.identity).GetComponent<GridObject>();
                }
                OnLaunchedSpell(spell);
            }
        }

        private void OnLaunchedSpell(SpellParameters spell)
        {
            m_Character.SpendActionPoints(spell.m_ActionPoints);
            m_InMovementState = true;
            m_Character.ClearAttackRange();
            m_Character.CalculateMovementRange(true);
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

        private SpellParameters GetSpellWithEnum(SpellsEnum spellEnum)
        {
            if (m_Attacks == null || m_Attacks.Length == 0) return null;
            for (int i = 0, l = m_Attacks.Length; i < l; ++i)
                if (m_Attacks[i].m_Spell == spellEnum)
                    return m_Attacks[i];
            return null;
        }
    }
}