using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities
{
    public class AbilityData
    {
        GameObject user;
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
    }
}