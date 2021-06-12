using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Saving;
using UnityEngine;
namespace RPG.Quests
{
    public class QuestList : MonoBehaviour, ISaveable
    {
        List<QuestStatus> statuses = new List<QuestStatus>();

        public event Action onQuestListUpdated;

        public List<QuestStatus> Statuses
        {
            get
            {
                return statuses;
            }
        }

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

        public QuestStatus GetStatus(Quest quest)
        {
            foreach (QuestStatus status in statuses)
            {
                if (status.Quest == quest)
                {
                    return status;
                }
            }
            return null;
        }

        public bool CompleteObjective(Quest quest, string objective)
        {
            QuestStatus status = GetStatus(quest);
            if (status.CompletedObjectives.Contains(objective)) return false;
            status.CompletedObjectives.Add(objective);
            if (onQuestListUpdated != null)
            {
                onQuestListUpdated();
            }
            return true;
        }

        public object CaptureState()
        {
            List<object> state = new List<object>();

            foreach (QuestStatus status in statuses)
            {
                state.Add(status.CaptureState());
            }
            return state;
        }

        public void RestoreState(object state)
        {
            List<object> stateList = state as List<object>;
            if (stateList == null) return;

            statuses.Clear();
            foreach (object objectState in stateList)
            {
                QuestStatus.QuestStatusRecord statusRecord = objectState as QuestStatus.QuestStatusRecord;
                if (statusRecord == null) return;
                statuses.Add(new QuestStatus(statusRecord));
            }
        }
    }
}