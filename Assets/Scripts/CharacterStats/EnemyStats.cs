using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace roguelike
{
    public class EnemyStats : CharacterStats
    {
        public float detectRange = 3.5f;

        public DungeonCompletion dungeonCompletion;

        private void Start()
        {
            maxHealth = 100;
            currHealth = maxHealth;
            attackRange = 1f;
            attackCooldown = 2f;
        }

        private void Update()
        {
            CheckHealth();
        }

        public override void Die()
        {
            Destroy(gameObject);
            dungeonCompletion.enemiesInScene = dungeonCompletion.enemiesInScene - 1;
        }
    }
}
  