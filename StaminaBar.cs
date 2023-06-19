using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scriptes
{
    public class StaminaBar : MonoBehaviour
    {
        [SerializeField] private Stamina playerStamina;
        [SerializeField] private Image totalStaminaBar;
        [SerializeField] private Image currentStaminaBar;
        void Start()
        {
            totalStaminaBar.fillAmount = playerStamina.currently_stamina / 100;
        }
        void Update()
        {
            currentStaminaBar.fillAmount = playerStamina.currently_stamina / 100;
        }
    }
}
