using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] QuestItemUI questPrefab;
        [SerializeField] Transform incompleteQuestList;
        [SerializeField] Transform completedQuestList;

        void Start()
        {
            UpdateUI();
            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.onQuestListUpdated += UpdateUI;
        }

        private void UpdateUI()
        {
            DestroyItems();
            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            foreach (QuestStatus status in questList.Statuses)
            {
                QuestItemUI uiInstance = Instantiate<QuestItemUI>(questPrefab, incompleteQuestList.transform);
                uiInstance.Setup(status);
            }
        }

        private void DestroyItems()
        {
            foreach (Transform item in incompleteQuestList.transform)
            {
                Destroy(item.gameObject);
            }
            foreach (Transform item in completedQuestList.transform)
            {
                Destroy(item.gameObject);
            }
        }
    }
}