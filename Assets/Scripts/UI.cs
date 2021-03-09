using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace roguelike
{
    public class UI : MonoBehaviour
    {
        public Text healthAmount;

        public GameObject deathScreen;
        public GameObject ingameUI;
        public GameObject player;

        private float currHealth;
        private void Update()
        {
            currHealth = player.GetComponent<PlayerStats>().currHealth;
            if (currHealth <= 20)
            {
                healthAmount.color = Color.red; 
            } else
            {
                healthAmount.color = Color.green;
            }
            healthAmount.text = currHealth.ToString();
        }
    }
}

