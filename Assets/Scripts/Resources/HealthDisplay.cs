using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resources
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        Text text;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            text = GetComponent<Text>();
        }

        private void Update() {
            text.text = health.HealthPoints.ToString();
        }
    }
}