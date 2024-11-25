using TMPro;
using UnityEngine;

public class MapScreen : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private GameObject levelCanvas;
    
    public bool LevelCanvasActive;
    
    [HideInInspector] public LevelInfo currentlySelectedLevel = null;
    public static MapScreen Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.LogWarning("MAP SCREEN STARTED!");
        GameManager.Instance.SetNewMainCamera(sceneCamera);
        levelCanvas.SetActive(false);
    }

    public void SetLevelCanvas(bool active)
    {
        levelCanvas.SetActive(active);
    }

    public void StartLevel()
    {
        GameManager.Instance.currentLevel = currentlySelectedLevel;
        GameManager.Instance.EnterState(Enum_GameStates.Gameplay);
    }
}
