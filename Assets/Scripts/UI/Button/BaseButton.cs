using UnityEngine;
using UnityEngine.UI;

namespace TBSG.UI
{
    public class BaseButton : MonoBehaviour
    {
        private Button m_Button;

        private void Awake()
        {
            m_Button = GetComponent<Button>();
            m_Button.onClick.AddListener(OnButtonClick);
        }

        protected virtual void OnButtonClick() { }
    }
}