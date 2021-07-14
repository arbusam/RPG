using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;
using RPG.Attributes;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {
        public string conversantName = "Name";
        [SerializeField] Dialogue dialogue = null;

        public CursorMapping GetCursor(PlayerControls callingControls)
        {
            return callingControls.GetCursorMapping(CursorType.Dialogue);
        }

        public bool HandleRaycast(PlayerControls callingController)
        {
            if (dialogue == null)
            {
                return false;
            }
            if (GetComponent<Health>().IsDead) return false;

            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<PlayerConversant>().StartDialogue(this, dialogue);
            }
            return true;
        }
    }
}