using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class ManaDisplay : MonoBehaviour
    {
        Mana mana;
        Text text;

        private void Awake()
        {
            mana = GameObject.FindWithTag("Player").GetComponent<Mana>();
            text = GetComponent<Text>();
        }

        private void Update()
        {
            text.text = mana.GetMana().ToString() + "/" + mana.GetMaxMana().ToString();
        }
    }
}