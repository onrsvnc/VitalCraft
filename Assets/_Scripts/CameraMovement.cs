using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Vector3? basePointerPosition = null;
    public float cameraMovementSpeed = 0.05f;
    private int cameraXMin, cameraXMax, cameraZMin, cameraZMax;

    void Start()
    {

    }

    void Update()
    {

    }

    public void MoveCamera(Vector3 pointerPosition)
    {
        if (!basePointerPosition.HasValue)
        {
            basePointerPosition = pointerPosition;
        }

        Vector3 newPosition = pointerPosition - basePointerPosition.Value;
        newPosition = new Vector3(-newPosition.x, 0, -newPosition.y*1.5f); // Z*1.5f for isometric camera's unequal axis movement.
        transform.Translate(newPosition * cameraMovementSpeed);
        LimitPositionInsideBounds();
    }

    private void LimitPositionInsideBounds()
    {
        transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x, cameraXMin, cameraXMax),
                    0,
                    Mathf.Clamp(transform.position.z, cameraZMin, cameraZMax)
                    );
    }

    public void StopCameraMovement()
    {
        basePointerPosition = null;
    }

    public void SetCameraLimits(int cameraXMin, int cameraXMax, int cameraZMin, int cameraZMax)
    {
        this.cameraXMax = cameraXMax;
        this.cameraXMin = cameraXMin;
        this.cameraZMax = cameraZMax;
        this.cameraZMin = cameraZMin;
    }


}
