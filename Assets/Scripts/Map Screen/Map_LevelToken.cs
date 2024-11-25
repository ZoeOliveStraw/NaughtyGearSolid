using System;
using UnityEngine;

public class Map_LevelToken : MonoBehaviour
{
    [SerializeField] private LevelInfo _levelInfo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MapScreen.Instance.currentlySelectedLevel = _levelInfo;   
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MapScreen.Instance.currentlySelectedLevel = _levelInfo;   
        }
    }
}
