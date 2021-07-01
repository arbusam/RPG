using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [SerializeField] Image heartPrefab;

        private void Update()
        {
            if (heartPrefab == null) return;

            GetHearts();
        }

        private void GetHearts()
        {
            Health health = GameObject.FindWithTag("Player").GetComponent<Health>();

            int hearts = (int) health.HealthPoints / 10;

            if (((health.HealthPoints / 10) % 1) != 0)
            {
                hearts++;
            }

            foreach (Transform item in this.transform)
            {
                Destroy(item.gameObject);
            }

            for (int i = hearts - 1; i >= 0; i--)
            {
                Image heartInstance = Instantiate(heartPrefab, this.transform);

                if (health.HealthPoints <= i*10)
                {
                    heartInstance.fillAmount = 0;
                }
                else if (health.HealthPoints > (i+1)*10)
                {
                    heartInstance.fillAmount = 1;
                }
                else
                {
                    heartInstance.fillAmount = (health.HealthPoints - (i*10)) / 10;
                }
            }
        }
    }
}