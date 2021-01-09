using UnityEngine;
using GameDevStack.Animation;

namespace TBSG.UI
{
    public class DamageUIPopup : UIPopup
    {
        [Header("Move")]
        [SerializeField] private float m_TargetAdditionalYPos = 1;
        [SerializeField] private float m_TimeToReachPos = 1;
        [SerializeField] private AnimationCurve m_MoveCurve = new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));

        [Header("Fade")]
        [SerializeField] private float m_TimeToFadeOut = 1;
        [SerializeField] private AnimationCurve m_FadeCurve = new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));

        private CanvasGroup m_CanvasGroup;

        private void Awake()
        {
            m_CanvasGroup = GetComponent<CanvasGroup>();
        }

        protected override void Initialize()
        {
            Vector3 targetPos = transform.position + new Vector3(0, m_TargetAdditionalYPos, 0);
            CustomAnimationManager.AnimPositionWithTime(transform, transform.position + new Vector3(0, m_TargetAdditionalYPos, 0), m_TimeToReachPos).SetCurve(m_MoveCurve);
            CustomAnimationManager.AnimFloatWithTime(1, 0, m_TimeToFadeOut).SetCurve(m_FadeCurve).SetOnUpdate(UpdateCanvasGroup);
        }

        private void UpdateCanvasGroup(float alpha)
        {
            m_CanvasGroup.alpha = alpha;
        }
    }
}