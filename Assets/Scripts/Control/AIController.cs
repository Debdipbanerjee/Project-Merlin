using RPG.Combat;
using RPG.Core;
using System.Collections;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;

        Fighter fighter;
        GameObject player;
        Health health;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
        }

        private void Update()
        {
            //if AI is dead, don't do anything
            if (health.IsDead() == true) return;

            if (InAttackRangeOfPlayer(player)  && fighter.CanAttack(player))
            {
                fighter.Attack(player);
            }
            else
            {
                //stop attacking if player is out of range
                fighter.Cancel();
            }
        }

        private bool InAttackRangeOfPlayer(GameObject player)
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }

        //Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}