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
            yield return new ShopItem(InventoryItem.GetFromID("71e9656a-d5cc-40eb-a0d3-5faa5ba56217"), 10, 10.00f, 0);
            yield return new ShopItem(InventoryItem.GetFromID("f365a9be-ffb1-4acc-92c4-a46e3ce4ecc8"), 5, 100.00f, 0);
            yield return new ShopItem(InventoryItem.GetFromID("69323c06-7d38-4474-888a-ac6c94df3015"), 5, 50.00f, 0);
        }
        public void SelectFilter(ItemCategory category) {}
        public ItemCategory GetFilter() { return ItemCategory.None; }
        public void SelectMode(bool isBuying) {}
        public bool IsBuyingMode() { return true; }
        public bool CanTransact() { return true; }
        public void ConfirmTransaction() {}
        public float TransactionTotal() { return 0; }
        public void AddToTransaction(InventoryItem item, int quantity) {}

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