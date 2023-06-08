using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerShootLight : MonoBehaviour
{
    [SerializeField] private Light2D light2d;
    [SerializeField] private float flashDuration = 0.2f;

    public void FlashGun()
    {
        StartCoroutine(FlashCoroutine());
    }

    private System.Collections.IEnumerator FlashCoroutine()
    {
        // Set the intensity to 1
        light2d.intensity = 1f;

        // Wait for the specified duration
        yield return new WaitForSeconds(flashDuration);

        // Set the intensity back to 0
        light2d.intensity = 0f;
    }
}
