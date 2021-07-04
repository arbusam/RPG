using System;
using System.Collections;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Delay Composite", menuName = "RPG/Abilities/Effects/Delay Composite")]
    public class DelayCompositeEffect : EffectStrategy
    {
        [SerializeField] float delay = 0;
        [SerializeField] EffectStrategy[] delayedEffects;
        [SerializeField] bool abortIfCancelled = false;

        public override void StartEffect(AbilityData data, Action finished)
        {
            data.StartCoroutine(DelayedEffects(data, finished));
        }

        private IEnumerator DelayedEffects(AbilityData data, Action finished)
        {
            yield return new WaitForSeconds(delay);
            if (abortIfCancelled && data.Cancelled) yield break;
            foreach (EffectStrategy effect in delayedEffects)
            {
                effect.StartEffect(data, finished);
            }
        }
    }
}