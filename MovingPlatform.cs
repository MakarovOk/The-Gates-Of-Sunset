using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scriptes
{
    public class MovingPlatform : MonoBehaviour
    {
        public bool moving_left;
        [SerializeField] Transform target1;
        [SerializeField] Transform target2;
        [SerializeField] private float speed;
        private void FixedUpdate()
        {
            if (moving_left)
            {
                transform.position = Vector3.MoveTowards(transform.position, target1.position, speed * Time.deltaTime);
                if (transform.position == target1.position)
                {
                    moving_left = false;
                }
            }

            if (moving_left == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, target2.position, speed * Time.deltaTime);
                if (transform.position == target2.position)
                {
                    moving_left = true;
                }
            }
        }
    }
}