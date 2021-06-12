using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] Quest quest;
        [SerializeField] string[] objectives;

        public void CompleteObjective()
        {
            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            List<QuestStatus> statuses = questList.Statuses;
            if (!questList.HasQuest(quest)) return;

            for (int i = 0; i < objectives.Length; i++)
            {
                foreach (string objective in quest.Objectives)
                {
                    if (objective == objectives[i])
                    {
                        if (questList.CompleteObjective(quest, objective))
                        {
                            return;
                        }
                    }
                }
            }
        }
    }
}