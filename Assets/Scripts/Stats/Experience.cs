using GameDevTV.Saving;
using UnityEngine;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable {
        [SerializeField] float experiencePoints = 0;

        public event Action onExperienceGained;

        public float ExperiencePoints
        {
            get
            {
                return experiencePoints;
            }
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.UpArrow) && Debug.isDebugBuild)
            {
                experiencePoints += 1;
                onExperienceGained();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && Debug.isDebugBuild)
            {
                experiencePoints -= 1;
                onExperienceGained();
            }
        }

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            onExperienceGained();
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }  
}