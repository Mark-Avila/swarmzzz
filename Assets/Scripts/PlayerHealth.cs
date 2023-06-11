using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float damageCooldown = 0.5f;
    [SerializeField] private EnemyFlash playerFlash;
    [SerializeField] private AudioClip[] playerHurtAudios;

    private int currentHealth;
    private bool canTakeDamage = true;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canTakeDamage)
        {
            if (collision.gameObject.CompareTag("zombie") || collision.gameObject.CompareTag("beetle"))
            {
                Damage(1);
                PlayHurtSound();
            }
            else if (collision.gameObject.CompareTag("alien"))
            {
                Damage(3);
                PlayHurtSound();
            }

        }

        text.SetText($"Health: {currentHealth}/{maxHealth}");
    }

    private void PlayHurtSound()
    {
        int randomIndex = Random.Range(0, playerHurtAudios.Length);
        AudioClip randomHurtSound = playerHurtAudios[randomIndex];

        AudioManager.Instance.PlayQuickAudio(randomHurtSound);
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
        StartCoroutine(StartDamageCooldown());
        playerFlash.FlashSprite();
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

    private IEnumerator StartDamageCooldown()
    {
        canTakeDamage = false;

        // Wait for the cooldown duration
        yield return new WaitForSeconds(damageCooldown);

        // Cooldown complete, allow player to take damage again
        canTakeDamage = true;
    }
}
