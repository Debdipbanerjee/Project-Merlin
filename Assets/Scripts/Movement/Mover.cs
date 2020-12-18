using RPG.Combat;
using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {
        NavMeshAgent navMeshAgent;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateAnimator();
        }

        //Movement animation update
        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            //global velocity to local velocity
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            GetComponent<Fighter>().Cancel();
            MoveTo(destination);
        }


        // move to a certain point if ray hit somewhere
        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }
        
        //Cancel the movement
        public void Stop()
        {
            navMeshAgent.isStopped = true;
        }
    }

}