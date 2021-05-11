using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class WeaponLabel : MonoBehaviour
    {
        Fighter fighter;
        Text text;

        void Start()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            text = GetComponent<Text>();
        }

        void Update()
        {
            if (fighter.IsReady)
            {
                text.text = "Weapon Ready";
            }
            else
            {
                text.text = "Charging Up";
            }
        }
    }

}