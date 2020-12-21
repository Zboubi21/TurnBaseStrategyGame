using UnityEngine;
using GameDevStack.Patterns;

namespace TBSG.Combat
{
public class SpellManager : SingletonMonoBehaviour<SpellManager>
    {
        [SerializeField] private Spell[] m_Spells = null;

        public Spell GetSpellWithSpellEnum(SpellsEnum spellEnum)
        {
            if (m_Spells == null || m_Spells.Length == 0) return null;
            for (int i = 0, l = m_Spells.Length; i < l; ++i)
                if (m_Spells[i].SpellEnum == spellEnum)
                    return m_Spells[i];
            return null;
        }
    }
}