using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using GameDevTV.Saving;
using GameDevTV.Utils;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour, ISaveable, IPredicateEvaluator
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

        private void Update()
        {
            CompleteObjectivesByPredicate();
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

        public void ClaimQuest(Quest quest)
        {
            GetStatus(quest).claimed = true;
        }

        public bool CompleteObjective(Quest quest, string objective)
        {
            QuestStatus status = GetStatus(quest);
            if (status.CompletedObjectives.Contains(objective)) return false;
            status.CompletedObjectives.Add(objective);
            if (status.IsComplete())
            {
                GiveReward(quest);
            }
            if (onQuestListUpdated != null)
            {
                onQuestListUpdated();
            }
            return true;
        }

        private void GiveReward(Quest quest)
        {
            foreach (var reward in quest.Rewards)
            {
                bool success = GetComponent<Inventory>().AddToFirstEmptySlot(reward.item, reward.number);
                if (!success)
                {
                    GetComponent<ItemDropper>().DropItem(reward.item, reward.number);
                }
            }
        }

        private void CompleteObjectivesByPredicate()
        {
            foreach (QuestStatus status in statuses)
            {
                if (status.IsComplete()) continue;
                Quest quest = status.Quest;
                foreach (var objective in quest.Objectives)
                {
                    if (status.CompletedObjectives.Contains(objective.reference)) continue;
                    if (!objective.usesCondition) continue;
                    if (objective.completionCondition.Check(GetComponents<IPredicateEvaluator>())) CompleteObjective(quest, objective.reference);
                }
            }
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

        public bool? Evaluate(string predicate, string[] parameters)
        {
            switch (predicate)
            {
                case "HasQuest": 
                return HasQuest(Quest.GetByName(parameters[0]));
                case "CompletedQuest":
                return GetStatus(Quest.GetByName(parameters[0])).IsComplete();
                case "IsClaimed":
                return GetStatus(Quest.GetByName(parameters[0])).claimed;
            }

            return null;
        }
    }
}