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
        this.gameManager.TransitionToState(this.gameManager.selectionState, null);
    }

    public override void EnterState(string structureName)
    {
        this.structureName = structureName;
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        Debug.Log("Area built");
        this.buildingManager.PlaceStructureAt(position,structureName,StructureType.Zone);
    }

}
