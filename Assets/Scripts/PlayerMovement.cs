using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //public Rigidbody2D rigidBody;
    public Transform player;
    public Animator animator;
    public float speed = 5f;
    public float smoothing = 0.1f;
    
    private Vector2 previousPosition;
    private Vector2 moveInput;
    private Rigidbody2D playerRb;
    private Vector2 targetVelocity;

    private bool canMove = true;

    // Update is called once per frame
    private void Start()
    {
        previousPosition = player.position;
        playerRb = player.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            //playerRb.MovePosition(playerRb.position + moveInput * speed * Time.fixedDeltaTime);
            targetVelocity = Vector2.Lerp(targetVelocity, moveInput * speed, smoothing);
            playerRb.velocity = targetVelocity;
            animator.SetBool("isMoving", playerRb.velocity.magnitude > 0.1f);
            
            if (animator.GetBool("isDead"))
            {
                canMove = false;
                playerRb.isKinematic = true;
                playerRb.bodyType = RigidbodyType2D.Static;
            }
            else
            {
                previousPosition = playerRb.position;
                FaceTowardsMouse();
            }

        }
    }

    private void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
    }

    private void FaceTowardsMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = worldMousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        player.rotation = Quaternion.Euler(0f, 0f, angle);
    }

}
