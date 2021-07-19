using System;
using System.Collections.Generic;
using GameDevTV.Inventories;
using GameDevTV.Saving;
using GameDevTV.Utils;
using UnityEngine;

namespace RPG.Stats
{
    public class EnhancementStore : MonoBehaviour, IModifierProvider, ISaveable, IPredicateEvaluator, IItemStore
    {
        [SerializeField] EnhancementBonus[] bonusConfig;

        [System.Serializable]
        public class EnhancementBonus
        {
            public EnhancementCategory category;
            public Stat stat;
            public float additiveBonusPerPoint = 0;
            public float percentageBonusPerPoint = 0;
        }

        Dictionary<EnhancementCategory, int> assignedPoints = new Dictionary<EnhancementCategory, int>();
        Dictionary<EnhancementCategory, int> stagedPoints = new Dictionary<EnhancementCategory, int>();

        Dictionary<Stat, Dictionary<EnhancementCategory, float>> additiveBonusCache;
        Dictionary<Stat, Dictionary<EnhancementCategory, float>> percentageBonusCache;

        int extraPoints = 0;

        private void Awake()
        {
            additiveBonusCache = new Dictionary<Stat, Dictionary<EnhancementCategory, float>>();
            percentageBonusCache = new Dictionary<Stat, Dictionary<EnhancementCategory, float>>();
            foreach (var bonus in bonusConfig)
            {
                if (!additiveBonusCache.ContainsKey(bonus.stat))
                {
                    additiveBonusCache[bonus.stat] = new Dictionary<EnhancementCategory, float>();
                }
                if (!percentageBonusCache.ContainsKey(bonus.stat))
                {
                    percentageBonusCache[bonus.stat] = new Dictionary<EnhancementCategory, float>();
                }
                additiveBonusCache[bonus.stat][bonus.category] = bonus.additiveBonusPerPoint;
                percentageBonusCache[bonus.stat][bonus.category] = bonus.percentageBonusPerPoint;
            }
        }
        
        public int GetProposedPoints(EnhancementCategory category)
        {
            return GetPoints(category) + GetStagedPoints(category);
        }

        public int GetPoints(EnhancementCategory category)
        {
            return assignedPoints.ContainsKey(category) ? assignedPoints[category] : 0;
        }

        public int GetStagedPoints(EnhancementCategory category)
        {
            return stagedPoints.ContainsKey(category)? stagedPoints[category] : 0;
        }

        public void AssignPoints(EnhancementCategory category, int points)
        {
            if (!CanAssignPoints(category, points)) return;

            stagedPoints[category] = GetStagedPoints(category) + points;
        }

        public bool CanAssignPoints(EnhancementCategory category, int points)
        {
            if (GetStagedPoints(category) + points < 0) return false;
            if (GetUnassignedPoints() < points) return false;
            return true;
        }

        public int GetUnassignedPoints()
        {
            return GetAssignablePoints() - GetTotalProposedPoints();
        }

        public int GetTotalProposedPoints()
        {
            int total = 0;
            foreach (int points in assignedPoints.Values)
            {
                total += points;
            }
            foreach (int points in stagedPoints.Values)
            {
                total += points;
            }
            return total;
        }

        public void Commit()
        {
            foreach (EnhancementCategory category in stagedPoints.Keys)
            {
                assignedPoints[category] = GetProposedPoints(category);
            }
            stagedPoints.Clear();
        }

        public int GetAssignablePoints()
        {
            return (int)GetComponent<BaseStats>().GetStat(Stat.TotalEnhancementPoints);
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.TotalEnhancementPoints)
            {
                yield return extraPoints;
                yield break;
            }
            if (!additiveBonusCache.ContainsKey(stat)) yield break;

            foreach (EnhancementCategory category in additiveBonusCache[stat].Keys)
            {
                float bonus = additiveBonusCache[stat][category];
                yield return bonus * GetPoints(category);
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (!percentageBonusCache.ContainsKey(stat)) yield break;

            foreach (EnhancementCategory category in percentageBonusCache[stat].Keys)
            {
                float bonus = percentageBonusCache[stat][category];
                yield return bonus * GetPoints(category);
            }
        }

        public object CaptureState()
        {
            return assignedPoints;
        }

        public void RestoreState(object state)
        {
            assignedPoints = new Dictionary<EnhancementCategory, int>((IDictionary<EnhancementCategory, int>)state);
        }

        public bool? Evaluate(string predicate, string[] parameters)
        {
            if (predicate != "MinimumEnhancement") return null;

            EnhancementCategory category;
            int number;
            if (!Enum.TryParse<EnhancementCategory>(parameters[0], out category) || !Int32.TryParse(parameters[1], out number)) return null;

            return GetPoints(category) >= number;
        }

        public int AddItems(InventoryItem item, int number)
        {
            EnhancementItem enhancementItem = item as EnhancementItem;
            if (enhancementItem == null) return 0;
            extraPoints += number;
            return number;
        }
    }
}