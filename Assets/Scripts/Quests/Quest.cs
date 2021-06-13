using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    using GameDevTV.Inventories;
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "Quest", menuName = "RPG/Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField] List<Objective> objectives;
        [SerializeField] List<Reward> rewards = new List<Reward>();

        [System.Serializable]
        public class Reward
        {
            [Min(1)] public int number;
            public InventoryItem item;
        }

        [System.Serializable]
        public class Objective  
        {
            public string reference;
            public string description;
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