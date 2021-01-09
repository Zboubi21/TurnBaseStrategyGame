using UnityEngine;
using TMPro;

namespace TBSG.Combat
{
    public class EnemyHeadUpData : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Character m_Character = null;
        [SerializeField] private TextMeshProUGUI m_LifePointsTxt = null;

        private void Start()
        {
            m_Character.OnLifePointsChanged += Character_OnLifePointsChanged;

            Initialize();
        }

        private void LateUpdate()
        {
            transform.rotation = CameraManager.Instance.GetCurrentCamera().transform.rotation;
        }

        private void Initialize()
        {
            Character_OnLifePointsChanged();
        }

        private void Character_OnLifePointsChanged()
        {
            m_LifePointsTxt.text = m_Character.CurrentLifePoints.ToString();
        }

        private void OnDestroy()
        {
            m_Character.OnLifePointsChanged -= Character_OnLifePointsChanged;
        }
    }
}