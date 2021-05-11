using RPG.Combat;
using RPG.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;
        Text text;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            text = GetComponent<Text>();
        }

        private void Update() {
            Transform target = fighter.Target;
            if (target == null)
            {
                text.text = "N/A";
                return;
            }
            text.text = target.GetComponent<Health>().HealthPoints.ToString();
        }
    }
}