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
        [SerializeField] private Spell m_SpellOne = null;
        [Serializable] private class Spell
        {
            public SpellsEnum m_Spell = 0;
            public Button m_Button = null;
        }

        private void Awake()
        {
            m_SpellOne.m_Button.onClick.AddListener(On_ClickSpellOne);
        }

        private void On_ClickSpellOne() { OnClickSpell?.Invoke(m_SpellOne.m_Spell); }
    }
}