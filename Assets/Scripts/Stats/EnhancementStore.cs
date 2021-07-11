using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class EnhancementStore : MonoBehaviour
    {
        Dictionary<EnhancementCategory, int> assignedPoints = new Dictionary<EnhancementCategory, int>();
        Dictionary<EnhancementCategory, int> stagedPoints = new Dictionary<EnhancementCategory, int>();
        
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
    }
}