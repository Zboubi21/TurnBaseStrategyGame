using UnityEngine;

namespace TBSG.Combat
{
    public class Entity : MonoBehaviour
    {
        [SerializeField] private int m_StartLifePoints = 1;
        
        [NaughtyAttributes.ReadOnly, SerializeField] private int m_CurrentLifePoints;

        protected virtual void Start()
        {
            m_CurrentLifePoints = m_StartLifePoints;
        }

        public void TakeDamage(int damage)
        {
            m_CurrentLifePoints -= damage;
            if (m_CurrentLifePoints <= 0)
                OnDie();
        }

        private void OnDie()
        {
            Destroy(gameObject);
        }

    }
}