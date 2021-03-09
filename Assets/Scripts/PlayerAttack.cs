using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace roguelike
{
    public class PlayerAttack : MonoBehaviour
    {
        public Transform attackPoint;
        public float attackRange = 0.5f;
        public LayerMask enemyLayers;
        public Animator animator;
        public PlayerController movement;
        public PlayerStats playerStats;

        private float damage;
        private float lastAttack;
        private float attackCooldown;

        private void Start()
        {
            damage = playerStats.damage;
            lastAttack = playerStats.lastAttack;
            attackCooldown = playerStats.attackCooldown;
        }

        void Update()
        {
            damage = gameObject.GetComponent<PlayerStats>().damage;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Time.time - lastAttack >= attackCooldown)
                {
                    lastAttack = Time.time;
                    StartCoroutine(MyCoroutine());
                }
            }
        }

        IEnumerator MyCoroutine()
        {
            Attack();
            yield return new WaitForSeconds(1.21f);
            AttackEnd();
        }

        void Attack()
        {
            GetComponent<PlayerController>().enabled = false;

            animator.SetTrigger("Attack");

            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyStats>().TakeDamage(damage);
            }
        }

        void AttackEnd()
        {
            GetComponent<PlayerController>().enabled = true;
        }

        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null)
            {
                return;
            }
            else
            {
                Gizmos.DrawWireSphere(attackPoint.position, attackRange);
            }
        }
    }
}