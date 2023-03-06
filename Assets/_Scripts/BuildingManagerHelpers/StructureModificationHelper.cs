using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StructureModificationHelper 
{
    protected Dictionary<Vector3Int, GameObject> structuresToBeModified = new Dictionary<Vector3Int, GameObject>();
    protected readonly StructureRepository structureRepository;
    protected readonly GridStructure grid;
    protected readonly PlacementManager placementManager;

    public StructureModificationHelper(StructureRepository structureRepository, GridStructure grid, PlacementManager placementManager)
    {
        this.structureRepository = structureRepository;
        this.grid = grid;
        this.placementManager = placementManager;
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

    public abstract void PrepareStructureForModification(Vector3 inputPosition, string structureName, StructureType structureType);
    public abstract void ConfirmModification();
    public abstract void CancelModification();


}
