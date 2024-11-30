using System.Collections;
using UnityEngine;

public class State_Patrol : State_Abstract
{
    [SerializeField] private PatrolRoute route;
    [SerializeField] private Color visionConeColor;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float timeToSeesPlayer;

    private PatrolNode _currentNode;
    private bool isGoingBack;
    private bool isStopped;
    
    public override void EnterState(State_Manager manager)
    {
        base.EnterState(manager);
        _stateManager.timeToSeePlayer = timeToSeesPlayer;
        _navMeshAgent.speed = moveSpeed;
        NavigateToNextNode();
        _vision.SetVisionConeColor(visionConeColor);
    }

    // Update is called once per frame
    public override void UpdateState()
    {
        base.UpdateState();

        if (_vision.CanSeeObjectWithTag("Player"))
        {
            _stateManager.SetState(Enum_GuardStates.CanSeePlayer);
        }
        
        if (isStopped) return;
        if (_navMeshAgent.remainingDistance == 0)
        {
            StartCoroutine(WaitBeforeNextNode());
        }
    }

    private IEnumerator WaitBeforeNextNode()
    {
        isStopped = true;
        if (_currentNode != null)
        {
            yield return new WaitForSeconds(_currentNode.waitAtNode);
        }
        isStopped = false;
        NavigateToNextNode();
    }
    
    private void NavigateToNextNode()
    {
        if (_currentNode == null && _stateManager._currentNodeTarget != null)
        {
            _currentNode = _stateManager._currentNodeTarget;
            return;
        }
        
        if (route.NodeCount() <= 1)
        {
            isStopped = true;
            return;
        }
        _currentNode = route.GetNextNode(_currentNode, ref isGoingBack);
        if (_currentNode == null) return;
        _navMeshAgent.SetDestination(_currentNode.transform.position);
    }

    public override void ExitState()
    {
        base.ExitState();
        _stateManager._currentNodeTarget = _currentNode;
    }
}
