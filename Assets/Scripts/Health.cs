using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth = 50;
    float health;

    [SerializeField] HealthbarBehavior healthbar;

    void Start()
    {
        health = maxHealth;
        healthbar.SetHealth(health, maxHealth);
    }

    public void ReduceHealth(float damage)
    {
        health -= damage;
        healthbar.SetHealth(health, maxHealth);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
