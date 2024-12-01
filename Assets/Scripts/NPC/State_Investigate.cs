using System.Collections;
using UnityEngine;

public class State_Investigate : State_Abstract
{
    [SerializeField] float durationOfWait = 2.0f;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float timeToSeePlayer;
    [SerializeField] private Color visionConeColor;
    
    private Coroutine _coroutine;
    private Vector3 _targetPosition;
    private bool _hasStartedLooking;
    
    public override void EnterState(State_Manager manager)
    {
        base.EnterState(manager);
        _vision.SetVisionConeColor(visionConeColor);
        _stateManager.timeToSeePlayer = timeToSeePlayer;
        _targetPosition = _stateManager.InvestigationTarget;
        _coroutine = StartCoroutine(LookAround());
    }

    private IEnumerator LookAround()
    {
        Debug.LogWarning("LOOK AROUND ENTERED");
        _navMeshAgent.isStopped = true;
        Vector3 rightTarget = transform.right; // Local right
        Vector3 leftTarget = -transform.right; // Local left
        
        Vector3 targetVector = _targetPosition;
        
        while (RotateTowardTarget(targetVector, rotationSpeed) > 0.1f)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1);
        _navMeshAgent.isStopped = false;
        
        _navMeshAgent.SetDestination(_targetPosition);
        Debug.LogWarning("SETTING NAV TARGET");
        while (_navMeshAgent.remainingDistance != 0)
        {
            yield return new WaitForEndOfFrame();
        }
        
        Debug.LogWarning("LOOKING AROUND");
        
        targetVector = Random.Range(0, 1) < 0.5f ? leftTarget : rightTarget;
        
        yield return new WaitForSeconds(durationOfWait);
        while (RotateTowardTarget(targetVector, rotationSpeed) > 0.1f)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(durationOfWait);
        if (targetVector == rightTarget) targetVector = leftTarget;
        else targetVector = rightTarget;
        while (RotateTowardTarget(targetVector, rotationSpeed) > 0.1f)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(durationOfWait);
        _stateManager.SetState(Enum_GuardStates.Patrol);
    }

    public override void ExitState()
    {
        base.ExitState();
        if(_coroutine != null) StopCoroutine(_coroutine);
    }
}
