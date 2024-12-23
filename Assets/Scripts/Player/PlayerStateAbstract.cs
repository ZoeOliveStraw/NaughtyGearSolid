using UnityEngine;

public class PlayerStateAbstract : MonoBehaviour
{
    [SerializeField] protected CharacterController characterController;
    [SerializeField] protected float moveMaxSpeed;
    protected Vector3 _3dMoveVector;
    
    public virtual void EnterState()
    {
        
    }

    public virtual void UpdateState()
    {
        
    }

    public virtual void ExitState()
    {
        
    }
}
