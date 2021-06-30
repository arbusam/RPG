using System;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Abilities.Filters
{
    [CreateAssetMenu(fileName = "Health Effect", menuName = "RPG/Abilities/Effects/Health")]
    public class HealthEffect : EffectStrategy
    {
        [SerializeField] float healthChange;

        public override void StartEffect(GameObject user, IEnumerable<GameObject> targets, Action finished)
        {
            foreach (GameObject target in targets)
            {
                Health health = target.GetComponent<Health>();
                if (health == null) continue;

                health.TakeDamage(user, -healthChange);
            }
            finished();
        }
    }
}