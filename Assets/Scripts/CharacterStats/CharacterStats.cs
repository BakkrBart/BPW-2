using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace roguelike
{
    public class CharacterStats : MonoBehaviour
    {
        public float currHealth;
        public float lastAttack = 0;

        public float maxHealth;
        public float attackCooldown;
        public float damage;

        public bool isDead = false;

        public float attackRange;
        public void CheckHealth()
        {
            if (currHealth >= maxHealth)
            {
                currHealth = maxHealth;
            }

            if (currHealth <= 0)
            {
                currHealth = 0;
                isDead = true;
            }

            if (isDead == true)
            {
                Die();
            }
        }

        public virtual void Die()
        {
            
        }

        public void TakeDamage(float damage)
        {
            currHealth -= damage;
        }
    }
}