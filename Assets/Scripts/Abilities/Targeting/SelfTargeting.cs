using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Self Targeting", menuName = "RPG/Abilities/Targeting/Self")]
    public class SelfTargeting : TargetingStrategy
    {
        public override void StartTargeting(AbilityData data, Action finished)
        {
            data.Targets = new GameObject[]{data.User};
            data.TargetedPoint = data.User.transform.position;
            finished();
        }
    }
}