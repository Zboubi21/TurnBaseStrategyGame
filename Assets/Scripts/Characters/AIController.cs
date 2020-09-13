namespace TBSG.Combat
{
    public class AIController : CharacterController
    {
        public override void StartCharacterTurn()
        {
            base.StartCharacterTurn();
            StartTurn();
        }

        private void StartTurn()
        {
            // Con't show the movement range 
            if (DeterminePath())
                MoveToTargetPath();
            else
                OnCharacterIsCACOfTarget();
        }

        private bool DeterminePath()
        {
            CharacterPathWalker pathWalker = m_Character.PathWalker;
            pathWalker.DeterminePath(CombatManager.Instance.CharacterController.GetComponent<GridObject>().m_GridPosition, false);

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
            TriggerEndCharacterTurn();
        }

        private void OnCharacterIsCACOfTarget()
        {
            TriggerEndCharacterTurn();
        }

        private void TriggerEndCharacterTurn()
        {
            CombatManager.Instance.TriggerEndCharacterTurn();
        }
    }
}