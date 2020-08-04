using System;
using UnityEngine;
using UnityEngine.UI;
using TBSG.Combat;
using TMPro;

namespace TBSG.UI
{
    public class CharacterCanvas : BaseCanvas
    {
        public static event Action<SpellsEnum> OnClickSpell;

        [SerializeField] private TextMeshProUGUI m_LifePointsTxt = null;
        [SerializeField] private TextMeshProUGUI m_ActionPointsTxt = null;
        [SerializeField] private TextMeshProUGUI m_MovementPointsTxt = null;

        Character m_Character;

        private void Start()
        {
            m_Character = CombatManager.Instance.CharacterController.Character;
            m_Character.OnLifePointsChanged += Character_OnLifePointsChanged;
            m_Character.OnActionPointsChanged += Character_OnActionPointsChanged;
            m_Character.OnMovementPointsChanged += Character_OnMovementPointsChanged;
            Initialize();
        }

        private void Initialize()
        {
            Character_OnLifePointsChanged();
            Character_OnActionPointsChanged();
            Character_OnMovementPointsChanged();
        }

        private void Character_OnLifePointsChanged()
        {
            m_LifePointsTxt.text = CombatManager.Instance.CharacterController.Character.CurrentLifePoints.ToString();
        }
        private void Character_OnActionPointsChanged()
        {
            m_ActionPointsTxt.text = CombatManager.Instance.CharacterController.Character.CurrentActionPoints.ToString();
        }
        private void Character_OnMovementPointsChanged()
        {
            m_MovementPointsTxt.text = CombatManager.Instance.CharacterController.Character.CurrentMouvementPoints.ToString();
        }

        public void On_ClickSpell(SpellsEnum spell)
        {
            OnClickSpell?.Invoke(spell);
        }
    }
}