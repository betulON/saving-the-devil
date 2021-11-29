using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarBehavior : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
    [SerializeField] Vector3 healthSliderOffset;
    [SerializeField] Slider manaSlider;
    [SerializeField] Vector3 manaSliderOffset;

    void Update()
    {
        healthSlider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + healthSliderOffset);
        manaSlider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + manaSliderOffset);
    }

    public void SetHealth(float health, float maxHealth)
    {
        healthSlider.gameObject.SetActive(health < maxHealth);
        healthSlider.value = health;
        healthSlider.maxValue = maxHealth;
    }

    public void SetMana(float mana, float maxMana)
    {
        bool isPlayer = transform.parent.tag == "Player";
        manaSlider.gameObject.SetActive(isPlayer && mana < maxMana);
        manaSlider.value = mana;
        manaSlider.maxValue = maxMana;
    }
}
