using UnityEngine;
using Friedforfun.ContextSteering.PlanarMovement;

public class ZombieMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float maxRayDistance = 1.0f;
    [SerializeField] private SelfSchedulingPlanarController steer;
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
        //// Define a small offset value to add to the starting position of each raycast
        Vector2 direction = targetPosition - rb.position;
        rb.AddForce(maxSpeed * 10 * Time.fixedDeltaTime * direction);
        FaceTowardsVelocity();
    }

    private void MoveTowardsOld(Vector2 targetPosition, float maxSpeed)
    {
        ////Define a small offset value to add to the starting position of each raycast
        float offset = 1.0f;

        //// Perform a raycast in each of the four directions (top, left, down, and right)
        RaycastHit2D hitTop = Physics2D.Raycast(parent.position + new Vector3(0, offset, 0), Vector2.up, maxRayDistance);
        RaycastHit2D hitLeft = Physics2D.Raycast(parent.position + new Vector3(-offset, 0, 0), Vector2.left, maxRayDistance);
        RaycastHit2D hitDown = Physics2D.Raycast(parent.position + new Vector3(0, -offset, 0), Vector2.down, maxRayDistance);
        RaycastHit2D hitRight = Physics2D.Raycast(parent.position + new Vector3(offset, 0, 0), Vector2.right, maxRayDistance);

        Debug.DrawRay(parent.position + new Vector3(0, offset, 0), Vector2.up * maxRayDistance, Color.red);
        Debug.DrawRay(parent.position + new Vector3(-offset, 0, 0), Vector2.left * maxRayDistance, Color.red);
        Debug.DrawRay(parent.position + new Vector3(0, -offset, 0), Vector2.down * maxRayDistance, Color.red);
        Debug.DrawRay(parent.position + new Vector3(offset, 0, 0), Vector2.right * maxRayDistance, Color.red);


        ////// Calculate the direction towards the target position
        Vector2 direction = targetPosition - rb.position;

        float turnSpeed = 30f;

        // Check if each raycast hit a collider that is not the player nor another zombie
        if (hitRight.collider != null && hitRight.collider.name != "Player" && !hitRight.collider.CompareTag("Zombie"))
        {

            float degrees = GetAngle(direction);

            if (degrees >= 90f && degrees <= 180f)
            {
                rb.AddForce(maxSpeed * turnSpeed * Time.fixedDeltaTime * Vector2.down);
            }
            else
            {
                rb.AddForce(maxSpeed * turnSpeed * Time.fixedDeltaTime * Vector2.up);
            }
        }
        else if (hitLeft.collider != null && hitLeft.collider.name != "player" && !hitLeft.collider.CompareTag("Zombie"))
        {
            float degrees = GetAngle(direction);

            if (degrees >= 0f && degrees <= 90f)
            {
                rb.AddForce(maxSpeed * turnSpeed * Time.fixedDeltaTime * Vector2.down);
            }
            else
            {
                rb.AddForce(maxSpeed * turnSpeed * Time.fixedDeltaTime * Vector2.up);
            }
        }
        else if (hitTop.collider != null && hitTop.collider.name != "Player" && !hitTop.collider.CompareTag("Zombie"))
        {
            float degrees = GetAngle(direction);

            if (degrees >= 0f && degrees <= -90f)
            {
                rb.AddForce(maxSpeed * turnSpeed * Time.fixedDeltaTime * Vector2.left);
            }
            else
            {
                rb.AddForce(maxSpeed * turnSpeed * Time.fixedDeltaTime * Vector2.right);
            }
        }
        else if (hitDown.collider != null && hitDown.collider.name != "Player" && !hitDown.collider.CompareTag("Zombie"))
        {
            float degrees = GetAngle(direction);

            if (degrees >= 90f && degrees <= 180f)
            {
                rb.AddForce(maxSpeed * turnSpeed * Time.fixedDeltaTime * Vector2.right);
            }
            else
            {
                rb.AddForce(maxSpeed * turnSpeed * Time.fixedDeltaTime * Vector2.left);
            }
        }
        else
        {
            rb.AddForce(maxSpeed * 7 * Time.fixedDeltaTime * direction);
        }

        FaceTowardsVelocity();
    }

    private float GetAngle(Vector2 direction)
    {
        return Vector2.SignedAngle(Vector2.down, direction);
    }


    private void Flip()
    {
        // Multiply the player's x local scale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
