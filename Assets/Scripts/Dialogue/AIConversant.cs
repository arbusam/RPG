using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;
using RPG.Attributes;
using RPG.Movement;
using System;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {
        public string conversantName = "Name";
        [SerializeField] Dialogue dialogue = null;
        [SerializeField] float talkDistance = 2;

        public CursorMapping GetCursor(PlayerControls callingControls)
        {
            return callingControls.GetCursorMapping(CursorType.Dialogue);
        }

        public bool HandleRaycast(PlayerControls callingControls)
        {
            if (dialogue == null)
            {
                return false;
            }
            if (GetComponent<Health>().IsDead) return false;
            if (!callingControls.GetComponent<PlayerConversant>().CanConverse(dialogue)) return false;

            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(callingControls.GetComponent<Mover>().MoveToCoroutine(Vector3.MoveTowards(this.transform.position, callingControls.transform.position, talkDistance), 1f, () => StartDialogue(callingControls)));
            }
            return true;
        }

        private void StartDialogue(PlayerControls callingControls)
        {
            this.transform.LookAt(callingControls.transform, Vector3.up);
            callingControls.GetComponent<PlayerConversant>().StartDialogue(this, dialogue);
        }
    }
}