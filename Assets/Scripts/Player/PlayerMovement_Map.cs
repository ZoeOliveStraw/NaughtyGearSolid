using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement_Map : MonoBehaviour
{
    //Private members
    private Controls _controls;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float moveMaxSpeed;
    private Vector3 _3dMoveVector;

    public void Awake()
    {
        _controls = new Controls();
    }

    private void Start()
    {
        InputHandler.Controls.UI.Submit.performed += ctx => Debug.LogWarning("INTERACT");
    }

    private void FixedUpdate()
    {
        if (MapScreen.Instance.LevelCanvasActive) return;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Level Token"))
        {
            InputHandler.Controls.UI.Submit.performed += ctx => SelectLevel();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Level Token"))
        {
            InputHandler.Controls.UI.Submit.performed -= ctx => SelectLevel();
        }   
    }

    private void SelectLevel()
    {
        MapScreen.Instance.SetLevelCanvas(!MapScreen.Instance.LevelCanvasActive);
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
