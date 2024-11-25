using UnityEngine;

[CreateAssetMenu(fileName = "LevelStage", menuName = "Scriptable Objects/LevelStage")]
public class LevelStage : ScriptableObject
{
    [SerializeField] public string levelSceneName;
}
