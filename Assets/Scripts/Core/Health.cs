using System.Collections;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;

        bool isDead = false;

        //returns if the target is dead or not
        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            //health might go below 0 so take the higher one
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if(healthPoints == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            //if enemy is already dead, don't trigger death animation
            if (isDead) return;

            isDead = true;
            //Trigger death animation
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}