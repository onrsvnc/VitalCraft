using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingRoadState : PlayerState
{
    BuildingManager buildingManager;
    string structureName;

    public PlayerBuildingRoadState(GameManager gameManager, BuildingManager buildingManager) : base(gameManager)
    {
        this.buildingManager = buildingManager;
    }

    public override void OnCancel()
    {
        this.buildingManager.CancelModification();
        this.gameManager.TransitionToState(this.gameManager.selectionState, null);
    }

    public override void OnBuildZone(string structureName)
    {
        this.buildingManager.CancelModification();
        base.OnBuildZone(structureName);
    }

    public override void OnBuildSingleStructure(string structureName)
    {
        this.buildingManager.CancelModification();
        base.OnBuildSingleStructure(structureName);
    }

    public override void OnConfirmAction()
    {
        this.buildingManager.ConfirmModification();
        base.OnConfirmAction();
    }

    public override void EnterState(string structureName)
    {
        this.buildingManager.PrepareBuildingManager(this.GetType());
        this.structureName = structureName;
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        this.buildingManager.PrepareStructureForModification(position,structureName, StructureType.Road);
    }

}
