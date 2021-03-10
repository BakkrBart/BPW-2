using UnityEngine;
using UnityEngine.AI;

namespace roguelike
{
    public class Enemy : MonoBehaviour
    {
        public Transform player;
        public Animator animator;
        public EnemyStats enemyStats;

        private float detectRange;
        private float attackRange;
        private float lastAttack;
        private float attackCooldown;
        private float damage;

        public enum StateEnum { idle, walk, attack}
        [SerializeField]
        private StateEnum state;

        NavMeshAgent agent;

        private void Start()
        {
            player = GameObject.Find("Player").transform;

            detectRange = enemyStats.detectRange;
            attackRange = enemyStats.attackRange;
            lastAttack = enemyStats.lastAttack;
            attackCooldown = enemyStats.attackCooldown;
            damage = enemyStats.damage;

            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            CheckState();
        }

        private void CheckState()
        {
            switch (state)
            {
                case StateEnum.idle: IdleBehaviour(); break;
                case StateEnum.walk: WalkBehaviour(); break;
                case StateEnum.attack: AttackBehaviour(); break;
            }
        }

        private void IdleBehaviour()
        {
            animator.SetFloat("Speed", 0);
            if (Vector3.Distance(transform.position, player.position) < detectRange)
            {
                state = (StateEnum.walk);
            }

            agent.isStopped = true;
        }

        private void WalkBehaviour()
        {
            animator.SetFloat("Speed", 1);

            if (Vector3.Distance(transform.position, player.position) > detectRange)
            {
                state = (StateEnum.idle);
            }

            if (Vector3.Distance(transform.position, player.position) < attackRange)
            {
                state = (StateEnum.attack);
            }

            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }

        private void AttackBehaviour()
        {
            animator.SetFloat("Speed", 0);

            agent.isStopped = true;

            transform.LookAt(player);

            if (Time.time - lastAttack >= attackCooldown)
            {
                animator.SetTrigger("Attack");
                lastAttack = Time.time;
                GiveDamage();
            }

            if (Vector3.Distance(transform.position, player.position) > attackRange)
            {
                state = (StateEnum.walk);
            }
        }

        private void GiveDamage()
        {
            player.GetComponent<PlayerStats>().TakeDamage(damage);
        }
    }
}
