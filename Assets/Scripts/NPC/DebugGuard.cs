using UnityEngine;

namespace NPC
{
    public class DebugGuard : MonoBehaviour
    {
        private Controls _controls;
        private State_Manager _stateManager;

        private void Awake()
        {
            _controls = new Controls();
        }

        private void Start()
        {
            _stateManager = GetComponent<State_Manager>();
            _controls.Debug.Investigate.performed += ctx => InvestigatePlayer();
        }

        private void InvestigatePlayer()
        {
            if (StageManager.Instance == null) return;
        
            _stateManager.InvestigationTarget = StageManager.Instance.GetPlayerTransform().position;
            _stateManager.SetState(Enum_GuardStates.Investigate);
        }

        private void OnEnable()
        {
            _controls.Enable();
        }

        private void OnDisable()
        {
            _controls.Disable();
        }
    }
}
