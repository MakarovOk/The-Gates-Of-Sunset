using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scriptes
{
    public class FinalBossRun : StateMachineBehaviour
    {
        [SerializeField] private float attack_cooldown;
        public float speed = 3f;
        public float attack_range = 4f;
        private float cooldown_attack_timer = 100000;
        private Transform player_pos;
        private Rigidbody2D rb;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            player_pos = GameObject.FindGameObjectWithTag("Player").transform;
            rb = animator.GetComponent<Rigidbody2D>();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            cooldown_attack_timer += Time.deltaTime;
            //boss_look.LookAtPlayer();
            Vector2 target = new Vector2(player_pos.position.x, rb.position.y);
            Vector2 new_pos = Vector2.MoveTowards(rb.position, target, speed * Time.deltaTime);
            rb.MovePosition(new_pos);

            if(Vector2.Distance(player_pos.position, rb.position) <= attack_range)
            {
                animator.SetTrigger("Attack");
            }

        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.ResetTrigger("Attack");

        }
    }
}
