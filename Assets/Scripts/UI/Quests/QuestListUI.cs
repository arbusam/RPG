using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] Quest[] tempQuests;
        [SerializeField] QuestItemUI questPrefab;

        void Start()
        {
            foreach (Transform item in this.transform)
            {
                Destroy(item.gameObject);
            }
            foreach (Quest quest in tempQuests)
            {
                QuestItemUI uiInstance =  Instantiate<QuestItemUI>(questPrefab, this.transform);
                uiInstance.Setup(quest);
            }
        }
    }
}