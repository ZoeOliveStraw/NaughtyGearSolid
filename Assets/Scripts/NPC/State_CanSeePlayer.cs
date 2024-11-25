using UnityEditor.SpeedTree.Importer;
using UnityEngine;
using Color = UnityEngine.Color;

public class State_CanSeePlayer : State_Abstract
{
    [SerializeField] private float rotationSpeed;
    private float timeToSeePlayer { get; set; }
    private Enum_GuardStates _stateToReturnTo;
    private float _currentTimeToSeePlayer;
    private Transform _playerTransform;
    [SerializeField] private Color visionConeColor;
    
    public override void EnterState(State_Manager manager)
    {
        if (StageManager.Instance != null)
        {
            _playerTransform = StageManager.Instance.GetPlayerTransform();
        }
        else
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        
        base.EnterState(manager);
        timeToSeePlayer = _stateManager.timeToSeePlayer;
        timeToSeePlayer = _stateManager.timeToSeePlayer;
        _currentTimeToSeePlayer = 0;
        _stateToReturnTo = _stateManager.previousState;
        _vision.SetVisionConeColor(visionConeColor);
    }

    // Update is called once per frame
    public override void UpdateState()
    {
        bool canSeePlayer = _vision.CanSeeObjectWithTag("Player");
        _currentTimeToSeePlayer = 
            Mathf.Clamp(_currentTimeToSeePlayer += canSeePlayer  ? Time.deltaTime : -Time.deltaTime,-1, timeToSeePlayer);
        if(canSeePlayer) RotateTowardsPlayer();
        if (_currentTimeToSeePlayer < 0)
        {
            _stateManager.SetState(_stateToReturnTo);
        }
        else if (_currentTimeToSeePlayer > timeToSeePlayer)
        {
            Debug.LogWarning($"PLAYER SPOTTED");
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 playPos = _playerTransform.position - transform.position;
        Vector3 adjustedPlayerPos = new Vector3(playPos.x, 0, playPos.z);
        Quaternion targetRotation = Quaternion.LookRotation(adjustedPlayerPos);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
    }
}