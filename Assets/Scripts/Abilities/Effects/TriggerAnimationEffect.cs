using System;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Trigger Animation Effect", menuName = "RPG/Abilities/Effects/Trigger Animation")]
    public class TriggerAnimationEffect : EffectStrategy
    {
        [SerializeField] string animationTrigger;

        public override void StartEffect(AbilityData data, Action finished)
        {
            Animator animator = data.User.GetComponent<Animator>();
            animator.SetTrigger(animationTrigger);
            finished();
        }
    }
}