using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] private PlayerShoot playerShoot;
    [SerializeField] private AudioClip pickUpAudio;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerShoot.Reload();
            Destroy(gameObject);
        }
    }
}
