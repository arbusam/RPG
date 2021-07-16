using System;
using GameDevTV.Inventories;
using GameDevTV.Saving;
using UnityEngine;

namespace RPG.Inventories
{
    public class Purse : MonoBehaviour, ISaveable, IItemStore
    {
        [SerializeField] float startingBalance = 100f;

        public event Action onChange;

        float balance = 0;

        private void Awake()
        {
            balance = startingBalance;
        }
        
        public float GetBalance()
        {
            return balance;
        }

        public void UpdateBalance(float amount)
        {
            balance += amount;
            if (onChange != null)
            {
                onChange();
            }
        }

        public object CaptureState()
        {
            return balance;
        }

        public void RestoreState(object state)
        {
            balance = (float) state;
        }

        public int AddItems(InventoryItem item, int number)
        {
            CurrencyItem currencyItem = item as CurrencyItem;
            if (currencyItem == null) return 0;

            UpdateBalance(currencyItem.GetPrice() * number);
            return number;
        }
    }
}