using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float health = 100f;
        BaseStats baseStats;
        public float HealthPoints
        {
            get
            {
                return health;
            }
        }
        bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        private void Start() {
            baseStats = GetComponent<BaseStats>();
            health = baseStats.GetHealth();
        }

        private void Die()
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().StopCurrentAction();
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            health = Mathf.Max(health-damage, 0);
            if (health <= 0)
            {
                if (!isDead)
                {
                    Die();
                    Experience experience = instigator.GetComponent<Experience>();
                    if (experience == null) return;
                    experience.GainExperience(baseStats.GetExperienceAward());
                }
            }
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

