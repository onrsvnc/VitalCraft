using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NSubstitute;

public class StructureRepositoryEditModeTest
{
    StructureRepository structureRepository;
    GameObject testRoad;
    GameObject testSingleStructure;
    GameObject testZone;

    [OneTimeSetUp]
    public void Init()
    {
        structureRepository = Substitute.For<StructureRepository>();
        CollectionSO collection = new CollectionSO();
        testRoad = new GameObject();
        testSingleStructure = new GameObject();
        testZone = new GameObject();
        var road = new RoadStructureSO();
        road.buildingName = "Road";
        road.prefab = testRoad;
        var zone = new ZoneStructureSO();
        zone.buildingName = "Commercial";
        zone.prefab = testZone;
        var facility = new SingleFacilitySO();
        facility.buildingName = "PowerPlant";
        facility.prefab = testSingleStructure;
        collection.roadStructure = road;
        collection.singleStructureList = new List<SingleStructureBaseSO>();
        collection.singleStructureList.Add(facility);
        collection.zonesList = new List<ZoneStructureSO>();
        collection.zonesList.Add(zone);
        structureRepository.modelDataCollection = collection;
    }
    // A Test behaves as an ordinary method
    [Test]
    public void StructureRepositoryEditModeGetRoadPrefabPasses()
    {
        GameObject returnObject = structureRepository.GetBuildingPrefabByName("Road", StructureType.Road);
        Assert.AreEqual(testRoad, returnObject);
    }

    [Test]
    public void StructureRepositoryEditModeGetSingleStructurePrefabPasses()
    {
        GameObject returnObject = structureRepository.GetBuildingPrefabByName("PowerPlant", StructureType.SingleStructure);
        Assert.AreEqual(testSingleStructure, returnObject);
    }

    [Test]
    public void StructureRepositoryEditModeGetZonePrefabPasses()
    {
        GameObject returnObject = structureRepository.GetBuildingPrefabByName("Commercial", StructureType.Zone);
        Assert.AreEqual(testZone, returnObject);
    }

    public void StructureRepositoryEditModeGetSingleStructurePrefabNullPasses()
    {
        Assert.That(()=> structureRepository.GetBuildingPrefabByName("Powerplant", StructureType.SingleStructure),
            Throws.Exception);
    }

    public void StructureRepositoryEditModeGetZonePrefabNullPasses()
    {
        Assert.That(() => structureRepository.GetBuildingPrefabByName("Commerciel", StructureType.Zone),
            Throws.Exception);
    }

    public void StructureRepositoryEditModeGetRoadPrefabNullPasses()  //At the moment there is no List that contains multiple RoadStructureSO's. If different RoadStructureSO's are implemented test must be updated accordingly.
    {
        Assert.That(() => structureRepository.GetBuildingPrefabByName("Raod", StructureType.Road),
            Throws.Exception);
    }



    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator StructureRepositoryEditModeTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
