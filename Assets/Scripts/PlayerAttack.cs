using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace roguelike
{
    public class PlayerAttack : MonoBehaviour
    {
        public Transform attackPoint;//x
        public float attackRange = 0.5f;//x
        public LayerMask enemyLayers;//x
        public Animator animator;//x
        public PlayerController movement;//x (not used)
        public PlayerStats playerStats;//x

        private float damage;//x
        private float lastAttack;//x
        private float attackCooldown;//x

        private void Start()//x
        {
            damage = playerStats.damage;
            lastAttack = playerStats.lastAttack;
            attackCooldown = playerStats.attackCooldown;
        }

        void Update()//x
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

        IEnumerator MyCoroutine()//x
        {
            Attack();
            yield return new WaitForSeconds(1.21f);
            AttackEnd();
        }

        void Attack()
        {
            GetComponent<PlayerController>().enabled = false;
            animator.SetTrigger("Attack");
        }

        void AttackEnd()
        {
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyStats>().TakeDamage(damage);
            }
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