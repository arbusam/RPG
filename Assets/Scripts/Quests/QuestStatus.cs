using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [System.Serializable]
    public class QuestStatus
    {
        [SerializeField] Quest quest;
        [SerializeField] List<string> completedObjectives;

        public Quest Quest
        {
            get
            {
                return quest;
            }
        }

        public List<string> CompletedObjectives
        {
            get
            {
                return completedObjectives;
            }
        }
    }
}