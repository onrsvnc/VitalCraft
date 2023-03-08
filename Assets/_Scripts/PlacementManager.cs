using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlacementManager
{
    GameObject CreateGhostStructure(Vector3 gridPosition, GameObject buildingPrefab);
    void DestroySingleStructure(GameObject structure);
    void DestroyStructures(IEnumerable<GameObject> structureCollection);
    void PlaceStructuresOnTheMap(IEnumerable<GameObject> structureCollection);
    void ResetBuildingLook(GameObject structure);
    void SetBuildingForDemolition(GameObject structureToDemolish);
}

public class PlacementManager : MonoBehaviour, IPlacementManager
{
    public Transform ground;
    public Material transparentMaterial;
    private Dictionary<GameObject, Material[]> originalMaterials = new Dictionary<GameObject, Material[]>();

    // CreateBuilding() can be placed in BuildingManager for now it is here.
    // Old Non-Ghost building system code.
    // public void CreateBuilding(Vector3 gridPosition, GridStructure grid, GameObject buildingPrefab)
    // {
    //     UnityEngine.GameObject newStructure = Instantiate(buildingPrefab, ground.position + gridPosition, Quaternion.identity);
    //     grid.PlaceStructureOnTheGrid(newStructure, gridPosition);
    // }

    public GameObject CreateGhostStructure(Vector3 gridPosition, GameObject buildingPrefab)
    {
        GameObject newStructure = Instantiate(buildingPrefab, ground.position + gridPosition, Quaternion.identity);
        Color colorToSet = Color.green;
        ModifyStructurePrefabLook(newStructure, colorToSet);
        return newStructure;
    }

    private void ModifyStructurePrefabLook(GameObject newStructure, Color colorToSet)
    {
        foreach (Transform child in newStructure.transform)
        {
            var renderer = child.GetComponent<MeshRenderer>();
            if (!originalMaterials.ContainsKey(child.gameObject))
            {
                originalMaterials.Add(child.gameObject, renderer.materials);
            }
            Material[] materialsToSet = new Material[renderer.materials.Length];
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
}
