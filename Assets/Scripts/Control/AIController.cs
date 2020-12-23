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
        [SerializeField] float waypointDwellTime = 3f;

        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;

        Vector3 gaurdPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
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

            //if not dead, check if there's a player & is in range & is alive
            if (InAttackRangeOfPlayer(player) && fighter.CanAttack(player))
            {
                //if is in range and alive attack

                //Attack State
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                //if not range, be suspicious

                //Suspicion state
                SuspicionBehaviour();
            }
            else
            {
                //if there's no player or not in range or not alive, start patrol

                //Patrolling State
                //stop attacking if player is out of range & return to Patrol
                PatrolBehaviour();
            }

            //update time Since last saw player & time since Arrived at waypoint
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            //increasing time since last saw player
            timeSinceLastSawPlayer += Time.deltaTime;

            //increasing the time after arriving a waypoint to halt there for couple of seconds
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        //Patrol state
        private void PatrolBehaviour()
        {
            //if we have no patrol path
            Vector3 nextPosition = gaurdPosition;

            //if we have a patrol path
            if(patrolPath != null)
            {
                if(AtWaypoint())
                {
                    // reseting time after reaching a waypoint
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if(timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                //Moving to next waypoint
                mover.StartMoveAction(nextPosition);
            } 
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
            // reseting time since last saw player 
            timeSinceLastSawPlayer = 0;

            //Attack
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