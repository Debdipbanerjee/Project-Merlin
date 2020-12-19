using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100f;

        public void TakeDamage(float damage)
        {
            //health might go below 0 so take the higher one
            health = Mathf.Max(health - damage, 0);
            print(health);
        }
    }
}