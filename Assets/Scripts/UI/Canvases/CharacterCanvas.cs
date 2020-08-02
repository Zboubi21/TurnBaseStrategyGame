using System;
using UnityEngine;
using UnityEngine.UI;
using TBSG.Combat;

namespace TBSG.UI
{
    public class CharacterCanvas : BaseCanvas
    {
        public static event Action<SpellsEnum> OnClickSpell;

        [Header("Spells")]
        [SerializeField] private Spell m_SpellZero = null;
        [SerializeField] private Spell m_SpellOne = null;
        [SerializeField] private Spell m_SpellTwo = null;
        [Serializable] private class Spell
        {
            public SpellsEnum m_Spell = 0;
            public Button m_Button = null;
        }

        private void Awake()
        {
            m_SpellZero.m_Button.onClick.AddListener(On_ClickSpellZero);
            m_SpellOne.m_Button.onClick.AddListener(On_ClickSpellOne);
            m_SpellTwo.m_Button.onClick.AddListener(On_ClickSpellTwo);
        }

        private void On_ClickSpellZero() { OnClickSpell?.Invoke(m_SpellZero.m_Spell); }
        private void On_ClickSpellOne() { OnClickSpell?.Invoke(m_SpellOne.m_Spell); }
        private void On_ClickSpellTwo() { OnClickSpell?.Invoke(m_SpellTwo.m_Spell); }
    }
}