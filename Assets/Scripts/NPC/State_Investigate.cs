using System.Collections;
using UnityEngine;

namespace NPC
{
    public class State_Investigate : State_Abstract
    {
        [SerializeField] float durationOfWait = 2.0f;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float timeToSeePlayer = 0.5f;
        [SerializeField] private Color visionConeColor;
    
        private Coroutine _coroutine;
        private Vector3 _targetPosition;
        private bool _hasStartedLooking;
    
        public override void EnterState(State_Manager manager)
        {
            base.EnterState(manager);
            _vision.SetVisionConeColor(visionConeColor);
            _stateManager.timeToSeePlayer = timeToSeePlayer;
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if (_vision.CanSeeObjectWithTag("Player"))
            {
                _stateManager.SetState(Enum_GuardStates.CanSeePlayer);
            }
        
            if (_coroutine == null)
            {
                _coroutine = StartCoroutine(LookAround());
            }
        }

        private IEnumerator LookAround()
        {
            _targetPosition = _stateManager.InvestigationTarget;
            _navMeshAgent.isStopped = true;
            while (RotateTowardTarget(_targetPosition, rotationSpeed) > 0.1f)
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(durationOfWait);
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_targetPosition);
            while (_navMeshAgent.remainingDistance > 0)
            {
                yield return new WaitForEndOfFrame();
            }
            Vector3 _rightTarget = transform.right; // Local right
            Vector3 _leftTarget = -transform.right; // Local left
            _targetPosition = Random.Range(0, 1) < 0.5f ? _rightTarget : _leftTarget;
            while (RotateTowardTarget(_targetPosition, rotationSpeed) > 0.1f)
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(durationOfWait);
            if (_targetPosition == _rightTarget) _targetPosition = _leftTarget;
            else _targetPosition = _rightTarget;
        
            while (RotateTowardTarget(_targetPosition, rotationSpeed) > 0.1f)
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(durationOfWait);
        
            _stateManager.SetState(Enum_GuardStates.Patrol);
        }

        public override void ExitState()
        {
            base.ExitState();
            _navMeshAgent.isStopped = false;
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
}
