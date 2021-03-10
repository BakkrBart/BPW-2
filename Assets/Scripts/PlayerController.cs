using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace roguelike
{
    public class PlayerController : MonoBehaviour
    {
        private float moveSpeed; //x
        private float rotateSpeed;//x

        [SerializeField]
        private bool rotatingToMouse;//x

        public Rigidbody rigidBody;//x
        public Camera cam;//x
        public PlayerStats playerStats;//x
        public Animator animator;//x

        Vector3 movement;//x

        private void Start()//x
        {
            moveSpeed = playerStats.moveSpeed;//x
            rotateSpeed = playerStats.rotateSpeed;//x
        }

        void Update()//x
        {
            CheckInput();//x
        }

        private void FixedUpdate()//x
        {
            Movement();//x
            if (rotatingToMouse)//x
            {
                RotateToMouse();
            }
            else
            {
                RotateToMovement();
            }
        }

        private void RotateToMovement() //x
        {
            if (movement.magnitude == 0)
            {
                return;
            }
            var rotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);
        }

        private void Movement()//x
        {
            rigidBody.MovePosition(rigidBody.position + movement * moveSpeed * Time.fixedDeltaTime);
            animator.SetFloat("Speed", Mathf.Abs(movement.magnitude));
        }

        private void CheckInput()//x
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.z = Input.GetAxisRaw("Vertical");
        }

        private void RotateToMouse()//x
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
