using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon;
        [SerializeField] float respawnTime = 10f;

        void OnTriggerEnter(Collider other) {
            if (other.gameObject.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                StartCoroutine(HideFor(respawnTime));
            }
            // Destroy(this.gameObject);
        }

        private IEnumerator HideFor(float seconds)
        {
            HidePickup();
            yield return new WaitForSeconds(seconds);
            ShowPickup();
        }

        private void ShowPickup()
        {
            GetComponent<Collider>().enabled = true;
            foreach (Transform child in this.transform)
            {
                child.gameObject.SetActive(true);
            }
        }

        private void HidePickup()
        {
            GetComponent<Collider>().enabled = false;
            foreach (Transform child in this.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

}