using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Abilities
{
    public class AbilityData: IAction
    {
        GameObject user;
        Vector3 targetedPoint;
        IEnumerable<GameObject> targets;
        bool cancelled;

        public AbilityData(GameObject user)
        {
            this.user = user;
        }

        public IEnumerable<GameObject> Targets
        {
            get
            {
                return targets;
            }

            set
            {
                targets = value;
            }
        }

        public GameObject User
        {
            get
            {
                return user;
            }
        }

        public Vector3 TargetedPoint
        {
            get
            {
                return targetedPoint;
            }
            
            set
            {
                targetedPoint = value;
            }
        }

        public void StartCoroutine(IEnumerator coroutine)
        {
            user.GetComponent<MonoBehaviour>().StartCoroutine(coroutine);
        }

        public void Cancel()
        {
            cancelled = true;
        }

        public bool Cancelled
        {
            get
            {
                return cancelled;
            }
        }
    }
}