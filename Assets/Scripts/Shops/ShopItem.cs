using System;
using System.Globalization;
using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Shops
{
    public class ShopItem
    {
        InventoryItem item;
        int availability;
        float price;
        int quantityInTransaction;

        public ShopItem(InventoryItem item, int availability, float price, int quantityInTransaction)
        {
            this.item = item;
            this.availability = availability;
            this.price = price;
            this.quantityInTransaction = quantityInTransaction;
        }

        public Sprite GetSprite()
        {
            return item.GetIcon();
        }

        public string GetName()
        {
            return item.GetDisplayName();
        }

        public string GetAvailablility()
        {
            return availability.ToString();
        }

        public string GetPrice()
        {
            return $"{price.ToString("C", CultureInfo.CurrentCulture)}";
        }

        public int GetQuantity()
        {
            return quantityInTransaction;
        }

        public InventoryItem GetInventoryItem()
        {
            return item;
        }
    }
}