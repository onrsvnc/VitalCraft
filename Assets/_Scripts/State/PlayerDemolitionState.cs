using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDemolitionState : PlayerState
{

    public PlayerDemolitionState(GameManager gameManager, BuildingManager buildingManager):base (gameManager, buildingManager)
    {
        
    }

    public override void OnCancel() 
    {
        this.buildingManager.CancelModification();
        this.gameManager.TransitionToState(this.gameManager.selectionState, null);
    }

    public override void OnConfirmAction()
    {
        this.buildingManager.ConfirmModification();
        base.OnConfirmAction();
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

    public override void EnterState(string variable)
    {
        this.buildingManager.PrepareBuildingManager(this.GetType());
    }
}
