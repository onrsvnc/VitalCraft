using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StructureModificationHelper
{
    protected Dictionary<Vector3Int, GameObject> structuresToBeModified = new Dictionary<Vector3Int, GameObject>();
    protected readonly StructureRepository structureRepository;
    protected StructureBaseSO structureData;
    protected readonly GridStructure grid;
    protected readonly IPlacementManager placementManager;
    protected IResourceManager resourceManager;

    public StructureModificationHelper(StructureRepository structureRepository, GridStructure grid, IPlacementManager placementManager, IResourceManager resourceManager)
    {
        this.structureRepository = structureRepository;
        this.grid = grid;
        this.placementManager = placementManager;
        this.resourceManager = resourceManager;
    }

    //This method is created for unit testing purposes.
    public GameObject AccessStructureInDictionary(Vector3 gridPosition)
    {
        var gridPositionInt = Vector3Int.FloorToInt(gridPosition);
        if (structuresToBeModified.ContainsKey(gridPositionInt))
        {
            return structuresToBeModified[gridPositionInt];
        }
        return null;
    }

    public virtual void PrepareStructureForModification(Vector3 inputPosition, string structureName, StructureType structureType)
    {
        if (structureData == null && structureType != StructureType.None)
        {
            structureData = this.structureRepository.GetStructureData(structureName, structureType);
        }
    }

    public virtual void ConfirmModification()
    {
        placementManager.PlaceStructuresOnTheMap(structuresToBeModified.Values);
        foreach (var keyValuePair in structuresToBeModified)
        {
            grid.PlaceStructureOnTheGrid(keyValuePair.Value, keyValuePair.Key, GameObject.Instantiate(structureData));
        }
        ResetHelpersData();
    }

    public virtual void CancelModification()
    {
        placementManager.DestroyStructures(structuresToBeModified.Values);
        ResetHelpersData();
    }

    private void ResetHelpersData()
    {
        structuresToBeModified.Clear();
        structureData = null;
    }

    public virtual void StopContinuousPlacement()
    {

    }



}
