using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scriptes
{
    public class Trap_Saw : MonoBehaviour
    {
        [SerializeField] private float damage;
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                collision.GetComponent<Health>().TakeDamage(Random.Range(damage,damage + 10),transform);
        }
    }
}