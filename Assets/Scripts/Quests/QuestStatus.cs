using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [Serializable]
    public class QuestStatus
    {
        [SerializeField] Quest quest;
        [SerializeField] List<string> completedObjectives = new List<string>();

        [Serializable]
        public class QuestStatusRecord
        {
            public string questName;
            public List<string> completedObjectives;

            public QuestStatusRecord(string questName, List<string> completedObjectives)
            {
                this.questName = questName;
                this.completedObjectives = completedObjectives;
            }
        }

        public QuestStatus(QuestStatusRecord statusRecord)
        {
            quest = Quest.GetByName(statusRecord.questName);
            completedObjectives = statusRecord.completedObjectives;
        }

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
        }

        public Quest Quest
        {
            get
            {
                return quest;
            }
        }

        public int GetCompletedCount()
        {
            return completedObjectives.Count;
        }

        public List<string> CompletedObjectives
        {
            get
            {
                return completedObjectives;
            }
        }

        public object CaptureState()
        {
            QuestStatusRecord statusRecord = new QuestStatusRecord(quest.name, completedObjectives);
            return statusRecord;
        }

        public bool IsComplete()
        {
            foreach (var objective in quest.Objectives)
            {
                if (!completedObjectives.Contains(objective.reference))
                {
                    return false;
                }
            }
            return true;
        }
    }
}