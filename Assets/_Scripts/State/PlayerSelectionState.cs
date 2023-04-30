using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionState : PlayerState
{
    Vector3? previousPosition;
    GameObject? previousModel;

    private Dictionary<GameObject, Material[]> originalMaterials = new Dictionary<GameObject, Material[]>();

    public PlayerSelectionState(GameManager gameManager, BuildingManager buildingManager) : base(gameManager, buildingManager)
    {

    }

    private void UpdateStructureInfoPanel(StructureBaseSO data)
    {
        Type dataType = data.GetType();
        if (dataType == typeof(SingleFacilitySO))
        {
            this.gameManager.uiController.DisplayFacilityStructureInfo((SingleFacilitySO)data);
        }
        else if (dataType == typeof(ZoneStructureSO))
        {
            this.gameManager.uiController.DisplayZoneStructureInfo((ZoneStructureSO)data);
        }
        else
        {
            this.gameManager.uiController.DisplayBasicStructureInfo((StructureBaseSO)data);
        }
    }

    private void DeactivateRangeIndicator()
    {
        foreach (Transform child in previousModel.transform)
        {
            MeshRenderer structureMesh = child.GetComponent<MeshRenderer>();
            Color color = structureMesh.material.color;
            color.a = 0f;
            structureMesh.material.color = color;
        }
    }

    private void ActivateRangeIndicator(GameObject structureModel)
    {
        foreach (Transform child in structureModel.transform)
        {
            MeshRenderer structureMesh = child.GetComponent<MeshRenderer>();
            Color color = structureMesh.material.color;
            color.a = 1f;
            structureMesh.material.color = color;
        }
        previousModel = structureModel;
    }

    public override void OnInputPointerChange(Vector3 position)
    {
        // Debug.Log(position); // GroundPos at y=-1 with height=1 causes y-coordinate issue: -0.5
    }


    public override void OnInputPointerDown(Vector3 position)
    {
        StructureBaseSO data = buildingManager.GetStructureDataFromPosition(position);
        UnityEngine.GameObject structureModel = buildingManager.CheckForStructureInGrid(position);
        if (data)
        {
            if (data.GetType() == typeof(SingleFacilitySO))
            {
                ActivateRangeIndicator(structureModel);
            }
            else if(previousModel)
            {
                DeactivateRangeIndicator();
            }
            UpdateStructureInfoPanel(data);
            previousPosition = position;
        }
        else
        {
            if (previousModel)
            {
                DeactivateRangeIndicator();
            }
            this.gameManager.uiController.HideStructureInfo();
            data = null;
            previousPosition = null;
        }
        return;
    }

    

    public override void OnInputPointerUp()
    {
        return;
    }

    public override void OnCancel()
    {
        return;
    }

    public override void OnBuildZone(string structureName)
    {
        this.gameManager.TransitionToState(this.gameManager.buildingZoneState, structureName);
    }

    public override void OnBuildSingleStructure(string structureName)
    {
        this.gameManager.TransitionToState(this.gameManager.buildingSingleStructureState, structureName);
    }

    public override void OnBuildRoad(string structureName)
    {
        this.gameManager.TransitionToState(this.gameManager.buildingRoadState, structureName);
    }

    public override void OnDemolishAction()
    {
        this.gameManager.TransitionToState(this.gameManager.demolishState, null);
    }

    public override void EnterState(string variable)
    {
        base.EnterState(variable);
        if (this.gameManager.uiController.GetStructureInfoVisibility())
        {
            StructureBaseSO data = buildingManager.GetStructureDataFromPosition(previousPosition.Value);
            if (data)
            {
                UpdateStructureInfoPanel(data);
            }
            else
            {
                this.gameManager.uiController.HideStructureInfo();
                previousPosition = null;
            }

        }
    }
}
