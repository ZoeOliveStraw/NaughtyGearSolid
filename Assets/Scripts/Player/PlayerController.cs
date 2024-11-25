using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Private members
    private Controls _controls;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float moveMaxSpeed;
    private Vector3 _3dMoveVector;
    
    //Public members


    public void Awake()
    {
        _controls = new Controls();
    }

    private void FixedUpdate()
    {
        Inputs();
        MoveCharacter();
        RotateCharacter();
    }
    
    private void Inputs()
    {
        Vector2 _movementVector = _controls.Player.Move.ReadValue<Vector2>();
        _3dMoveVector = new Vector3(_movementVector.x, 0, _movementVector.y);
    }

    private void MoveCharacter()
    {
        Vector3 lerpedMovement = Vector3.Lerp(_3dMoveVector * moveMaxSpeed, characterController.velocity, Time.fixedDeltaTime * 10f);
        characterController.SimpleMove(lerpedMovement);
    }

    private void RotateCharacter()
    {
        if (_3dMoveVector != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_3dMoveVector);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }
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
