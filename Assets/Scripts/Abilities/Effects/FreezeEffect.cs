using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Abilities.Filters
{
    [CreateAssetMenu(fileName = "Freeze Effect", menuName = "RPG/Abilities/Effects/Freeze")]
    public class FreezeEffect : EffectStrategy
    {
        [SerializeField] float freezeTime;

        public override void StartEffect(GameObject user, IEnumerable<GameObject> targets, Action finished)
        {
            foreach (GameObject target in targets)
            {
                Mover mover = target.GetComponent<Mover>();
                if (mover == null) continue;

                mover.GetComponent<ActionScheduler>().StopCurrentAction();
                mover.canMove = false;
                mover.StartCoroutine(UnfreezeInSeconds(freezeTime, targets));
            }
            finished();
        }

        private IEnumerator UnfreezeInSeconds(float freezeTime, IEnumerable<GameObject> targets)
        {
            yield return new WaitForSeconds(freezeTime);
            foreach (GameObject target in targets)
            {
                Mover mover = target.GetComponent<Mover>();
                if (mover == null) continue;

                mover.canMove = true;
            }
        }
    }
}