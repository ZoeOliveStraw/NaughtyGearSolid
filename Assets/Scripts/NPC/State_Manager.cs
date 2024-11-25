using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class State_Manager : MonoBehaviour
{
    [SerializeField] private State_Abstract patrolState;
    [SerializeField] private State_Abstract canSeePlayerState;
    [SerializeField] private Enum_GuardStates initialState;
    
    [SerializeField] public NavMeshAgent navAgent;
    [SerializeField] public Sense_Vision senseVision;
    [SerializeField] public Sense_Hearing senseHearing;
    
    private State_Abstract currentState;

    public float timeToSeePlayer;
    
    public Enum_GuardStates previousState;
    public Enum_GuardStates currentStateEnum;

    private void Start()
    {
        SetState(initialState);
    }

    public void SetState(Enum_GuardStates state)
    {
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
        }
    }

    public void ChangeState(State_Abstract newState)
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