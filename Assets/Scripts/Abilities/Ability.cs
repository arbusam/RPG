using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Attributes;
using RPG.Core;
using UnityEngine;

namespace RPG.Abilities
{   
    [CreateAssetMenu(fileName = "New Ability", menuName = "RPG/Abilities/Ability")]
    public class Ability : ActionItem
    {
        [SerializeField] TargetingStrategy targetingStrategy;
        [SerializeField] FilterStrategy[] filterStrategies;
        [SerializeField] EffectStrategy[] effectStrategies;

        [SerializeField] float manaRequirement;
        [SerializeField] float cooldownTime;

        [SerializeField] float duration = 0;
        [SerializeField] [Min(0.1f)] float delay = 0.1f;

        public override void Use(GameObject user)
        {
            if (user.GetComponent<CooldownStore>().GetTimeRemaining(this) != 0) return;
            if (!user.GetComponent<Mana>().UseMana(manaRequirement)) return;
            
            AbilityData data = new AbilityData(user);

            ActionScheduler actionScheduler = user.GetComponent<ActionScheduler>();
            actionScheduler.StartAction(data);

            if (duration > 0)
            {
                user.GetComponent<MonoBehaviour>().StartCoroutine(TargetingLoop(data));
            }
            else
            {
                targetingStrategy.StartTargeting(data, () => {
                    TargetAquired(data);
                });
            }
        }

        private IEnumerator TargetingLoop(AbilityData data)
        {
            float timePassed = 0;
            
            while (true)
            {
                yield return new WaitForSeconds(delay);
                timePassed += delay;
                targetingStrategy.StartTargeting(data, () => {
                    TargetAquired(data);
                });
                if (timePassed >= duration) yield break;
            }
        }

        private void TargetAquired(AbilityData data)
        {
            if (data.Cancelled) return;

            data.User.GetComponent<CooldownStore>().StartCooldown(this, cooldownTime);
            foreach (var filterStrategy in filterStrategies)
            {
                data.Targets = filterStrategy.Filter(data.Targets);
            }

            foreach (var effect in effectStrategies)
            {
                effect.StartEffect(data, EffectFinished);
            }
        }

        private void EffectFinished()
        {
            
        }
    }
}