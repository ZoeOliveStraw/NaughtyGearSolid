using UnityEngine;

public class State_Search : State_Abstract
{
    [SerializeField] private float searchRadius;
    [SerializeField] private Color visionConeColor;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float timeToSeesPlayer;

    private Vector3 _currentTarget;
    private Vector3 _startPosition;
    private float _timeElapsed;
    private float _searchDuration;

    public override void EnterState(State_Manager manager)
    {
        base.EnterState(manager);
        _searchDuration = _stateManager.timeToSearch;
        _stateManager.timeToSeePlayer = timeToSeesPlayer;
        _navMeshAgent.speed = moveSpeed;
        _vision.SetVisionConeColor(visionConeColor);
        _startPosition = transform.position;
        _timeElapsed = 0;
    }

    public override void UpdateState()
    {
        if (_timeElapsed == 0)
        {
            NavigateToNextNode();
        }

        if (_navMeshAgent.remainingDistance == 0)
        {
            NavigateToNextNode();
        }

        if (_timeElapsed >= _searchDuration && _navMeshAgent.remainingDistance == 0)
        {
            _stateManager.SetState(Enum_GuardStates.Patrol);
        }
        _timeElapsed += Time.deltaTime;
    }
    
    private void NavigateToNextNode()
    {
        if (SearchNodeManager.Instance != null )
        {
            _currentTarget = SearchNodeManager.Instance.GetRandomNodeInRange(_startPosition, searchRadius);
        }
        _navMeshAgent.SetDestination(_currentTarget);
    }
}
