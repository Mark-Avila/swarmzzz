using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public Animator animator;
    public float speed = 5f;
    Vector2 movement;

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        FaceTowardsMouse();
    }

    private void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + movement * speed * Time.fixedDeltaTime);

        Debug.Log("y: " + movement.y + "   " + "x: " + movement.x);

        if (movement.x == 0 && movement.y == 0)
        {
            animator.SetBool("isMoving", false);
        }
        else
        {
            animator.SetBool("isMoving", true);
        }
    } 

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    private void FaceTowardsMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 direction = worldMousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
