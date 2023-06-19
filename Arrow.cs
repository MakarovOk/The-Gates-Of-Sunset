using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scriptes
{
    public class Arrow : Damage_From_Enemy
    {
        [SerializeField] private float speed;
        [SerializeField] private float reset_time;
        [SerializeField] private AudioClip[] hit_audios;
        private float life_time;
        private bool hit;
        private Animator anim;
        private BoxCollider2D arrow_collider;
        private void Awake()
        {
            anim = GetComponent<Animator>();
            arrow_collider = GetComponent<BoxCollider2D>();
        }
        public void ActivateArrow()
        {
            hit = false;
            life_time = 0;
            gameObject.SetActive(true);
            arrow_collider.enabled = true;
        }
        private void Update()
        {
            if (hit) return;
            float movementSpeed = speed * Time.deltaTime;
            transform.Translate(movementSpeed, 0, 0);

            life_time += Time.deltaTime;
            if (life_time > reset_time)
                gameObject.SetActive(false);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            hit = true;
            base.OnTriggerStay2D(collision);
            SoundManager.sounder.ActivateSounds(hit_audios);
            arrow_collider.enabled = false;
            if (anim != null)
                anim.SetTrigger("ArrowHit");
            else
                gameObject.SetActive(false);
        }
        private void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}