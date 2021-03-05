using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace roguelike.minimap
{
    public class Minimap : MonoBehaviour
    {
        public Transform player;

        [SerializeField]
        private float camHeight;

        private void Start()
        {
        
        }

        private void LateUpdate()
        {
            Vector3 newPosition = player.position;
            newPosition.y = transform.position.y;
            transform.position = newPosition;
        }
    }
}
