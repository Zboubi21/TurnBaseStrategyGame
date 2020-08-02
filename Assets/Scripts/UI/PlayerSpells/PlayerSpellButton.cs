using UnityEngine;
using UnityEngine.UI;
using TBSG.Combat;
using CharacterController = TBSG.Combat.CharacterController;

namespace TBSG.UI
{
    public class PlayerSpellButton : MonoBehaviour
    {
        [SerializeField] private SpellsEnum m_Spell = 0;
        [SerializeField] private Button m_Button = null;

        private CharacterCanvas m_CharacterCanvas;
        private PlayerSpellParameters m_PlayerSpellParameters;

        private void Start()
        {
            UpdateUI();
            CombatManager.OnTurnChanged += UpdateUI;
            CharacterController.OnLaunchSpell += UpdateUI;
            m_CharacterCanvas = GetComponentInParent<CharacterCanvas>();
            m_Button.onClick.AddListener(OnClickButton);
        }

        private void OnClickButton()
        {
            m_CharacterCanvas.On_ClickSpell(m_Spell);
        }

        private void UpdateUI()
        {
            m_Button.interactable = IsSpellAvailable();
        }

        private bool IsSpellAvailable()
        {
            PlayerSpellParameters spell = SpellManager.Instance.GetPlayerSpellWithEnum(m_Spell);
            return CombatManager.Instance.CharacterController.Character.HasEnoughActionPoints(spell) && CombatManager.Instance.CharacterController.IsItInTheRightState(spell);
        }
    }
}