using TMPro;
using UnityEngine;

public class UI_MapScreenCanvas : MonoBehaviour
{
    [HideInInspector] public LevelInfo levelInfo;

    [SerializeField] private TextMeshProUGUI txtLevelName;
    [SerializeField] private TextMeshProUGUI txtLevelDescription;
    

    public void OnEnable()
    {
        MapScreen.Instance.LevelCanvasActive = true;
        levelInfo = MapScreen.Instance.currentlySelectedLevel;
        gameObject.SetActive(true);
        Initialize();
    }

    private void Initialize()
    {
        txtLevelName.text = levelInfo.levelName;
        txtLevelDescription.text = levelInfo.levelDescription;
    }

    public void OnDisable()
    {
        MapScreen.Instance.LevelCanvasActive = false;
        levelInfo = null;
        gameObject.SetActive(false);
    }

    public void Click_Play()
    {
        Debug.LogWarning("UI_MapScreenCanvas.Click_Play() called!");
        MapScreen.Instance.StartLevel();
    }
}
