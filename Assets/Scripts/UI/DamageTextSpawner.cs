using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageText;

        public void Spawn(float damageAmount)
        {
            DamageText instance = Instantiate(damageText, this.transform);
            instance.SetValue(damageAmount);
        }
    }
}
