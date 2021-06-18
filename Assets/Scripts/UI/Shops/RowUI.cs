using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Shops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{    
    public class RowUI : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] TextMeshProUGUI nameField;
        [SerializeField] TextMeshProUGUI availability;
        [SerializeField] TextMeshProUGUI price;
        [SerializeField] TextMeshProUGUI quantity;

        Shop currentShop = null;
        ShopItem currentItem;

        public void Setup(Shop shop, ShopItem shopItem)
        {
            icon.sprite = shopItem.GetSprite();
            nameField.text = shopItem.GetName();
            availability.text = shopItem.GetAvailablility();
            price.text = shopItem.GetPrice();
            quantity.text = shopItem.GetQuantity().ToString();
            currentShop = shop;
            currentItem = shopItem;
        }

        public void Add()
        {
            if (currentShop == null) return;
            
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                currentShop.AddToTransaction(currentItem.GetInventoryItem(), 10);
            }
            else
            {
                currentShop.AddToTransaction(currentItem.GetInventoryItem(), 1);
            }
        }

        public void Remove()
        {
            if (currentShop == null) return;
            
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                currentShop.AddToTransaction(currentItem.GetInventoryItem(), -10);
            }
            else
            {
                currentShop.AddToTransaction(currentItem.GetInventoryItem(), -1);
            }
        }
    }
}