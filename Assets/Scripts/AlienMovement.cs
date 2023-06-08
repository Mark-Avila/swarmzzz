using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienMovement : MonoBehaviour
{
    [SerializeField]
    private Transform parent;

    private Rigidbody2D rb;

    void Start()
    {
        rb = parent.GetComponent<Rigidbody2D>();
    }

    private void FaceTowardsVelocity()
    {
        Vector2 velocity = rb.velocity;
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        parent.rotation = Quaternion.AngleAxis(angle + -90f, Vector3.forward);
    }

    public void MoveTowards(Vector2 targetPosition, float maxSpeed)
    {
        //// Define a small offset value to add to the starting position of each raycast
        Vector2 direction = targetPosition - rb.position;
        rb.AddForce(maxSpeed * 10 * Time.fixedDeltaTime * direction);
        FaceTowardsVelocity();
    }
}
