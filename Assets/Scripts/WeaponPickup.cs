using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private PlayerWeapon playerWeapon;
    [SerializeField] private AudioClip pickUpAudio;
    [SerializeField] private WeaponItem weaponItem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerWeapon.SwitchWeapon(weapon, weaponItem);
            AudioManager.Instance.PlayQuickAudio(pickUpAudio);
            Destroy(gameObject);
        }
    }
}
