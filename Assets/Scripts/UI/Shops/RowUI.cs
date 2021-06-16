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

        public void Setup(ShopItem shopItem)
        {
            icon.sprite = shopItem.GetSprite();
            nameField.text = shopItem.GetName();
            availability.text = shopItem.GetAvailablility();
            price.text = shopItem.GetPrice();
        }
    }
}