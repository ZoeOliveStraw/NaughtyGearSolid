using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class UI_GameplayHUD : MonoBehaviour
{
    [Header("Alarms")]
    private int _remainingAlarms;
    [SerializeField] private GameObject alarmIconPrefab;
    [SerializeField] private Transform alarmParent;
    private List<GameObject> _alarmIcons;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (LevelManager.Instance != null)
        {
            _remainingAlarms = LevelManager.Instance.CurrentAlarms;
            RenderAlarms();
        }
    }

    private void RenderAlarms()
    {
        if(_alarmIcons == null) _alarmIcons = new List<GameObject>();
        foreach (var go in _alarmIcons.ToList())
        {
            _alarmIcons.Remove(go);
            Destroy(go);
        }
        for (int i = 0; i < _remainingAlarms; i++)
        {
            Instantiate(alarmIconPrefab, alarmParent);
        }
    }
}
