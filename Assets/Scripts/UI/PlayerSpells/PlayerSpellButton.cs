﻿using System.Collections.Generic;
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
        [SerializeField] private GameObject m_PossibleUseObject = null;
        [SerializeField] private TextMeshProUGUI m_PossibleUseTxt = null;

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
            SpellParameters spell = SpellManager.Instance.GetSpellWithSpellEnum(m_Spell);
            m_Button.interactable = IsSpellAvailable(spell);
            UpdateCooldownSpells(spell);
        }

        private bool IsSpellAvailable(SpellParameters spell)
        {
            return CombatManager.Instance.PlayerController.CanLaunchSpell(spell);
        }

        private void UpdateCooldownSpells(SpellParameters spell)
        {
            Dictionary<SpellParameters, int> cooldownDict = CombatManager.Instance.PlayerController.TurnsBetweenThrowsSpells;
            if (cooldownDict.ContainsKey(spell) && cooldownDict[spell] != 0)
            {
                if (!m_CooldownObject.activeSelf)
                    m_CooldownObject.SetActive(true);
                m_CooldownTxt.text = cooldownDict[spell].ToString();
            }
            else
            {
                if (m_CooldownObject.activeSelf)
                    m_CooldownObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            CombatManager.Instance.PlayerController.OnLaunchSpell -= UpdateUI;
            m_Button.onClick.RemoveListener(OnClickButton);
        }
    }
}