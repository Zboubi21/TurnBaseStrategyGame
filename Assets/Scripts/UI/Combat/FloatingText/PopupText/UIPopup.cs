using UnityEngine;
using TMPro;

namespace TBSG.UI
{
    public class UIPopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_MainText = null;

        private void OnEnable() => Initialize();

        protected virtual void Initialize() { }

        public void SetMainText(string text, Color? color = null)
        {
            m_MainText.text = text;
            if (color.HasValue)
                m_MainText.color = color.Value;
        }
    }
}