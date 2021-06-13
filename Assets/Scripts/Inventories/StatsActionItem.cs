using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(menuName = "RPG/Inventory/Action Item", order = 0)]
    public class StatsActionItem : ActionItem, IModifierProvider
    {
        [SerializeField]
        float time;
        [SerializeField]
        Modifier[] additiveModifiers;
        [SerializeField]
        Modifier[] percentageModifiers;

        [System.Serializable]
        struct Modifier
        {
            public Stat stat;
            public float value;
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            foreach (Modifier modifier in additiveModifiers)
            {
                Debug.Log("Success, Sort of");
                if (modifier.stat != stat) continue;
                if (timeSinceUse > time) continue;
                Debug.Log("Success");

                yield return modifier.value;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            foreach (Modifier modifier in percentageModifiers)
            {
                Debug.Log("Success, Sort of");
                if (modifier.stat != stat) continue;
                if (timeSinceUse > time) continue;
                Debug.Log("Success");

                yield return modifier.value;
            }
        }

        public override void Use(GameObject user)
        {
            base.Use(user);
            timeSinceUse = 0;
            Debug.Log("Stuff");
        }
    }
}