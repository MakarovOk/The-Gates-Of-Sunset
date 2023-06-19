using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scriptes
{
    public class HealthPotion : MonoBehaviour
    {
        [SerializeField] private float health_potion_value;
        [SerializeField] private AudioClip[] heal_sound;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                SoundManager.sounder.ActivateSounds(heal_sound);
                collision.GetComponent<Health>().AddHealth(health_potion_value);
                gameObject.SetActive(false);
            }
        }

    }
}