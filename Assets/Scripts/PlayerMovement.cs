using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public Animator animator;
    public float speed = 5f;
    Vector2 movement;
    Vector2 previousPosition;

    private Vector2 moveInput;

    // Update is called once per frame
    private void Start()
    {
        previousPosition = rigidBody.position;
    }

    private void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + moveInput * speed * Time.fixedDeltaTime);

        if (rigidBody.position == previousPosition)
        {
            animator.SetBool("isMoving", false);
        }
        else
        {
            animator.SetBool("isMoving", true);
        }

        previousPosition = rigidBody.position;

        FaceTowardsMouse();
    }

    private void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    private void FaceTowardsMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = worldMousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

}
