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


    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetNewMainCamera(stageCamera);
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
}
