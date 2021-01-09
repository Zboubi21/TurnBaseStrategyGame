using System;
using UnityEngine;
using TBSG.UI;

namespace TBSG.Combat
{
    public class Entity : MonoBehaviour
    {
        public event Action OnLifePointsChanged;
        public static event Action<Entity> OnEntityDie;

        [SerializeField] private int m_StartLifePoints = 1;
        
        [NaughtyAttributes.ReadOnly, SerializeField] private int m_CurrentLifePoints;
        public int CurrentLifePoints => m_CurrentLifePoints;

        [NaughtyAttributes.ReadOnly, SerializeField] protected Spell m_InvokedSpell = null;

        protected virtual void Start()
        {
            m_CurrentLifePoints = m_StartLifePoints;
            OnLifePointsChanged?.Invoke();
        }

        public void OnEntityInvoked(Spell invokedSpell)
        {
            m_InvokedSpell = invokedSpell;
        }

        public void TakeDamage(int damage)
        {
            m_CurrentLifePoints -= damage;
            UIPopupManager.Instance.AddDamagedPopup(transform.position, damage, new Vector3(0, 1.5f, 0));
            OnLifePointsChanged?.Invoke();
            if (m_CurrentLifePoints <= 0)
                OnDie();
        }

        protected virtual void OnDie()
        {
            OnTriggerDie();
            Destroy(gameObject);
        }

        protected virtual void OnTriggerDie()
        {
            OnEntityDie?.Invoke(this);
        }
    }
}