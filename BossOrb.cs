using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scriptes
{
    public class BossOrb : Damage_From_Enemy
    {
        [SerializeField] private float speed;
        [SerializeField] private float reset_time;
        private Transform player;
        private GameObject boss;
        private float life_time;
        private bool hit;
        private Animator anim;
        private BoxCollider2D orb_collider;
        private void Awake()
        {
            anim = GetComponent<Animator>();
            orb_collider = GetComponent<BoxCollider2D>();
            player = GameObject.Find("Player").transform;
            boss = GameObject.Find("Boss");
        }

        public void ActivateOrb()
        {
            hit = false;
            life_time = 0;
            gameObject.SetActive(true);
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            orb_collider.enabled = true;
        }

        private void Update()
        {
            life_time += Time.deltaTime;
            float movementSpeed = speed * Time.deltaTime;
            if (hit) return;
            transform.position = Vector3.MoveTowards(transform.position, player.position, movementSpeed);
            if (transform.position.x > 0)
                transform.Rotate(0f, 0f, 25f * Time.deltaTime);
            if (transform.position.x < 0)
                transform.Rotate(0f, 0f, 25f * -Time.deltaTime);
            if (life_time >= reset_time - 0.6f && life_time <= reset_time - 0.4f)
            {
                hit = true;
                anim.SetTrigger("Hit");
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            hit = true;

            base.OnTriggerStay2D(collision);
            orb_collider.enabled = false;

            if (anim != null)
                anim.SetTrigger("Hit");
            else
                gameObject.SetActive(false);
        }
        private void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}
    
