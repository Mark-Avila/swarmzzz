using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitpoints : MonoBehaviour
{
    [SerializeField] private int hitpoints = 3;
    [SerializeField] private GameObject parent;
    [SerializeField] private float onHitForce = 5f;

    private int damage;
    private Rigidbody2D rb;
    private EnemyFlash enemyFlash;

    private void Start()
    {
        enemyFlash = parent.GetComponent<EnemyFlash>();
        rb = parent.GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage)
    {
        hitpoints -= damage;

        if (hitpoints <= 0)
        {
            Destroy(parent);       
        } 
        else
        {
            enemyFlash.FlashSprite();
        }
    }

    public void Bump(Vector2 direction)
    {
        rb.AddForce(direction * onHitForce, ForceMode2D.Impulse);
    }
}
