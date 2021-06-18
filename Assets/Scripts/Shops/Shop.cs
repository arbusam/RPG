using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Control;
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

        public string ShopName
        {
            get
            {
                return shopName;
            }
        }
        
        public event Action onChange;

        public IEnumerable<ShopItem> GetFilteredItems()
        {
            foreach (StockItemConfig config in stockConfig)
            {
                float price = config.item.GetPrice() * (1 - config.buyingDiscountPercentage/100);
                yield return new ShopItem(config.item, config.initialStock, price, 0);
            }
        }
        public void SelectFilter(ItemCategory category) {}
        public ItemCategory GetFilter() { return ItemCategory.None; }
        public void SelectMode(bool isBuying) {}
        public bool IsBuyingMode() { return true; }
        public bool CanTransact() { return true; }
        public void ConfirmTransaction() {}
        public float TransactionTotal() { return 0; }
        public void AddToTransaction(InventoryItem item, int quantity)
        {
            print("Added " + item.name + " x " + quantity);
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