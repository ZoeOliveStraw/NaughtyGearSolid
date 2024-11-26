using UnityEngine;

public class State_LookAround : State_Abstract
{
    [SerializeField] private Color visionConeColor;
    [SerializeField] private float durationOfCheck;
    [SerializeField] private float timeToSeesPlayer;
    [SerializeField] private float rotationSpeed;

    private Vector3 _rightVector;
    private Vector3 _leftVector;
    private Vector3 _targetVector;
    private bool _hasLookedOnce;
    private bool _isWaiting;
    private float _currentWaitTime = 0;
    
    public override void EnterState(State_Manager manager)
    {
        base.EnterState(manager);
        float coin = Random.Range(0f, 1f);
        _targetVector = coin < 0.5f ? _rightVector : _leftVector;
        _vision.SetVisionConeColor(visionConeColor);
        _stateManager.timeToSeePlayer = timeToSeesPlayer;
        _rightVector = transform.right;
        _leftVector = -transform.right;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_vision.CanSeeObjectWithTag("Player"))
        {
            _stateManager.SetState(Enum_GuardStates.CanSeePlayer);
        }
        RotateTowardsTarget();
    }
    
    private void RotateTowardsTarget()
    {
        if (_isWaiting)
        {
            _currentWaitTime += Time.deltaTime;
            if (_currentWaitTime > durationOfCheck)
            {
                _isWaiting = false;
            }
            else
            {
                return;
            }
        }
        
        if (!_hasLookedOnce)
        {
            _hasLookedOnce = true;
            _targetVector = _targetVector == _rightVector ? _leftVector : _rightVector;
        }
        else
        {
            _stateManager.SetState(Enum_GuardStates.Patrol);
            return;
        }
        Quaternion targetRotation = Quaternion.LookRotation(_targetVector);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
    }
}
