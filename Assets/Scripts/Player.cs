using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace roguelike
{
    public class Player : MonoBehaviour
    {
        public enum StateEnum { idle, walk, attack, death }
        [SerializeField]
        private StateEnum state;

        private float moveSpeed;
        private float rotateSpeed;
        private float damage;
        private float lastAttack;
        private float attackCooldown;

        public float attackRange = 0.5f;

        [SerializeField]
        private bool rotatingToMouse;

        public Rigidbody rigidBody;
        public Camera cam;
        public PlayerStats playerStats;
        public Animator animator;
        public Transform attackPoint;

        public LayerMask enemyLayers;

        Vector3 movement;
        void Start()
        {
            moveSpeed = playerStats.moveSpeed;
            rotateSpeed = playerStats.rotateSpeed;
            damage = playerStats.damage;
            lastAttack = playerStats.lastAttack;
            attackCooldown = playerStats.attackCooldown;
        }

        // Update is called once per frame
        void Update()
        {
            CheckState();
        }

        void CheckState()
        {
            switch (state)
            {
                case StateEnum.idle: IdleBehaviour(); break;
                case StateEnum.walk: WalkBehaviour(); break;
                case StateEnum.attack: AttackBehaviour(); break;
                case StateEnum.death: DeathBehaviour(); break;
            }
        }

        void IdleBehaviour()
        {
            animator.SetFloat("Speed", 0);
            CheckInput();
            CheckRotate();
            if (movement.x != 0)
            {
                state = (StateEnum.walk);
            }
            if (movement.z != 0)
            {
                state = (StateEnum.walk);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Time.time - lastAttack >= attackCooldown)
                {
                    lastAttack = Time.time;
                    state = (StateEnum.attack);
                }
            }
        }

        void WalkBehaviour()
        {
            animator.SetFloat("Speed", 1);
            CheckInput();
            Movement();
            CheckRotate();
            if (movement.x == 0)
            {
                if (movement.z == 0)
                {
                    state = (StateEnum.idle);
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Time.time - lastAttack >= attackCooldown)
                {
                    lastAttack = Time.time;
                    state = (StateEnum.attack);
                }
            }
        }

        void AttackBehaviour()
        {
            animator.SetFloat("Speed", 0);
            damage = gameObject.GetComponent<PlayerStats>().damage;
            StartCoroutine(AttackStart());
            for (int i = 0; i < 42; i++)
            {
                if (i == 41)
                {
                    if (movement.x != 0)
                    {
                        state = (StateEnum.walk);
                    }
                    if (movement.z != 0)
                    {
                        state = (StateEnum.walk);
                    }
                    if (movement.x == 0)
                    {
                        if (movement.z == 0)
                        {
                            state = (StateEnum.idle);
                        }
                    }
                }
            }
        }

        void DeathBehaviour()
        {
            animator.SetFloat("Speed", 0);
        }

        private void CheckInput()
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.z = Input.GetAxisRaw("Vertical");
        }

        private void Movement()
        {
            rigidBody.MovePosition(rigidBody.position + movement * moveSpeed * Time.fixedDeltaTime);
        }

        private void CheckRotate()
        {
            if (rotatingToMouse)
            {
                RotateToMouse();
            }
            else
            {
                RotateToMovement();
            }
        }

        private void RotateToMovement()
        {
            if (movement.magnitude == 0)
            {
                return;
            }
            var rotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);
        }

        private void RotateToMouse()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, 300f))
            {
                var target = hitInfo.point;
                target.y = transform.position.y;
                transform.LookAt(target);
            }
        }

        IEnumerator AttackStart()
        {
            Attack();
            yield return new WaitForSeconds(1.21f);
        }

        void Attack()
        {
            animator.SetTrigger("Attack");

            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyStats>().TakeDamage(damage);
            }
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
