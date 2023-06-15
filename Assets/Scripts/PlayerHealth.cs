using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float damageCooldown = 0.5f;
    [SerializeField] private EnemyFlash playerFlash;
    [SerializeField] private AudioClip[] playerHurtAudios;
    [SerializeField] private AudioClip playerDeadAudio;
    [SerializeField] private GameOverManager gameOverScreen;
    [Tooltip("Player animator"), SerializeField] private Animator animator;

    private int currentHealth;
    private bool isAlive = true;
    
    private bool canTakeDamage = true;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    private void FixedUpdate()
    {
        if (currentHealth <= 0 && isAlive)
        {
            animator.SetBool("isDead", true);
            AudioManager.Instance.PlayQuickAudio(playerDeadAudio);
            isAlive = false;
            StartCoroutine(DeathCoroutine());
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (canTakeDamage && isAlive)
        {
            if (collision.gameObject.CompareTag("zombie"))
                Damage(1);
            else if (collision.gameObject.CompareTag("alien"))
                Damage(3);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canTakeDamage && isAlive)
        {
            if (collision.gameObject.CompareTag("beetle"))
                Damage(1);
        }
    }

    private void UpdateHealthText()
    {
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
        PlayHurtSound();
        UpdateHealthText();
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
        UpdateHealthText();
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

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(3.5f);
        gameOverScreen.ShowGameOverScreen();
    }
}
