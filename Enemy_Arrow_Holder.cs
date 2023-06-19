using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scriptes
{
    public class Enemy_Arrow_Holder : MonoBehaviour
    {
        [SerializeField] private Transform enemy;


        private void Update()
        {
            transform.localScale = enemy.localScale;
        }
    }
}