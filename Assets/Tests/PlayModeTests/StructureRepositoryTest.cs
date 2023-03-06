using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class StructureRepositoryTest
    {
        StructureRepository repository;

        [OneTimeSetUp]
        public void Init()
        {
            CollectionSO collection = ScriptableObject.CreateInstance<CollectionSO>();
            var road = ScriptableObject.CreateInstance<RoadStructureSO>();
            road.buildingName = "Road";
            var facility = ScriptableObject.CreateInstance<SingleFacilitySO>();
            facility.buildingName = "PowerPlant";
            var zone = ScriptableObject.CreateInstance<ZoneStructureSO>();
            zone.buildingName = "Commercial";
            collection.roadStructure = road;
            collection.singleStructureList = new List<SingleStructureBaseSO>();
            collection.singleStructureList.Add(facility);
            collection.zonesList = new List<ZoneStructureSO>();
            collection.zonesList.Add(zone);
            GameObject testObject = new GameObject();
            repository = testObject.AddComponent<StructureRepository>();
            repository.modelDataCollection = collection;
        }

        [UnityTest]
        public IEnumerator StructureRepositoryTestZoneListQuantityPasses()
        {
            //yield return new WaitForEndOfFrame();
            //yield return new WaitForEndOfFrame();
            int quantity = repository.GetZoneNames().Count;
            yield return new WaitForEndOfFrame();
            Assert.AreEqual(1, quantity);
        }

        [UnityTest]
        public IEnumerator StructureRepositoryTestSingleStructureListNamePasses()
        {
            string name = repository.GetSingleStructureNames()[0];
            yield return new WaitForEndOfFrame();
            Assert.AreEqual("PowerPlant", name);
        }

    }

}
