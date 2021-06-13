using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class DeadEnemies : MonoBehaviour
    {
        public Dictionary<int, int> dead = new Dictionary<int, int>(); // level, number
        public UnityEvent<int> onDeadAdded;

        public void AddDead(int level)
        {
            if (!dead.ContainsKey(level)) dead[level] = 0;
            dead[level]++;
            onDeadAdded.Invoke(dead[level]);
        }
    }
}