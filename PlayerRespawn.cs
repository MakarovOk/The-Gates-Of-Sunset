using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scriptes
{
    public class PlayerRespawn : MonoBehaviour
    {
        private Transform currentCheckpoint;
        private Health player_health;
        private UIManager ui_manager;
        [SerializeField] private GameObject player;
        [SerializeField] private AudioClip[] checkpoint;
        [SerializeField] private Behaviour[] enemies;

        private void Awake()
        {
            player_health = GetComponent<Health>();            
            ui_manager = FindObjectOfType<UIManager>();
        }

        public void GameOverScreen()
        {
            ui_manager.GameOver();
        }

        public void RespawnPlayer()
        {
            player_health.Respawn();
            if (currentCheckpoint == null)
                transform.position = player.GetComponent<PlayerMovement>().start_position;
            else
                transform.position = currentCheckpoint.position;
            foreach (Behaviour component in enemies)
            {
                component.GetComponent<Health>().RespawnEnemy(enemies);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Checkpoint"))
            {
                currentCheckpoint = collision.transform;
                collision.GetComponent<Collider2D>().enabled = false;
                SoundManager.sounder.ActivateSounds(checkpoint);
                collision.GetComponent<Animator>().SetTrigger("Checkpoint_Activate");
            }
        }
    }
}
