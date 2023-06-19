using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scriptes
{
    public class ActivateLever : MonoBehaviour
    {
        private Animator anim;
        [SerializeField] private GameObject door;
        [SerializeField] private GameObject buttonLever;
        public int needed_dead_enemies;
        public int score = 0;

       
        private void Awake()
        {
            anim = GetComponent<Animator>();
            buttonLever.SetActive(false);
        }

        public void Kill()
        {
            score++;
        }

        public void BtnLever()
        {
            Debug.Log(needed_dead_enemies <= score);
            Debug.Log(score);
            Debug.Log(needed_dead_enemies);
            if (needed_dead_enemies <= score)
            {
                Debug.Log("actv");

                anim.Play("Lever_on");
                door.SetActive(false);
                buttonLever.SetActive(false);
                score = 0;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && door.activeSelf)
                buttonLever.SetActive(true);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                buttonLever.SetActive(false);
        }
    }
}
