using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scriptes
{
    public class Stairway : MonoBehaviour
    {
        public bool IsUp;
        private GameObject player;
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        private void Update()
        {
            if (player.GetComponent<PlayerMovement>().vertical_input <= -0.7f || Input.GetKeyDown(KeyCode.S) && player.GetComponent<PlayerMovement>().IsGrounded())
            {
                transform.parent.GetComponent<Collider2D>().enabled = false;
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                transform.parent.GetComponent<Collider2D>().enabled = IsUp;
            }
        }
    }
}