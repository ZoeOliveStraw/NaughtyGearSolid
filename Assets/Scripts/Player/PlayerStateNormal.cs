using UnityEngine;

public class PlayerStateNormal : PlayerStateAbstract
{
    [SerializeField] private float stealthSpeed;

    private bool _isStealthPressed;
    
    public override void UpdateState()
    {
        base.UpdateState();
        Inputs();
        MoveCharacter();
        RotateCharacter();
    }
    
    private void Inputs()
    {
        Vector2 _movementVector = InputHandler.MoveDir();
        _3dMoveVector = new Vector3(_movementVector.x, 0, _movementVector.y);
    }

    private void MoveCharacter()
    {
        Vector3 lerpedMovement = Vector3.Lerp(_3dMoveVector * CurrentMoveSpeed(), characterController.velocity, Time.deltaTime * 10f);
        characterController.SimpleMove(lerpedMovement);
    }

    private float CurrentMoveSpeed()
    {
        _isStealthPressed = InputHandler.Stealth.IsPressed();
        return _isStealthPressed ? stealthSpeed : moveMaxSpeed;
    }

    private void RotateCharacter()
    {
        if (_3dMoveVector != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_3dMoveVector);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}
