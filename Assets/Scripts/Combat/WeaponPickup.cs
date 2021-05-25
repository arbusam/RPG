using System.Collections;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weapon;
        [SerializeField] float respawnTime = 10f;

        void OnTriggerEnter(Collider other) {
            if (other.gameObject.tag == "Player")
            {
                Pickup(other.GetComponent<Fighter>());
            }
        }

        private void Pickup(Fighter other)
        {
            other.EquipWeapon(weapon);
            StartCoroutine(HideFor(respawnTime));
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

        public bool HandleRaycast(PlayerControls callingControls)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(callingControls.GetComponent<Fighter>());
            }
            return true;
        }

        public CursorMapping GetCursor(PlayerControls callingControls)
        {
            return callingControls.GetCursorMapping(CursorType.Pickup);
        }
    }

}