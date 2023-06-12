using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [Tooltip("Rigid body")] [SerializeField] private Transform parent;

    private Rigidbody2D rb;

    private void Start()
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
        ////    Prevents enemies from moving right after instantiating
        ////    
        ////    (I don't know why this happens trust me an 
        ////    error pops up if I remove this)
        if (rb != null)
        {
            // Define a small offset value to add to the starting position of each raycast
            Vector2 direction = targetPosition - rb.position;
            rb.AddForce(maxSpeed * 10 * Time.fixedDeltaTime * direction);
            FaceTowardsVelocity();
        }
    }
}
