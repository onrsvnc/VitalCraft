using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected BuildingManager buildingManager;
    protected GameManager gameManager;
    protected CameraMovement cameraMovement;

    public PlayerState(GameManager gameManager, BuildingManager buildingManager)
    {
        this.gameManager = gameManager;
        this.buildingManager = buildingManager;
        cameraMovement = gameManager.cameraMovement;
    }

    public virtual void OnConfirmAction()
    {
        this.gameManager.TransitionToState(this.gameManager.selectionState, null);
    }

    public virtual void OnInputPointerDown(Vector3 position)
    {

    }
    public virtual void OnInputPointerChange(Vector3 position)
    {

    }
    public virtual void OnInputPointerUp()
    {

    }

    public virtual void OnInputPanChange(Vector3 panPosition)
    {
        cameraMovement.MoveCamera(panPosition);
    }

    public virtual void OnInputPanUp()
    {
        cameraMovement.StopCameraMovement();
    }
    
    public virtual void EnterState(string variable)
    {
        
    }

    public abstract void OnCancel();

    public virtual void OnBuildZone(string structureName)
    {
        this.buildingManager.CancelModification();
        this.gameManager.TransitionToState(this.gameManager.buildingZoneState, structureName);
    }
    public virtual void OnBuildSingleStructure(string structureName)
    {
        this.buildingManager.CancelModification();
        this.gameManager.TransitionToState(this.gameManager.buildingSingleStructureState, structureName);
    }
    public virtual void OnBuildRoad(string structureName)
    {
        this.buildingManager.CancelModification();
        this.gameManager.TransitionToState(this.gameManager.buildingRoadState, structureName);
    }
    public virtual void OnDemolishAction()
    {
        this.buildingManager.CancelModification();
        this.gameManager.TransitionToState(this.gameManager.demolishState, null);
    }

}
