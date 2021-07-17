using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using GameDevTV.Saving;
using RPG.Control;
using RPG.Inventories;
using RPG.Movement;
using RPG.Stats;
using UnityEngine;

namespace RPG.Shops
{
    public class Shop : MonoBehaviour, IRaycastable, ISaveable
    {
        [SerializeField] string shopName;

        [SerializeField] StockItemConfig[] stockConfig;

        [SerializeField] float sellingPercentage = 80;

        [SerializeField] float shopDistance = 2;

        [Serializable]
        class StockItemConfig
        {
            public InventoryItem item;
            public int initialStock;
            public float buyingDiscountPercentage;
            [Min(1)] public int unlockLevel = 1;
        }

        Dictionary<InventoryItem, int> transaction = new Dictionary<InventoryItem, int>();
        Dictionary<InventoryItem, int> stockSold = new Dictionary<InventoryItem, int>();
        Shopper currentShopper = null;
        bool isBuyingMode = true;

        ItemCategory currentItemFilter;

        public string ShopName
        {
            get
            {
                return shopName;
            }
        }
        
        public event Action onChange;

        public void SetShopper(Shopper shopper)
        {
            currentShopper = shopper;
        }

        public IEnumerable<ShopItem> GetFilteredItems()
        {
            foreach (ShopItem item in GetAllItems())
            {
                InventoryItem inventoryItem = item.GetInventoryItem();
                if (currentItemFilter == ItemCategory.None || inventoryItem.GetCategory() == currentItemFilter)
                {
                    yield return item;
                }
            }
        }

        public IEnumerable<ShopItem> GetAllItems()
        {
            Dictionary<InventoryItem, float> prices = GetPrices();
            Dictionary<InventoryItem, int> availablilities = GetAvailabilities();
            foreach (InventoryItem item in availablilities.Keys)
            {
                if (availablilities[item] <= 0) continue;

                float price = prices[item];
                int quantityInTransaction = 0;
                transaction.TryGetValue(item, out quantityInTransaction);
                int availability = availablilities[item];
                yield return new ShopItem(item, availability, price, quantityInTransaction);
            }
        }

        private int CountItemsInInventory(InventoryItem item)
        {
            Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
            if (shopperInventory == null) return 0;

            int items = 0;

            for (int i = 0; i < shopperInventory.GetSize(); i++)
            {
                InventoryItem itemInSlot = shopperInventory.GetItemInSlot(i);
                if (itemInSlot == item) items += shopperInventory.GetNumberInSlot(i);
            }
            return items;
        }

        private Dictionary<InventoryItem, float> GetPrices()
        {
            Dictionary<InventoryItem, float> prices = new Dictionary<InventoryItem, float>();

            foreach (var config in GetAvailableConfigs())
            {
                if (isBuyingMode)
                {
                    if (!prices.ContainsKey(config.item))
                    {
                        prices[config.item] = config.item.GetPrice();
                    }

                    prices[config.item] *= (1 - config.buyingDiscountPercentage / 100);
                }
                else
                {
                    prices[config.item] = config.item.GetPrice() * (sellingPercentage / 100);
                }
            }

            return prices;
        }

        private Dictionary<InventoryItem, int> GetAvailabilities()
        {
            Dictionary<InventoryItem, int> availabilities = new Dictionary<InventoryItem, int>();

            foreach (var config in GetAvailableConfigs())
            {
                if (isBuyingMode)
                {
                    if (!availabilities.ContainsKey(config.item))
                    {
                        int soldAmount = 0;
                        stockSold.TryGetValue(config.item, out soldAmount);
                        availabilities[config.item] = -soldAmount;
                    }
                    availabilities[config.item] += config.initialStock;
                }
                else
                {
                    availabilities[config.item] = CountItemsInInventory(config.item);
                }
            }

            return availabilities;
        }

        private IEnumerable<StockItemConfig> GetAvailableConfigs()
        {
            int shopperLevel = GetShopperLevel();
            foreach (var config in stockConfig)
            {
                if (config.unlockLevel > shopperLevel) continue;
                yield return config;
            }
        }

        public void SelectFilter(ItemCategory category)
        {
            currentItemFilter = category;
            if (onChange != null)
            {
                onChange();
            }
        }

        public ItemCategory GetFilter()
        { 
            return currentItemFilter;
        }
        public void SelectMode(bool isBuying)
        {
            isBuyingMode = isBuying;
            if (onChange != null)
            {
                onChange();
            }
        }
        
        public bool IsBuyingMode()
        {
            return isBuyingMode;
        }
        
        public bool CanTransact()
        {
            if (IsTransactionEmpty()) return false;
            if (!HasSufficientFunds()) return false;
            if (!HasInventorySpace()) return false;
            return true;
        }

        public bool HasInventorySpace()
        {
            if (!isBuyingMode) return true;

            Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
            List<InventoryItem> flatItems = new List<InventoryItem>();
            foreach (ShopItem shopItem in GetAllItems())
            {
                InventoryItem item = shopItem.GetInventoryItem();
                int quantity = shopItem.GetQuantity();
                for (int i = 0; i < quantity; i++)
                {
                    flatItems.Add(item);
                }
            }
            return shopperInventory.HasSpaceFor(flatItems);
        }

        public void ConfirmTransaction()
        {
            Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
            Purse shopperPurse = currentShopper.GetComponent<Purse>();
            if (shopperInventory == null || shopperPurse == null) return;

            foreach (ShopItem shopItem in GetAllItems())
            {
                InventoryItem item = shopItem.GetInventoryItem();
                int quantity = shopItem.GetQuantity();
                float price = shopItem.GetPrice();
                for (int i = 0; i < quantity; i++)
                {
                    if (isBuyingMode)
                    {
                        BuyItem(shopperInventory, shopperPurse, item, price);
                    }
                    else
                    {
                        SellItem(shopperInventory, shopperPurse, item, price);
                    }
                }
            }
            if (onChange != null)
            {
                onChange();
            }
        }

        private void BuyItem(Inventory shopperInventory, Purse shopperPurse, InventoryItem item, float price)
        {
            if (shopperPurse.GetBalance() < price) return;
            bool success = shopperInventory.AddToFirstEmptySlot(item, 1);
            if (success)
            {
                AddToTransaction(item, -1);
                shopperPurse.UpdateBalance(-price);
                if (!stockSold.ContainsKey(item))
                {
                    stockSold[item] = 0;
                }
                stockSold[item]++;
            }
        }

        private void SellItem(Inventory shopperInventory, Purse shopperPurse, InventoryItem item, float price)
        {
            int slot = FindFirstItemSlot(shopperInventory, item);
            if (slot == -1) return;

            AddToTransaction(item, -1);
            shopperInventory.RemoveFromSlot(slot, 1);
            shopperPurse.UpdateBalance(price);
            if (!stockSold.ContainsKey(item))
            {
                stockSold[item] = 0;
            }
            stockSold[item]--;
        }

        private int FindFirstItemSlot(Inventory shopperInventory, InventoryItem item)
        {
            for (int i = 0; i < shopperInventory.GetSize(); i++)
            {
                InventoryItem itemInSlot = shopperInventory.GetItemInSlot(i);
                if (itemInSlot == item) return i;
            }
            return -1;
        }

        public float TransactionTotal()
        {
            float total = 0;
            foreach (ShopItem item in GetAllItems())
            {
                total += item.GetPrice() * item.GetQuantity();
            }
            return total;
        }

        public void AddToTransaction(InventoryItem item, int quantity)
        {
            if (!transaction.ContainsKey(item))
            {
                transaction[item] = 0;
            }

            var availabilities = GetAvailabilities();
            int availability = availabilities[item];
            if (transaction[item] + quantity > availability)
            {
                transaction[item] = availability;
            }
            else
            {
                transaction[item] += quantity;
            }

            if (transaction[item] <= 0)
            {
                transaction.Remove(item);
            }

            if (onChange != null)
            {
                onChange();
            }
        }

        public bool HasSufficientFunds()
        {
            if (!isBuyingMode) return true;

            Purse shopperPurse = currentShopper.GetComponent<Purse>();
            if (shopperPurse == null) return false;

            return shopperPurse.GetBalance() >= TransactionTotal();
        }

        private bool IsTransactionEmpty()
        {
            return transaction.Count == 0;
        }

        private int GetShopperLevel()
        {
            BaseStats stats = currentShopper.GetComponent<BaseStats>();
            if (stats == null) return 0;

            return stats.GetLevel();
        }

        public CursorMapping GetCursor(PlayerControls callingControls)
        {
            return callingControls.GetCursorMapping(CursorType.Shop);
        }

        public bool HandleRaycast(PlayerControls callingControls)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(callingControls.GetComponent<Mover>().MoveToCoroutine(Vector3.MoveTowards(this.transform.position, callingControls.transform.position, shopDistance), 1f, () => SetShop(callingControls)));
            }
            return true;
        }

        private void SetShop(PlayerControls callingControls)
        {
            callingControls.GetComponent<Shopper>().SetActiveShop(this);
        }

        public object CaptureState()
        {
            Dictionary<string, int> saveObject = new Dictionary<string, int>();
            foreach (var pair in stockSold)
            {
                saveObject[pair.Key.GetItemID()] = pair.Value;
            }
            return saveObject;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, int> saveObject = (Dictionary<string, int>) state;
            stockSold.Clear();
            foreach (var pair in saveObject)
            {
                stockSold[InventoryItem.GetFromID(pair.Key)] = pair.Value;                
            }
        }
    }
}