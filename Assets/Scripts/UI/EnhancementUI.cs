using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class EnhancementUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI unassignedPointsText;
        [SerializeField] Button commitButton;

        EnhancementStore playerEnhancementStore = null;

        private void Start()
        {
            playerEnhancementStore = GameObject.FindGameObjectWithTag("Player").GetComponent<EnhancementStore>();
            commitButton.onClick.AddListener(playerEnhancementStore.Commit);
        }

        private void Update()
        {
            unassignedPointsText.text = playerEnhancementStore.GetUnassignedPoints().ToString();
        }
    }
}