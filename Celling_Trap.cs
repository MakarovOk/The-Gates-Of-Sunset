using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scriptes {
    public class Celling_Trap : MonoBehaviour
    {
        [SerializeField] private float damage;
        [SerializeField] private float activation_delay;
        [SerializeField] private float active_time;
        private Animator anim;
        private bool triggered;
        private bool active;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (!triggered)
                {
                    StartCoroutine(ActivateCellingTrap());
                }
                if (active)
                {
                    collision.GetComponent<Health>().TakeDamage(damage, transform);
                }
            }
        }
        private IEnumerator ActivateCellingTrap()
        {
            triggered = true;
            yield return new WaitForSeconds(activation_delay);
            active = true;
            anim.SetBool("activated_trap", true);
            yield return new WaitForSeconds(active_time);
            active = false;
            triggered = false;
            anim.SetBool("activated_trap", false);

        }
    }
}