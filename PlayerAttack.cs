using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scriptes
{
    public class PlayerAttack : MonoBehaviour
    {
        [Header("Параметры Атаки")]
        [SerializeField] private float splash_cooldown;
        [SerializeField] private float lunge_cooldown;
        [SerializeField] private float big_cooldown;
        [SerializeField] private float range; // Размер области урона по x
        [SerializeField] private float splash_damage; // Урон по области
        [SerializeField] private float lunge_damage; // Урон выпада
        [SerializeField] private float big_damage; // Урон сильной атаки
        [SerializeField] private Transform point_splash; // Точка, откуда ведется огонь
        [SerializeField] private Transform point_lunge; // Точка, откуда ведется огонь
        [SerializeField] private Transform point_big; // Точка, откуда ведется огонь
        [Header("Слой маски врага")]
        [SerializeField] private LayerMask enemy_layer; // Захват маски игрока
        [Header("Аудио Эффекты")]
        [SerializeField] private AudioClip[] audio_splash;
        [SerializeField] private AudioClip[] audio_big;
        private Animator anim;
        private PlayerMovement playerMovement;
        private Stamina stamina_controller;
        internal float cooldown_timer = 100;
        private void Awake()
        {
            anim = GetComponent<Animator>();
            playerMovement = GetComponent<PlayerMovement>();
            stamina_controller = GetComponent<Stamina>();
        }

        private void FixedUpdate()
        {
            cooldown_timer += Time.deltaTime;
            if (playerMovement.isDashing) 
                return;
        }

        public void BtnSplash()
        {
            if (CanAttack(splash_cooldown))
            {
                SoundManager.sounder.ActivateSounds(audio_splash);
                if (playerMovement.horizontal_input != 0 || playerMovement.horizontal_input_kbrd != 0)
                    anim.SetTrigger("RunAttack1");
                else if (playerMovement.horizontal_input == 0 || playerMovement.horizontal_input_kbrd == 0)
                    anim.Play("Attack1");
                GetComponent<Stamina>().ReduceStamina(10.0f);
                cooldown_timer = 0;
            }
        }
        public void BtnLunge()
        {
            if (CanAttack(lunge_cooldown) && playerMovement.horizontal_input <= 0.1f && playerMovement.horizontal_input >= -0.1f)
            {
                SoundManager.sounder.ActivateSounds(audio_splash);
                anim.SetTrigger("Attack2");
                GetComponent<Stamina>().ReduceStamina(15.0f);
                cooldown_timer = 0;
            }
        }
        public void BtnBig()
        {
            if (CanAttack(big_cooldown))
            {
                SoundManager.sounder.ActivateSounds(audio_big);
                if (playerMovement.horizontal_input != 0 || playerMovement.horizontal_input_kbrd != 0)
                    anim.SetTrigger("RunAttack3");
                else if (playerMovement.horizontal_input == 0 || playerMovement.horizontal_input_kbrd == 0)
                    anim.Play("Attack3");
                GetComponent<Stamina>().ReduceStamina(25.0f);
                cooldown_timer = 0;
            }
        }
        private bool CanAttack (float _cooldown)
        {
            return cooldown_timer >= _cooldown && playerMovement.IsGrounded()
            && stamina_controller.currently_stamina != 0;
        }
        private void Splash_Attack()
        {
            Attack(point_splash.position, range, splash_damage / 2);
        }

        private void Lunge_Attack()
        {
            Attack(point_lunge.position, range / 2, lunge_damage);
        }

        private void Big_Attack()
        {
            Attack(point_big.position, range + 0.3f, big_damage);
        }
        // Метод для разных видов атак
        private void Attack(Vector3 Point, float Range, float damage)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(Point, Range, enemy_layer);
            if (enemies.Length != 0)
            {
                for (int i = 0; i < enemies.Length; i++)
                {
                    if (enemies[i].GetComponent<Health>() != null)
                        enemies[i].GetComponent<Health>().TakeDamage(damage,transform);
                }
            }

        }

        private void OnDrawGizmos()
        {
            //SplashAttack
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(point_splash.position, range);

            //LungeAttack
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(point_lunge.position, range / 2);

            //BigAttack
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(point_big.position, range + 0.3f);
        }
    }
}
