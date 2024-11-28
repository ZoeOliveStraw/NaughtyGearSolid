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
    
    public override void EnterState(State_Manager manager)
    {
        _hasLookedOnce = false;
        _isWaiting = false;
        base.EnterState(manager);
        float coin = Random.Range(0f, 1f);
        _vision.SetVisionConeColor(visionConeColor);
        _stateManager.timeToSeePlayer = timeToSeesPlayer;
        SetLeftAndrightTarget();
    }

    private void SetLeftAndrightTarget()
    {
        Vector3 leftTarget = (-Vector3.right).normalized;
        Vector3 rightTarget = (Vector3.right).normalized;
        _targetVector = Random.Range(0,1) < 0.5f ? rightTarget : leftTarget;
    }   

    public override void UpdateState()
    {
        base.UpdateState();
        if (_vision.CanSeeObjectWithTag("Player"))
        {
            _stateManager.SetState(Enum_GuardStates.CanSeePlayer);
        }
        WhatDo();
    }
    
    private void WhatDo()
    {
        float currentAngle = RotateTowardTarget();
        Debug.LogWarning($"POS: {transform.position}, TARGET: {_targetVector}, ANGLE: {currentAngle}");
        if (_isWaiting)
        {
            Debug.LogWarning("WAITING");
            _currentWaitTime += Time.deltaTime;
            if (_currentWaitTime >= durationOfCheck)
            {
                _currentWaitTime = 0;
                _isWaiting = false;
            }
        }
        else if( _hasLookedOnce)
        {
            if (currentAngle < 0.1f)
            {
                if(!_hasLookedOnce)
                {
                    if(_targetVector == _leftTarget)
                    {
                        _targetVector = _rightTarget;
                    }
                    else
                    {
                        _targetVector = _leftTarget;
                    }

                    _currentWaitTime = 0;
                    _isWaiting = true;
                    _hasLookedOnce = true;
                }
                else
                {
                    _stateManager.SetState(Enum_GuardStates.Patrol);
                }
            }
        }
    }
    
    private float RotateTowardTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(transform.position - _targetVector);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        float angleToTarget = Quaternion.Angle(transform.rotation, targetRotation);
        return angleToTarget;
    }
}
