using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingZoneState : PlayerState
{

    BuildingManager buildingManager;
    string structureName;

    public PlayerBuildingZoneState(GameManager gameManager, BuildingManager buildingManager) : base(gameManager)
    {
        this.buildingManager = buildingManager;

    }

    public override void OnCancel()
    {
        this.buildingManager.CancelPlacement();
        this.gameManager.TransitionToState(this.gameManager.selectionState, null);
    }

    public override void OnBuildRoad(string structureName)
    {
        this.buildingManager.CancelPlacement();
        base.OnBuildRoad(structureName);
    }
    public override void OnBuildSingleStructure(string structureName)
    {
        this.buildingManager.CancelPlacement();
        base.OnBuildSingleStructure(structureName);
    }

    public override void OnConfirmAction()
    {
        this.buildingManager.ConfirmPlacement();
        base.OnConfirmAction();
    }

    public override void EnterState(string structureName)
    {
        this.structureName = structureName;
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        this.buildingManager.PrepareStructureForPlacement(position,structureName,StructureType.Zone);
    }

}
