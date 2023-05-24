using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform offset;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private AudioClip shootAudio;

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
                AudioManager.Instance.PlayAudio(shootAudio);
                FireBullet();

                _timeLastFire = Time.time;
            }
        }        
    }

    private void FireBullet()
    {
        GameObject newBullet = Instantiate(bullet, offset.position, transform.rotation);
        Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();

        rb.velocity = bulletSpeed * transform.right;
    }

    private void OnFire(InputValue inputValue)
    {
        _continuous = inputValue.isPressed;
    } 
}
