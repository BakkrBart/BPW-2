using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace roguelike
{
    public class PlayerController : MonoBehaviour
    {
        private float moveSpeed;
        private float rotateSpeed;

        [SerializeField]
        private bool rotatingToMouse;

        public Rigidbody rigidBody;
        public Camera cam;
        public PlayerStats playerStats;
        public Animator animator;

        Vector3 movement;

        private void Start()
        {
            moveSpeed = playerStats.moveSpeed;
            rotateSpeed = playerStats.rotateSpeed;
        }

        void Update()
        {
            CheckInput();
        }

        private void FixedUpdate()
        {
            Movement();
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

        private void Movement()
        {
            rigidBody.MovePosition(rigidBody.position + movement * moveSpeed * Time.fixedDeltaTime);
            animator.SetFloat("Speed", Mathf.Abs(movement.magnitude));
        }

        private void CheckInput()
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.z = Input.GetAxisRaw("Vertical");
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
    }
}
