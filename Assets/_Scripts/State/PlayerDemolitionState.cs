using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDemolitionState : PlayerState
{
    private Action OnDemolitionConfirmHandler;

    public PlayerDemolitionState(GameManager gameManager, BuildingManager buildingManager) : base(gameManager, buildingManager)
    {
        
    }

    public override void OnCancel()
    {
        RemoveListenerOnDemolitionConfirmEvent(AudioManager.Instance.PlayDemolitionSound);
        this.buildingManager.CancelModification();
        this.gameManager.TransitionToState(this.gameManager.selectionState, null);
    }

    public override void OnConfirmAction()
    {
        OnDemolitionConfirmHandler?.Invoke();
        RemoveListenerOnDemolitionConfirmEvent(AudioManager.Instance.PlayDemolitionSound);
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
        AddListenerOnDemolitionConfirmEvent(AudioManager.Instance.PlayDemolitionSound);
        this.buildingManager.PrepareBuildingManager(this.GetType());
    }

    public override void OnBuildRoad(string structureName)
    {
        RemoveListenerOnDemolitionConfirmEvent(AudioManager.Instance.PlayDemolitionSound);
        base.OnBuildRoad(structureName);
    }

    public override void OnBuildSingleStructure(string structureName)
    {
        RemoveListenerOnDemolitionConfirmEvent(AudioManager.Instance.PlayDemolitionSound);
        base.OnBuildSingleStructure(structureName);
    }

    public override void OnBuildZone(string structureName)
    {
        RemoveListenerOnDemolitionConfirmEvent(AudioManager.Instance.PlayDemolitionSound);
        base.OnBuildZone(structureName);
    }

    public override void OnDemolishAction()
    {
        RemoveListenerOnDemolitionConfirmEvent(AudioManager.Instance.PlayDemolitionSound);
        base.OnDemolishAction();
    }



    public void AddListenerOnDemolitionConfirmEvent(Action listener)
    {
        OnDemolitionConfirmHandler += listener;
    }
    public void RemoveListenerOnDemolitionConfirmEvent(Action listener)
    {
        OnDemolitionConfirmHandler -= listener;
    }

    
}
