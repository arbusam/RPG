using System;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Health Effect", menuName = "RPG/Abilities/Effects/Health")]
    public class HealthEffect : EffectStrategy
    {
        [SerializeField] float healthChange;

        public override void StartEffect(AbilityData data, Action finished)
        {
            foreach (GameObject target in data.Targets)
            {
                Health health = target.GetComponent<Health>();
                if (health == null) continue;
                if (health.IsDead()) continue;

                health.TakeDamage(data.User, -healthChange);
            }
            finished();
        }
    }
}