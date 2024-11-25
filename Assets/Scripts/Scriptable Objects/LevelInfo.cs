using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfo", menuName = "Scriptable Objects/LevelInfo")]
public class LevelInfo : ScriptableObject
{
    [SerializeField] public string levelName;
    [SerializeField] public string levelDescription;
    [SerializeField] public int alarmsUntilFailure =  3;
    
    [SerializeField] public List<LevelStage> levelStages;
}
