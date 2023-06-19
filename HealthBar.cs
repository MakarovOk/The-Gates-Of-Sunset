using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scriptes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health playerHealth;
        [SerializeField] private Image totalHealthBar;
        [SerializeField] private Image currentHealthBar;

        private void Start()
        {
            totalHealthBar.fillAmount = playerHealth.currently_health / 100;
        }

        private void Update()
        {
            currentHealthBar.fillAmount = playerHealth.currently_health / 100;
        }
    }
}
