using UnityEngine;

public class State_Search : State_Abstract
{
    [SerializeField] private float searchRadius;
    [SerializeField] private Color visionConeColor;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float timeToSeesPlayer;

    private SearchNode _currentNode;
    private Vector3 startPosition;

    public override void EnterState(State_Manager manager)
    {
        base.EnterState(manager);
        _stateManager.timeToSeePlayer = timeToSeesPlayer;
        _navMeshAgent.speed = moveSpeed;
        _vision.SetVisionConeColor(visionConeColor);
        startPosition = transform.position;
    }

    public override void UpdateState()
    {
        if (_navMeshAgent.remainingDistance == 0)
        {
            NavigateToNextNode();
        }
    }
    
    private void NavigateToNextNode()
    {
        if (SearchNodeManager.Instance != null)
        {
            _navMeshAgent.SetDestination(SearchNodeManager.Instance.GetRandomNodeInRange(startPosition, searchRadius));
        }
    }

    public void ExitState()
    {
        
    }
}
