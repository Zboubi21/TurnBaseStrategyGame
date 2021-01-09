using UnityEngine;
using GameDevStack.Patterns;
using TBSG.Combat;

namespace TBSG.UI
{ 
    public class UIPopupManager : SingletonMonoBehaviour<UIPopupManager>
    {
        [Header("Prefabs")]
        [SerializeField] private UIPopup m_LowDamagePopupPrefab;
        [SerializeField] private UIPopup m_MidDamagePopupPrefab;
        [SerializeField] private UIPopup m_HighDamagePopupPrefab;

        public void AddDamagedPopup(Vector3 worldPosition, int damage, Vector3? positionOffset = null)
        {
            UIPopup uiPopup = null;
            switch (CombatManager.Instance.GetDamageLevel(damage))
            {
                case DamageLevel.Low:
                    uiPopup = m_LowDamagePopupPrefab;
                    break;
                case DamageLevel.Medium:
                    uiPopup = m_MidDamagePopupPrefab;
                    break;
                case DamageLevel.High:
                    uiPopup = m_HighDamagePopupPrefab;
                    break;
            }
            CreatePopup(uiPopup, worldPosition, damage.ToString(), null, positionOffset);
        }

        private UIPopup CreatePopup(UIPopup uiPopup, Vector3 worldPosition, string mainText, Color? mainTextColor = null, Vector3? positionOffset = null)
        {
            if (positionOffset.HasValue)
                worldPosition += positionOffset.Value;

            var instantiatedPopup = Instantiate(uiPopup, worldPosition, CameraManager.Instance.GetCurrentCamera().transform.rotation, transform);
            instantiatedPopup.SetMainText(mainText, mainTextColor);

            return instantiatedPopup;
        }
    }
}