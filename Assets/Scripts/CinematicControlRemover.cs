using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {

        GameObject player;
        
        private void Start() {
            player = GameObject.FindWithTag("Player");
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        public void DisableControl(PlayableDirector director)
        {
            player.GetComponent<ActionScheduler>().StopCurrentAction();
            player.GetComponent<PlayerControls>().enabled = false;
        }
        void EnableControl(PlayableDirector director)
        {
            player.GetComponent<PlayerControls>().enabled = true;
        }
    }

}
