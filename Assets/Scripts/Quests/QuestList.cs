using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Quests
{
    public class QuestList : MonoBehaviour
    {
        List<QuestStatus> statuses = new List<QuestStatus>();

        public event Action onQuestListUpdated;

        public void AddQuest(Quest quest)
        {
            if (HasQuest(quest)) return;
            QuestStatus newStatus = new QuestStatus(quest);
            statuses.Add(newStatus);
            if (onQuestListUpdated != null)
            {
                onQuestListUpdated();
            }
        }

        public bool HasQuest(Quest quest)
        {
            foreach (QuestStatus status in statuses)
            {
                if (status.Quest == quest)
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<QuestStatus> Statuses
        {
            get
            {
                return statuses;
            }
        }
    }
}