using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scriptes
{
    public class BossBehaviour : MonoBehaviour
    {
        [Header("Параметры Атаки")]
        [SerializeField] private float attack_cooldown;
        [SerializeField] private float reverse_cooldown;
        [SerializeField] private float range_x; // Размер области урона по x
        [SerializeField] private float range_y; // Размер области урона по y
        [SerializeField] private float range_x_range; // Размер области обнаружения игрока сзади по x//
        [SerializeField] private float range_y_range; // Размер области обнаружения игрока сзади по y//
        [SerializeField] private float damage; // Урон
        [SerializeField] private Transform orbPortal_point;
        [SerializeField] private GameObject[] orbs;
        [Header("Параметры колайдера")]
        [SerializeField] private float collider_distance; // Значение для перемещения колайдера атаки относительно колайдера объекта
        [SerializeField] private float collider_distance_range; // Значение для перемещения колайдера сзади относительно колайдера объекта//
        [SerializeField] private BoxCollider2D box_collider; // Поле колайдера для врага
        [Header("Слой маски игрока")]
        [SerializeField] private LayerMask player_layer; // Захват маски игрока

        [SerializeField] private Transform player;
        [SerializeField] private float speed;
        public bool isFlipped = false;
        public bool isAttaking = false;
        public bool isRaged = false;
        public bool isInvulnerable = false;
        private Animator anim;
        private Health player_health;
        private Rigidbody2D rb;
        private float cooldown_attack_timer = 100000;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            player_health = GetComponent<Health>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            cooldown_attack_timer += Time.deltaTime;
            // вторая стадия и смена характеристик
            if (this.GetComponent<Health>().currently_health <= 50 && isRaged == false)
            {
                range_x = 3.4f; range_y = 1.5f;
                isRaged = true;
                isInvulnerable = true;
                anim.SetTrigger("Raged");
                damage *= 2;
                StartCoroutine(Cooldown(0, 3f));
            }
            // Движение вкл
            if (!PlayerSpotted() && isAttaking == false && isRaged == false && isInvulnerable == false)
            {
                Movement(speed);
                anim.SetBool("MovementFB", true);
            }
            else if(!PlayerSpotted() && isAttaking == false && isRaged == true && isInvulnerable == false)// джвижение во второй стадии
            {
                Movement(speed + 1);
                anim.SetBool("MovementFB", true);
            }
            else
            {
                anim.SetBool("MovementFB", false);
            }


            LookAtPlayer();
            MeleeAttackBoss();
            RangedAttackBoss();
        }

        private void MeleeAttackBoss()// ближний бой
        {
            if (PlayerSpotted() && cooldown_attack_timer >= attack_cooldown && isRaged == false && isAttaking == false)
            {
                cooldown_attack_timer = 0;
                anim.SetTrigger("Attack");
                isAttaking = true;
                StartCoroutine(Cooldown(1, 1.5f));
            }
            else if (PlayerSpotted() && cooldown_attack_timer >= attack_cooldown && isRaged == true && isAttaking == false)// ближний бой во второй стадии
            {
                cooldown_attack_timer = 0;
                anim.SetTrigger("RageAttack");
                isAttaking = true;
                StartCoroutine(Cooldown(1, 1.2f));
            }
        }
        
        private void RangedAttackBoss()// дальняя атака и атака сферами
        {
            if (PlayerSpottedFar() && cooldown_attack_timer >= attack_cooldown && isRaged == false && isAttaking == false)
            {
                cooldown_attack_timer = 0;
                anim.SetTrigger("RangedAttack");
                isAttaking = true;
                StartCoroutine(Cooldown(1, 1.5f));
            }
            else if (PlayerSpottedFar() && cooldown_attack_timer >= attack_cooldown && isRaged == true && isAttaking == false)// дальний бой во второй стадии
            {
                cooldown_attack_timer = 0;

                anim.SetTrigger("OrbPortalAttack");
                
                isAttaking = true;
                StartCoroutine(Cooldown(1, 1.5f));
            }
            
        }

        private void OrbAttack()// активация атаки сверху
        {
            if (isRaged == false)
            {
                orbPortal_point.position = new Vector2(player.position.x, player.position.y + 5);
                for (int i = 0; i < orbs.Length; i++)
                {
                    if (orbs[i].activeInHierarchy)
                        orbs[i].SetActive(false);
                }
                cooldown_attack_timer = 0;
                orbs[FindOrbs()].transform.position = orbPortal_point.position;
                orbs[FindOrbs()].GetComponent<BossOrb>().ActivateOrb();
            }
        }
        private int FindOrbs()// перебор неактивных сфер в массиве
        {
            for (int i = 0; i < orbs.Length; i++)
            {
                if (!orbs[i].activeInHierarchy)
                    return i;
            }
            return 0;
        }
        private void OrbPortalAttack()
        {
            if (isRaged == true)
            {
                orbPortal_point.position = new Vector2(player.position.x, player.position.y + 1.85f);
                orbPortal_point.GetComponent<Animator>().SetTrigger("Attack");
            }
        }
        private void Movement(float _speed)// передвижение 
        {
            Vector3 targetPosition = player.transform.position;
            targetPosition.y = transform.position.y;
            targetPosition.z = transform.position.z;
            if (isFlipped == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
            }
            else if (isFlipped == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);

            }
        }
        public void LookAtPlayer() // разворот
        {
            Vector3 flipped = transform.localScale;
            flipped.z *= -1f;

            if (transform.position.x > player.position.x && isFlipped && isAttaking == false)
            {
                transform.localScale = flipped;
                transform.Rotate(0f, 180f, 0f);
                isFlipped = false;
            }
            else if (transform.position.x < player.position.x && !isFlipped && isAttaking == false)
            {
                transform.localScale = flipped;
                transform.Rotate(0f, 180f, 0f);
                isFlipped = true;
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
                rb.velocity = new Vector2(0, 0);
            }

            return hit.collider != null;
        } // обнаружение игрока в области для ближней атаки

        private bool PlayerSpottedFar()
        {
            RaycastHit2D spotted = Physics2D.BoxCast(box_collider.bounds.center
                + collider_distance_range * range_x_range * transform.localScale.x * transform.right,
                new Vector3(box_collider.bounds.size.x * range_x_range, box_collider.bounds.size.y * range_y_range, box_collider.bounds.size.z),
                0, Vector2.left, 0, player_layer);

            if (spotted.collider != null)
                player_health = spotted.transform.GetComponent<Health>();

            return spotted.collider != null;
        } // обнаружение игрока в области для дальней атаки
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(box_collider.bounds.center
                + collider_distance * range_x * transform.localScale.x * transform.right,
                new Vector3(box_collider.bounds.size.x * range_x, box_collider.bounds.size.y * range_y, box_collider.bounds.size.z));

            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(box_collider.bounds.center
                + collider_distance_range * range_x_range * transform.localScale.x * transform.right,
                new Vector3(box_collider.bounds.size.x * range_x_range, box_collider.bounds.size.y * range_y_range, box_collider.bounds.size.z));
        } // разметка для областей атаки
        private void DamageFromFinalBoss() // нанесение урона по игроку
        {
            if (PlayerSpotted())
            {
                // Если враг в зоне досягаемости, наносится урон
                player_health.TakeDamage(damage,transform);
            }
        }
        private IEnumerator Cooldown(int _value, float timer) // перезарядка или задержка (int 1 - для атаки, int 0 для активации 2 стадии)
        {
            Physics2D.IgnoreLayerCollision(6, 15, true);
            yield return new WaitForSeconds(timer);
            Physics2D.IgnoreLayerCollision(6, 15, false);
            if (_value == 1)
                isAttaking = false;
            else if(_value == 0)
            {
                anim.ResetTrigger("Attack");
                isInvulnerable = false;
            }    
        }
    }
}