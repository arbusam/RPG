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
    }
}