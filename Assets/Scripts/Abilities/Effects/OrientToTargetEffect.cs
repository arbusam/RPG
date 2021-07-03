using System;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Orient To Target Effect", menuName = "RPG/Abilities/Effects/Orient To Target")]
    public class OrientToTargetEffect : EffectStrategy
    {
        public override void StartEffect(AbilityData data, Action finished)
        {
            data.User.transform.LookAt(data.TargetedPoint);
            finished();
        }
    }
}