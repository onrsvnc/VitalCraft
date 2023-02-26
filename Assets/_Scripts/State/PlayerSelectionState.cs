using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionState : PlayerState
{
    CameraMovement cameraMovement;

    public PlayerSelectionState(GameManager gameManager, CameraMovement cameraMovement) : base(gameManager)
    {
        this.cameraMovement = cameraMovement;
    }

    public override void OnInputPanChange(Vector3 panPosition)
    {
        cameraMovement.MoveCamera(panPosition);
    }

    public override void OnInputPanUp()
    {
        cameraMovement.StopCameraMovement();
    }

    public override void OnInputPointerChange(Vector3 position)
    {
       // Debug.Log(position); // GroundPos at y=-1 with height=1 causes y-coordinate issue: -0.5
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        return;
    }

    public override void OnInputPointerUp()
    {
        return;
    }

    public override void OnCancel()
    {
        return;
    }
}
