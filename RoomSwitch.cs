using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scriptes
{
    public class RoomSwitch : MonoBehaviour
    {
        [SerializeField] private GameObject active_room;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                active_room.SetActive(true);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                active_room.SetActive(false);
        }
    }
}
