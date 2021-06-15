using System.Collections;
using System.Collections.Generic;
using RPG.Shops;
using TMPro;
using UnityEngine;

namespace RPG.UI.Shops
{    
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI shopName;

        Shopper shopper = null;
        Shop currentShop = null;

        void Start()
        {
            shopper = GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>();
            if (shopper == null) return;

            shopper.activeShopChanged += ShopChanged;

            ShopChanged();
        }

        private void ShopChanged()
        {
            currentShop = shopper.GetActiveShop();
            if (currentShop != null)
            {
                shopName.text = currentShop.ShopName;
            }
            this.gameObject.SetActive(currentShop != null);
        }

        public void Close()
        {
            shopper.SetActiveShop(null);
        }
    }
}