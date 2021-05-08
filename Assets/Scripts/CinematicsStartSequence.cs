using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicsStartSequence : MonoBehaviour, ISaveable
    {

        [SerializeField] bool played = false;

        public object CaptureState()
        {
            return played;
        }

        public void RestoreState(object state)
        {
            played = (bool)state;
        }

        public void StartSequence()
        {
            if (played) return;
            played = true;
            GetComponent<PlayableDirector>().Play();
            GetComponent<CinematicControlRemover>().DisableControl(GetComponent<PlayableDirector>());
        }

        private void Update() {
            if (Debug.isDebugBuild)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    GetComponent<PlayableDirector>().Stop();
                }
            }
        }
    }
}