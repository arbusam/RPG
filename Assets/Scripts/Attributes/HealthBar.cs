using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas canvas;

        private void Update()
        {
            if (Mathf.Approximately(healthComponent.HealthFraction, 1) || Mathf.Approximately(healthComponent.HealthFraction, 0))
            {
                canvas.enabled = false;
                return;
            }
            canvas.enabled = true;
            foreground.localScale = new Vector3(healthComponent.HealthFraction, 1, 1);
        }
    }
}