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
        Debug.Log(panPosition);
        Debug.Log("mouse pressd");
        cameraMovement.MoveCamera(panPosition);
    }

    public override void OnInputPanUp()
    {
        Debug.Log("mouse unpressd");
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

    public override void OnBuildZone(string structureName)
    {
        this.gameManager.TransitionToState(this.gameManager.buildingZoneState, structureName);
    }

    public override void OnBuildSingleStructure(string structureName)
    {
        this.gameManager.TransitionToState(this.gameManager.buildingSingleStructureState, structureName);
    }
    
    public override void OnBuildRoad(string structureName)
    {
        this.gameManager.TransitionToState(this.gameManager.buildingRoadState, structureName);
    }

    public override void OnDemolishAction()
    {
        this.gameManager.TransitionToState(this.gameManager.demolishState, null);
    }
}
