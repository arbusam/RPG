using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses;

        public float GetHealth(CharacterClass characterClass, int level)
        {
            foreach (ProgressionCharacterClass progClass in characterClasses)
            {
                if (progClass.characterClass == characterClass)
                {
                    foreach (ProgressionStat progStat in progClass.stats)
                    {
                        if (progStat.stat == Stat.Health)
                        {
                            return progStat.levels[level-1];
                        }
                    }
                    
                }
            }
            return 0;
        }

        [System.Serializable]
        public class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        public class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}