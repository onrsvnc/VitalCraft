using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingZoneState : PlayerState
{
    string structureName;
    private Action OnBuildingZoneConfirmHandler;

    public PlayerBuildingZoneState(GameManager gameManager, BuildingManager buildingManager) : base(gameManager, buildingManager)
    {
        
    }

    public override void OnCancel()
    {
        RemoveListenerOnBuildingZoneConfirmEvent(AudioManager.Instance.PlayPlaceBuildSound);
        this.buildingManager.CancelModification();
        this.gameManager.TransitionToState(this.gameManager.selectionState, null);
    }

    public override void OnConfirmAction()
    {
        OnBuildingZoneConfirmHandler?.Invoke();
        RemoveListenerOnBuildingZoneConfirmEvent(AudioManager.Instance.PlayPlaceBuildSound);
        this.buildingManager.ConfirmModification();
        base.OnConfirmAction();
    }

    public override void EnterState(string structureName)
    {
        AddListenerOnBuildingZoneConfirmEvent(AudioManager.Instance.PlayPlaceBuildSound);
        this.buildingManager.PrepareBuildingManager(this.GetType());
        this.structureName = structureName;
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        this.buildingManager.PrepareStructureForModification(position, structureName, StructureType.Zone);
    }

    public override void OnInputPointerChange(Vector3 position)
    {
        this.buildingManager.PrepareStructureForModification(position, structureName, StructureType.Zone);
    }

    public override void OnInputPointerUp()
    {
        this.buildingManager.StopContinuousPlacement();
    }

    public override void OnBuildRoad(string structureName)
    {
        RemoveListenerOnBuildingZoneConfirmEvent(AudioManager.Instance.PlayPlaceBuildSound);
        base.OnBuildRoad(structureName);
    }

    public override void OnBuildSingleStructure(string structureName)
    {
        RemoveListenerOnBuildingZoneConfirmEvent(AudioManager.Instance.PlayPlaceBuildSound);
        base.OnBuildSingleStructure(structureName);
    }

    public override void OnBuildZone(string structureName)
    {
        RemoveListenerOnBuildingZoneConfirmEvent(AudioManager.Instance.PlayPlaceBuildSound);
        base.OnBuildZone(structureName);
    }

    public override void OnDemolishAction()
    {
        RemoveListenerOnBuildingZoneConfirmEvent(AudioManager.Instance.PlayPlaceBuildSound);
        base.OnDemolishAction();
    }


    public void AddListenerOnBuildingZoneConfirmEvent(Action listener)
    {
        OnBuildingZoneConfirmHandler += listener;
    }
    public void RemoveListenerOnBuildingZoneConfirmEvent(Action listener)
    {
        OnBuildingZoneConfirmHandler -= listener;
    }

}
