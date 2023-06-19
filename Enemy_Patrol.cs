using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scriptes
{
    public class Enemy_Patrol : MonoBehaviour
    {
        [Header("Края патрулирования")]
        [SerializeField] internal Transform left_edge;
        [SerializeField] internal Transform right_edge;
        [Header("Враг")]
        [SerializeField] internal Transform enemy;
        [Header("Параметры движения")]
        [SerializeField] internal float speed;
        internal bool moving_left;
        internal Vector3 initial_scale;
        [Header("Параметры простоя врага")]
        [SerializeField] private float idle_duration;
        private float idle_timer;
        [Header ("Анимация врага")]
        [SerializeField] private Animator anim;
        [Header("Аудио Эффекты")]
        [SerializeField] private AudioClip[] enemy_steps;
        private float audio_step_cooldown;


        private void Awake()
        {
            initial_scale = enemy.localScale;
        }
        private void OnDisable() // Вызывается каждый раз когда объект отключается или уничтожается
        {
            anim.SetBool("Moving", false);
        }
        private void FixedUpdate()
        {
            if (moving_left)
            {
                if (enemy.position.x >= left_edge.position.x)
                    DirectionMove(-1);
                else
                    ChangeDirection();
            }
            else
            {
                if (enemy.position.x <= right_edge.position.x)
                    DirectionMove(1);
                else
                    ChangeDirection();
            }

        }

        public void ChangeDirection()
        {
            anim.SetBool("Moving", false);

            idle_timer += Time.deltaTime;

            if (idle_timer > idle_duration)
            {
                moving_left = !moving_left;
            }
        }
        public void DirectionMove(int _direction)
        {
            idle_timer = 0;
            anim.SetBool("Moving", true);
            enemy.localScale = new Vector3(Mathf.Abs(initial_scale.x) * _direction, initial_scale.y, initial_scale.z);
            enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed, enemy.position.y, enemy.position.z);
            audio_step_cooldown += Time.deltaTime;
            audio_step_cooldown = Mathf.Clamp(audio_step_cooldown, 0, 5f);
            if (audio_step_cooldown > 0.5f)
            {
                SoundManager.sounder.ActivateSounds(enemy_steps);
                audio_step_cooldown = 0f;
            }
        }
    }
}
