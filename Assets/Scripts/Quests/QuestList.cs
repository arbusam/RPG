using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour
    {
        [SerializeField] QuestStatus[] statuses;
        public QuestStatus[] Statuses
        {
            get
            {
                return statuses;
            }
        }
    }
}