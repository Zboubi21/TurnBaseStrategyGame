using UnityEngine;
using GameDevStack.Patterns;

namespace TBSG.Combat
{
public class SpellManager : SingletonMonoBehaviour<SpellManager>
    {
        // [SerializeField] private SpellParameters[] m_Spells = null;
        [SerializeField] private PlayerSpellParameters[] m_PlayerSpells = null;

        // public SpellParameters GetDataSpellWithSpellEnum(SpellsEnum spellEnum)
        // {
        //     if (m_Spells == null || m_Spells.Length == 0) return null;
        //     for (int i = 0, l = m_Spells.Length; i < l; ++i)
        //         if (m_Spells[i].m_Spell == spellEnum)
        //             return m_Spells[i];
        //     return null;
        // }
        public PlayerSpellParameters GetPlayerSpellWithEnum(SpellsEnum spellEnum)
        {
            if (m_PlayerSpells == null || m_PlayerSpells.Length == 0) return null;
            for (int i = 0, l = m_PlayerSpells.Length; i < l; ++i)
                if (m_PlayerSpells[i].m_Spell == spellEnum)
                    return m_PlayerSpells[i];
            return null;
        }
    }
}