using UnityEngine;

namespace TBSG.Combat
{
    public class AIController : CharacterController
    {
        [SerializeField] private SpellsEnum m_BasicSpell = SpellsEnum.None;

        private GridObject m_TargetObject = null;

        public override void StartCharacterTurn()
        {
            base.StartCharacterTurn();
            m_CurrentSpell = SpellManager.Instance.GetSpellWithSpellEnum(m_BasicSpell);
            m_TargetObject = CombatManager.Instance.CharacterController.GetComponent<GridObject>();
            StartTurn();
        }

        private void StartTurn()
        {
            // Don't show the movement range 

            if (CanLaunchSpellOnTarget(m_CurrentSpell, m_TargetObject))
                LaunchSpellOnTarget(m_CurrentSpell, m_TargetObject);
            else if (DeterminePath(m_TargetObject.m_GridPosition))
                MoveToTargetPath();
            else
                TriggerEndCharacterTurn();
        }

        private bool DeterminePath(Vector2Int targetPos)
        {
            CharacterPathWalker pathWalker = m_Character.PathWalker;
            pathWalker.DeterminePath(targetPos, false);

            if (pathWalker.m_Path.Count <= 1) return false;

            pathWalker.m_Path.RemoveAt(pathWalker.m_Path.Count - 1);

            int count = pathWalker.m_Path.Count - m_Character.CurrentMouvementPoints;
            if (count > 0)
                pathWalker.m_Path.RemoveRange(m_Character.CurrentMouvementPoints, count);
            
            return true;
        }

        private void MoveToTargetPath()
        {
            MoveCharacter(m_Character.PathWalker.GetLastGridTilePath());
        }

        protected override void OnCharacterReachedTargetPos()
        {
            if (CanLaunchSpellOnTarget(m_CurrentSpell, m_TargetObject))
                LaunchSpellOnTarget(m_CurrentSpell, m_TargetObject);
            else
                TriggerEndCharacterTurn();
        }

        private bool CanLaunchSpellOnTarget(SpellParameters spell, GridObject target)
        {
            m_Character.CalculateAttackRange(spell.m_Range, false);

            if (CanLaunchSpell(spell) && CanLaunchSpellOnTile(spell, target.m_CurrentGridTile))
                return true;
            else
                return false;
        }

        private void LaunchSpellOnTarget(SpellParameters spell, GridObject target)
        {
            LaunchSpell(spell, target.m_CurrentGridTile);
            TriggerEndCharacterTurn();
        }

        private void TriggerEndCharacterTurn()
        {
            CombatManager.Instance.TriggerEndCharacterTurn();
        }
    }
}