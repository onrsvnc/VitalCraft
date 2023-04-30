using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingRoadState : PlayerState
{
    string structureName;
    private Action OnBuildingRoadConfirmHandler;

    public PlayerBuildingRoadState(GameManager gameManager, BuildingManager buildingManager) : base(gameManager, buildingManager)
    {
       
    }

    public override void OnCancel()
    {
        RemoveListenerOnBuildingRoadConfirmEvent(AudioManager.Instance.PlayPlaceBuildSound);
        this.buildingManager.CancelModification();
        this.gameManager.TransitionToState(this.gameManager.selectionState, null);
    }

    public override void OnConfirmAction()
    {
        OnBuildingRoadConfirmHandler?.Invoke();
        RemoveListenerOnBuildingRoadConfirmEvent(AudioManager.Instance.PlayPlaceBuildSound);
        this.buildingManager.ConfirmModification();
        base.OnConfirmAction();
    }

    public override void EnterState(string structureName)
    {
        AddListenerOnBuildingRoadConfirmEvent(AudioManager.Instance.PlayPlaceBuildSound);
        this.buildingManager.PrepareBuildingManager(this.GetType());
        this.structureName = structureName;
    }

    public override void OnInputPointerChange(Vector3 position)
    {
        this.buildingManager.PrepareStructureForModification(position, structureName, StructureType.Road);
    }

    public override void OnBuildRoad(string structureName)
    {
        RemoveListenerOnBuildingRoadConfirmEvent(AudioManager.Instance.PlayPlaceBuildSound);
        base.OnBuildRoad(structureName);
    }

    public override void OnBuildSingleStructure(string structureName)
    {
        RemoveListenerOnBuildingRoadConfirmEvent(AudioManager.Instance.PlayPlaceBuildSound);
        base.OnBuildSingleStructure(structureName);
    }

    public override void OnBuildZone(string structureName)
    {
        RemoveListenerOnBuildingRoadConfirmEvent(AudioManager.Instance.PlayPlaceBuildSound);
        base.OnBuildZone(structureName);
    }

    public override void OnDemolishAction()
    {
        RemoveListenerOnBuildingRoadConfirmEvent(AudioManager.Instance.PlayPlaceBuildSound);
        base.OnDemolishAction();
    }


    private void AddListenerOnBuildingRoadConfirmEvent(Action listener)
    {
        OnBuildingRoadConfirmHandler += listener;
    }

    private void RemoveListenerOnBuildingRoadConfirmEvent(Action listener)
    {
        OnBuildingRoadConfirmHandler -= listener;
    }

}
