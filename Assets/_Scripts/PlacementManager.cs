using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public UnityEngine.GameObject buildingPrefab;
    public Transform ground;

    //CreateBuilding() can be placed in BuildingManager for now it is here.
    public void CreateBuilding(Vector3 gridPosition, GridStructure grid)
    {
        UnityEngine.GameObject newStructure = Instantiate(buildingPrefab, ground.position + gridPosition, Quaternion.identity);
        grid.PlaceStructureOnTheGrid(newStructure, gridPosition);
    }

    public void RemoveBuilding(Vector3 gridPosition, GridStructure grid)
    {
        var structure = grid.GetStructureFromTheGrid(gridPosition);
        if(structure!=null)
        {
            Destroy(structure); //For now structure is simply destroyed.
            grid.RemoveStructureFromTheGrid(gridPosition);
        }
    }
}
