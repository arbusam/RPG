using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class EnhancementRowUI : MonoBehaviour
    {
        [SerializeField] EnhancementCategory category;
        [SerializeField] TextMeshProUGUI valueText;
        [SerializeField] Button minusButton;
        [SerializeField] Button plusButton;

        EnhancementStore playerEnhancementStore = null;

        int value = 0;

        private void Start()
        {
            playerEnhancementStore = GameObject.FindGameObjectWithTag("Player").GetComponent<EnhancementStore>();
            minusButton.onClick.AddListener(() => Allocate(-1));
            plusButton.onClick.AddListener(() => Allocate(+1));
        }

        private void Update()
        {
            minusButton.interactable = playerEnhancementStore.CanAssignPoints(category, -1);
            plusButton.interactable = playerEnhancementStore.CanAssignPoints(category, +1);

            valueText.text = playerEnhancementStore.GetProposedPoints(category).ToString();
        }

        public void Allocate(int points)
        {
            playerEnhancementStore.AssignPoints(category, points);
        }

    }
}