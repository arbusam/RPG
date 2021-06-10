using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI progress;

        Quest quest;

        public void Setup(Quest quest)
        {
            this.quest = quest;
            title.text = quest.Title;
            progress.text = "0/" + quest.ObjectiveCount;
        }

        public Quest GetQuest()
        {
            return quest;
        }
    }
}