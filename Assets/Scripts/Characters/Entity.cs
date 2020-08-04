using System;
using UnityEngine;

namespace TBSG.Combat
{
    public class Entity : MonoBehaviour
    {
        public event Action OnLifePointsChanged;
        public event Action OnEntityDie;

        [SerializeField] private int m_StartLifePoints = 1;
        
        [NaughtyAttributes.ReadOnly, SerializeField] private int m_CurrentLifePoints;
        public int CurrentLifePoints => m_CurrentLifePoints;

        protected virtual void Start()
        {
            m_CurrentLifePoints = m_StartLifePoints;
            OnLifePointsChanged?.Invoke();
        }

        public void TakeDamage(int damage)
        {
            m_CurrentLifePoints -= damage;
            OnLifePointsChanged?.Invoke();
            if (m_CurrentLifePoints <= 0)
                OnDie();
        }

        private void OnDie()
        {
            OnEntityDie?.Invoke();
            Destroy(gameObject);
        }

    }
}