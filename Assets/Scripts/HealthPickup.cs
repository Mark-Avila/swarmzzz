using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private AudioClip pickUpAudio;
    [SerializeField] private int healPower = 3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerHealth.Heal(3);
            AudioManager.Instance.PlayQuickAudio(pickUpAudio);
            Destroy(gameObject);
        }
    }

}
