using UnityEngine;
using UnityEngine.UI;
using TBSG.Combat;

namespace TBSG.UI
{
    public class PlayerSpellButton : MonoBehaviour
    {
        [SerializeField] private SpellsEnum m_Spell = 0;
        [SerializeField] private Button m_Button = null;

        private CharacterCanvas m_CharacterCanvas;

        private void Start()
        {
            UpdateUI();
            CombatManager.Instance.PlayerController.OnLaunchSpell += UpdateUI;
            m_CharacterCanvas = GetComponentInParent<CharacterCanvas>();
            m_CharacterCanvas.AddPlayerSpellButton(this);
            m_Button.onClick.AddListener(OnClickButton);
        }

        private void OnClickButton()
        {
            m_CharacterCanvas.On_ClickSpell(m_Spell);
        }

        public void UpdateUI()
        {
            m_Button.interactable = IsSpellAvailable();
        }

        private bool IsSpellAvailable()
        {
            SpellParameters spell = SpellManager.Instance.GetSpellWithSpellEnum(m_Spell);
            return CombatManager.Instance.PlayerController.CanLaunchSpell(spell);
        }
    }
}