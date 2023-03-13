using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GridStructureTests
    {
        GridStructure structure;
        [OneTimeSetUp]
        public void Init()
        {
            structure = new GridStructure(3, 100, 100);
        }

        #region GridPositionTest
        // A Test behaves as an ordinary method
        [Test]
        public void CalculateGridPositionPasses()
        {
            //Arrange
            Vector3 position = new Vector3(0, 0, 0);
            //Act
            Vector3 returnPosition = structure.CalculateGridPosition(position);
            //Assert
            Assert.AreEqual(Vector3.zero, returnPosition);
        }

        [Test]
        public void CalculateGridPositionFloatsPasses()
        {
            //Arrange
            Vector3 position = new Vector3(2.9f, 0, 2.9f);
            //Act
            Vector3 returnPosition = structure.CalculateGridPosition(position);
            //Assert
            Assert.AreEqual(Vector3.zero, returnPosition);
        }

        [Test]
        public void CalculateGridPositionFail()
        {
            //Arrange
            Vector3 position = new Vector3(3.1f, 0, 0);
            //Act
            Vector3 returnPosition = structure.CalculateGridPosition(position);
            //Assert
            Assert.AreNotEqual(Vector3.zero, returnPosition);
        }
        #endregion

        #region GridIndexTest
        // A Test behaves as an ordinary method
        [Test]
        public void PlaceStructure303AndCheckIsTakenPasses()
        {
            //Arrange
            Vector3 position = new Vector3(3, 0, 3);
            //Act
            Vector3 returnPosition = structure.CalculateGridPosition(position);
            UnityEngine.GameObject testGameObject = new UnityEngine.GameObject("TestGameObject");
            structure.PlaceStructureOnTheGrid(testGameObject, returnPosition, null);
            //Assert
            Assert.IsTrue(structure.IsCellTaken(returnPosition));
        }

        [Test]
        public void PlaceStructureMINAndCheckIsTakenPasses()
        {
            //Arrange
            Vector3 position = new Vector3(0, 0, 0);
            //Act
            Vector3 returnPosition = structure.CalculateGridPosition(position);
            UnityEngine.GameObject testGameObject = new UnityEngine.GameObject("TestGameObject");
            structure.PlaceStructureOnTheGrid(testGameObject, returnPosition, null);
            //Assert
            Assert.IsTrue(structure.IsCellTaken(returnPosition));
        }

        [Test]
        public void PlaceStructureMAXAndCheckIsTakenPasses()
        {
            //Arrange
            Vector3 position = new Vector3(297, 0, 297);
            //Act
            Vector3 returnPosition = structure.CalculateGridPosition(position);
            UnityEngine.GameObject testGameObject = new UnityEngine.GameObject("TestGameObject");
            structure.PlaceStructureOnTheGrid(testGameObject, returnPosition, null);
            //Assert
            Assert.IsTrue(structure.IsCellTaken(returnPosition));
        }

        [Test]
        public void PlaceStructure303AndCheckIsTakenNullObjectShouldFail()
        {
            //Arrange
            Vector3 position = new Vector3(3, 0, 3);
            //Act
            Vector3 returnPosition = structure.CalculateGridPosition(position);
            UnityEngine.GameObject testGameObject = null;
            structure.PlaceStructureOnTheGrid(testGameObject, returnPosition, null);
            //Assert
            Assert.IsFalse(structure.IsCellTaken(returnPosition));
        }

        [Test]
        public void PlaceStructureAndCheckIsTakenIndexOutOfBoundsFail()
        {
            //Arrange
            Vector3 position = new Vector3(303, 0, 303);
            //Act
            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => structure.IsCellTaken(position));
        }
        #endregion

        #region CellTest
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
        public void GetPositionOfTheNeighbourIfExistsTestPass()
        {

            Vector3 position = new Vector3(0, 0, 0);

            var neighbourPosition = structure.GetPositionOfTheNeighbourIfExists(position, Direction.Up);
            Assert.AreEqual(new Vector3Int(0, 0, 3), neighbourPosition.Value);
        }

        [Test]
        public void GetPositionOfTheNeighbourIfExistsTestFail()
        {

            Vector3 position = new Vector3(0, 0, 0);

            var neighbourPosition = structure.GetPositionOfTheNeighbourIfExists(position, Direction.Down);
            Assert.IsFalse(neighbourPosition.HasValue);
        }
        
    }

    

}

