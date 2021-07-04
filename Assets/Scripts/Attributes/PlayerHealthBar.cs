using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [SerializeField] PlayerHealthIcon heartPrefab;
        [SerializeField] Image emptyHealthPrefab;

        private void Update()
        {
            if (heartPrefab == null || emptyHealthPrefab == null) return;

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

            for (int i = 0; i < hearts; i++)
            {
                Image heartInstance;

                if (health.HealthPoints <= i*10)
                {
                    heartInstance = Instantiate(emptyHealthPrefab, this.transform);
                }
                else if (health.HealthPoints > (i+1)*10)
                {
                    heartInstance = Instantiate(heartPrefab, this.transform).heartImage;
                    heartInstance.fillAmount = 1;
                }
                else
                {
                    heartInstance = Instantiate(heartPrefab, this.transform).heartImage;
                    heartInstance.fillAmount = (health.HealthPoints - (i*10)) / 10;
                }
            }

            int emptyHearts = (int) (health.MaxHealthPoints - (hearts * 10)) / 10;

            for (int i = 0; i < emptyHearts; i++)
            {
                Instantiate(emptyHealthPrefab, this.transform);
            }
        }
    }
}