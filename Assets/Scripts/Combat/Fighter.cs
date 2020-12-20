using System.Collections;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour , IAction 
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 5f;

        Health target;
        float timeSinceLastAttack = 0;

        private void Update()
        {
            //increasing time since last attack in each seconds
            timeSinceLastAttack += Time.deltaTime;

            //if there's no enemy, don't fight
            if (target == null) return;

            //if target is dead, don't attack
            if (target.IsDead() == true) return;

            //if there's an enemy but not in range, move towards him
            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position);
            }
            else
            {
                //There's an enemy but in your range, Stop moving, start attacking
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                GetComponent<Animator>().SetTrigger("attack");
                //this will trigger Hit() event
                timeSinceLastAttack = 0;
            }
        }

        //Animation event
        void Hit()
        {
            target.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            //stop attack in the middle
            GetComponent<Animator>().SetTrigger("stopAttack");
            target = null;
        }

        
    }
}