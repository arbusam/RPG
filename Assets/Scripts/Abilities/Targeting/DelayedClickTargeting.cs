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
        [SerializeField] LayerMask layerMask;
        [SerializeField] float areaAffectRadius;
        [SerializeField] Transform targetingPrefab;

        Transform targetingInstance = null;

        public override void StartTargeting(GameObject user, Action<IEnumerable<GameObject>> finished)
        {
            PlayerControls playerControls = user.GetComponent<PlayerControls>();
            playerControls.StartCoroutine(Targeting(user, playerControls, finished));
        }

        private IEnumerator Targeting(GameObject user, PlayerControls playerControls, Action<IEnumerable<GameObject>> finished)
        {
            yield return new WaitWhile(() => Input.GetMouseButton(0));
            playerControls.enabled = false;
            if (targetingPrefab != null)
            {
                if (targetingInstance == null)
                {
                    targetingInstance = Instantiate(targetingPrefab);
                }
                else
                {
                    targetingInstance.gameObject.SetActive(true);
                }
                targetingInstance.localScale = new Vector3(areaAffectRadius*2, 1, areaAffectRadius*2);
            }
            
            while (true)
            {
                Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);

                RaycastHit raycastHit;
                if (Physics.Raycast(PlayerControls.GetMouseRay(), out raycastHit, 1000, layerMask))
                {
                    targetingInstance.position = raycastHit.point;

                    if (Input.GetMouseButtonDown(0))
                    {
                        yield return new WaitWhile(() => Input.GetMouseButton(0));
                        playerControls.enabled = true;
                        targetingInstance.gameObject.SetActive(false);
                        finished(GetGameObjectsInRadius(raycastHit.point));
                        yield break;
                    }
                }
                yield return null;
            }
        }

        private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 point)
        {
           
            RaycastHit[] hits = Physics.SphereCastAll(point, areaAffectRadius, Vector3.up, 0);

            foreach (var hit in hits)
            {
                yield return hit.collider.gameObject;
            }
        }
    }
}