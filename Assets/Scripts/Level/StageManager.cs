using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class StageManager : MonoBehaviour
{
    [SerializeField] private Camera stageCamera;
    [SerializeField] private GameObject playerController;
    
    public static StageManager Instance;

    private Controls _controls;


    private void Awake()
    {
        Instance = this;
        _controls = new Controls();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetNewMainCamera(stageCamera);
        }

        _controls.Debug.Alarm.performed += ctx => Alarm();
    }

    private void Alarm()
    {
        Debug.LogWarning("ALARM CAUSED!");
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.Alarm();
        }
    }

    public Transform GetPlayerTransform()
    {
        return playerController.transform;
    }

    public Vector3 GetPlayerPosition()
    {
        return playerController.transform.position;
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
