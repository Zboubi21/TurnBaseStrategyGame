using UnityEngine;
using TBSG.UI;

namespace TBSG.Combat
{
    public class PlayerController : CharacterController
    {
        private enum PlayerState { None, Creation, Destruction }

        private bool m_InCreationState = true;

        protected override void Awake()
        {
            base.Awake();
            CharacterCanvas.OnClickSpell += SetCurrentPlayerSpell;
        }

        private void Update()
        {
            DetectMouse(); 
        }

        private void DetectMouse()
        {
            if (!m_IsMyTurn) return;
            if (!GridManager.Instance.m_HoveredGridTile) return;

            if (Input.GetMouseButtonDown(0))
                DoMainAction();
            if (Input.GetMouseButtonDown(2))
                DestroyMountainOnTile(GridManager.Instance.m_HoveredGridTile);
        }

        private void DoMainAction()
        {
            if (m_InMovementState)
                MoveCharacter(GridManager.Instance.m_HoveredGridTile);
            else
                if (CanLaunchSpell(m_CurrentSpell) && CanLaunchSpellOnTile(m_CurrentSpell, GridManager.Instance.m_HoveredGridTile))
                    LaunchSpell(m_CurrentSpell, GridManager.Instance.m_HoveredGridTile);
        }

        protected override bool IsItInTheRightState(SpellParameters spell)
        {
            return spell.m_NeedState == (int)PlayerState.None ||
                (m_InCreationState && spell.m_NeedState == (int)PlayerState.Creation) || 
                (!m_InCreationState && spell.m_NeedState == (int)PlayerState.Destruction);
        }

        private void SetCurrentPlayerSpell(SpellsEnum spellEnum)
        {
            if (!m_IsMyTurn) return;
            SpellParameters spell = SpellManager.Instance.GetSpellWithSpellEnum(spellEnum);
            if (!m_Character.HasEnoughActionPoints(spell)) return;
            m_CurrentSpell = spell;
            m_Character.CalculateAttackRange(spell.m_Range, true);
            m_InMovementState = false;
        }

        protected override void LaunchSpell(SpellParameters spell, GridTile gridTile)
        {
            base.LaunchSpell(spell, gridTile);
            if (spell.m_AttackType == AttackType.StateChange)
                ChangeCharacterState(spell, gridTile);
        }

        private void ChangeCharacterState(SpellParameters spell, GridTile gridTile)
        {
            m_InCreationState =! m_InCreationState;
            OnLaunchedSpell(spell);
        }

        private void DestroyMountainOnTile(GridTile gridTile)
        {
            GridObject go = GridManager.Instance.GetGridObjectAtPosition(gridTile.m_GridPosition);
            if (go != null && go.TryGetComponent(out Mountain mountain))
                GridManager.Instance.EraseGridObjectAtPosition(gridTile.m_GridPosition);
        }
    }
}