using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlacementManager placementManager;
    public IInputManager inputManager;
    public UIController uiController;
    public int width, length;
    public CameraMovement cameraMovement;
    public LayerMask inputMask;
    private BuildingManager buildingManager;
    private int cellSize = 3;

    private PlayerState state;

    public PlayerSelectionState selectionState;
    public PlayerBuildingSingleStructureState buildingSingleStructureState;
    public PlayerRemoveBuildingState demolishState;

    public PlayerState State { get => state; } //Exposed for testing purposes

    void Awake()
    {
        buildingManager = new BuildingManager(cellSize, width, length, placementManager);
        selectionState = new PlayerSelectionState(this, cameraMovement);
        demolishState = new PlayerRemoveBuildingState(this, buildingManager);
        buildingSingleStructureState = new PlayerBuildingSingleStructureState(this, buildingManager);
        state = selectionState;
#if (UNITY_EDITOR && TEST) || !(UNITY_IOS || UNITY_ANDROID)
        inputManager = gameObject.AddComponent<InputManager>();
#endif
#if (UNITY_IOS || UNITY_ANDROID)

#endif        
    }

    void Start()
    {
        PrepareGameComponents();
        AssignInputListeners();
        AssignUIControllerListeners();
    }

    private void PrepareGameComponents()
    {
        inputManager.MouseInputMask = inputMask;
        cameraMovement.SetCameraLimits(0, width, 0, length);
    }

    private void AssignUIControllerListeners()
    {
        uiController.AddListenerOnBuildAreaEvent(StartPlacementMode);
        uiController.AddListenerOnCancelActionEvent(CancelAction);
        uiController.AddListenerOnDemolishActionEvent(StartDemolishMode);
    }

    private void AssignInputListeners()
    {
        inputManager.AddListenerOnPointerDownEvent(HandleInput);
        inputManager.AddListenerOnPointerChangeEvent(HandlePointerChange);
        inputManager.AddListenerOnPointerSecondChangeEvent(HandleInputCameraPan);
        inputManager.AddListenerOnPointerSecondUpEvent(HandleInputCameraPanStop);
    }

    private void StartDemolishMode()
    {
        TransitionToState(demolishState, null);
    }

    private void HandlePointerChange(Vector3 position)
    {
        state.OnInputPointerChange(position);
    }

    private void HandleInputCameraPanStop()
    {
        state.OnInputPanUp();
    }

    private void HandleInputCameraPan(Vector3 position)
    {
        state.OnInputPanChange(position);
    }

    private void HandleInput(Vector3 position)
    {
        state.OnInputPointerDown(position);
    }

    private void StartPlacementMode(string variable)
    {
        TransitionToState(buildingSingleStructureState, variable);
    }

    private void CancelAction()
    {
        state.OnCancel();
    }

    public void TransitionToState(PlayerState newState, string variable)
    {
        // States handling state transitions could be better, but not implemented at the moment.
        this.state = newState;
        this.state.EnterState(variable);
    }

}
