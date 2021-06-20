using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using RPG.Shops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{    
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI shopName;
        [SerializeField] Transform listRoot;
        [SerializeField] RowUI rowPrefab;
        [SerializeField] TextMeshProUGUI totalField;
        [SerializeField] Button confirmButton;
        [SerializeField] TextMeshProUGUI inventoryFullText;

        Shopper shopper = null;
        Shop currentShop = null;

        Color originalTotalTextColor;

        void Start()
        {
            originalTotalTextColor = totalField.color;

            shopper = GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>();
            if (shopper == null) return;

            shopper.activeShopChanged += ShopChanged;
            confirmButton.onClick.AddListener(ConfirmTransaction);

            ShopChanged();
        }

        private void ShopChanged()
        {
            if (currentShop != null) currentShop.onChange -= RefreshUI;
            currentShop = shopper.ActiveShop;
            this.gameObject.SetActive(currentShop != null);
            if (currentShop == null) return;
            shopName.text = currentShop.ShopName;

            currentShop.onChange += RefreshUI;

            RefreshUI();
        }

        private void RefreshUI()
        {
            foreach (Transform item in listRoot)
            {
                Destroy(item.gameObject);
            }
            foreach (ShopItem shopItem in currentShop.GetFilteredItems())
            {
                RowUI row = Instantiate<RowUI>(rowPrefab, listRoot);
                row.Setup(currentShop, shopItem);
            }

            totalField.text = $"Total: {currentShop.TransactionTotal().ToString("C", CultureInfo.CurrentCulture)}";
            totalField.color = currentShop.HasSufficientFunds() ? originalTotalTextColor : Color.red;
            inventoryFullText.gameObject.SetActive(!currentShop.HasInventorySpace());
            confirmButton.interactable = currentShop.CanTransact();
        }

        public void Close()
        {
            shopper.SetActiveShop(null);
        }

        public void ConfirmTransaction()
        {
            currentShop.ConfirmTransaction();
        }
    }
}