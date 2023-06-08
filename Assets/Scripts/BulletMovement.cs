using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private new Camera camera;

    private void Awake()
    {
        camera = Camera.main;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInChildren<EnemyHitpoints>())
        {
            EnemyHitpoints enemy = collision.GetComponentInChildren<EnemyHitpoints>();
            enemy.Bump((transform.up - enemy.transform.up).normalized);

            enemy.TakeDamage();
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        DestroyOnInvisible();
    }

    private void DestroyOnInvisible()
    {
        Vector2 screenPos = camera.WorldToScreenPoint(transform.position);
        if (screenPos.x < 0 || screenPos.x > camera.pixelWidth || screenPos.y < 0 || screenPos.y > camera.pixelHeight)
        {
            Destroy(gameObject);
        }
    }
}
