using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TBSG.Combat;
using TMPro;

namespace TBSG.UI
{
    public class PlayerSpellButton : MonoBehaviour
    {
        [SerializeField] private SpellsEnum m_Spell = 0;
        [SerializeField] private Button m_Button = null;
        [Header("Cooldown")]
        [SerializeField] private GameObject m_CooldownObject = null;
        [SerializeField] private TextMeshProUGUI m_CooldownTxt = null;
        [Header("Possible Use")]
        [SerializeField] private GameObject m_ThrowPerTurnObject = null;
        [SerializeField] private TextMeshProUGUI m_ThrowPerTurnTxt = null;

        private CharacterCanvas m_CharacterCanvas;

        private void Start()
        {
            UpdateUI();

            m_CharacterCanvas = GetComponentInParent<CharacterCanvas>();
            m_CharacterCanvas.AddPlayerSpellButton(this);

            CombatManager.Instance.PlayerController.OnLaunchSpell += UpdateUI;
            m_Button.onClick.AddListener(OnClickButton);
        }

        private void OnClickButton()
        {
            m_CharacterCanvas.On_ClickSpell(m_Spell);
        }

        public void UpdateUI()
        {
            Spell spell = SpellManager.Instance.GetSpellWithSpellEnum(m_Spell);
            m_Button.interactable = IsSpellAvailable(spell);
            UpdateCooldownSpells(spell);
            UpdateThrowPerTurnSpells(spell);
        }

        private bool IsSpellAvailable(Spell spell)
        {
            return CombatManager.Instance.PlayerController.CanLaunchSpell(spell);
        }

        private void UpdateCooldownSpells(Spell spell)
        {
            Dictionary<Spell, int> cooldownDict = CombatManager.Instance.PlayerController.TurnsBetweenThrowsSpells;
            if (cooldownDict.ContainsKey(spell) && cooldownDict[spell] != 0)
            {
                m_CooldownTxt.text = cooldownDict[spell].ToString();
                ActivateGO(m_CooldownObject, true);
            }
            else
                ActivateGO(m_CooldownObject, false);
        }

        private void UpdateThrowPerTurnSpells(Spell spell)
        {
            Dictionary<Spell, int> throwedSpellDict = CombatManager.Instance.PlayerController.ThrowedPerTurnSpells;
            if (throwedSpellDict.ContainsKey(spell))
            {
                m_ThrowPerTurnTxt.text = (spell.SpellParameters.ThrowsPerTurnNbr - throwedSpellDict[spell]).ToString();
                ActivateGO(m_ThrowPerTurnObject, true);
            }
            else
                ActivateGO(m_ThrowPerTurnObject, false);
        }

        private void ActivateGO(GameObject go, bool activate)
        {
            if (go.activeSelf != activate)
                go.SetActive(activate);
        }

        private void OnDestroy()
        {
            CombatManager.Instance.PlayerController.OnLaunchSpell -= UpdateUI;
            m_Button.onClick.RemoveListener(OnClickButton);
        }
    }
}