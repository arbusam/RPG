using System;
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

        public override void Use(GameObject user)
        {
            if (!user.GetComponent<Mana>().UseMana(manaRequirement)) return;

            if (user.GetComponent<CooldownStore>().GetTimeRemaining(this) != 0) return;
            
            AbilityData data = new AbilityData(user);

            ActionScheduler actionScheduler = user.GetComponent<ActionScheduler>();
            actionScheduler.StartAction(data);

            targetingStrategy.StartTargeting(data, () => {
                TargetAquired(data);
            });
        }

        private void TargetAquired(AbilityData data)
        {
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