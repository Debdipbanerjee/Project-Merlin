using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;

        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;

        Vector3 gaurdPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        int currentWaypointIndex = 0;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();

            // to return the guard to previous location
            gaurdPosition = transform.position;
        }

        // Custom Finite State Machine for AI
        private void Update()
        {
            //if AI is dead, don't do anything
            if (health.IsDead() == true) return;

            if (InAttackRangeOfPlayer(player)  && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                //Attack State
                AttackBehaviour();
            }
            else if(timeSinceLastSawPlayer < suspicionTime)
            {
                //Suspicion state
                SuspicionBehaviour();
            }
            else
            {
                //Patrolling State
                //stop attacking if player is out of range & return to Patrol
                PatrolBehaviour();
            }

            //increasing time since last saw player
            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            //if we have no patrol path
            Vector3 nextPosition = gaurdPosition;

            //if we have a patrol path
            if(patrolPath != null)
            {
                if(AtWaypoint())
                {
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            //Moving for patrol
            mover.StartMoveAction(nextPosition);
        }

        //Check if AI is at waypoints
        private bool AtWaypoint()
        {
            //how far Ai is from waypoints
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        //For cycling Waypoints
        private void CycleWaypoint()
        {

            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        //Checking the current waypoint
        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        //Suspicion State
        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        //Attack State
        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }

        //Check player is in range of AI
        private bool InAttackRangeOfPlayer(GameObject player)
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }


        //For drawing attack range gizmos
        //Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}