using UnityEngine;

namespace TBSG.Combat
{
    public class CharacterController : MonoBehaviour
    {
        private Character m_Character; 

        private void Awake()
        {
            m_Character = GetComponent<Character>();
        }

        private void Update()
        {
            DetectMouse(); 
        }

        private void DetectMouse()
        {
            if (Input.GetMouseButtonDown(0))
                if (GridManager.Instance.m_HoveredGridTile != null)
                    if (m_Character.CanMoveToTile(GridManager.Instance.m_HoveredGridTile))
                        m_Character.MoveToTile(GridManager.Instance.m_HoveredGridTile, OnCharacterReachedTargetPos); 
        }

        private void OnCharacterReachedTargetPos()
        {
            m_Character.CalculateMovementRange(true);
        }
    }
}