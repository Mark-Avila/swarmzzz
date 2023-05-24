using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the player's transform
    public Vector3 offset; // Offset to adjust the camera position relative to the player

    private void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the desired position of the camera
            Vector3 desiredPosition = target.position + offset;

            // Smoothly move the camera towards the desired position
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime);
        }
    }
}
