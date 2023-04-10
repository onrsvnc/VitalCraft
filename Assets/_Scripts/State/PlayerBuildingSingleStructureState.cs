using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingSingleStructureState : PlayerState
{
    string structureName;
    private Action OnBuildingSingleStructureConfirmHandler;

    public PlayerBuildingSingleStructureState(GameManager gameManager, BuildingManager buildingManager) : base(gameManager, buildingManager)
    {
        
    }

    public override void OnConfirmAction()
    {
        OnBuildingSingleStructureConfirmHandler?.Invoke();
        RemoveListenerOnBuildingSingleStructureConfirmEvent(AudioManager.Instance.PlayPlaceBuildSound);
        this.buildingManager.ConfirmModification();
        base.OnConfirmAction();
    }

    public override void OnInputPointerChange(Vector3 position)
    {
        return;
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        this.buildingManager.PrepareStructureForModification(position, structureName, StructureType.SingleStructure);
    }

    public override void OnInputPointerUp()
    {
        return;
    }

    public override void OnCancel()
    {
        RemoveListenerOnBuildingSingleStructureConfirmEvent(AudioManager.Instance.PlayPlaceBuildSound);
        this.buildingManager.CancelModification();
        this.gameManager.TransitionToState(this.gameManager.selectionState, null);
    }



    public override void EnterState(string structureName)
    {
        AddListenerOnBuildingSingleStructureConfirmEvent(AudioManager.Instance.PlayPlaceBuildSound);
        this.buildingManager.PrepareBuildingManager(this.GetType());
        this.structureName = structureName;
    }

    public override void OnBuildRoad(string structureName)
    {
        RemoveListenerOnBuildingSingleStructureConfirmEvent(AudioManager.Instance.PlayPlaceBuildSound);
        base.OnBuildRoad(structureName);
    }

    public override void OnBuildSingleStructure(string structureName)
    {
        RemoveListenerOnBuildingSingleStructureConfirmEvent(AudioManager.Instance.PlayPlaceBuildSound);
        base.OnBuildSingleStructure(structureName);
    }

    public override void OnBuildZone(string structureName)
    {
        RemoveListenerOnBuildingSingleStructureConfirmEvent(AudioManager.Instance.PlayPlaceBuildSound);
        base.OnBuildZone(structureName);
    }

    public override void OnDemolishAction()
    {
        RemoveListenerOnBuildingSingleStructureConfirmEvent(AudioManager.Instance.PlayPlaceBuildSound);
        base.OnDemolishAction();
    }

    

    private void AddListenerOnBuildingSingleStructureConfirmEvent(Action listener)
    {
        OnBuildingSingleStructureConfirmHandler += listener;
    }

    private void RemoveListenerOnBuildingSingleStructureConfirmEvent(Action listener)
    {
        OnBuildingSingleStructureConfirmHandler -= listener;
    }

}
