using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scriptes
{
    public class Health : MonoBehaviour
    {
        public UnityEvent OnHitBegin, OnHitDone, OnDead;

        [SerializeField] private float starting_health;
        [SerializeField] private float cooldown_damage;
        public float currently_health { get; private set; }
        public Animator anim;
        private Stamina stamina_controller;
        private GameObject lever;
        public bool dead;
        private Rigidbody2D rbody;
        [Header("јудиоЁффекты")]
        [SerializeField] private AudioClip[] hurt;
        [SerializeField] private AudioClip[] died;

        [Header("ќбъекты")]
        [SerializeField] private Behaviour[] objects;
        private void Awake()
        {
            currently_health = starting_health;
            anim = GetComponent<Animator>();
            stamina_controller = GetComponent<Stamina>();
            lever = GameObject.Find("Lever");
            rbody = GetComponent<Rigidbody2D>();
        }
        public void AddHealth(float value_health)
        {
            currently_health = Mathf.Clamp(currently_health + value_health, 0, starting_health);
        }

        public void Respawn()
        {
            dead = false;
            AddHealth(starting_health);
            stamina_controller.AddStamina(stamina_controller.starting_stamina);
            anim.ResetTrigger("Dead");
            anim.Play("idle");
            foreach (Behaviour component in objects)
                component.enabled = true;
        }
        public void RespawnEnemy(Behaviour[] _enemies)
        {
            foreach (Behaviour component in _enemies)
            {
                if (component.enabled != true && component.transform.GetComponent<Rigidbody2D>() != null &&
                    component.GetComponentInChildren<BoxCollider2D>() != null)
                {
                    component.enabled = true;
                    component.GetComponentInChildren<BoxCollider2D>().enabled = true;
                    component.GetComponent<Health>().dead = false;
                    component.GetComponent<Health>().anim.ResetTrigger("Dead");
                    component.GetComponent<Health>().anim.SetBool("Moving", true);
                }
                AddHealth(starting_health);
            }
        }
        public void PushAway(Transform sender)
        {
            if (rbody == null)
            {
                return;
            }
            if(transform.position.x - sender.transform.position.x > 0)
            {
                rbody.velocity = new Vector2(0, 0);
                rbody.AddForce(Vector3.right * 3, ForceMode2D.Impulse);
                rbody.AddForce(Vector2.up * 7, ForceMode2D.Impulse);
            }
            else
            {
                rbody.velocity = new Vector2(0, 0);
                rbody.AddForce(Vector3.left * 3, ForceMode2D.Impulse);
                rbody.AddForce(Vector2.up * 7, ForceMode2D.Impulse);
            }
        }
        public void TakeDamage(float _damage, Transform attacker)
        {
            if (this.GetComponent<BossBehaviour>() != null && this.GetComponent<BossBehaviour>().isInvulnerable)
                return;
            if (this.GetComponent<PlayerMovement>() != null && this.GetComponent<PlayerMovement>().isDashing && attacker.CompareTag("Enemy"))
                return;

            currently_health = Mathf.Clamp(currently_health - _damage, 0, starting_health);
                if (currently_health > 0)
            {
                OnHitBegin?.Invoke();
                StartCoroutine(KnockBacking());
                anim.SetTrigger("Hurt");                
                SoundManager.sounder.ActivateSounds(hurt);
                PushAway(attacker);
                if (this.GetComponent<PlayerAttack>() != null)
                    this.GetComponent<PlayerAttack>().cooldown_timer = 100;
                StartCoroutine(ToDamage());
            }
            else
            {
                if (!dead)
                {
                    OnDead?.Invoke();
                    anim.SetTrigger("Dead");
                    dead = true;
                    Debug.Log(dead);
                    SoundManager.sounder.ActivateSounds(hurt);
                    SoundManager.sounder.ActivateSounds(died);
                    foreach (Behaviour component in objects)
                    {
                        component.enabled = false;
                        if (component.GetComponentInChildren<BoxCollider2D>().enabled == true & component.CompareTag("Enemy"))
                        {
                            if(lever != null)
                                lever.GetComponent<ActivateLever>().score++;
                            component.GetComponentInChildren<BoxCollider2D>().enabled = false;
                        }
                    }
                    
                }
            }
        }
        private IEnumerator KnockBacking()
        {
            yield return new WaitForSeconds(0.5f);
            OnHitDone?.Invoke();
        }
        private IEnumerator ToDamage()
        {
            Physics2D.IgnoreLayerCollision(8, 11, true);
            yield return new WaitForSeconds(cooldown_damage);
            Physics2D.IgnoreLayerCollision(8, 11, false);
        }
    }
}