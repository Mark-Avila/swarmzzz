using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ZombieMovement>())
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
