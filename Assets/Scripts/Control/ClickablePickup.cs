using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Control
{
    [RequireComponent(typeof(Pickup))]
    public class ClickablePickup : MonoBehaviour, IRaycastable
    {
        Pickup pickup;

        private void Awake()
        {
            pickup = GetComponent<Pickup>();
        }

        public CursorMapping GetCursor(PlayerControls callingControls)
        {
            if (pickup.CanBePickedUp())
            {
                return callingControls.GetCursorMapping(CursorType.Pickup);
            }
            else
            {
                return callingControls.GetCursorMapping(CursorType.FullPickup);
            }
        }

        public bool HandleRaycast(PlayerControls callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                pickup.PickupItem();
            }
            return true;
        }
    }
}