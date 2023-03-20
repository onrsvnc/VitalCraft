using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    [TestFixture] //Added when some data will be placed inside.
    public class GridStructureTests
    {
        GridStructure grid;
        [SetUp]
        public void Init()
        {
            grid = new GridStructure(3, 100, 100);
        }

        #region GridPositionTests
        // A Test behaves as an ordinary method
        [Test]
        public void CalculateGridPositionPasses()
        {
            //Arrange
            Vector3 position = new Vector3(0, 0, 0);
            //Act
            Vector3 returnPosition = grid.CalculateGridPosition(position);
            //Assert
            Assert.AreEqual(Vector3.zero, returnPosition);
        }

        [Test]
        public void CalculateGridPositionFloatsPasses()
        {
            //Arrange
            Vector3 position = new Vector3(2.9f, 0, 2.9f);
            //Act
            Vector3 returnPosition = grid.CalculateGridPosition(position);
            //Assert
            Assert.AreEqual(Vector3.zero, returnPosition);
        }

        [Test]
        public void CalculateGridPositionFail()
        {
            //Arrange
            Vector3 position = new Vector3(3.1f, 0, 0);
            //Act
            Vector3 returnPosition = grid.CalculateGridPosition(position);
            //Assert
            Assert.AreNotEqual(Vector3.zero, returnPosition);
        }

        [Test]
        public void GetPositionOfTheNeighbourIfExistsTestPass()
        {

            Vector3 position = new Vector3(0, 0, 0);

            var neighbourPosition = grid.GetPositionOfTheNeighbourIfExists(position, Direction.Up);
            Assert.AreEqual(new Vector3Int(0, 0, 3), neighbourPosition.Value);
        }

        [Test]
        public void GetPositionOfTheNeighbourIfExistsTestFail()
        {

            Vector3 position = new Vector3(0, 0, 0);

            var neighbourPosition = grid.GetPositionOfTheNeighbourIfExists(position, Direction.Down);
            Assert.IsFalse(neighbourPosition.HasValue);
        }

        [Test]
        public void GetAllPositionsFromTo()
        {
            Vector3Int startPosition = new Vector3Int(0, 0, 0);
            Vector3Int endPosition = new Vector3Int(6, 0, 3);

            var returnValues = grid.GetAllPositionsFromTo(startPosition, endPosition);
            Assert.IsTrue(returnValues.Count == 6);
            Assert.IsTrue(returnValues.Contains(new Vector3Int(0, 0, 0)));
            Assert.IsTrue(returnValues.Contains(new Vector3Int(3, 0, 0)));
            Assert.IsTrue(returnValues.Contains(new Vector3Int(6, 0, 0)));
            Assert.IsTrue(returnValues.Contains(new Vector3Int(0, 0, 3)));
            Assert.IsTrue(returnValues.Contains(new Vector3Int(3, 0, 3)));
            Assert.IsTrue(returnValues.Contains(new Vector3Int(6, 0, 3)));
        }
        #endregion

        #region GridIndexTests
        // A Test behaves as an ordinary method
        [Test]
        public void PlaceStructure303AndCheckIsTakenPasses()
        {
            //Arrange
            Vector3 position = new Vector3(3, 0, 3);
            //Act
            Vector3 returnPosition = grid.CalculateGridPosition(position);
            UnityEngine.GameObject testGameObject = new UnityEngine.GameObject("TestGameObject");
            grid.PlaceStructureOnTheGrid(testGameObject, returnPosition, null);
            //Assert
            Assert.IsTrue(grid.IsCellTaken(returnPosition));
        }

        [Test]
        public void PlaceStructureMINAndCheckIsTakenPasses()
        {
            //Arrange
            Vector3 position = new Vector3(0, 0, 0);
            //Act
            Vector3 returnPosition = grid.CalculateGridPosition(position);
            UnityEngine.GameObject testGameObject = new UnityEngine.GameObject("TestGameObject");
            grid.PlaceStructureOnTheGrid(testGameObject, returnPosition, null);
            //Assert
            Assert.IsTrue(grid.IsCellTaken(returnPosition));
        }

        [Test]
        public void PlaceStructureMAXAndCheckIsTakenPasses()
        {
            //Arrange
            Vector3 position = new Vector3(297, 0, 297);
            //Act
            Vector3 returnPosition = grid.CalculateGridPosition(position);
            UnityEngine.GameObject testGameObject = new UnityEngine.GameObject("TestGameObject");
            grid.PlaceStructureOnTheGrid(testGameObject, returnPosition, null);
            //Assert
            Assert.IsTrue(grid.IsCellTaken(returnPosition));
        }

        [Test]
        public void PlaceStructure303AndCheckIsTakenNullObjectShouldFail()
        {
            //Arrange
            Vector3 position = new Vector3(3, 0, 3);
            //Act
            Vector3 returnPosition = grid.CalculateGridPosition(position);
            UnityEngine.GameObject testGameObject = null;
            grid.PlaceStructureOnTheGrid(testGameObject, returnPosition, null);
            //Assert
            Assert.IsFalse(grid.IsCellTaken(returnPosition));
        }

        [Test]
        public void PlaceStructureAndCheckIsTakenIndexOutOfBoundsFail()
        {
            //Arrange
            Vector3 position = new Vector3(303, 0, 303);
            //Act
            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => grid.IsCellTaken(position));
        }
        #endregion

        #region GridCellTests
        [Test]
        public void CellSetGameObjectPass()
        {
            //Arrange
            Cell cell = new Cell();
            //Act
            cell.SetConstruction(new UnityEngine.GameObject(), null);
            //Assert
            Assert.IsTrue(cell.IsTaken);
        }

        [Test]
        public void CellSetGameObjectNullFail()
        {
            //Arrange
            Cell cell = new Cell();
            //Act
            cell.SetConstruction(null, null);
            //Assert
            Assert.IsFalse(cell.IsTaken);
        }
        #endregion

        [Test]
        public void GetDataStructureTest()
        {
            RoadStructureSO road = ScriptableObject.CreateInstance<RoadStructureSO>();
            SingleStructureBaseSO singleStructure = ScriptableObject.CreateInstance<SingleFacilitySO>();
            GameObject gameObject = new GameObject();
            grid.PlaceStructureOnTheGrid(gameObject, new Vector3(0, 0, 0), road);
            grid.PlaceStructureOnTheGrid(gameObject, new Vector3(99, 0, 0), road);
            grid.PlaceStructureOnTheGrid(gameObject, new Vector3(0, 0, 99), singleStructure);
            grid.PlaceStructureOnTheGrid(gameObject, new Vector3(99, 0, 99), singleStructure);
            var list = grid.GetAllStructures().ToList();
            Assert.IsTrue(list.Count == 4);
        }


    }



}

