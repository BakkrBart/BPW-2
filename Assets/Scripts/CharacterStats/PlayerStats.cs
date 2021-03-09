using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace roguelike
{
    public class PlayerStats : CharacterStats
    {
        public PlayerController movement;

        public float moveSpeed;
        public float rotateSpeed;
        private void Start()
        {
            maxHealth = 100;
            currHealth = maxHealth;

            attackCooldown = 1.11f;
            attackRange = 0.5f;

            moveSpeed = 2.5f;
            rotateSpeed = 11f;

            GetComponent<PlayerController>().enabled = true;
        }

        private void Update()
        {
            CheckHealth();
        }

        public override void Die()
        {
            GetComponent<PlayerController>().enabled = false;
            //UI for death
        }
    }
}
