using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities
{
    public class AbilityData
    {
        GameObject user;
        Vector3 targetedPoint;
        IEnumerable<GameObject> targets;

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
    }
}