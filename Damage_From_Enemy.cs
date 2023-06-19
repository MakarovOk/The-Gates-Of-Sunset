using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scriptes
{
    public class Damage_From_Enemy : MonoBehaviour
    {
        [SerializeField] protected float damage;

        protected void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                collision.GetComponent<Health>().TakeDamage(damage,transform);
        }
    }
}