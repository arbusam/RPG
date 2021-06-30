using System;
using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Abilities
{
    public class CooldownStore : MonoBehaviour
    {
        Dictionary<InventoryItem, float> cooldownTimers = new Dictionary<InventoryItem, float>();
        Dictionary<InventoryItem, float> initialTimerValues = new Dictionary<InventoryItem, float>();

        void Update()
        {
            List<InventoryItem> keys = new List<InventoryItem>(cooldownTimers.Keys);
            foreach (InventoryItem item in keys)
            {
                cooldownTimers[item] -= Time.deltaTime;
                if (cooldownTimers[item] <= 0)
                {
                    cooldownTimers.Remove(item);
                    initialTimerValues.Remove(item);
                }
            }
        }

        public void StartCooldown(InventoryItem item, float cooldownTime)
        {
            cooldownTimers[item] = cooldownTime;
            initialTimerValues[item] = cooldownTime;
        }

        public float GetTimeRemaining(InventoryItem item)
        {
            if (!cooldownTimers.ContainsKey(item))
            {
                return 0;
            }

            return cooldownTimers[item];
        }

        public float GetFractionRemaining(InventoryItem item)
        {
            if (item == null) return 0;

            if (!cooldownTimers.ContainsKey(item)) return 0;

            return cooldownTimers[item] / initialTimerValues[item];
        }
    }
}