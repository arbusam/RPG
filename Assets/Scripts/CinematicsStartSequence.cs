using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicsStartSequence : MonoBehaviour
    {
        void Start()
        {
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