using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureManager : MonoBehaviour
{
    public GameObject tree;
    public Transform natureParent;
    int width, length;
    GridStructure grid;
    public int radius = 5;

    public GridStructure Grid { get => grid; }

    public void PrepareNature(int cellSize, int width, int length)
    {
        this.grid = new GridStructure(cellSize, width, length);
        this.width = width;
        this.length = length;
        PrepareTrees();
    }

    private void PrepareTrees()
    {
        TreeGenerator generator = new TreeGenerator(width, length, radius);
        foreach (Vector2 samplePosition in generator.Samples())
        {
            PlaceObjectOnTheMap(samplePosition, tree);
        }
    }

    private void PlaceObjectOnTheMap(Vector2 samplePosition, GameObject objectToCreate)
    {
        var positionInt = Vector2Int.CeilToInt(samplePosition);
        var positionGrid = grid.CalculateGridPosition(new Vector3(positionInt.x, 0, positionInt.y));
        var natureElement = Instantiate(objectToCreate, positionGrid, Quaternion.identity, natureParent);
        grid.AddNatureToCell(positionGrid, natureElement);
    }

    public void DestroyNatureAtLocation(Vector3 position)
    {
        var elementsToDestroy = grid.GetNatureObjectsToRemove(position);
        foreach (var element in elementsToDestroy)
        {
            Destroy(element);
        }
    }
}
