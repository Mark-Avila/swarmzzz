using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform offset;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private AudioClip[] shootAudios;
    [SerializeField] private PlayerShootLight playerShootLight;
    [SerializeField] private PlayerWeapon playerWeapon;

    private bool _continuous;
    private float _timeLastFire;
    
    // Update is called once per frame
    void Update()
    {
        if (_continuous)
        {
            float timeSinceLastFire = Time.time - _timeLastFire;

            if (timeSinceLastFire >= timeBetweenShots)
            {
                int randomIndex = Random.Range(0, shootAudios.Length);
                AudioClip randomGunSound = shootAudios[randomIndex];

                AudioManager.Instance.PlayAudio2d(randomGunSound);

                playerShootLight.FlashGun();

                FireBullet();

                _timeLastFire = Time.time;

            }
        }        
    }

    private void FireBullet()
    {
        if (playerWeapon.GetCurrentWeapon() == Weapon.item_shotgun)
        {
            FireShotgun();
        }
        else
        {
            GameObject newBullet = Instantiate(bullet, offset.position, transform.rotation);
            Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();

            rb.velocity = bulletSpeed * transform.right;
        }
    }

    private void FireShotgun()
    {
        // Instantiate the center bullet
        GameObject centerBullet = Instantiate(bullet, offset.position, transform.rotation);
        Rigidbody2D centerBulletRB = centerBullet.GetComponent<Rigidbody2D>();
        centerBulletRB.velocity = bulletSpeed * transform.right;

        // Calculate the angle for the angled bullets
        float angle = 15f; // Adjust this value to control the angle of the angled bullets

        // Instantiate the first angled bullet
        GameObject angledBullet1 = Instantiate(bullet, offset.position, transform.rotation * Quaternion.Euler(0f, 0f, angle));
        Rigidbody2D angledBullet1RB = angledBullet1.GetComponent<Rigidbody2D>();
        angledBullet1RB.velocity = Quaternion.Euler(0f, 0f, angle) * transform.right * bulletSpeed;

        // Instantiate the second angled bullet
        GameObject angledBullet2 = Instantiate(bullet, offset.position, transform.rotation * Quaternion.Euler(0f, 0f, -angle));
        Rigidbody2D angledBullet2RB = angledBullet2.GetComponent<Rigidbody2D>();
        angledBullet2RB.velocity = Quaternion.Euler(0f, 0f, -angle) * transform.right * bulletSpeed;
    }


    public void setTimeBetweenShots(float newTimeBetweenShots)
    {
        timeBetweenShots = newTimeBetweenShots;
    }

    private void OnFire(InputValue inputValue)
    {
        _continuous = inputValue.isPressed;
    } 
}
