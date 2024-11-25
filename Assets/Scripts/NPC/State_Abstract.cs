using UnityEngine;
using UnityEngine.AI;

public abstract class State_Abstract : MonoBehaviour
{
    protected Sense_Vision _vision;
    protected NavMeshAgent _navMeshAgent;
    protected Animator _animator;
    protected Transform _transform;
    protected State_Manager _stateManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void EnterState(State_Manager manager)
    {
        _vision = manager.senseVision;
        _stateManager = manager;
        _navMeshAgent = manager.navAgent;
        _transform = manager.transform;
    }

    // Update is called once per frame
    public virtual void UpdateState()
    {
        
    }

    public virtual void ExitState()
    {
        
    }
}
