using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scriptes {
    public class Cursed_Archer : MonoBehaviour
    {
        [Header("Параметры Атаки")]
        [SerializeField] private float attack_cooldown; // Перезарядка атаки
        [SerializeField] private float reverse_cooldown;
        [SerializeField] private float range_x; // Размер области урона по x
        [SerializeField] private float range_y; // Размер области урона по y
        [SerializeField] private float range_x_behind; // Размер области обнаружения игрока сзади по x//
        [SerializeField] private float range_y_behind; // Размер области обнаружения игрока сзади по y//
        [SerializeField] private float damage; // Урон
        [SerializeField] private Transform point_for_attack; // Точка, откуда ведется огонь
        [SerializeField] private GameObject[] arrows;
        [Header("Параметры колайдера")]
        [SerializeField] private float collider_distance; // Значение для перемещения колайдера атаки относительно колайдера объекта
        [SerializeField] private float collider_distance_behind; // Значение для перемещения колайдера сзади относительно колайдера объекта//
        [SerializeField] private BoxCollider2D box_collider; // Поле колайдера для врага
        [Header("Слой маски игрока")]
        [SerializeField] private LayerMask player_layer; // Захват слоя игрока
        [Header("Аудио эффекты")]
        [SerializeField] private AudioClip[] attack_audio;

        private Animator anim;
        private Enemy_Patrol enemy_patrol;
        private float cooldown_attack_timer = 100000;
        private float cooldown_reverse_timer = 100000;
        private bool player_behind;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            enemy_patrol = GetComponentInParent<Enemy_Patrol>();
        }

        private void Update()
        {
            cooldown_attack_timer += Time.deltaTime;
            cooldown_reverse_timer += Time.deltaTime;
            if (PlayerSpotted())
            {
                if (cooldown_attack_timer >= attack_cooldown)
                {
                    cooldown_attack_timer = 0;
                    anim.SetTrigger("Attack_Archer");
                    SoundManager.sounder.ActivateSounds(attack_audio);
                }
            }
            if (enemy_patrol != null && !PlayerBehind() && !player_behind) // Патурлирование вкл, когда игрок не обнаружен, и выкл когда игрок обнаружен
                enemy_patrol.enabled = !PlayerSpotted();

            if (PlayerBehind() && !enemy_patrol.moving_left)
            {

                enemy_patrol.enabled = false;
                if (cooldown_reverse_timer >= reverse_cooldown)
                {
                    player_behind = true;
                    cooldown_reverse_timer = 0;
                    enemy_patrol.enemy.localScale = new Vector3(Mathf.Abs(enemy_patrol.initial_scale.x) * -1,
                        enemy_patrol.initial_scale.y, enemy_patrol.initial_scale.z);

                    //enemy_patrol.enemy.position = new Vector3(enemy_patrol.enemy.position.x + Time.deltaTime * -1 * enemy_patrol.speed, 
                    //    enemy_patrol.enemy.position.y, enemy_patrol.enemy.position.z);
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

                    //enemy_patrol.enemy.position = new Vector3(enemy_patrol.enemy.position.x + Time.deltaTime * 1 * enemy_patrol.speed,
                    //    enemy_patrol.enemy.position.y, enemy_patrol.enemy.position.z);
                    StartCoroutine(EnablePatrol());
                }
            }
        }
        
        private void Attack_Archer()
        {
            cooldown_attack_timer = 0;
            arrows[FindArrows()].transform.position = point_for_attack.position;
            arrows[FindArrows()].GetComponent<Arrow>().ActivateArrow();
        }

        private int FindArrows()
        {
            for (int i = 0; i < arrows.Length; i++)
            {
                if (!arrows[i].activeInHierarchy)
                    return i;
            }
            return 0;
        }

        private bool PlayerSpotted()
        {
            RaycastHit2D hit = Physics2D.BoxCast(box_collider.bounds.center
                + collider_distance * range_x * transform.localScale.x * transform.right,
                new Vector3(box_collider.bounds.size.x * range_x, box_collider.bounds.size.y * range_y, box_collider.bounds.size.z),
                0, Vector2.left, 0, player_layer);

            return hit.collider != null;
        }

        private bool PlayerBehind()//
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

            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(box_collider.bounds.center
                + collider_distance_behind * range_x_behind * transform.localScale.x * transform.right,
                new Vector3(box_collider.bounds.size.x * range_x_behind, box_collider.bounds.size.y * range_y_behind, box_collider.bounds.size.z));
        }

        private IEnumerator EnablePatrol()
        {
            yield return new WaitForSeconds(reverse_cooldown);
            player_behind = false;
        }
    }
}
