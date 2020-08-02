using System;
using UnityEngine;
using UnityEngine.UI;
using TBSG.Combat;

namespace TBSG.UI
{
    public class CharacterCanvas : BaseCanvas
    {
        public static event Action<SpellsEnum> OnClickSpell;

        public void On_ClickSpell(SpellsEnum spell)
        {
            OnClickSpell?.Invoke(spell);
        }
    }
}