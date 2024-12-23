using UnityEngine;
using Color = UnityEngine.Color;

namespace NPC
{
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
            _navMeshAgent.isStopped = true;
        }

        // Update is called once per frame
        public override void UpdateState()
        {
            bool canSeePlayer = _vision.CanSeeObjectWithTag("Player");
            Debug.LogWarning($"CANSEEPLAYER: {canSeePlayer}");
            _currentTimeToSeePlayer = _currentTimeToSeePlayer += canSeePlayer ? Time.deltaTime : -Time.deltaTime;
            Mathf.Clamp(_currentTimeToSeePlayer,0, timeToSeePlayer);
            if(canSeePlayer) RotateTowardsPlayer();
            if (_currentTimeToSeePlayer <= 0)
            {
                _stateManager.SetState(_stateToReturnTo);
            }
            else if (_currentTimeToSeePlayer >= timeToSeePlayer)
            {
                Debug.LogWarning($"BAR FILLED, CHASING");
                _stateManager.SetState(Enum_GuardStates.Chase);
            }
        }

        private void RotateTowardsPlayer()
        {
            Vector3 playPos = _playerTransform.position - transform.position;
            Vector3 adjustedPlayerPos = new Vector3(playPos.x, 0, playPos.z);
            Quaternion targetRotation = Quaternion.LookRotation(adjustedPlayerPos);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
        }

        public override void ExitState()
        {
            base.ExitState();
            _navMeshAgent.isStopped = false;
        }
    
    }
}
