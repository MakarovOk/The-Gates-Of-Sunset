using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scriptes {
    public class Skeleton : MonoBehaviour
    {
        [Header("Параметры Атаки")]
        [SerializeField] private float attack_cooldown;
        [SerializeField] private float reverse_cooldown;
        [SerializeField] private float range_x; // Размер области урона по x
        [SerializeField] private float range_y; // Размер области урона по y
        [SerializeField] private float range_x_behind; // Размер области обнаружения игрока сзади по x//
        [SerializeField] private float range_y_behind; // Размер области обнаружения игрока сзади по y//
        [SerializeField] private float damage; // Урон
        [Header("Параметры колайдера")]
        [SerializeField] private float collider_distance; // Значение для перемещения колайдера атаки относительно колайдера объекта
        [SerializeField] private float collider_distance_behind; // Значение для перемещения колайдера сзади относительно колайдера объекта//
        [SerializeField] private BoxCollider2D box_collider; // Поле колайдера для врага
        [Header("Слой маски игрока")]
        [SerializeField] private LayerMask player_layer; // Захват маски игрока
        [SerializeField] private AudioClip[] attack_sounds;
        #region OTHER VALUES
        private Animator anim;
        private Health player_health;
        private Enemy_Patrol enemy_patrol;
        private float cooldown_attack_timer = 100;
        private float cooldown_reverse_timer = 100;
        private bool player_behind;
        private GameObject player;
        #endregion
        private void Awake()
        {
            anim = GetComponent<Animator>();
            enemy_patrol = GetComponentInParent<Enemy_Patrol>();
            player = GameObject.Find("Player");
        }
        private void FixedUpdate()
        {
            PlayerBehind();
            cooldown_attack_timer += Time.deltaTime;
            cooldown_reverse_timer += Time.deltaTime;
            if (PlayerSpotted())
            {
                if (cooldown_attack_timer >= attack_cooldown)
                {
                    cooldown_attack_timer = 0;
                    anim.SetTrigger("Attack");
                }
            }
            if (enemy_patrol != null && !PlayerBehind() && !player_behind)
            { 
                enemy_patrol.enabled = !PlayerSpotted();                
            }
            if (PlayerBehind() && !enemy_patrol.moving_left)
            {
                
                enemy_patrol.enabled = false;
                if (cooldown_reverse_timer >= reverse_cooldown)
                {
                    player_behind = true;
                    cooldown_reverse_timer = 0;
                    enemy_patrol.enemy.localScale = new Vector3(Mathf.Abs(enemy_patrol.initial_scale.x) * -1, 
                        enemy_patrol.initial_scale.y, enemy_patrol.initial_scale.z);
                    StartCoroutine(EnablePatrol());
                }
            }
            if (PlayerBehind() && enemy_patrol.moving_left)
            {
                enemy_patrol.enabled = false;
                if (cooldown_reverse_timer >= reverse_cooldown)
                {
                    player_behind = true;
                    cooldown_reverse_timer = 0;
                    enemy_patrol.enemy.localScale = new Vector3(Mathf.Abs(enemy_patrol.initial_scale.x) * 1, 
                        enemy_patrol.initial_scale.y, enemy_patrol.initial_scale.z);
                    StartCoroutine(EnablePatrol());
                }
            }
        }
        private bool PlayerSpotted()
        {
            RaycastHit2D hit = Physics2D.BoxCast(box_collider.bounds.center 
                + collider_distance * range_x * transform.localScale.x * transform.right, 
                new Vector3(box_collider.bounds.size.x * range_x, box_collider.bounds.size.y * range_y, box_collider.bounds.size.z),
                0, Vector2.left, 0, player_layer);

            if (hit.collider != null)
            {
                player_health = hit.transform.GetComponent<Health>();
            }
            return hit.collider != null;
        }
        private bool PlayerBehind()
        {
            RaycastHit2D behind = Physics2D.BoxCast(box_collider.bounds.center
                + collider_distance_behind * range_x_behind * transform.localScale.x * transform.right,
                new Vector3(box_collider.bounds.size.x * range_x_behind, box_collider.bounds.size.y * range_y_behind, box_collider.bounds.size.z),
                0, Vector2.left, 0, player_layer);

            return behind.collider != null;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(box_collider.bounds.center 
                + collider_distance * range_x * transform.localScale.x * transform.right, 
                new Vector3(box_collider.bounds.size.x * range_x, box_collider.bounds.size.y * range_y, box_collider.bounds.size.z));
            //
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(box_collider.bounds.center
                + collider_distance_behind * range_x_behind * transform.localScale.x * transform.right,
                new Vector3(box_collider.bounds.size.x * range_x_behind, box_collider.bounds.size.y * range_y_behind, box_collider.bounds.size.z));
        }
        private void DamageFromSkeleton()
        {
            SoundManager.sounder.ActivateSounds(attack_sounds);
            if (PlayerSpotted())
            {
                // Если враг в зоне досягаемости, наносится урон
                player_health.TakeDamage(damage,transform);
            }
        }
        private IEnumerator EnablePatrol()
        {
            yield return new WaitForSeconds(reverse_cooldown);
            player_behind = false;
        }
    }

}