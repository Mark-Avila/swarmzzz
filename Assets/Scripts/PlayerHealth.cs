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
    [SerializeField] private AudioClip playerDeadAudio;
    [Tooltip("Player animator"), SerializeField] private Animator animator;

    private int currentHealth;
    private bool isAlive = true;
    
    private bool canTakeDamage = true;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        if (currentHealth <= 0 && isAlive)
        {
            animator.SetBool("isDead", true);
            AudioManager.Instance.PlayQuickAudio(playerDeadAudio);
            isAlive = false;
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
        text.SetText($"Health: {currentHealth}/{maxHealth}");
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
