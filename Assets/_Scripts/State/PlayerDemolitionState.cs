using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDemolitionState : PlayerState
{
    BuildingManager buildingManager;

    public PlayerDemolitionState(GameManager gameManager, BuildingManager buildingManager):base (gameManager)
    {
        this.buildingManager = buildingManager; 
    }

    public override void OnCancel()
    {
        this.buildingManager.CancelDemolition();
        this.gameManager.TransitionToState(this.gameManager.selectionState, null);
    }

    public override void OnConfirmAction()
    {
        this.buildingManager.ConfirmDemolition();
        base.OnConfirmAction();
    }

    public override void OnBuildRoad(string structureName)
    {
        this.buildingManager.CancelDemolition();
        base.OnBuildRoad(structureName);
    }

    public override void OnBuildZone(string structureName)
    {
        this.buildingManager.CancelDemolition();
        base.OnBuildZone(structureName);
    }

    public override void OnBuildSingleStructure(string structureName)
    {
        this.buildingManager.CancelDemolition();
        base.OnBuildSingleStructure(structureName);
    }

    public override void OnInputPointerChange(Vector3 position)
    {
        return;
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        this.buildingManager.PrepareStructureForDemolitionAt(position);
    }

    public override void OnInputPointerUp()
    {
        return;
    } 
}
