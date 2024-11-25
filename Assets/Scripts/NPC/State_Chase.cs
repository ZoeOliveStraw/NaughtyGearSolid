using UnityEngine;
using UnityEngine.EventSystems;

public class State_Chase : State_Abstract
{
    private bool _canSeePlayer;
    private Transform _playerTransform;
    private Vector3 _lastKnownPlayerPosition;
    [SerializeField] private Color visionConeColor;
    
    public override void EnterState(State_Manager manager)
    {
        base.EnterState(manager);
        SetPlayerReference();
        _vision.SetVisionConeColor(visionConeColor);
    }

    public override void UpdateState()
    {
        _canSeePlayer = CanSeePlayer();
        if(_canSeePlayer) UpdatePlayerPosition();
        if (Vector3.Distance(_lastKnownPlayerPosition, transform.position) < 0.5f)
        {
            if (_canSeePlayer)
            {
                Debug.LogWarning("PLAYER CAUGHT!");
            }
            else
            {
                Debug.LogWarning("PLAYER LOST, EXITING STATE");
                _stateManager.SetState(Enum_GuardStates.Patrol);
            }
        }
    }

    public override void ExitState()
    {
        
    }

    private void SetPlayerReference()
    {
        if (StageManager.Instance != null)
        {
            _playerTransform = StageManager.Instance.GetPlayerTransform();
        }
        else
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private bool CanSeePlayer()
    {
        return _vision.CanSeeObjectWithTag("Player");
    }

    private void UpdatePlayerPosition()
    {
        if (Vector3.Distance(_playerTransform.position, _lastKnownPlayerPosition) > 0.1f)
        {
            _lastKnownPlayerPosition = _playerTransform.position;
        }
        _navMeshAgent.SetDestination(_lastKnownPlayerPosition);
    }
}
