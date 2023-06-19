using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scriptes
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Настройки движения игрока")]
        [SerializeField] private float jump_power;
        [SerializeField] private float speed;
        [SerializeField] private float jump_cooldown;
        [SerializeField] private LayerMask groundLayer;
        private bool jump_ready;
        private float jump_cooldown_current = 0.0f;
        internal float horizontal_input;
        internal float horizontal_input_kbrd;
        internal float vertical_input;
        public Joystick joystick;
        [Header("Настройки подката")]
        [SerializeField] private float dashing_power;
        [SerializeField] private float dashing_time;
        [SerializeField] private float dashing_cooldown;
        private bool can_dash = true;
        public bool isDashing;
        [Header("Аудио Эффекты")]
        [SerializeField] private AudioClip[] audio_jump;
        [SerializeField] private AudioClip[] audio_steps;
        [SerializeField] private AudioClip[] audio_dash;
        private float audio_step_cooldown;
        [Header("Настройки отбрасывания")]
        [SerializeField] private float knockback_power;
        [SerializeField] private float knockback_time;
        [SerializeField] private Transform _center;
        private bool knockbacked = false;
        #region OTHER VALUES
        private Rigidbody2D body;
        private Animator anim;
        private BoxCollider2D boxCollider;
        private Stamina stamina_controller;
        internal Vector3 start_position;
        #endregion 
        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            boxCollider = GetComponent<BoxCollider2D>();
            stamina_controller = GetComponent<Stamina>();
            start_position = body.transform.position;
        }
        private void FixedUpdate()
        {
            jump_cooldown_current += Time.deltaTime;
            jump_cooldown_current = Mathf.Clamp(jump_cooldown_current, 0, jump_cooldown);
            if (isDashing) return;
            vertical_input = joystick.Vertical;
            horizontal_input = joystick.Horizontal;
            horizontal_input_kbrd = Input.GetAxis("Horizontal");
            if (!knockbacked) Movement();
            audio_step_cooldown += Time.deltaTime;
            audio_step_cooldown = Mathf.Clamp(audio_step_cooldown, 0, 5f);
            if (audio_step_cooldown > 0.3f && horizontal_input != 0 && IsGrounded())
            {
                SoundManager.sounder.ActivateSounds(audio_steps);
                audio_step_cooldown = 0f;
            }
            

            if (vertical_input >= 0.7f)
                Jump();
            anim.SetBool("Grounded", IsGrounded());   
        }
        private void Movement()
        {
            if (horizontal_input > 0.01f)
                transform.localScale = new Vector3((float)4.5, (float)4.5, 1);
            else if (horizontal_input < -0.01f)
                transform.localScale = new Vector3((float)-4.5, (float)4.5, 1);

            if (horizontal_input >= 0.2f || horizontal_input_kbrd > 0.01f)
            {
                anim.SetBool("MovementLR", horizontal_input >= 0.2f);
                body.velocity = new Vector2(horizontal_input * speed, body.velocity.y);
            }
            else if (horizontal_input <= -0.2f)
            {
                anim.SetBool("MovementLR", horizontal_input <= -0.2f);
                body.velocity = new Vector2(horizontal_input * speed, body.velocity.y);
            }
            else
            {
                anim.SetBool("MovementLR", false);
            }
        }
        public void Jump()
        {
            if (jump_cooldown_current >= jump_cooldown)
            {
                jump_ready = true;
                if (CanJump())
                {
                    stamina_controller.ReduceStamina(5.0f);
                    body.velocity = new Vector2(body.velocity.x, jump_power);
                    anim.SetTrigger("Jump");
                    SoundManager.sounder.ActivateSounds(audio_jump);
                    jump_cooldown_current = 0;
                }
            }
            else
            {
                jump_ready = false;
                
            }
        }

        public void StartDash()
        {
            if (CanDash())
            {
                StartCoroutine(Dash());
            }
        }
        public bool IsGrounded() // Проверка на поверхности ли игрок
        {
            RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
            return raycastHit.collider != null;
        }
        public bool CanDash()// Условия для использовния скольжения
        {
            return horizontal_input != 0 && can_dash & IsGrounded() & stamina_controller.currently_stamina != 0;
        }

        public bool CanJump()// Проверка на использование прыжка
        {
            return jump_ready && IsGrounded() & stamina_controller.currently_stamina != 0;
        }
        public IEnumerator Dash()
        {
            can_dash = false;
            isDashing = true;
            anim.SetTrigger("Slide");
            stamina_controller.ReduceStamina(20.0f);
            yield return new WaitForSeconds(0.1f);
            SoundManager.sounder.ActivateSounds(audio_dash);
            body.velocity = new Vector2(transform.localScale.x * dashing_power, 0f);
            yield return new WaitForSeconds(dashing_time);
            isDashing = false;
            yield return new WaitForSeconds(dashing_cooldown);
            can_dash = true;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("MovingPlatform")) 
                this.transform.parent = collision.transform;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("MovingPlatform"))
                this.transform.parent = null;
        }
    }
}