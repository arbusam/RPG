using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {

        Transform target;

        private void Update()
        {
            if (target != null) 
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;
        }
    }

}