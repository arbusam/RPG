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

        QuestStatus status;

        public void Setup(QuestStatus status)
        {
            this.status = status;
            title.text = status.Quest.Title;
            progress.text = "0/" + status.Quest.ObjectiveCount;
        }

        public QuestStatus Status
        {
            get
            {
                return status;
            }
        }
    }
}