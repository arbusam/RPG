using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Abilities
{   
    [CreateAssetMenu(fileName = "New Ability", menuName = "RPG/Abilities/Ability")]
    public class Ability : ActionItem
    {
        [SerializeField] TargetingStrategy targetingStrategy;
        [SerializeField] FilterStrategy[] filterStrategies;

        public override void Use(GameObject user)
        {
            targetingStrategy.StartTargeting(user, TargetAquired);
        }

        private void TargetAquired(IEnumerable<GameObject> targets)
        {
            foreach (var filterStrategy in filterStrategies)
            {
                targets = filterStrategy.Filter(targets);
            }

            foreach (GameObject target in targets)
            {
                Debug.Log(target.name);
            }
        }
    }
}