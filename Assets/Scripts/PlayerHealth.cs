using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private TextMeshProUGUI text;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("zombie"))
        {
            Damage(1);
            return;
        }
        else if (collision.gameObject.CompareTag("alien"))
        {
            Damage(3);
        }
        else if (collision.gameObject.CompareTag("beetle"))
        {
            Damage(1);
        }

        text.SetText($"Health: {currentHealth}/{maxHealth}");
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void Damage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }
}
