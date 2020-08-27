using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TBSG.Combat;
using TMPro;
using CharacterController = TBSG.Combat.CharacterController;

namespace TBSG.UI
{
    public class CharacterCanvas : BaseCanvas
    {
        public static event Action<SpellsEnum> OnClickSpell;

        [SerializeField] private TextMeshProUGUI m_LifePointsTxt = null;
        [SerializeField] private TextMeshProUGUI m_ActionPointsTxt = null;
        [SerializeField] private TextMeshProUGUI m_MovementPointsTxt = null;
        [SerializeField] private Button m_NextTurnButton = null;

        Character m_Character;
        List<PlayerSpellButton> m_PlayerSpellButtons = new List<PlayerSpellButton>();

        private void Start()
        {
            CombatManager.OnCharacterTurnStart += CombatManager_OnCharacterTurnStart;
            m_Character = CombatManager.Instance.CharacterController.Character;
            m_Character.OnLifePointsChanged += Character_OnLifePointsChanged;
            m_Character.OnActionPointsChanged += Character_OnActionPointsChanged;
            m_Character.OnMovementPointsChanged += Character_OnMovementPointsChanged;
            m_NextTurnButton.onClick.AddListener(OnClickNextTurnButton);
            Initialize();
        }

        private void Initialize()
        {
            Character_OnLifePointsChanged();
            Character_OnActionPointsChanged();
            Character_OnMovementPointsChanged();
        }

        public void AddPlayerSpellButton(PlayerSpellButton playerSpellButtons)
        {
            m_PlayerSpellButtons.Add(playerSpellButtons);
        }

        private void CombatManager_OnCharacterTurnStart(CharacterController cc)
        {
            if (cc.Character.CharacterTypes == CharacterTypes.Player)
                for (int i = 0, l = m_PlayerSpellButtons.Count; i < l; ++i)
                    m_PlayerSpellButtons[i].UpdateUI();
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

        private void OnClickNextTurnButton()
        {
            CombatManager.Instance.TriggerEndCharacterTurn();
        }
    }
}