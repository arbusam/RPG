using System.Globalization;
using RPG.Inventories;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class PurseUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI balance;

        Purse playerPurse = null;

        private void Start()
        {
            playerPurse = GameObject.FindGameObjectWithTag("Player").GetComponent<Purse>();
            if (playerPurse != null) playerPurse.onChange += RefreshUI;

            RefreshUI();
        }

        private void RefreshUI()
        {
            balance.text = playerPurse.GetBalance().ToString("C", CultureInfo.CurrentCulture);
        }
    }
}