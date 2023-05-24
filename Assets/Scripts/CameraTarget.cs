using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public new Camera camera;
    public Transform target;
    public float yThreshold;
    public float xThreshold;

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPosition = (target.position + mousePosition) / 2f;

        targetPosition.x = Mathf.Clamp(targetPosition.x, -xThreshold + target.position.x, xThreshold + target.position.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -yThreshold + target.position.y, yThreshold + target.position.y);

        this.transform.position = targetPosition;
    }
}
