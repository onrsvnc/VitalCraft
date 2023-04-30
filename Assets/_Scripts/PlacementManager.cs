using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour, IPlacementManager
{
    public Transform ground;
    public Material transparentMaterial;
    private Dictionary<GameObject, Material[]> originalMaterials = new Dictionary<GameObject, Material[]>();
    private NatureManager natureManager;

    // CreateBuilding() can be placed in BuildingManager for now it is here.
    // Old Non-Ghost building system code.
    // public void CreateBuilding(Vector3 gridPosition, GridStructure grid, GameObject buildingPrefab)
    // {
    //     UnityEngine.GameObject newStructure = Instantiate(buildingPrefab, ground.position + gridPosition, Quaternion.identity);
    //     grid.PlaceStructureOnTheGrid(newStructure, gridPosition);
    // }

    public GameObject CreateGhostStructure(Vector3 gridPosition, GameObject buildingPrefab, RotationValue rotationValue = RotationValue.R0)
    {
        GameObject newStructure = PlaceStructureOnTheMap(gridPosition, buildingPrefab, rotationValue);
        Color colorToSet = Color.green;
        ModifyStructurePrefabLook(newStructure, colorToSet);
        return newStructure;
    }

    public GameObject PlaceStructureOnTheMap(Vector3 gridPosition, GameObject buildingPrefab, RotationValue rotationValue)
    {
        GameObject newStructure = Instantiate(buildingPrefab, ground.position + gridPosition, Quaternion.identity);
        Vector3 rotation = Vector3.zero;
        switch (rotationValue)
        {
            case RotationValue.R0:
                break;
            case RotationValue.R90:
                rotation = new Vector3(0, 90, 0);
                break;
            case RotationValue.R180:
                rotation = new Vector3(0, 180, 0);
                break;
            case RotationValue.R270:
                rotation = new Vector3(0, 270, 0);
                break;
        }
        foreach (Transform child in newStructure.transform)
        {
            child.Rotate(rotation, Space.World);
        }
        return newStructure;
    }

    public void ModifyStructurePrefabLook(GameObject newStructure, Color colorToSet) //exposed to use from structure info to see the range
    {
        foreach (Transform child in newStructure.transform)
        {
            var renderer = child.GetComponent<MeshRenderer>();
            if (!originalMaterials.ContainsKey(child.gameObject))
            {
                originalMaterials.Add(child.gameObject, renderer.materials);
            }
            Material[] materialsToSet = new Material[renderer.materials.Length];
            colorToSet.a = 0.5f;
            for (int i = 0; i < materialsToSet.Length; i++)
            {
                materialsToSet[i] = transparentMaterial;
                materialsToSet[i].color = colorToSet;
            }
            renderer.materials = materialsToSet;
        }
    }

    public void PlaceStructuresOnTheMap(IEnumerable<GameObject> structureCollection)
    {
        foreach (var structure in structureCollection)
        {
            natureManager.DestroyNatureAtLocation(structure.transform.position);
            ResetBuildingLook(structure);
        }
        originalMaterials.Clear();
    }

    public void ResetBuildingLook(GameObject structure)
    {
        foreach (Transform child in structure.transform)
        {
            var renderer = child.GetComponent<MeshRenderer>();
            if (originalMaterials.ContainsKey(child.gameObject))
            {
                renderer.materials = originalMaterials[child.gameObject];
            }
        }
    }

    public void DestroyStructures(IEnumerable<GameObject> structureCollection)
    {
        foreach (var structure in structureCollection)
        {
            DestroySingleStructure(structure);
        }
        originalMaterials.Clear();
    }

    public void DestroySingleStructure(GameObject structure)
    {
        Destroy(structure);
    }

    // Old Code
    // public void RemoveBuilding(Vector3 gridPosition, GridStructure grid)
    // {
    //     var structure = grid.GetStructureFromTheGrid(gridPosition);
    //     if(structure!=null)
    //     {
    //         Destroy(structure); //For now structure is simply destroyed.
    //         grid.RemoveStructureFromTheGrid(gridPosition);
    //     }
    // }

    public void SetBuildingForDemolition(GameObject structureToDemolish)
    {
        Color colorToSet = Color.red;
        ModifyStructurePrefabLook(structureToDemolish, colorToSet);
    }

    public GameObject MoveStructureOnTheMap(Vector3Int positionToPlaceStructure, GameObject gameObjectToReuse, GameObject prefab)
    {
        gameObjectToReuse.transform.position = positionToPlaceStructure;
        gameObjectToReuse.transform.rotation = prefab.transform.rotation;
        for (int i = 0; i < gameObjectToReuse.transform.childCount; i++)
        {
            gameObjectToReuse.transform.GetChild(i).rotation = prefab.transform.GetChild(i).rotation;
        }
        return gameObjectToReuse;
    }

    public void PreparePlacementManager(NatureManager natureManager)
    {
        this.natureManager = natureManager;
    }
}
