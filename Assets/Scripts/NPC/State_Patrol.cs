using System.Collections;
using UnityEngine;

public class State_Patrol : State_Abstract
{
    [SerializeField] private PatrolRoute route;
    [SerializeField] private Color visionConeColor;

    private PatrolNode _currentNode;
    private bool isGoingBack;
    private bool isStopped;
    
    public override void EnterState(State_Manager manager)
    {
        base.EnterState(manager);
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
        if (route.NodeCount() <= 1)
        {
            isStopped = true;
            return;
        }
        _currentNode = route.GetNextNode(_currentNode, ref isGoingBack);
        if (_currentNode == null) return;
        _navMeshAgent.SetDestination(_currentNode.transform.position);
    }
}
