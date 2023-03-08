using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using UnityEngine;

public static class TestHelpers
{
    public static StructureRepository CreateStructureRepositoryContainingRoad()
    {
        StructureRepository structureRepository = Substitute.For<StructureRepository>();
        CollectionSO collection = new CollectionSO();
        RoadStructureSO road = new RoadStructureSO();
        road.buildingName = "Road";
        road.prefab = GetAGameObjectWithMaterial();
        collection.roadStructure = road;
        structureRepository.modelDataCollection = collection;
        return structureRepository;
    }

    public static GameObject GetAGameObjectWithMaterial()
    {
        GameObject roadChild = new GameObject("Road", typeof(MeshRenderer));
        var renderer = roadChild.GetComponent<MeshRenderer>();
        var materialBlue = Resources.Load("BlueMaterial", typeof(Material)) as Material; //Load blue material from resources, easiest way due to edit modes strict material creation rules.
        renderer.material = materialBlue;
        GameObject roadPrefab = new GameObject("Road");
        roadChild.transform.SetParent(roadPrefab.transform);
        return roadPrefab;
    }
}

