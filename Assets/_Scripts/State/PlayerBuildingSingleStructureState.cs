using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingSingleStructureState : PlayerState
{
    BuildingManager buildingManager;
    string structureName;

    public PlayerBuildingSingleStructureState(GameManager gameManager, BuildingManager buildingManager) : base(gameManager)
    {
        this.buildingManager = buildingManager;
    }

    public override void OnInputPanChange(Vector3 position)
    {
        return;
    }

    public override void OnInputPanUp()
    {
        return;
    }

    public override void OnInputPointerChange(Vector3 position)
    {
        return;
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        Debug.Log("Facility Built");
        this.buildingManager.PlaceStructureAt(position, structureName, StructureType.SingleStructure);
    }

    public override void OnInputPointerUp()
    {
        return;
    }

    public override void OnCancel()
    {
        this.gameManager.TransitionToState(this.gameManager.selectionState, null);
    }

    public override void EnterState(string structureName)
    {
        this.structureName = structureName;
    }

}
