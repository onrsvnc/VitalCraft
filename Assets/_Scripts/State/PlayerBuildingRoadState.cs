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
        this.gameManager.TransitionToState(this.gameManager.selectionState, null);
    }

    public override void EnterState(string structureName)
    {
        this.structureName = structureName;
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        Debug.Log("Road Built.");
        this.buildingManager.PlaceStructureAt(position,structureName, StructureType.Road);
    }

}
