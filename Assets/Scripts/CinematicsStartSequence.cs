using System.Collections;
using System.Collections.Generic;
using GameDevTV.Saving;
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
            GetComponent<CinematicControlRemover>().DisableControl(GetComponent<PlayableDirector>());
            GetComponent<PlayableDirector>().Play();
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