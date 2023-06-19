using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scriptes
{
    public class OffBoxEnemies : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                transform.parent.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                transform.parent.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }
}
