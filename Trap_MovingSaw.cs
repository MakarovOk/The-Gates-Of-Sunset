using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scriptes
{
    public class Trap_MovingSaw : MonoBehaviour
    {
        private HingeJoint2D hinge;
        [SerializeField] private float damage;
        [SerializeField] private float speed_x;
        [SerializeField] private float speed_y;
        [SerializeField] private float moving_distance;
        private bool moving_left;
        private bool moving_up;
        private float left_edge;
        private float right_edge;
        private float up_edge;
        private float down_edge;

        private void Awake()
        {
            left_edge = transform.position.x - moving_distance;
            right_edge = transform.position.x + moving_distance;
            up_edge = transform.position.y - moving_distance;
            down_edge = transform.position.y + moving_distance;
            hinge = GetComponent<HingeJoint2D>();
        }
        private void FixedUpdate()
        {
            if (this.hinge != null)
            {
                JointMotor2D motorHinge = hinge.motor;
                JointAngleLimits2D limitsHinge = hinge.limits;

                if (hinge.jointAngle >= limitsHinge.max)
                {
                    motorHinge.motorSpeed = -50;
                    hinge.motor = motorHinge;
                }
                else if (hinge.jointAngle <= limitsHinge.min)
                {
                    motorHinge.motorSpeed = 50;
                    hinge.motor = motorHinge;
                }
            }
            if (speed_x != 0 && speed_y == 0)
                MovementX();
            if (speed_x == 0 && speed_y != 0)
                MovementY();


        }

        private void MovementX()
        {
            if (moving_left)
            {
                if (transform.position.x > left_edge)
                {
                    transform.position = new Vector3(transform.position.x - speed_x * Time.deltaTime, transform.position.y, transform.position.z);
                }
                else
                    moving_left = false;
            }
            else
            {
                if (transform.position.x < right_edge)
                {
                    transform.position = new Vector3(transform.position.x + speed_x * Time.deltaTime, transform.position.y, transform.position.z);
                }
                else
                    moving_left = true;
            }
        }

        private void MovementY()
        {
            if (moving_up)
            {
                if (transform.position.y > up_edge)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - speed_y * Time.deltaTime, transform.position.z);
                }
                else
                    moving_up = false;
            }
            else
            {
                if (transform.position.y < down_edge)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + speed_y * Time.deltaTime, transform.position.z);
                }
                else
                    moving_up = true;
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<Health>().TakeDamage(damage,transform);
            }
        }
    }
}