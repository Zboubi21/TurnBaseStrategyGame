using System;
using UnityEngine;
using UnityEngine.UI;

namespace TBSG.UI
{
    public class CharacterCanvas : BaseCanvas
    {
        public static event Action OnClickSpell;

        [SerializeField] private Button m_SpellButton = null;

        private void Awake()
        {
            m_SpellButton.onClick.AddListener(On_ClickSpell);
        }

        private void On_ClickSpell()
        {
            OnClickSpell?.Invoke();
        }
    }
}