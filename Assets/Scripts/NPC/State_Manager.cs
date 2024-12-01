using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class State_Manager : MonoBehaviour
{
    [SerializeField] private State_Abstract patrolState;
    [SerializeField] private State_Abstract canSeePlayerState;
    [SerializeField] private State_Abstract chaseState;
    [SerializeField] private State_Abstract lookAroundState;
    [SerializeField] private State_Abstract searchState;
    [SerializeField] private State_Abstract investigateState;
    [SerializeField] private Enum_GuardStates initialState;
    
    [SerializeField] public NavMeshAgent navAgent;
    [SerializeField] public Sense_Vision senseVision;
    [SerializeField] public Sense_Hearing senseHearing;
    
    public Enum_GuardStates previousState;
    public Enum_GuardStates currentStateEnum;
    public float timeToSeePlayer;
    public float timeToSearch = 20;
    public PatrolNode _currentNodeTarget;
    public Vector3 InvestigationTarget;
    
    private State_Abstract currentState;

    private void Start()
    {
        SetState(initialState);
    }

    public void SetState(Enum_GuardStates state)
    {
        Debug.LogWarning($"ENTERING STATE: {state}");
        previousState = currentStateEnum;
        currentStateEnum = state;
        
        switch (state)
        {
            case Enum_GuardStates.Patrol:
                ChangeState(patrolState);
                break;
            case Enum_GuardStates.CanSeePlayer:
                ChangeState(canSeePlayerState);
                break;
            case Enum_GuardStates.Chase:
                ChangeState(chaseState);
                break;
            case Enum_GuardStates.LookAround:
                ChangeState(lookAroundState);
                break;
            case Enum_GuardStates.Searching:
                ChangeState(searchState);
                break;
            case Enum_GuardStates.Investigate:
                ChangeState(investigateState);
                break;
        }
    }

    private void ChangeState(State_Abstract newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
            currentState.enabled = false;
        }

        currentState = newState;
        currentState.enabled = true;
        newState.EnterState(this);
    }

    private void Update()
    {
        if(currentState != null) currentState.UpdateState();
    }
}
