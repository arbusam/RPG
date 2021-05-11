using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [ExecuteAlways]
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] int level;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        
        private void Update() {
            if (level < 1)
            {
                level = 1;
            }
        }

        public float GetHealth()
        {
            return progression.GetHealth(characterClass, level);
        }

        public float GetExperienceAward()
        {
            return 1;
        }
    }
}
