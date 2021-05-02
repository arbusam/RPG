using UnityEngine;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float health = 100f;
        bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        void Update()
        {
            if (health <= 0)
            {
                if (!isDead)
                {
                    Die();
                }
            }
        }

        private void Die()
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().StopCurrentAction();
        }

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health-damage, 0);
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float)state;
        }
    }  
}

