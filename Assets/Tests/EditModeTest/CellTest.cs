﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CellTest
    {

        // A Test behaves as an ordinary method
        [Test]
        public void CellSetGameObjectPass()
        {
            Cell cell = new Cell();
            cell.SetConstruction(new GameObject(),null);
            Assert.IsTrue(cell.IsTaken);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void CellSetGameObjectNullFail()
        {
            Cell cell = new Cell();
            cell.SetConstruction(null,null);
            Assert.IsFalse(cell.IsTaken);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void CellSetGameObjectRemovePass()
        {
            Cell cell = new Cell();
            cell.SetConstruction(new GameObject(),null);
            cell.RemoveStructure();
            Assert.IsFalse(cell.IsTaken);
        }

    }
}
