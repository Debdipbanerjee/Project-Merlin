using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;

        bool isDead = false;

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
            //if enemy is dead, don't attack
            if (isDead) return;

            isDead = true;
            //Trigger death animation
            GetComponent<Animator>().SetTrigger("die");
        }
    }
}