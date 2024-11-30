using System.Collections;
using UnityEngine;

public class State_LookAround : State_Abstract
{
    [SerializeField] private Color visionConeColor;
    [SerializeField] private float durationOfCheck;
    [SerializeField] private float timeToSeesPlayer;
    [SerializeField] private float rotationSpeed;

    private Vector3 _rightTarget;
    private Vector3 _leftTarget;
    private Vector3 _targetVector;
    private bool _hasLookedOnce;
    private bool _isWaiting;
    private float _currentWaitTime = 0;
    private Coroutine _coroutine;
    
    public override void EnterState(State_Manager manager)
    {
        _hasLookedOnce = false;
        _isWaiting = false;
        base.EnterState(manager);
        float coin = Random.Range(0f, 1f);
        _vision.SetVisionConeColor(visionConeColor);
        _stateManager.timeToSeePlayer = timeToSeesPlayer;
        SetLeftAndrightTarget();
        _coroutine = StartCoroutine(WhatDo());
        _navMeshAgent.isStopped = true;
    }

    private void SetLeftAndrightTarget()
    {
        _rightTarget = transform.right; // Local right
        _leftTarget = -transform.right; // Local left
        _targetVector = Random.Range(0, 1) < 0.5f ? _rightTarget : _leftTarget;
    }   

    public override void UpdateState()
    {
        base.UpdateState();
        if (_vision.CanSeeObjectWithTag("Player"))
        {
            _stateManager.SetState(Enum_GuardStates.CanSeePlayer);
        }
    }
    
    private IEnumerator WhatDo()
    {
        yield return new WaitForSeconds(durationOfCheck);
        while (RotateTowardTarget() > 0.1f)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(durationOfCheck);
        if (_targetVector == _rightTarget) _targetVector = _leftTarget;
        else _targetVector = _rightTarget;
        while (RotateTowardTarget() > 0.1f)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(durationOfCheck);
        _stateManager.SetState(Enum_GuardStates.Searching);
    }
    
    private float RotateTowardTarget()
    {
        Vector3 targetPos = transform.position + _targetVector;
        Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        float angleToTarget = Quaternion.Angle(transform.rotation, targetRotation);
        return angleToTarget;
    }

    public override void ExitState()
    {
        base.ExitState();
        _navMeshAgent.isStopped = false;
        StopCoroutine(_coroutine);
    }
}
