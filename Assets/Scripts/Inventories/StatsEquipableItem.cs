using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(menuName = "RPG/Inventory/Equippable Item", order = 0)]
    public class StatsEquipableItem : EquipableItem, IModifierProvider
    {
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
                if (modifier.stat != stat) continue;

                yield return modifier.value;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            foreach (Modifier modifier in percentageModifiers)
            {
                if (modifier.stat != stat) continue;

                yield return modifier.value;
            }
        }
    }
}