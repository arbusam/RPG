using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Delayed Click Targeting", menuName = "RPG/Abilities/Targeting/Delayed Click")]
    public class DelayedClickTargeting : TargetingStrategy
    {
        [SerializeField] Texture2D cursorTexture;
        [SerializeField] Vector2 cursorHotspot;

        public override void StartTargeting(GameObject user, Action<IEnumerable<GameObject>> finished)
        {
            PlayerControls playerControls = user.GetComponent<PlayerControls>();
            playerControls.StartCoroutine(Targeting(user, playerControls, finished));
        }

        private IEnumerator Targeting(GameObject user, PlayerControls playerControls, Action<IEnumerable<GameObject>> finished)
        {
            playerControls.enabled = false;
            while (true)
            {
                Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);

                if (Input.GetMouseButtonDown(0))
                {
                    yield return new WaitWhile(() => Input.GetMouseButton(0));
                    playerControls.enabled = true;
                    finished(null);
                    yield break;
                }
                yield return null;
            }
        }
    }
}