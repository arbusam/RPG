using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using GameDevTV.Utils;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        LazyValue<float> health;
        bool restoredHealth = false;
        BaseStats baseStats;
        public float HealthPoints
        {
            get
            {
                return health.value;
            }
        }
        public float MaxHealthPoints
        {
            get
            {
                return GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

        bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        private void Awake() {
            baseStats = GetComponent<BaseStats>();
            health = new LazyValue<float>(GetInitialHeath);
            // if (!restoredHealth)
            // {
            //     health = baseStats.GetStat(Stat.Health);
        }

        private void Start() {
            health.ForceInit();
        }

        private float GetInitialHeath()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void OnEnable() {
            baseStats.onLevelUp += ResetHealth;
        }

        private void OnDisable() {
            baseStats.onLevelUp -= ResetHealth;
        }

        private void ResetHealth()
        {
            health.value = baseStats.GetStat(Stat.Health);
        }

        private void Die()
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().StopCurrentAction();
        }

        public void TakeDamage(GameObject instigator, float damage)
        {

            health.value = Mathf.Max(health.value - damage, 0);
            if (health.value <= 0)
            {
                if (!isDead)
                {
                    Die();
                    Experience experience = instigator.GetComponent<Experience>();
                    if (experience == null) return;
                    experience.GainExperience(baseStats.GetStat(Stat.ExperienceReward));
                }
            }
        }

        public object CaptureState()
        {
            return health.value;
        }

        public void RestoreState(object state)
        {
            restoredHealth = true;
            health.value = (float)state;
            if (health.value <= 0)
            {
                Die();
            }
        }
    }
}