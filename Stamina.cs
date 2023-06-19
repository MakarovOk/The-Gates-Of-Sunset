using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    [SerializeField] internal float starting_stamina;
    [SerializeField] private float regen_stamina_value;
    [SerializeField] private float regen_cooldown;
    public float currently_stamina { get; private set; }
    public float cooldown_timer;
    private bool isRegenerated;
    private void Awake()
    {
        currently_stamina = starting_stamina;
        isRegenerated = false;
    }

    public void AddStamina(float value_stamina)
    {
        currently_stamina = Mathf.Clamp(currently_stamina + value_stamina, 0, starting_stamina);
    }
    public void ReduceStamina(float reduce_stamina)
    {
        currently_stamina = Mathf.Clamp(currently_stamina - reduce_stamina, 0, starting_stamina);
        cooldown_timer = 0;
        isRegenerated = false;
    }
    private void FixedUpdate()
    {
        if (currently_stamina < starting_stamina)
        {
            cooldown_timer = Mathf.Clamp(cooldown_timer + Time.deltaTime, 0, regen_cooldown);
            if (cooldown_timer >= regen_cooldown && isRegenerated == false)
            {
                isRegenerated = true;
            }
        }
        if (isRegenerated && currently_stamina < starting_stamina)
        {
            currently_stamina += regen_stamina_value * Time.deltaTime;
            if (currently_stamina >= starting_stamina)
                isRegenerated = false;
        }
    }
}
