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
        if (_timeElapsed == 0 || _navMeshAgent.remainingDistance == 0)
        {
            Debug.LogWarning($"TIME 0, NAVIGATING");
            NavigateToNextNode();
        }

        if (_timeElapsed >= _searchDuration && _navMeshAgent.remainingDistance == 0)
        {
            Debug.LogWarning($"DURATION EXCEEDED: {_timeElapsed}, SETTING STATE");
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

        if (_currentTarget == transform.position)
        {
            _currentTarget = GetPositionWithoutCollision(transform.position, searchRadius, 50);
        }
    }
    
    public static Vector3 GetPositionWithoutCollision(Vector3 origin, float radius, float checkRadius = 0.5f, int maxAttempts = 100)
    {
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * radius;
            randomOffset.y = 0;
            Vector3 candidatePosition = origin + randomOffset;
            
            if (!Physics.CheckSphere(candidatePosition, checkRadius))
            {
                return candidatePosition;
            }
        }
        Debug.LogWarning("No valid position found after max attempts.");
        return origin;
    }
}
