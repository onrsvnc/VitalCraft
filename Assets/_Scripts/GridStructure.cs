using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridStructure
{
    private int cellSize;
    Cell[,] grid;
    private int width, length;

    public GridStructure(int cellSize, int width, int length)
    {
        this.cellSize = cellSize;
        this.width = width;
        this.length = length;
        grid = new Cell[this.width, this.length];
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int column = 0; column < grid.GetLength(1); column++)
            {
                grid[row, column] = new Cell();
            }
        }
    }


    public Vector3 CalculateGridPosition(Vector3 inputPosition)
    {
        int x = Mathf.FloorToInt((float)inputPosition.x / cellSize);
        int z = Mathf.FloorToInt((float)inputPosition.z / cellSize);
        return new Vector3(x * cellSize, 0, z * cellSize);

    }

    private Vector2Int CalculateGridIndex(Vector3 gridPosition)
    {
        return new Vector2Int((int)(gridPosition.x / cellSize), (int)(gridPosition.z / cellSize));
    }

    public bool IsCellTaken(Vector3 gridPosition)
    {
        var cellIndex = CalculateGridIndex(gridPosition);
        if (CheckIndexValidity(cellIndex))
            return grid[cellIndex.y, cellIndex.x].IsTaken;
        throw new IndexOutOfRangeException("No index " + cellIndex + " in grid");
    }

    public void PlaceStructureOnTheGrid(UnityEngine.GameObject structure, Vector3 gridPosition, StructureBaseSO structureData)
    {
        var cellIndex = CalculateGridIndex(gridPosition);
        if (CheckIndexValidity(cellIndex))
            grid[cellIndex.y, cellIndex.x].SetConstruction(structure, structureData);
    }

    private bool CheckIndexValidity(Vector2Int cellIndex)
    {
        if (cellIndex.x >= 0 && cellIndex.x < grid.GetLength(1) && cellIndex.y >= 0 && cellIndex.y < grid.GetLength(0))
            return true;
        return false;
    }

    public UnityEngine.GameObject GetStructureFromTheGrid(Vector3 gridPosition)
    {
        var cellIndex = CalculateGridIndex(gridPosition);
        return grid[cellIndex.y, cellIndex.x].GetStructure();
    }

    public StructureBaseSO GetStructureDataFromTheGrid(Vector3 gridPosition)
    {
        var cellIndex = CalculateGridIndex(gridPosition);
        return grid[cellIndex.y, cellIndex.x].GetStructureData();
    }

    public void RemoveStructureFromTheGrid(Vector3 gridPosition)
    {
        var cellIndex = CalculateGridIndex(gridPosition);
        grid[cellIndex.y, cellIndex.x].RemoveStructure();
    }

    public Vector3Int? GetPositionOfTheNeighbourIfExists(Vector3 gridPosition, Direction direction)
    {
        Vector3Int? neighbourPosition = Vector3Int.FloorToInt(gridPosition);
        switch (direction)
        {
            case Direction.Up:
                neighbourPosition += new Vector3Int(0, 0, cellSize);
                break;
            case Direction.Right:
                neighbourPosition += new Vector3Int(cellSize, 0, 0);
                break;
            case Direction.Down:
                neighbourPosition += new Vector3Int(0, 0, -cellSize);
                break;
            case Direction.Left:
                neighbourPosition += new Vector3Int(-cellSize, 0, 0);
                break;
        }
        var index = CalculateGridIndex(neighbourPosition.Value);
        if(CheckIndexValidity(index) == false)
        {
            return null;
        }
        return neighbourPosition;

    }
}

public enum Direction
{
    Up = 1,
    Right = 2,
    Down = 4,
    Left = 8
}