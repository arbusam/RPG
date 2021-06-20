using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Control;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Shops
{
    public class Shop : MonoBehaviour, IRaycastable
    {
        [SerializeField] string shopName;

        [SerializeField] StockItemConfig[] stockConfig;

        [Serializable]
        class StockItemConfig
        {
            public InventoryItem item;
            public int initialStock;
            public float buyingDiscountPercentage;
        }

        Dictionary<InventoryItem, int> transaction = new Dictionary<InventoryItem, int>();
        Dictionary<InventoryItem, int> stock = new Dictionary<InventoryItem, int>();
        Shopper currentShopper = null;

        public string ShopName
        {
            get
            {
                return shopName;
            }
        }
        
        public event Action onChange;

        private void Awake()
        {
            foreach (StockItemConfig config in stockConfig)
            {
                stock[config.item] = config.initialStock;
            }
        }

        public void SetShopper(Shopper shopper)
        {
            currentShopper = shopper;
        }

        public IEnumerable<ShopItem> GetFilteredItems()
        {
            return GetAllItems();
        }

        public IEnumerable<ShopItem> GetAllItems()
        {
            foreach (StockItemConfig config in stockConfig)
            {
                float price = config.item.GetPrice() * (1 - config.buyingDiscountPercentage/100);
                int quantityInTransaction = 0;
                transaction.TryGetValue(config.item, out quantityInTransaction);
                int currentStock = stock[config.item];
                yield return new ShopItem(config.item, currentStock, price, quantityInTransaction);
            }
        }

        public void SelectFilter(ItemCategory category) {}
        public ItemCategory GetFilter() { return ItemCategory.None; }
        public void SelectMode(bool isBuying) {}
        public bool IsBuyingMode() { return true; }
        public bool CanTransact()
        {
            if (IsTransactionEmpty()) return false;
            if (!HasSufficientFunds()) return false;
            if (!HasInventorySpace()) return false;
            return true;
        }

        public bool HasInventorySpace()
        {
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
                    if (shopperPurse.GetBalance() < price) break;
                    bool success = shopperInventory.AddToFirstEmptySlot(item, 1);
                    if (success)
                    {
                        AddToTransaction(item, -1);
                        shopperPurse.UpdateBalance(-price);
                        stock[item] --;
                    }
                }
            }
            if (onChange != null)
            {
                onChange();
            }
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

            transaction[item] += quantity;

            if (transaction[item] > stock[item])
            {
                transaction[item] = stock[item];
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
            Purse shopperPurse = currentShopper.GetComponent<Purse>();
            if (shopperPurse == null) return false;

            return shopperPurse.GetBalance() >= TransactionTotal();
        }

        private bool IsTransactionEmpty()
        {
            return transaction.Count == 0;
        }

        public CursorMapping GetCursor(PlayerControls callingControls)
        {
            return callingControls.GetCursorMapping(CursorType.Shop);
        }

        public bool HandleRaycast(PlayerControls callingControls)
        {
            if (Input.GetMouseButtonDown(0))
            {
                callingControls.GetComponent<Shopper>().SetActiveShop(this);
            }
            return true;
        }
    }
}