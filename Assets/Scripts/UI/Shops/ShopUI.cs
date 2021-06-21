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
        [SerializeField] TextMeshProUGUI confirmButtonText;
        [SerializeField] TextMeshProUGUI inventoryFullText;
        [SerializeField] Button switchButton;

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
            switchButton.onClick.AddListener(SwitchMode);

            ShopChanged();
        }

        private void ShopChanged()
        {
            if (currentShop != null) currentShop.onChange -= RefreshUI;
            currentShop = shopper.ActiveShop;
            this.gameObject.SetActive(currentShop != null);

            foreach (FilterButtonUI button in GetComponentsInChildren<FilterButtonUI>())
            {
                button.SetShop(currentShop);
            }

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

            foreach (FilterButtonUI button in GetComponentsInChildren<FilterButtonUI>())
            {
                button.RefreshUI();
            }

            TextMeshProUGUI switchText = switchButton.GetComponentInChildren<TextMeshProUGUI>();
            if (currentShop.IsBuyingMode())
            {
                switchText.text = "Switch To Selling";
                confirmButtonText.text = "Buy";
            }
            else
            {
                switchText.text = "Switch To Buying";
                confirmButtonText.text = "Sell";
            }
        }

        public void Close()
        {
            shopper.SetActiveShop(null);
        }

        public void ConfirmTransaction()
        {
            currentShop.ConfirmTransaction();
        }

        public void SwitchMode()
        {
            currentShop.SelectMode(!currentShop.IsBuyingMode());
        }
    }
}