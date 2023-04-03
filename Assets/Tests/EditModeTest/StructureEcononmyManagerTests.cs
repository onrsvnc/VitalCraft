using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    [TestFixture]

    public class StructureEcononmyManagerTests
    {
        GridStructure grid;
        GameObject structureObject = new GameObject();

        [SetUp]

        public void Init()
        {
            grid = new GridStructure(3, 10, 10);
        }

        [Test]
        public void StructureEconomyManagerAddResidentalStructureNoRoads()
        {
            ZoneStructureSO residentialZone = CreateZoneAtPosition(new Vector3Int(0, 0, 0));
            Assert.False(residentialZone.HasRoadAccess());
        }

        [Test]
        public void StructureEconomyManagerAddResidentialStructureNearRoadConnection()
        {
            CreateRoadAtPosition(new Vector3Int(3, 0, 0));
            ZoneStructureSO residentialZone = CreateZoneAtPosition(new Vector3Int(0, 0, 0));
            Assert.True(residentialZone.HasRoadAccess());
        }

        [Test]
        public void StructureEconomyManagerAddResidetialStructureNearRoadDiagonalNoConnection()
        {
            CreateRoadAtPosition(new Vector3Int(3, 0, 3));
            ZoneStructureSO residentialZone = CreateZoneAtPosition(new Vector3Int(0, 0, 0));
            Assert.False(residentialZone.HasRoadAccess());
        }

        [Test]
        public void StructureEconomyManagerAddRoadNearResidentalStructureWithConnection()
        {
            ZoneStructureSO residentialZone = CreateZoneAtPosition(new Vector3Int(0, 0, 0));
            CreateRoadAtPosition(new Vector3Int(3, 0, 0));
            Assert.True(residentialZone.HasRoadAccess());
        }

        [Test]
        public void StructureEconomyManagerAddRoadNearResidentalStructureDiagonalNoConnection()
        {
            ZoneStructureSO residentialZone = CreateZoneAtPosition(new Vector3Int(0, 0, 0));
            CreateRoadAtPosition(new Vector3Int(3, 0, 3));
            Assert.False(residentialZone.HasRoadAccess());
        }

        [Test]
        public void StructureEconomyManagerRemoveRoadNearResidentalStructureWithConnection()
        {
            ZoneStructureSO residentialZone = CreateZoneAtPosition(new Vector3Int(0, 0, 0));
            //RoadStructureSO roadStructure = ScriptableObject.CreateInstance<RoadStructureSO>();
            CreateRoadAtPosition(new Vector3Int(3, 0, 0));
            StructureEconomyManager.PrepareRoadDemolition(new Vector3Int(3, 0, 0), grid);
            grid.RemoveStructureFromTheGrid(new Vector3(3, 0, 0));
            Assert.False(residentialZone.HasRoadAccess());
        }

        [Test]
        public void StructureEconomyManagerRemoveRoadNearResidentalStructureDiagonalNoConnection()
        {
            ZoneStructureSO residentialZone = CreateZoneAtPosition(new Vector3Int(0, 0, 0));
            CreateRoadAtPosition(new Vector3Int(3, 0, 3));
            StructureEconomyManager.PrepareRoadDemolition(new Vector3Int(3, 0, 3), grid);
            grid.RemoveStructureFromTheGrid(new Vector3(3, 0, 3));
            Assert.False(residentialZone.HasRoadAccess());
        }

        [Test]
        public void StructureEconomyManagerAddRoadNear3ResidentialStructureConnectionWith1()
        {
            ZoneStructureSO residentialZone = CreateZoneAtPosition(new Vector3Int(0, 0, 0));
            ZoneStructureSO residentialZone1 = CreateZoneAtPosition(new Vector3Int(0, 0, 3));
            ZoneStructureSO residentialZone2 = CreateZoneAtPosition(new Vector3Int(0, 0, 6));
            CreateRoadAtPosition(new Vector3Int(3, 0, 3));
            Assert.True(residentialZone1.HasRoadAccess());
            Assert.False(residentialZone.HasRoadAccess());
            Assert.False(residentialZone2.HasRoadAccess());
        }

        [Test]
        public void StructureEconomyManagerAdd3ResidentialStructureNearRoadConnectionWith1()
        {
            CreateRoadAtPosition(new Vector3Int(3, 0, 3));
            ZoneStructureSO residentialZone = CreateZoneAtPosition(new Vector3Int(0, 0, 0));
            ZoneStructureSO residentialZone1 = CreateZoneAtPosition(new Vector3Int(0, 0, 3));
            ZoneStructureSO residentialZone2 = CreateZoneAtPosition(new Vector3Int(0, 0, 6));
            Assert.True(residentialZone1.HasRoadAccess());
            Assert.False(residentialZone.HasRoadAccess());
            Assert.False(residentialZone2.HasRoadAccess());
        }

        [Test]
        public void StructureEconomyManagerAdd3FacilityStructureNearRoadConnectionWith1()
        {
            CreateRoadAtPosition(new Vector3Int(3, 0, 3));
            SingleFacilitySO facility = CreateFacilityAtPosition(new Vector3Int(0, 0, 0));
            SingleFacilitySO facility1 = CreateFacilityAtPosition(new Vector3Int(0, 0, 3));
            SingleFacilitySO facility2 = CreateFacilityAtPosition(new Vector3Int(0, 0, 6));
            Assert.True(facility1.HasRoadAccess());
            Assert.False(facility.HasRoadAccess());
            Assert.False(facility2.HasRoadAccess());
        }

        [Test]
        public void StructureEconomyManagerAddPowerFacilityNear3Residential3Connected()
        {
            ZoneStructureSO residentialZone = CreateZoneAtPosition(new Vector3Int(0, 0, 0));
            ZoneStructureSO residentialZone1 = CreateZoneAtPosition(new Vector3Int(0, 0, 3));
            ZoneStructureSO residentialZone2 = CreateZoneAtPosition(new Vector3Int(0, 0, 6));
            SingleFacilitySO facility = CreateFacilityAtPosition(new Vector3Int(6, 0, 6), FacilityType.Power);
            Assert.True(residentialZone.HasPower());
            Assert.True(residentialZone1.HasPower());
            Assert.True(residentialZone2.HasPower());
            Assert.True(facility.GetNumberOfCustomers() == 3);
        }

        [Test]
        public void StructureEconomyManagerRemovePowerFacilityNear3Residential3Connected()
        {
            ZoneStructureSO residentialZone = CreateZoneAtPosition(new Vector3Int(0, 0, 0));
            ZoneStructureSO residentialZone1 = CreateZoneAtPosition(new Vector3Int(0, 0, 3));
            ZoneStructureSO residentialZone2 = CreateZoneAtPosition(new Vector3Int(0, 0, 6));
            SingleFacilitySO facility = CreateFacilityAtPosition(new Vector3Int(6, 0, 6), FacilityType.Power);
            StructureEconomyManager.PrepareFacilityDemolition(new Vector3Int(6, 0, 6), grid);
            grid.RemoveStructureFromTheGrid(new Vector3Int(6, 0, 6));
            Assert.False(residentialZone.HasPower());
            Assert.False(residentialZone1.HasPower());
            Assert.False(residentialZone2.HasPower());
            Assert.True(grid.GetStructureDataFromTheGrid(new Vector3Int(6, 0, 6)) == null);
        }

        [Test]
        public void StructureEconomyManager3ResidentialsConnectedToFacilityRemove2()
        {
            ZoneStructureSO residentialZone = CreateZoneAtPosition(new Vector3Int(0, 0, 0));
            ZoneStructureSO residentialZone1 = CreateZoneAtPosition(new Vector3Int(0, 0, 3));
            ZoneStructureSO residentialZone2 = CreateZoneAtPosition(new Vector3Int(0, 0, 6));
            SingleFacilitySO facility = CreateFacilityAtPosition(new Vector3Int(6, 0, 6), FacilityType.Power);
            StructureEconomyManager.PrepareStructureForDemolition(new Vector3Int(0, 0, 0), grid);
            StructureEconomyManager.PrepareStructureForDemolition(new Vector3Int(0, 0, 3), grid);
            grid.RemoveStructureFromTheGrid(new Vector3Int(0, 0, 0));
            grid.RemoveStructureFromTheGrid(new Vector3Int(0, 0, 3));
            Assert.True(residentialZone2.HasPower());
            Assert.True(facility.GetNumberOfCustomers() == 1);
        }

        [Test]
        public void StructureEconomyManagerPlaceResidentialAfterFacilityConnect()
        {
            SingleFacilitySO facility = CreateFacilityAtPosition(new Vector3Int(6, 0, 0), FacilityType.Power);
            ZoneStructureSO residentialZone = CreateZoneAtPosition(new Vector3Int(0, 0, 0));
            Assert.True(residentialZone.HasPower());
            Assert.True(facility.GetNumberOfCustomers() == 1);
        }

        [Test]
        public void StructureEconomyManagerPlaceResidentialAfterFacilityTooFar()
        {
            SingleFacilitySO facility = CreateFacilityAtPosition(new Vector3Int(6, 0, 6), FacilityType.Power);
            ZoneStructureSO residentialZone = CreateZoneAtPosition(new Vector3Int(0, 0, 0));
            Assert.False(residentialZone.HasPower());
            Assert.True(facility.GetNumberOfCustomers() == 0);
        }

        [Test]
        public void StructureEconomyManagerPlaceResidentialAfterFacilityTooSmallFacilityRange()
        {
            SingleFacilitySO facility = CreateFacilityAtPosition(new Vector3Int(6, 0, 0), FacilityType.Power);
            facility.singleStructureRange = 1;
            ZoneStructureSO residentialZone = CreateZoneAtPosition(new Vector3Int(0, 0, 0));
            Assert.False(residentialZone.HasPower());
            Assert.True(facility.GetNumberOfCustomers() == 0);
        }







        private SingleFacilitySO CreateFacilityAtPosition(Vector3Int position, FacilityType facilityType = FacilityType.None)
        {
            SingleFacilitySO facility = new SingleFacilitySO();
            facility.requireRoadAccess = true;
            facility.singleStructureRange = 3;
            facility.facilityType = facilityType;
            facility.maxCustomers = 3;
            grid.PlaceStructureOnTheGrid(structureObject, position, facility);
            StructureEconomyManager.PrepareFacilityStructure(position, grid);
            return facility;
        }


        private void CreateRoadAtPosition(Vector3Int position)
        {
            RoadStructureSO roadStructure = ScriptableObject.CreateInstance<RoadStructureSO>();
            grid.PlaceStructureOnTheGrid(structureObject, position, roadStructure);
            StructureEconomyManager.PrepareRoadStructure(position, grid);
        }

        private ZoneStructureSO CreateZoneAtPosition(Vector3Int position)
        {
            ZoneStructureSO residentialZone = CreateResidentialZone();
            grid.PlaceStructureOnTheGrid(structureObject, position, residentialZone);
            StructureEconomyManager.PrepareZoneStructure(position, grid);
            return residentialZone;
        }

        private static ZoneStructureSO CreateResidentialZone()
        {
            ZoneStructureSO residentialZone = ScriptableObject.CreateInstance<ZoneStructureSO>();
            residentialZone.requireRoadAccess = true;
            residentialZone.requirePower = true;
            residentialZone.requireWater = true;
            residentialZone.upkeepCost = 30;
            residentialZone.maxFacilitySearchRange = 2;
            return residentialZone;
        }
    }


}
