using UnityEngine;
using GameDevStack.Animation;

namespace TBSG.Combat
{
    public class Mountain : Entity
    {
        [Header("References")]
        [SerializeField] private Transform m_Model = null;

        [Header("Parameters")]
        [SerializeField] private int m_ImpactDamage = 1; // Be able to manage this variable directly in SpellParameters

        [Header("Animations")]
        [SerializeField] private float m_StartYPos = 10;
        [SerializeField] private float m_TimeToAnimate = 0.5f;
        [SerializeField] private AnimationCurve m_AnimCurve = new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));

        protected override void Start()
        {
            base.Start();
            m_Model.position = new Vector3(m_Model.position.x, m_StartYPos, m_Model.position.z);
            CustomAnimationManager.AnimPositionWithTime(m_Model, Vector3.zero, m_TimeToAnimate).SetCurve(m_AnimCurve).SetOnComplete(OnAnimEnded);
        }

        private void OnAnimEnded()
        {
            Entity entity = CombatManager.Instance.GetEntityOnGridTile(GridManager.Instance.GetGridTileAtPosition(GridManager.Instance.ConvertWorldPosToGridPos(transform.position)));
            if (entity != null)
            {
                if (entity == this) return;
                entity.TakeDamage(m_ImpactDamage);
                Destroy(gameObject);
            }
        }
    }
}