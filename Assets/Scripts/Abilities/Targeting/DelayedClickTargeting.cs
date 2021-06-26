using System.Collections;
using RPG.Control;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Delayed Click Targeting", menuName = "RPG/Abilities/Targeting/Delayed Click")]
    public class DelayedClickTargeting : TargetingStrategy
    {
        [SerializeField] Texture2D cursorTexture;
        [SerializeField] Vector2 cursorHotspot;

        public override void StartTargeting(GameObject user)
        {
            PlayerControls playerControls = user.GetComponent<PlayerControls>();
            playerControls.StartCoroutine(Targeting(user, playerControls));
        }

        private IEnumerator Targeting(GameObject user, PlayerControls playerControls)
        {
            playerControls.enabled = false;
            while (true)
            {
                Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);

                if (Input.GetMouseButtonDown(0))
                {
                    while (Input.GetMouseButtonDown(0))
                    {
                        yield return null;
                    }
                    playerControls.enabled = true;
                    yield break;
                }
                yield return null;
            }
        }
    }
}