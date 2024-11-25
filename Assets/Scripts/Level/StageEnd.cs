using System;
using UnityEngine;

public class StageEnd : MonoBehaviour
{
    public StageManager StageManager { get; set; }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager.Instance.TryLoadNextStage();
        }
    }
}
