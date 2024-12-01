using System;
using UnityEngine;

public class DebugGuard : MonoBehaviour
{
    private Controls _controls;
    private State_Manager _stateManager;

    private void Awake()
    {
        _controls = new Controls();
    }

    private void Start()
    {
        _stateManager = GetComponent<State_Manager>();
        _controls.Debug.Investigate.performed += ctx => InvestigatePlayer();
    }

    private void InvestigatePlayer()
    {
        if (StageManager.Instance == null) return;

        Vector3 playerPos = StageManager.Instance.GetPlayerTransform().position;
        _stateManager.InvestigationTarget = playerPos;
        _stateManager.SetState(Enum_GuardStates.Investigate);
        
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }
}
