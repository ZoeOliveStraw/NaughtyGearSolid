using UnityEngine;

namespace Player
{
    public enum PlayerStates
    {
        Normal,
        Carrying
    }
    public class PlayerStateController : MonoBehaviour
    {
        [SerializeField] private PlayerStateAbstract PlayerStateNormal;
        [SerializeField] private PlayerStateAbstract PlayerStateCarrying;
        [SerializeField] private PlayerStates initialState;

        private PlayerStates previousState;
        private PlayerStates currentStateEnum;
        private PlayerStateAbstract currentState;
        
            private void Start()
            {
                SetState(initialState);
            }
        
            public void SetState(PlayerStates state)
            {
                Debug.LogWarning($"ENTERING STATE: {state}");
                previousState = currentStateEnum;
                currentStateEnum = state;
                
                switch (state)
                {
                    case PlayerStates.Normal:
                        ChangeState(PlayerStateNormal);
                        break;
                    case PlayerStates.Carrying:
                        ChangeState(PlayerStateCarrying);
                        break;
                }
            }
        
            private void ChangeState(PlayerStateAbstract newState)
            {
                if (currentState != null)
                {
                    currentState.ExitState();
                    currentState.enabled = false;
                }
        
                currentState = newState;
                currentState.enabled = true;
                newState.EnterState();
            }
        
            private void Update()
            {
                if(currentState != null) currentState.UpdateState();
            }
    }
}
