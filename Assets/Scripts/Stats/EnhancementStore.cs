using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class EnhancementStore : MonoBehaviour
    {
        Dictionary<EnhancementCategory, int> assignedPoints = new Dictionary<EnhancementCategory, int>();
        Dictionary<EnhancementCategory, int> stagedPoints = new Dictionary<EnhancementCategory, int>();

        int unassignedPoints = 10;

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
            unassignedPoints -= points;
        }

        public bool CanAssignPoints(EnhancementCategory category, int points)
        {
            if (GetStagedPoints(category) + points < 0) return false;
            if (unassignedPoints < points) return false;
            return true;
        }

        public int GetUnassignedPoints()
        {
            return unassignedPoints;
        }

        public void Commit()
        {
            foreach (EnhancementCategory category in stagedPoints.Keys)
            {
                assignedPoints[category] = GetProposedPoints(category);
            }
            stagedPoints.Clear();
        }
    }
}