using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Transform objectiveContainer;
        [SerializeField] GameObject objectivePrefab;
        [SerializeField] GameObject uncheckedObjectivePrefab;

        public void Setup(QuestStatus status)
        {
            Quest quest = status.Quest;
            title.text = quest.Title;
            foreach (Transform item in objectiveContainer)
            {
                Destroy(item.gameObject);
            }
            foreach (string objective in quest.Objectives)
            {
                GameObject prefab = uncheckedObjectivePrefab;
                if (status.CompletedObjectives.Contains(objective))
                {
                    prefab = objectivePrefab;
                }
                print(prefab.name);
                GameObject objectiveInstance = Instantiate(prefab, objectiveContainer);
                TextMeshProUGUI objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
                objectiveText.text = objective;
            }
        }
    }
}