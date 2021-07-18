using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameDevTV.Inventories;
using GameDevTV.Utils;

namespace RPG.Quests
{
    
    [CreateAssetMenu(fileName = "Quest", menuName = "RPG/Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField] List<Objective> objectives;
        [SerializeField] List<Reward> rewards = new List<Reward>();

        [Serializable]
        public class Reward
        {
            [Min(1)] public int number;
            public InventoryItem item;
        }

        [Serializable]
        public class Objective  
        {
            public string reference;
            public string description;
            public bool usesCondition = false;
            public Condition completionCondition;
        }

        public string Title
        {
            get
            {
                return name;
            }
        }

        public int ObjectiveCount
        {
            get
            {
                return objectives.Count;
            }
        }

        public List<Objective> Objectives
        {
            get
            {
                return objectives;
            }
        }

        public List<Reward> Rewards
        {
            get
            {
                return rewards;
            }
        }

        public static Quest GetByName(string questName)
        {
            foreach (Quest quest in Resources.LoadAll<Quest>(""))
            {
                if (quest.name == questName)
                {
                    return quest;
                }
            }
            return null;
        }
    }
}