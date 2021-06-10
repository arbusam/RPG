using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] QuestItemUI questPrefab;

        void Start()
        {
            UpdateUI();
            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.onQuestListUpdated += UpdateUI;
        }

        private void UpdateUI()
        {
            foreach (Transform item in this.transform)
            {
                Destroy(item.gameObject);
            }
            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            foreach (QuestStatus status in questList.Statuses)
            {
                QuestItemUI uiInstance = Instantiate<QuestItemUI>(questPrefab, this.transform);
                uiInstance.Setup(status);
            }
        }
    }
}