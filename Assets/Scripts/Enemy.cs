using UnityEngine;
using UnityEngine.AI;

namespace roguelike.enemy
{
    public class Enemy : MonoBehaviour
    {
        public Transform player;

        public float detectRange = 3.5f;
        public float attackRange = 1f;

        public enum StateEnum { idle, walk, attack}
        [SerializeField]
        private StateEnum state;

        NavMeshAgent agent;
        private void Start()
        {
            player = GameObject.Find("Player").transform;
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
            if (Vector3.Distance(transform.position, player.position) < detectRange)
            {
                state = (StateEnum.walk);
            }

            agent.isStopped = true;
        }

        private void WalkBehaviour()
        {
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
            if (Vector3.Distance(transform.position, player.position) > attackRange)
            {
                state = (StateEnum.walk);
            }

            agent.isStopped = true;
        }
    }
}
