using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using NSubstitute; //Using Nsubsitute tool for mocking objects. 

namespace Tests
{
    [TestFixture] //The [TestFixture] attribute is used in NUnit framework to mark a class that contains unit tests. 
                  //The presence of this attribute signals to the framework that the class contains unit tests and that it should be processed accordingly.
    public class PlayerStatusTest
    {
        UIController uiController;
        GameManager gameManagerComponent;
        PlacementManager placementManager; //Added line for testing.

        // Old Test Setup without the use of Nsubstitute
        // [SetUp] // Init() will be executed before each test 
        // public void Init()
        // {
        //     UnityEngine.GameObject gameManagerObject = new UnityEngine.GameObject();
        //     var cameraMovementComponent = gameManagerObject.AddComponent<CameraMovement>();

        //     uiController = gameManagerObject.AddComponent<UIController>();
        //     UnityEngine.GameObject buttonBuildObject = new UnityEngine.GameObject();
        //     UnityEngine.GameObject cancelButtonObject = new UnityEngine.GameObject();
        //     UnityEngine.GameObject cancelPanel = new UnityEngine.GameObject();
        //     UnityEngine.GameObject closeBuildMenuButtonObject = new UnityEngine.GameObject();//note to self: added line
        //     uiController.cancelActionButton = cancelButtonObject.AddComponent<Button>();
        //     uiController.closeBuildMenuButton = closeBuildMenuButtonObject.AddComponent<Button>(); //note to self: added line
        //     var buttonBuildComponent = buttonBuildObject.AddComponent<Button>();
        //     uiController.buildResidentialAreaButton = buttonBuildComponent;
        //     //uiController.buildResidentialAreaButton.onClick.AddListener(uiController.OnBuildAreaCallback()); //note to self: added line Note: Commented out to be fixed later
        //     uiController.cancelActionPanel = cancelButtonObject;

        //     uiController.buildingMenuPanel = cancelPanel;
        //     uiController.openBuildMenuButton = uiController.cancelActionButton;
        //     uiController.demolishButton = uiController.cancelActionButton;

        //     gameManagerComponent = gameManagerObject.AddComponent<GameManager>();
        //     gameManagerComponent.cameraMovement = cameraMovementComponent;
        //     gameManagerComponent.uiController = uiController;
        // }

        [SetUp] // Init() will be executed before each test 
        public void Init()
        {
            UnityEngine.GameObject gameManagerObject = new UnityEngine.GameObject();
            UnityEngine.GameObject placementManagerTestGameObject = new UnityEngine.GameObject(); //Added line for testing
            
            var cameraMovementComponent = gameManagerObject.AddComponent<CameraMovement>();

            uiController = Substitute.For<UIController>(); //Usually interface or abstract classes are used to subsitute for.
            placementManager = Substitute.For<PlacementManager>(); //Added line for testing

            placementManagerTestGameObject.AddComponent<PlacementManager>(); //Added line for testing
            gameManagerComponent = gameManagerObject.AddComponent<GameManager>();
            gameManagerComponent.cameraMovement = cameraMovementComponent;
            gameManagerComponent.uiController = uiController;
            gameManagerComponent.placementManagerGameObject = placementManagerTestGameObject; //Added line for tesing
        }

        [UnityTest]
        public IEnumerator PlayerStatusPlayerBuildingSingleStructureStateTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return new WaitForEndOfFrame(); //This calls Awake
            yield return new WaitForEndOfFrame(); //This calls Start
            gameManagerComponent.State.OnBuildSingleStructure(null);
            yield return new WaitForEndOfFrame();
            Assert.IsTrue(gameManagerComponent.State is PlayerBuildingSingleStructureState);
            yield return null;
        }

        [UnityTest]
        public IEnumerator PlayerStatusPlayerBuildingAreaStateTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return new WaitForEndOfFrame(); //This calls Awake
            yield return new WaitForEndOfFrame(); //This calls Start
            gameManagerComponent.State.OnBuildZone(null);
            yield return new WaitForEndOfFrame();
            Assert.IsTrue(gameManagerComponent.State is PlayerBuildingZoneState);
            yield return null;
        }

        [UnityTest]
        public IEnumerator PlayerStatusPlayerBuildingRoadStateTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return new WaitForEndOfFrame(); //This calls Awake
            yield return new WaitForEndOfFrame(); //This calls Start
            gameManagerComponent.State.OnBuildRoad(null);
            yield return new WaitForEndOfFrame();
            Assert.IsTrue(gameManagerComponent.State is PlayerBuildingRoadState);
            yield return null;
        }

        [UnityTest]
        public IEnumerator PlayerStatusPlayerRemoveBuildingStateTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return new WaitForEndOfFrame(); //This calls Awake
            yield return new WaitForEndOfFrame(); //This calls Start
            gameManagerComponent.State.OnDemolishAction();
            yield return new WaitForEndOfFrame();
            Assert.IsTrue(gameManagerComponent.State is PlayerDemolitionState);
            yield return null;
        }

        [UnityTest]
        public IEnumerator PlayerStatusPlayerSelectionStateTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return new WaitForEndOfFrame(); //This calls Awake
            yield return new WaitForEndOfFrame(); //This calls Start
            Assert.IsTrue(gameManagerComponent.State is PlayerSelectionState);
            yield return null;
        }
    }


}

