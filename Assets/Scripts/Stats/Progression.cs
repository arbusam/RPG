using UnityEngine;
using System.Collections.Generic;
using System;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();

            float[] levels;

            try
            {
                levels = lookupTable[characterClass][stat];
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                return 0;
            }

            if (levels.Length == 0) return 0;

            if (levels.Length < level) return levels[levels.Length - 1];
            return levels[level-1];
            // foreach (ProgressionCharacterClass progClass in characterClasses)
            // {
            //     if (progClass.characterClass == characterClass)
            //     {
            //         foreach (ProgressionStat progStat in progClass.stats)
            //         {
            //             if (progStat.stat == stat)
            //             {
            //                 if (progStat.levels.Length < level) continue;
            //                 return progStat.levels[level-1];
            //             }
            //         }
                    
            //     }
            // }
            // return 0;
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookup();

            float[] levels = lookupTable[characterClass][stat];
            return levels.Length;
        }

        private void BuildLookup()
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass progClass in characterClasses)
            {
                Dictionary<Stat, float[]> values = new Dictionary<Stat, float[]>();
                foreach (ProgressionStat progStat in progClass.stats)
                {
                    values.Add(progStat.stat, progStat.levels);
                }
                lookupTable.Add(progClass.characterClass, values);
            }
        }

        [Serializable]
        public class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [Serializable]
        public class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}