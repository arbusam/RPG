using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "Quest", menuName = "RPG/Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField] string[] objectives;

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
                return objectives.Length;
            }
        }
    }
}