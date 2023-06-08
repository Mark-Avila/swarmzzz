using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlash : MonoBehaviour
{   
    [SerializeField] private SpriteRenderer spriteRenderer; // Reference to the sprite renderer component
    [SerializeField] private Color flashColor = Color.red; // Color to flash on hit
    [SerializeField] private float flashDuration = 0.2f; // Duration of each flash
    [SerializeField] private int flashCount = 3; // Number of times the sprite should flash

    private Color originalColor; // Original color of the sprite
    private bool isFlashing = false; // Flag to check if the flashing is already happening

    IEnumerator FlashRoutine()
    {
        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }

        isFlashing = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        originalColor = spriteRenderer.color;
    }

    public void FlashSprite()
    {
        if (isFlashing)
            return;

        isFlashing = true;
        StartCoroutine(FlashRoutine());
    }
}
