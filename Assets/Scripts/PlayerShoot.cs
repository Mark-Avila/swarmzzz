using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bullet;
    public float bulletSpeed;
    public Transform offset;
    public float timeBetweenShots;

    private bool continuous;
    private float timeLastFire;

    // Update is called once per frame
    void Update()
    {
        if (continuous)
        {

            float timeSinceLastFire = Time.time - timeLastFire;

            if (timeSinceLastFire >= timeBetweenShots)
            {
                FireBullet();

                timeLastFire = Time.time;
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
        continuous = inputValue.isPressed;
    } 
}
