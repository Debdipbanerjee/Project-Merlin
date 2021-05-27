using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {

        GameObject player;

        private void Start()
        {
            //Observer Pattern
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;

            player = GameObject.FindGameObjectWithTag("Player");
        }

        void DisableControl(PlayableDirector pd)
        {
            //cancels current action
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            //stops movement & player input
            player.GetComponent<PlayerController>().enabled = false;

        }

        void EnableControl(PlayableDirector pd)
        {
            //enables movement & player input
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}